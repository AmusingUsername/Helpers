using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Linq;

namespace Helper.IOHelper
{
    public class WebHelper
    {
#region Instance and Static Properties
        protected static string staticUserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.116 Safari/537.36";
        /// TODO: clean up the staticAccept. Images not necessary at current functionality. Undecided on binary implementation.
        protected static string staticAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";

        public string proxyURI { set; get; }
        public int proxyPort { set; get; }
        public string headerUserAgent { set { headerUserAgent = value; }
            get { return (!string.IsNullOrWhiteSpace(headerUserAgent) ? headerUserAgent : WebHelper.staticUserAgent); } }
        public string headerAccept { set { headerAccept = value; }
            get { return (!string.IsNullOrWhiteSpace(headerAccept) ? headerAccept : WebHelper.staticAccept); } }

        protected Dictionary<string, Func<Stream, Stream>> decoders;
#endregion Instance and Static Properties

#region Constructors
        public WebHelper()
        {
            this.decoders = WebHelper.getDefaultDecoders();
        }

        public WebHelper(string webProxyURI, int webProxyPort = -1) : this()
        {
            this.proxyURI = webProxyURI;
            this.proxyPort = webProxyPort;
        }
        #endregion Constructors

#region Internal/Protected/Private functions/methods
        /// <summary>
        /// Gets the list of default decoders associated with the name they typically appear as in response headers. As of this time, only GZip and Deflate streams are supported.
        /// </summary>
        /// <returns>List of default stream decoders, intended for decrypting response streams.</returns>
        protected static Dictionary<string, Func<Stream, Stream>> getDefaultDecoders()
        {
            Dictionary<string, Func<Stream, Stream>> staticDecoders = new Dictionary<string, Func<Stream, Stream>>();
            Func<Stream, Stream> fGetGZipStream = (s) => { return new GZipStream(s, CompressionMode.Decompress); };
            Func<Stream, Stream> fGetDeflateStream = (s) => { return new DeflateStream(s, CompressionMode.Decompress); };
            staticDecoders.Add("gzip", fGetGZipStream);
            staticDecoders.Add("deflate", fGetDeflateStream);
            return staticDecoders;
        }
        
        /// TODO: Future functionality: modify this to return a stream so it can be used to retrieve binary files and new functions for returning file from stream.
        /// <summary>
        /// Static/true worker retrievePage function. Retrives text pages from specified URI.
        /// </summary>
        /// <param name="httpAddress">URI to retrieve.</param>
        /// <param name="webProxyURI">Web proxy URI to use for this request.</param>
        /// <param name="webProxyPort">Web proxy port to use for this request.</param>
        /// <param name="paramUserAgent">User agent string to specify in request headers.</param>
        /// <param name="paramAccept">Accept string to specify in request headers.</param>
        /// <param name="paramDecoders">Name-Decoders Dictionary for decoding the response.</param>
        /// <returns></returns>
        protected static string retrievePage(string httpAddress, string webProxyURI, int webProxyPort, string paramUserAgent, string paramAccept, Dictionary<string, Func<Stream, Stream>> paramDecoders)
        {
            HttpWebRequest storeRequest = HttpWebRequest.CreateHttp(httpAddress);
            storeRequest.UserAgent = paramUserAgent;
            storeRequest.Accept = paramAccept;
            storeRequest.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            storeRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, StringHelper.stringArrayToCSV(paramDecoders.Keys.ToArray()));
            storeRequest.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");

            if (!string.IsNullOrWhiteSpace(webProxyURI))
            {
                storeRequest.Proxy = new WebProxy(webProxyURI, webProxyPort);
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)storeRequest.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NonAuthoritativeInformation)
                {
                    Stream responseStream = response.GetResponseStream();
                    string[] contentEncodings = StringHelper.csvToStringArray(response.ContentEncoding.Trim().ToLower());
                    for (int i = 0; i < contentEncodings.Length; i++)
                    {
                        contentEncodings[i] = contentEncodings[i].ToLower();
                        if ( paramDecoders != null && paramDecoders.ContainsKey(contentEncodings[i]))
                        {
                            responseStream = paramDecoders[contentEncodings[i]](responseStream);
                        }
                        else
                        {
                            throw new Exception(string.Format("WebHelper.retrievePage: Encoding not supported or not specified. Create an instance of WebHelper to specify a decoder for {0}.", contentEncodings[i]));
                        }
                    }

                    Encoding enc = Encoding.Default;
                    if (!string.IsNullOrWhiteSpace(response.CharacterSet))
                    {
                        enc = Encoding.GetEncoding(response.CharacterSet);
                    }
                    return new StreamReader(responseStream, enc).ReadToEnd();
                }
                else
                {
                    throw new Exception("Webserver returned a non-200 response.");
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
#endregion Internal/Protected/Private functions/methods

#region Public functions/methods
        /// <summary>
        /// Add a decoder to the supported decoders for decoding response streams.
        /// </summary>
        /// <param name="encoding">Encoding name as it appears in request accept and response content encoding headers.</param>
        /// <param name="fGetStream">Function taking an encrypted Stream and returning a decrypted Stream.</param>
        public void AddDecoder(string encoding, Func<Stream, Stream> fGetStream)
        {
            this.decoders.Add(encoding, fGetStream);
        }

        /// TODO: test this... no use for it yet... primarily intended for accepting new decoders and specifying custom user agent.
        /// <summary>
        /// Retrieve a text-based file from a specified URI.
        /// </summary>
        /// <param name="httpAddress">Address to retrieve</param>
        /// <param name="webProxyURI">Proxy URI, direct response is used if not specified, value is null, empty or whitespace. Modifies object proxy setting for future requests.</param>
        /// <param name="webProxyPort">Port for the proxy server. Ignored if webProxyURI value is null, empty or whitespace. Modifies object proxy setting for future requests.</param>
        /// <returns></returns>
       public string retrievePage(string httpAddress, string webProxyURI = "", int webProxyPort = 80)
        {
            this.proxyURI = webProxyURI;
            this.proxyPort = webProxyPort;
            return retrievePage(httpAddress, this.proxyURI, this.proxyPort, this.headerUserAgent, this.headerAccept, this.decoders);
        }

        /// <summary>
        /// Static method for retrieving a a text-based file from a specified URI.
        /// </summary>
        /// <param name="httpAddress">Address to retrieve</param>
        /// <param name="webProxyURI">Proxy URI, direct response is used if not specified, value is null, empty or whitespace.</param>
        /// <param name="webProxyPort">Port for the proxy server. Ignored if webProxyURI value is null, empty or whitespace.</param>
        /// <returns></returns>
        public static string retrievePageStatic(string httpAddress, string webProxyURI = "", int webProxyPort = 80)
        {
            return retrievePage(httpAddress, webProxyURI, webProxyPort, WebHelper.staticUserAgent, WebHelper.staticAccept, WebHelper.getDefaultDecoders());
        }

#endregion Public functions/methods
    }
}

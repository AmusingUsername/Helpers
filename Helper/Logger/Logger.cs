using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary></summary>
    public enum VerbosityLevel {
        /// <summary></summary>
        None, 
        /// <summary></summary>
        Fatal,
        /// <summary></summary>
        Exception,
        /// <summary></summary>
        Error,
        /// <summary></summary>
        Warning,
        /// <summary></summary>
        Info,
        /// <summary></summary>
        Debug,
        /// <summary></summary>
        Trace
    };
    /// <summary></summary>
    public enum LogOutputOption
    {
        /// <summary>Log no output. Ignored when masked with other options.</summary>
        None = 0,
        /// <summary>Log output using System.Diagnostics.Trace.Write.</summary>
        DebugTrace = 1,
        /// <summary>Log output to file. Will output to executable directory by default.</summary>
        File = 2,
        /// <summary>Log output to console.</summary>
        Console = 4,
        /// <summary>Log output using specified delegate. If no delegate is specified, nothing will be output.</summary>
        Delegate = 1024
    };
    /// <summary></summary>
    public enum LogOutputFormat
    {
        /// <summary>Output the message only. Format equivalent to "{0}".</summary>
        MessageOnly,
        /// <summary>Approximates the default Log4j format. See "http://logging.apache.org/log4j/2.x/".</summary>
        Log4j,
        /// <summary>Approximates the Log4j-XMLLayout format. See "http://logging.apache.org/log4j/1.2/apidocs/org/apache/log4j/xml/XMLLayout.html".</summary>
        Log4j_XMLLayout,        
        /// <summary>Approximates the Logback-classic format. See "http://logback.qos.ch/".</summary>
        LogBack,
        /// <summary>Custom consumer specified format. Call setFormatCustom(string) to specify format.</summary>
        Custom
    };

    /// <summary>Logger class provides logging in standard and custom formats at different verbosity levels and custom output options.</summary>
    public class Logger
    {
        /// <summary>Dictionary of possible logging formats</summary>
        protected Dictionary<LogOutputFormat, string> dictOutputFormats = new Dictionary<LogOutputFormat, string>()
        {
            { LogOutputFormat.MessageOnly, "{0}" },
            { LogOutputFormat.Log4j, "" },
            { LogOutputFormat.Log4j_XMLLayout, "" },
            { LogOutputFormat.LogBack, "" },
            { LogOutputFormat.Custom, "{0}" }
        };
        /// <summary>Output format for Logger output</summary>
        protected LogOutputFormat outputFormat;
        /// <summary>Minimum verbosity level message that will be output</summary>
        protected VerbosityLevel outputVerbosity;
        /// <summary>Mask of options for output</summary>
        protected LogOutputOption outputOptions;

        /// <summary>
        /// Instantiate a new logger with the specified options.
        /// </summary>
        /// <param name="outputVerbosity">Minimum verbosity level message that will be output</param>
        /// <param name="outputOption">Mask of options for output</param>
        /// <param name="logFormat">Output format for Logger output</param>
        public Logger(VerbosityLevel outputVerbosity, LogOutputOption outputOption = LogOutputOption.DebugTrace, LogOutputFormat logFormat = LogOutputFormat.MessageOnly)
        {
            setVerbosity(outputVerbosity);
            this.outputFormat = logFormat;
            this.outputOptions = outputOption;
        }

        /// <summary>Set the output verbosity level. All logging below this level will be ignored.</summary>
        /// <param name="outputVerbosity">Verbosity level for output</param>
        /// <remarks>Impacts only log items specified after calling. Items not yet output are not cached if not set to previous level.</remarks>
        public void setVerbosity(VerbosityLevel outputVerbosity)
        {
            this.outputVerbosity = outputVerbosity;
        }
        
        /// <summary>
        /// Set the custom format to format for logging and specify format.
        /// </summary>
        /// <param name="format">
        /// <para>String format for output using the composite format string rules (DateTime and Time formats can be specified). Output parts as follows:</para>
        /// <para>{0} - Message,
        /// {1} - DateTime message passed to Logger,
        /// {2} - Time message passed to Logger,
        /// {3} - Calling Function Name,
        /// {4} - Calling Source File Path,
        /// {5} - Calling Source Line Number,
        /// {6} - Process ID</para>
        /// </param>
        /// <seealso cref="https://msdn.microsoft.com/en-us/library/txafckwd(v=vs.110).aspx"/>
        public bool setFormatCustom(string format)
        {
            try
            {
                string.Format(format, "message", DateTime.UtcNow, DateTime.UtcNow.TimeOfDay, "caller()", "c:\\caller.cs", 13, 1313);
            }
            catch(FormatException)
            {
                return false;
            }
            this.outputFormat = LogOutputFormat.Custom;
            this.dictOutputFormats[LogOutputFormat.Custom] = format;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceLine"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        public async void logMethodEntry([System.Runtime.CompilerServices.CallerLineNumber] int sourceLine = -1,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceLine"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        public async void logMethodExit([System.Runtime.CompilerServices.CallerLineNumber] int sourceLine = -1,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Flush log entries synchronusly. 
        /// </summary>
        public void flushLogEntries()
        {

        }
    }
}

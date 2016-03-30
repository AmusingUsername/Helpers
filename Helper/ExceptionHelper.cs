using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Helper
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Format and output exception and nested inner exception details.
        /// </summary>
        /// <param name="ex">Exception to parse to string.</param>
        /// <param name="outputFormat">String format for exception output. {0}=Exception Type, {1}=Message, {2}=Stack Trace, {3}=Data Dictionary</param>
        /// <returns>Formatted string with exception details.</returns>
        public static string toStringExceptionFormatted(this Exception ex, string outputFormat, string outputDataDictFormat = "", UInt16 maxInnerExceptionDepth = 10)
        {
            return toStringExceptionFormatted(ex, true, true, false, outputFormat, outputDataDictFormat, maxInnerExceptionDepth);
        }

        /// <summary>
        /// Format and output exception and nested inner exception details.
        /// </summary>
        /// <param name="ex">Exception to parse to string.</param>
        /// <param name="outputMessage">Output the exception message?</param>
        /// <param name="outputStack">Output the exception stack trace?</param>
        /// <param name="outputDataDict">Output the exception data dictionary?</param>
        /// <param name="maxInnerExceptionDepth">Maximum inner exceptions to output.</param>
        /// <returns>Formatted string with exception details.</returns>
        public static string toStringExceptionFormatted(this Exception ex, bool outputMessage = true, bool outputStack = true, bool outputDataDict = false, UInt16 maxInnerExceptionDepth = 10)
        {
            return toStringExceptionFormatted(ex, outputMessage, outputStack, outputDataDict, null, null, maxInnerExceptionDepth);
        }

        /// <summary>
        /// Format and output exception and nested inner exception details.
        /// </summary>
        /// <param name="ex">Exception to parse to string.</param>
        /// <param name="outputMessage">Output the exception message?</param>
        /// <param name="outputStack">Output the exception stack trace?</param>
        /// <param name="outputDataDict">Output the exception data dictionary?</param>
        /// <param name="outputFormat">String format for exception output, following newline will be appended. {0}=Exception Type, {1}=Message, {2}=Stack Trace, {3}=Data Dictionary</param>
        /// <param name="outputDataDictFormat">String format for exception Data output. {0}=key, {1}=value</param>
        /// <param name="maxInnerExceptionDepth">Maximum inner exceptions to output.</param>
        /// <returns>Formatted string with exception details.</returns>
        private static string toStringExceptionFormatted(this Exception ex, bool outputMessage = true, bool outputStack = true, bool outputDataDict = false, string outputFormat = "", string outputDataDictFormat = "", UInt16 maxInnerExceptionDepth = 10)
        {
            string retVal = string.Empty;
            bool underlineException = false;
            if (string.IsNullOrWhiteSpace(outputFormat))
            {
                underlineException = true;
                outputFormat = "{0}";
                outputFormat += (outputMessage ? Environment.NewLine + "Exception Message: {1}" : string.Empty);
                outputFormat += (outputStack ? Environment.NewLine + "Exception Stack Trace: " + Environment.NewLine + "{2}" : string.Empty);
                outputFormat += (outputDataDict ? Environment.NewLine + "Exception Data: " + Environment.NewLine + "{3}" : string.Empty);
                outputFormat += Environment.NewLine;
            }
            if(string.IsNullOrWhiteSpace(outputDataDictFormat))
            {
                outputDataDictFormat = "{0} : {1}" + Environment.NewLine;
            }

            //break if there is no inner exception or we're at maxInnerExceptionDepth
            for (int innerExceptionCount = 0; ex != null && innerExceptionCount++ < maxInnerExceptionDepth+1; ex = ex.InnerException)
            {
                string dataDictString = string.Empty;
                if (outputDataDict && ex.Data != null)//format the Data property of this Exception
                {
                    foreach (DictionaryEntry pair in ex.Data)
                    {
                        dataDictString += string.Format(outputDataDictFormat, pair.Key.ToString(), pair.Value.ToString());
                    }
                }

                retVal += string.Format(outputFormat, (underlineException ? ex.GetType().ToString().underlineString() : ex.GetType().ToString())
                                        , ex.Message, ex.StackTrace, dataDictString) + Environment.NewLine;
            }
            return retVal;
        }

        /// <summary>
        /// Checks to see if the exception is or contains an inner exception of type T.
        /// </summary>
        /// <typeparam name="T">Exception type to check for.</typeparam>
        /// <param name="ex">Exception to check.</param>
        /// <param name="maxInnerExceptionDepth">Maximum inner exceptions to output.</param>
        /// <returns></returns>
        public static bool hasExceptionType<T>(Exception ex, UInt16 maxInnerExceptionDepth = 10) where T : Exception
        {
            bool retVal = false;
            for (int innerExceptionCount = 0; ex != null || innerExceptionCount++ > maxInnerExceptionDepth; ex = ex.InnerException)
            {
                if(ex.GetType() == typeof(T))
                {
                    retVal = true;
                    break;
                }
            }
            return retVal;
        }
    }
}

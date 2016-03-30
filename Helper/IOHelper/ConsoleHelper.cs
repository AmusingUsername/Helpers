using System;
using System.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helper;

namespace Helper.IOHelper
{
    public static class ConsoleHelper
    {
        //TODO: Add triple slash commenting
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Only Primitive types and uint types are accepted currently.</typeparam>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static T inputPrompt<T>(string prompt)
        {
            Console.Write(prompt + (hasPromptSuffix(prompt) ? string.Empty : ": "));
            if (typeof(T) == typeof(SecureString)) //obscure output, can't call console.readline.
            {
                return (T)Convert.ChangeType(inputGetSecureString(), typeof(T));
            }
            else
            {
                string response = Console.ReadLine();
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(response, typeof(T));
                }
                //TODO: see if we can improve inputPrompt conversions with ConvertFromString
                //else if(TypeHelper.checkType(typeof(T), typeof(int), typeof(short), typeof(long),
                //                            typeof(byte), typeof(sbyte), typeof(decimal), typeof(uint),
                //                            typeof(UInt16), typeof(UInt64), typeof(Single), typeof(double)))
                //{
                //    try
                //    {
                //        System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(response);
                //    }
                //    catch(Exception ex)
                //    {
                //        if(ExceptionHelper.hasExceptionType<OverflowException>(ex))
                //        {
                //            throw new InvalidResponseException("Response was out of the bounds for the requested type.");
                //        }
                //        else
                //        {
                //            throw;
                //        }
                //    }
                //}
#if DEBUG == false
                else if (typeof(T) == typeof(int))
                {
                    int parsedValue;
                    if (int.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(short))
                {
                    short parsedValue;
                    if (short.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(long))
                {
                    long parsedValue;
                    if (long.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(byte))
                {
                    byte parsedValue;
                    if (byte.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(sbyte))
                {
                    sbyte parsedValue;
                    if (sbyte.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(decimal))
                {
                    decimal parsedValue;
                    if (decimal.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(uint))
                {
                    uint parsedValue;
                    if (uint.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(UInt16))
                {
                    UInt16 parsedValue;
                    if (UInt16.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(UInt64))
                {
                    UInt64 parsedValue;
                    if (UInt64.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(Single))
                {
                    Single parsedValue;
                    if (Single.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(double))
                {
                    double parsedValue;
                    if (double.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
#endif
                else if (typeof(T) == typeof(DateTime))
                {
                    DateTime parsedValue;
                    if (DateTime.TryParse(response, out parsedValue))
                    {
                        return (T)Convert.ChangeType(parsedValue, typeof(T));
                    }
                }
                else if (typeof(T) == typeof(bool))
                {
                    if (string.Equals(response, "true", StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(response, "t", StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(response, "yes", StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(response, "y", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return (T)Convert.ChangeType(true, typeof(T)); ;
                    }
                    else if (string.Equals(response, "false", StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(response, "f", StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(response, "no", StringComparison.CurrentCultureIgnoreCase)
                        || string.Equals(response, "n", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return (T)Convert.ChangeType(false, typeof(T)); ;
                    }
                }
                else
                {
                    throw new TypeHelper.InvalidTypeParameterException("inputPrompt does not support type of " + typeof(T).ToString() + ".");
                }
            }
            throw new InvalidResponseException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static SecureString inputGetSecureString()
        {
            string obfuscationCharacter = "\x2550"; //═(box drawing double horizontal) obscures the length of the password visibly while being typed 
            string backspace = "\b \b";
            SecureString retVal = new SecureString();
            if(Console.IsInputRedirected)
            {
                foreach(char key in Console.ReadLine().ToCharArray())
                {
                    retVal.AppendChar(key);
                }
            }
            else
            {
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key != ConsoleKey.Enter
                        && keyInfo.Key != ConsoleKey.Backspace)
                    {
                        Console.Write(obfuscationCharacter);
                        retVal.AppendChar(keyInfo.KeyChar);
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (retVal.Length > 0)
                        {
                            Console.Write(backspace);//necessary to properly erase the character
                            retVal.RemoveAt(retVal.Length - 1);
                        }
                    }

                } while (keyInfo.Key != ConsoleKey.Enter);
                for (int i = 0; i < retVal.Length; i++) { Console.Write(backspace); } //further obfuscate by erasing the obfuscation characters
                Console.WriteLine();
            }
            return retVal;
        }
        
        private static bool hasPromptSuffix(string prompt)
        {
            bool retVal = false;
            if(System.Text.RegularExpressions.Regex.IsMatch(prompt, ".+[\\W]+\\z"))
            {
                retVal = true;
            }
            return retVal;
        }
    }
    public class InvalidResponseException : Exception
    {
        public InvalidResponseException() : base("Response invalid for prompt type.")
        {
        }
        public InvalidResponseException(string message) : base(message)
        {
        }
        public InvalidResponseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// Underlines the specified string with a line of same character length.
        /// </summary>
        /// <param name="stringToUnderline">String to underline</param>
        /// <param name="underlineChar">Character used for underlining the passed string</param>
        /// <returns>Underlined string</returns>
        public static string underlineString(this string stringToUnderline, char underlineChar = '-')
        {
            string retVal = stringToUnderline + Environment.NewLine;
            for(int i = 0; i < stringToUnderline.Length; i++)
            {
                retVal += underlineChar;
            }
            return retVal;
        }

        /// <summary>
        /// Splits a CSV string into items and trims whitespace.
        /// </summary>
        /// <param name="CSV">CSV string</param>
        /// <returns>CSV items in string array</returns>
        public static string[] csvToStringArray(string CSV)
        {
            string[] retVal = CSV.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < retVal.Length; i++) { retVal[i] = retVal[i].Trim(); }
            return retVal;
        }
    }
}

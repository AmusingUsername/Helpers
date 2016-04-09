using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    namespace TypeHelper
    {
        /// <summary>
        /// InvalidTypeParameterException indicates that an unsupported Type parameter was specified where generic type constraints are inadequate.
        /// </summary>
        public class InvalidTypeParameterException : Exception
        {
            public InvalidTypeParameterException() : base("Invalid Type parameter specified.")
            {
            }
            public InvalidTypeParameterException(string message) : base(message)
            {
            }
            public InvalidTypeParameterException(string message, Exception inner) : base(message, inner)
            {
            }
        }  
    }
}

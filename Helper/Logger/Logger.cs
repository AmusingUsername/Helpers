using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public enum VerbosityLevel { None, Fatal, Exception, Error, Warning, Info, Debug, Trace};
    public class Logger
    {

        public async void logMethodEntry([System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            
        }

        public async void logMethodExit([System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Utilities.RSG
{
    [Serializable]
   internal  class TokenFile
    {
        internal List<Token> Tokens { get; set; }
        internal string Pattern { get; set; } 
    }
}
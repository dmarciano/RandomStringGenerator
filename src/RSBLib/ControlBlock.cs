using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Utilities.RSG
{
    internal class ControlBlock
    {
        internal ControlBlockType Type { get; set; }
        internal string FunctionName { get; set; }
        internal Func<string> Function { get; set; }
        internal bool Lazy { get; set; } = true;
        internal bool Global { get; set; }
        internal char[] ExceptValues { get; set; }
    }
}
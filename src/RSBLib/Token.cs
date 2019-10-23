using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Utilities.RSG
{
    internal class Token
    {
        internal TokenType Type { get; set; }
        internal ModifierType? Modifier { get; set; } = null;
        internal int MinimumCount { get; set; } = 1;
        internal int MaximumCount { get; set; } = 1;
        internal string Value { get; set; }
    }
}

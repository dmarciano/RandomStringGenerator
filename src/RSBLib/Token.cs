using System;
using System.Collections.Generic;

namespace SMC.Utilities.RSG
{
    internal class Token
    {
        internal TokenType Type { get; set; }
        internal ModifierType Modifier { get; set; }
        internal int MinimumCount { get; set; } = 1;
        internal int MaximumCount { get; set; } = 1;
        internal string Value { get; set; }
        internal List<string> Values { get; set; }
        internal List<Range> Ranges { get; set; }
        internal ControlBlock ControlBlock { get; set; }
        internal string CultureName { get; set; }
    }
}
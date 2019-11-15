using System.Collections.Generic;

namespace SMC.Utilities.RSG
{
    internal class TokenizedGroup
    {
        internal ModifierType Modifier { get; set; }
        internal List<Token> Tokens { get; set; } = new List<Token>();
        internal int MinimumCount { get; set; } = 1;
        internal int MaximumCount { get; set; } = 1;
        internal ControlBlock ControlBlock { get; set; }
    }
}
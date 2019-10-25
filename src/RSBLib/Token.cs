using System;

namespace SMC.Utilities.RSG
{
    [Serializable]
    internal class Token
    {
        internal TokenType Type { get; set; }
        internal ModifierType Modifier { get; set; }
        public int MinimumCount { get; set; } = 1;
        public int MaximumCount { get; set; } = 1;
        internal string Value { get; set; }
        internal ControlBlock ControlBlock { get; set; }
    }
}
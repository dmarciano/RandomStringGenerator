using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SMC.Utilities.RSG
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class NoPatternException : Exception
    {
        public NoPatternException() : base("Pattern cannot be a null, empty, or whitespace-only string") { }

        public NoPatternException(string message) : base(message) { }

        public NoPatternException(string message, Exception innerException) : base(message, innerException) { }

        public NoPatternException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidPatternException : Exception
    {
        public InvalidPatternException() { }

        public InvalidPatternException(string message) : base(message) { }

        public InvalidPatternException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidPatternException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidModifierException : Exception
    {
        public InvalidModifierException() { }

        public InvalidModifierException(string message) : base(message) { }

        public InvalidModifierException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidModifierException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DuplicateModifierException : Exception
    {
        public DuplicateModifierException() { }

        public DuplicateModifierException(string message) : base(message) { }

        public DuplicateModifierException(string message, Exception innerException) : base(message, innerException) { }

        public DuplicateModifierException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
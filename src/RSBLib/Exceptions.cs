using System;
using System.Runtime.Serialization;

namespace SMC.Utilities.RSG
{
    [Serializable]
    public class NoPatternException : Exception
    {
        public NoPatternException() : base("Pattern cannot be a null, empty, or whitespace-only string") { }

        public NoPatternException(string message) : base(message) { }

        public NoPatternException(string message, Exception innerException) : base(message, innerException) { }

        public NoPatternException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class InvalidPatternException : Exception
    {
        public InvalidPatternException() { }

        public InvalidPatternException(string message) : base(message) { }

        public InvalidPatternException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidPatternException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
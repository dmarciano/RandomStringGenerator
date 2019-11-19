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

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DuplicateGlobalControlBlockException : Exception
    {
        public DuplicateGlobalControlBlockException() { }

        public DuplicateGlobalControlBlockException(string message) : base(message) { }

        public DuplicateGlobalControlBlockException(string message, Exception innerException) : base(message, innerException) { }

        public DuplicateGlobalControlBlockException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class InvalidCultureException : Exception
    {
        public InvalidCultureException() { }

        public InvalidCultureException(string message) : base(message) { }

        public InvalidCultureException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidCultureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class UnknownCultureException : Exception
    {
        public UnknownCultureException() { }

        public UnknownCultureException(string message) : base(message) { }

        public UnknownCultureException(string message, Exception innerException) : base(message, innerException) { }

        public UnknownCultureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class PatternBuilderException : Exception
    {
        public PatternBuilderException() { }

        public PatternBuilderException(string message) : base(message) { }

        public PatternBuilderException(string message, Exception innerException) : base(message, innerException) { }

        public PatternBuilderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
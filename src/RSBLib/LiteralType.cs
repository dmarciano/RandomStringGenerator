using System.ComponentModel;

namespace SMC.Utilities.RSG
{
    /// <summary>
    /// Specifies a specific literal token type.
    /// </summary>
    public enum LiteralType
    {
        /// <summary>
        /// The hypen "-" literal.
        /// </summary>
        [Description("-")]
        HYPEN,
        /// <summary>
        /// The underscore "_" literal.
        /// </summary>
        [Description("_")]
        UNDERSCORE,
        /// <summary>
        /// The backslash "\" literal.
        /// </summary>
        [Description("\\")]
        BACKSLASH,
        /// <summary>
        /// The forward slash "/" literal
        /// </summary>
        [Description("/")]
        FORWARD_SLASH,
        /// <summary>
        /// The whitespace " " literal.
        /// </summary>
        [Description(" ")]
        WHITESPACE
    }
}

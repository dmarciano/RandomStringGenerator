using System.ComponentModel;

namespace SMC.Utilities.RSG
{
    /// <summary>
    /// Specifies the token type.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The token is for a letter only.
        /// </summary>
        [Description("a")]
        LETTER,
        /// <summary>
        /// The token is for a number 0 - 9.
        /// </summary>
        [Description("0")]
        NUMBER,
        /// <summary>
        /// The token is for number 1 - 9
        /// </summary>
        [Description("9")]
        NUMBER_EXCEPT_ZERO,
        /// <summary>
        /// The token is for a symbol.
        /// </summary>
        [Description("@")]
        SYMBOL,
        /// <summary>
        /// The token is for a letter or number 0 - 9.
        /// </summary>
        [Description(".")]
        LETTER_NUMBER,
        /// <summary>
        /// The token is for a letter or symbol.
        /// </summary>
        [Description("+")]
        LETTER_SYMBOL,
        /// <summary>
        /// The token is for a number 0 - 9 or symbol.
        /// </summary>
        [Description("%")]
        NUMBER_SYMBOL,
        /// <summary>
        /// The token is for a letter, number 0 - 9, or symbol.
        /// </summary>
        [Description("*")]
        LETTER_NUMBER_SYMBOL,
        /// <summary>
        /// This token is for representing a character output.
        /// </summary>
        [Description("[]")]
        LITERAL,
        /// <summary>
        /// This token is for representing an optional list block.
        /// </summary>
        [Description("#")]
        OPTIONAL,
        /// <summary>
        /// This token is for representing a character range block.
        /// </summary>
        [Description("<>")]
        RANGE,
        /// <summary>
        /// This token is for representing either a Exclusion Control Block (ECB) or Function Control Block (FCB).
        /// </summary>
        [Description("{}")]
        CONTROL_BLOCK
    }
}
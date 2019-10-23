using System.ComponentModel;

namespace SMC.Utilities.RSG
{
    /// <summary>
    /// Specifies the modifier type.
    /// </summary>
    public enum ModifierType
    {
        /// <summary>
        /// Any letters should be uppercase only.
        /// </summary>
        [Description("^")]
        UPPERCASE,
        /// <summary>
        /// Any letters should be lowercase only.
        /// </summary>
        [Description("!")]
        LOWERCASE,
        /// <summary>
        /// Any numbers should exclude zero.
        /// </summary>
        [Description("~")]
        EXCLUDE_ZERO,
    }
}
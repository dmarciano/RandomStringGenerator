using System;
using System.ComponentModel;

namespace SMC.Utilities.RSG
{
    /// <summary>
    /// Specifies the modifier type.
    /// </summary>
    [Flags]
    public enum ModifierType
    {
        /// <summary>
        /// No modifier.
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Any letters should be uppercase only.
        /// </summary>
        [Description("^")]
        UPPERCASE = 1,
        /// <summary>
        /// Any letters should be lowercase only.
        /// </summary>
        [Description("!")]
        LOWERCASE = 2,
        /// <summary>
        /// Any numbers should exclude zero.
        /// </summary>
        [Description("~")]
        EXCLUDE_ZERO = 3,
    }
}
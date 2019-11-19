using System;
using System.Linq;
using System.Globalization;
using CI = System.Globalization.CultureInfo;

namespace SMC.Utilities.RSG
{
    public class CultureHelper
    {
        internal static CI[] cultures = CI.GetCultures(CultureTypes.AllCultures);

        internal static bool IsCultureValid(string name)
        {
            return null != cultures.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); 
        }
    }
}
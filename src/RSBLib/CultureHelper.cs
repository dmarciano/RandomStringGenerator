using System;
using System.Linq;
using System.Globalization;

namespace SMC.Utilities.RSG
{
    public class CultureHelper
    {
        internal static CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

        internal static bool IsCultureValid(string name)
        {
            return null != cultures.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); 
        }
    }
}
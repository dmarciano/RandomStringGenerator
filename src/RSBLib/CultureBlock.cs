using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMC.Utilities.RSG
{
    public class CultureInfo
    {
        public static readonly CultureInfo DEFAULT = new CultureInfo()
        {
            Name = "en-US",
            UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),
            LowercaseLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray(),
            Numbers = "0123456789".ToCharArray(),
            Symbols = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".ToCharArray(),
            UppercaseLowercaseSpecified = true,
            NumbersSpecified = true,
            SymbolsSpecified = true
        };

        public string Name { get; set; }
        public char[] UppercaseLetters { get; set; }
        public char[] LowercaseLetters { get; set; }
        public char[] Numbers { get; set; }
        public char[] Symbols { get; set; }

        internal bool UppercaseLowercaseSpecified = false;
        internal bool NumbersSpecified = false;
        internal bool SymbolsSpecified = false;
    }
}
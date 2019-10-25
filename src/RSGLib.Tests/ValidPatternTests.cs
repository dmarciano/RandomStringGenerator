using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RSGLib.Tests
{
    [TestClass]
    public class ValidPatternTests
    {
        #region Constructors
        [TestMethod]
        [TestCategory("Constructor")]
        public void ConstructorTest()
        {
            var generator = new Generator();

            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("Constructor")]
        public void ConstructorEmptyStringTest()
        {
            var generator = new Generator(string.Empty);

            Assert.IsTrue(true);
        }
        #endregion

        #region Basic Patterns
        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleCharacterTest()
        {
            var generator = new Generator("a");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberTest()
        {
            var generator = new Generator("0");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberExcludingZeroTest()
        {
            var generator = new Generator("9");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(Convert.ToInt32(output[0]) != 0);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleSymbolTest()
        {
            var generator = new Generator("@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleCharacterWithCryptoTest()
        {
            var generator = new Generator("a", new CryptoRandomGenerator());
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberWithCryptoTest()
        {
            var generator = new Generator("0", new CryptoRandomGenerator());
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberExcludingZeroWithCryptoTest()
        {
            var generator = new Generator("9", new CryptoRandomGenerator());
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(Convert.ToInt32(output[0]) != 0);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleSymbolWithCryptoTest()
        {
            var generator = new Generator("@", new CryptoRandomGenerator());
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLiteralTest()
        {
            var generator = new Generator("[ab]");
            var output = generator.GetString();

            Assert.AreEqual(output, "ab", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLiteralTest2()
        {
            var generator = new Generator("[[]]");
            var output = generator.GetString();

            Assert.AreEqual(output, "[]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLiteralTest3()
        {
            var generator = new Generator("[[ab]]");
            var output = generator.GetString();

            Assert.AreEqual(output, "[ab]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void LetterNumberTest()
        {
            var generator = new Generator(".");
            var output = generator.GetString();

            Assert.IsTrue(char.IsLetterOrDigit(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void LetterSymbolTest()
        {
            var generator = new Generator("+");
            var output = generator.GetString();

            Assert.IsTrue(char.IsLetter(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void NumberSymbolTest()
        {
            var generator = new Generator("%");
            var output = generator.GetString();

            Assert.IsTrue(char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void LetterNumberSymbolTest()
        {
            var generator = new Generator("%");
            var output = generator.GetString();

            Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleCharacterTest()
        {
            var generator = new Generator("a(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleNumberTest()
        {
            var generator = new Generator("0(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsNumber(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleNumberExcludingZeroTest()
        {
            var generator = new Generator("9(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsNumber(c));
                Assert.IsTrue(Convert.ToInt32(c) != 0);
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleSymbolTest()
        {
            var generator = new Generator("@(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsSymbol(c) || char.IsPunctuation(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleLiteralTest()
        {
            var generator = new Generator("[ab](2)");
            var output = generator.GetString();

            Assert.AreEqual(output, "abab", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleLiteralTest2()
        {
            var generator = new Generator("[[]](2)");
            var output = generator.GetString();

            Assert.AreEqual(output, "[][]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleLiteralTest3()
        {
            var generator = new Generator("[[ab]](2)");
            var output = generator.GetString();

            Assert.AreEqual(output, "[ab][ab]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void RangeRepeatTest()
        {
            var generator = new Generator("a(1,5)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length >= 1 && output.Length <= 5);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void ZeroRangeRepeatTest()
        {
            var generator = new Generator("a(0,2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length >= 0 && output.Length <= 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void ZeroRangeRepeatTest2()
        {
            var generator = new Generator("a(,2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length >= 0 && output.Length <= 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
            }
        }
        #endregion

        #region Basic Patterns with Modifiers
        [TestMethod]
        [TestCategory("Modifiers")]
        public void SingleUppercaseLetterTest()
        {
            var generator = new Generator("a^");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsUpper(output[0]));
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void SingleLowercaseLetterTest()
        {
            var generator = new Generator("a!");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void NumberSymbolExceptZeroTest()
        {
            var generator = new Generator("%~");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            Assert.AreNotEqual(Convert.ToInt32(output[0]), 0);
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void MultipleUppercaseLetterTest()
        {
            var generator = new Generator("a^(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
                Assert.IsTrue(char.IsUpper(c));
            }
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void MultipleLowercaseLetterTest()
        {
            var generator = new Generator("a!(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
                Assert.IsTrue(char.IsLower(c));
            }
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void ExcludeZeroTest()
        {
            var generator = new Generator("0~");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(Convert.ToInt32(output[0]) != 0);
        }
        #endregion

        #region Advanced Patterns
        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersTest()
        {
            var generator = new Generator("a0");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]) && char.IsNumber(output[1]));
            Assert.IsTrue(char.IsNumber(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersSymbolsTest()
        {
            var generator = new Generator("a@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void NumbersSymbolsTest()
        {
            var generator = new Generator("0@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void NumbersSymbolsTest2()
        {
            var generator = new Generator("0%");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(char.IsNumber(output[1]) || char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest()
        {
            var generator = new Generator("a0@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsNumber(output[1]));
            Assert.IsTrue(char.IsSymbol(output[2]) || char.IsPunctuation(output[2]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest2()
        {
            var generator = new Generator("a0%");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsNumber(output[1]));
            Assert.IsTrue(char.IsNumber(output[1]) || char.IsSymbol(output[2]) || char.IsPunctuation(output[2]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest3()
        {
            var generator = new Generator("**");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            Assert.IsTrue(char.IsLetter(output[1]) || char.IsNumber(output[1]) || char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest4()
        {
            var generator = new Generator("*a*");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            Assert.IsTrue(char.IsLetter(output[1]));
            Assert.IsTrue(char.IsLetter(output[2]) || char.IsNumber(output[2]) || char.IsSymbol(output[2]) || char.IsPunctuation(output[2]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersTest()
        {
            var generator = new Generator("[Order-]a0");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]));
            Assert.IsTrue(char.IsNumber(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersSymbolsTest()
        {
            var generator = new Generator("[Order-]a@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]));
            Assert.IsTrue(char.IsSymbol(output[7]) || char.IsPunctuation(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralNumbersSymbolsTest()
        {
            var generator = new Generator("[Order-]0@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsNumber(output[6]));
            Assert.IsTrue(char.IsSymbol(output[7]) || char.IsPunctuation(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersSymbolsTest()
        {
            var generator = new Generator("[Order-]a0@");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 9);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]));
            Assert.IsTrue(char.IsNumber(output[7]));
            Assert.IsTrue(char.IsSymbol(output[8]) || char.IsPunctuation(output[8]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersSymbolsTest2()
        {
            var generator = new Generator("[Order-]a0%");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 9);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]));
            Assert.IsTrue(char.IsNumber(output[7]));
            Assert.IsTrue(char.IsNumber(output[8]) || char.IsSymbol(output[8]) || char.IsPunctuation(output[8]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersSymbolsTest3()
        {
            var generator = new Generator("[Order-]**");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]) || char.IsNumber(output[6]) || char.IsSymbol(output[6]) || char.IsPunctuation(output[6]));
            Assert.IsTrue(char.IsLetter(output[7]) || char.IsNumber(output[7]) || char.IsSymbol(output[7]) || char.IsPunctuation(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersSymbolsTest4()
        {
            var generator = new Generator("[Order-]*a*");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 9);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]) || char.IsNumber(output[6]) || char.IsSymbol(output[6]) || char.IsPunctuation(output[6]));
            Assert.IsTrue(char.IsLetter(output[7]));
            Assert.IsTrue(char.IsLetter(output[8]) || char.IsNumber(output[8]) || char.IsSymbol(output[8]) || char.IsPunctuation(output[8]));
        }
        #endregion

        #region Control Blocks
        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalExclusionTest()
        {
            var excluded = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
            var generator = new Generator("{-ABCDEFGHIJKLM}a^");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalExclusionTest2()
        {
            var excluded = new List<char>() { 'a', '{', '}' };
            var generator = new Generator("{-a{}}a!");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void TokenExclusionTest()
        {
            var excluded = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
            var generator = new Generator("a^{-ABCDEFGHIJKLM}");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void TokenExclusionTest2()
        {
            var excluded = new List<char>() { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~' };
            var generator = new Generator("a^{-!\"#$%&'()*+,-./:;<>?@[\\]^_`{|}~}");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalAndTokenExclusionTest()
        {
            var excluded = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'Z' };
            var generator = new Generator("{-Z}a^{-ABCDEFGHIJKLM}");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GeneralDateTimeStringTest()
        {
            var pattern = "{T}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GeneralDateTimeStringRepeatTest()
        {
            var pattern = "{T}(2)";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            //Best way to test for two date/time string concated?
        }

        //TODO: Multiple Date/time

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateStringTest()
        {
            var pattern = "{T:d}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void TimeStringTest()
        {
            var pattern = "{T:t}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateTimeStringTest()
        {
            var pattern = "{T:G}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomDateStringTest()
        {
            var pattern = "{T:MMMM dd, yyyy}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomTimeStringTest2()
        {
            var pattern = "{T:HH:mm:ss}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomTimeStringRepeatTest()
        {
            var pattern = "{T:HH:mm:ss}(2)";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 16);

            var time1 = output.Substring(0, 8);
            var time2 = output.Substring(8);

            Assert.IsTrue(DateTime.TryParse(time1, out _));
            Assert.IsTrue(DateTime.TryParse(time2, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomDateTimeStringTest2()
        {
            var pattern = "{T:MMM dd, yyyy HH:mm:ss zzz}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateTimeStringForceTest()
        {
            var pattern = "{T:MM/dd/yyyy HH:mm:ss.ffffff?}";
            var generator = new Generator(pattern);
            var output1 = generator.ToString();
            var output2 = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output1, out var dt1));
            Assert.IsTrue(DateTime.TryParse(output2, out var dt2));
            Assert.IsTrue(DateTime.Compare(dt1, dt2) == 0);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDTest()
        {
            var pattern = "{G}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(Guid.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDTest2()
        {
            var pattern = "{G:N}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 32);
            Assert.IsTrue(Guid.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDTest3()
        {
            var pattern = "{G:D}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 36);
            Assert.IsTrue(Guid.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDRepeatTest()
        {
            var pattern = "{G}(2)";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            var guid1 = output.Substring(0, 36);
            var guid2 = output.Substring(36);

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDForceTest()
        {
            var pattern = "{G?}";
            var generator = new Generator(pattern);
            var guid1 = generator.ToString();
            var guid2 = generator.ToString();

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.AreEqual(guid1, guid2);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDForceTest2()
        {
            var pattern = "{G:D?}";
            var generator = new Generator(pattern);
            var guid1 = generator.ToString();
            var guid2 = generator.ToString();

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.AreEqual(guid1, guid2);
        }

        //TODO: Control blocks with other characters
        #endregion

        #region Real-World Formats
        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void SocialSecurityNumberTest()
        {
            var regex = new Regex("^\\d{3}-\\d{2}-\\d{4}$");
            var generator = new Generator("0(3)[-]0(2)[-]0(4)");
            var output = generator.GetString();

            Assert.IsTrue(regex.IsMatch(output));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void VisaCardTest()
        {
            var regex = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
            var generator = new Generator("[4]0(12)");
            var oldCardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(oldCardNumber));

            generator.SetPattern("[4]0(15)");
            var newCardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(newCardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void MasterCardTest()
        {
            // MC has many variations for their card numbers.
            var regex = new Regex("^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
            var generator = new Generator("[51]0(14)");
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[52]0(14)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[53]0(14)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[54]0(14)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[55]0(14)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            //MC card numbers can also start with values between 2221 through 2720 but will still be 16 characters long
            for (var start = 2221; start <= 2720; start++)
            {
                generator.SetPattern($"[{start.ToString()}]0(12)");
                cardNumber = generator.GetString();
                Assert.IsTrue(regex.IsMatch(cardNumber));
            }
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void AMEXTest()
        {
            var regex = new Regex("^3[47][0-9]{13}$");
            var generator = new Generator("[34]0(13)");
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[37]0(13)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void DinersClubCardText()
        {
            var regex = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
            var generator = new Generator("[36]0(12)");
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[38]0(12)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            for (var start = 300; start <= 305; start++)
            {
                generator.SetPattern($"[{start.ToString()}]0(11)");
                cardNumber = generator.GetString();
                Assert.IsTrue(regex.IsMatch(cardNumber));
            }
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void DiscoverCardTest()
        {
            var regex = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
            var generator = new Generator("[6011]0(12)");
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[65]0(14)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void JCBCardTest()
        {
            var regex = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");
            var generator = new Generator("[2131]0(11)");
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[1800]0(11)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            generator.SetPattern("[35]0(14)");
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void PhoneNumberTest()
        {

            var regex = new Regex("^((\\(\\d{3}\\) ?)|(\\d{3}-))?\\d{3}-\\d{4}$");
            var generator = new Generator("[(]90(2)[) ]90(2)[-]0(4)");
            var phoneNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(phoneNumber));
        }
        #endregion

        #region Other
        [TestMethod]
        [TestCategory("Other")]
        public void PatternPropertyTest()
        {
            var pattern = "a(2,3)";
            var generator = new Generator(pattern);

            Assert.AreEqual(pattern, generator.Pattern, false);
        }

        [TestMethod]
        [TestCategory("Other")]
        public void IEnumerableTest()
        {
            var count = 10;
            var generator = new Generator(".");
            IEnumerable<string> outputs = generator.GetStrings(count);

            var counted = 0;
            foreach (var output in outputs)
            {
                counted++;
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetterOrDigit(output[0]));
            }

            Assert.AreEqual(counted, count);
        }

        [TestMethod]
        [TestCategory("Other")]
        public void ToStringTest()
        {
            var pattern = "a";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
        }

        [TestMethod]
        [TestCategory("Other")]
        public void UsingTest()
        {
            using (var generator = new Generator("a"))
            {
                var output = generator.ToString();

                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Other")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest()
        {
            var generator = new Generator();
            generator.Dispose();
            generator.ToString();
        }

        [TestMethod]
        [TestCategory("Other")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest2()
        {
            var generator = new Generator();
            generator.Dispose();
            generator.GetString();
        }

        [TestMethod]
        [TestCategory("Other")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest3()
        {
            var generator = new Generator();
            generator.Dispose();
            foreach (var item in generator.GetStrings(10)) { }
        }

        [TestMethod]
        [TestCategory("Other")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest4()
        {
            var generator = new Generator();
            generator.Dispose();
            generator.SetPattern("a");
        }

        [TestMethod]
        [TestCategory("Other")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void UsingDisposeTest()
        {
            Generator generator;
            using (generator = new Generator("a"))
            {
                var output = generator.ToString();

                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]));
            }

            generator.ToString();
        }
        #endregion

    }
}
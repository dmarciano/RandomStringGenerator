using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RSGLib.Tests
{
    [TestClass]
    public class ValidPatternBuilderTests
    {
        #region Constructors
        [TestMethod]
        [TestCategory("Constructor")]
        public void ConstructorTest()
        {
            var builder = new PatternBuilder();
            Assert.IsTrue(true);
        }
        #endregion

        #region Basic Patterns
        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleCharacterTest()
        {
            var builder = new PatternBuilder().Letter();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberTest()
        {
            var builder = new PatternBuilder().Number();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberExcludingZeroTest()
        {
            var builder = new PatternBuilder().NumberExceptZero();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(Convert.ToInt32(output[0]) != 0);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleSymbolTest()
        {
            var builder = new PatternBuilder().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleCharacterWithCryptoTest()
        {
            var builder = new PatternBuilder().Letter();
            var output = new Generator(new CryptoRandomGenerator()).UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberWithCryptoTest()
        {
            var builder = new PatternBuilder().Number();
            var output = new Generator(new CryptoRandomGenerator()).UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleNumberExcludingZeroWithCryptoTest()
        {
            var builder = new PatternBuilder().NumberExceptZero();
            var output = new Generator(new CryptoRandomGenerator()).UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(Convert.ToInt32(output[0]) != 0);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleSymbolWithCryptoTest()
        {
            var builder = new PatternBuilder().Symbol();
            var output = new Generator(new CryptoRandomGenerator()).UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLiteralTest()
        {
            var builder = new PatternBuilder().Literal("ab");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.AreEqual(output, "ab", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLiteralTest2()
        {
            var builder = new PatternBuilder().Literal("[]");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.AreEqual(output, "[]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLiteralTest3()
        {
            var builder = new PatternBuilder().Literal("[ab]");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.AreEqual(output, "[ab]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void LetterNumberTest()
        {
            var builder = new PatternBuilder().LetterOrNumber();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(char.IsLetterOrDigit(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void LetterSymbolTest()
        {
            var builder = new PatternBuilder().LetterOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(char.IsLetter(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void NumberSymbolTest()
        {
            var builder = new PatternBuilder().NumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void LetterNumberSymbolTest()
        {
            var builder = new PatternBuilder().LetterNumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleCharacterTest()
        {
            var builder = new PatternBuilder().Letter().Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Number().Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().NumberExceptZero().Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Symbol().Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Literal("ab").Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.AreEqual(output, "abab", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleLiteralTest2()
        {
            var builder = new PatternBuilder().Literal("[]").Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.AreEqual(output, "[][]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleLiteralTest3()
        {
            var builder = new PatternBuilder().Literal("[ab]").Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.AreEqual(output, "[ab][ab]", false);
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void RangeRepeatTest()
        {
            var builder = new PatternBuilder().Letter().Repeat(1, 5);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Letter().Repeat(0, 5);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length >= 0 && output.Length <= 5);
            foreach (var c in output.ToCharArray())
            {
                Assert.IsTrue(char.IsLetter(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void StringInterpolationTest()
        {
            var builder = new PatternBuilder().Literal($"{"AAA".ToString()}").Symbol();
            var generator = new Generator().UseBuilder(builder);

            var output1 = generator.ToString();
            var output2 = generator.ToString();

            Assert.IsTrue(output1.Length == 4);
            Assert.AreEqual(output1.Substring(0, 3), "AAA", false);
            Assert.IsTrue(char.IsSymbol(output1[3]) || char.IsPunctuation(output1[3]));

            Assert.IsTrue(output2.Length == 4);
            Assert.AreEqual(output2.Substring(0, 3), "AAA", false);
            Assert.IsTrue(char.IsSymbol(output2[3]) || char.IsPunctuation(output2[3]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void StringInterpolationRepeatTest()
        {
            var builder = new PatternBuilder().Literal($"{"AAA".ToString()}").Repeat(2).Symbol();
            var generator = new Generator().UseBuilder(builder);
            var output1 = generator.ToString();
            var output2 = generator.ToString();

            Assert.IsTrue(output1.Length == 7);
            Assert.AreEqual(output1.Substring(0, 6), "AAAAAA", false);
            Assert.IsTrue(char.IsSymbol(output1[6]) || char.IsPunctuation(output1[6]));

            Assert.IsTrue(output2.Length == 7);
            Assert.AreEqual(output2.Substring(0, 6), "AAAAAA", false);
            Assert.IsTrue(char.IsSymbol(output2[6]) || char.IsPunctuation(output2[6]));
        }
        #endregion

        #region Basic Patterns with Modifiers
        [TestMethod]
        [TestCategory("Modifiers")]
        public void SingleUppercaseLetterTest()
        {
            var builder = new PatternBuilder().Letter().UppercaseOnly();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsUpper(output[0]));
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void SingleLowercaseLetterTest()
        {
            var builder = new PatternBuilder().Letter().LowercaseOnly();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void NumberSymbolExceptZeroTest()
        {
            var builder = new PatternBuilder().NumberOrSymbol().ExcludeZero();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            Assert.AreNotEqual(Convert.ToInt32(output[0]), 0);
        }

        [TestMethod]
        [TestCategory("Modifiers")]
        public void MultipleUppercaseLetterTest()
        {
            var builder = new PatternBuilder().Letter().UppercaseOnly().Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Letter().LowercaseOnly().Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Number().ExcludeZero();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(Convert.ToInt32(output[0]) != 0);
        }
        #endregion 

        #region Optionals
        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockTest()
        {
            var builder = new PatternBuilder().Optional("First", "Fizzy", "Fuzzy");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.Equals("First") || output.Equals("Fizzy") || output.Equals("Fuzzy"));
        }

        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockTest2()
        {
            var builder = new PatternBuilder().Optional(new List<string>() { "First", "Fizzy", "Fuzzy" });
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.Equals("First") || output.Equals("Fizzy") || output.Equals("Fuzzy"));
        }

        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockWithRepeatTest()
        {
            var builder = new PatternBuilder().Optional("First", "Fizzy", "Fuzzy").Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 10);

            var output1 = output.Substring(0, 5);
            var output2 = output.Substring(5);
            Assert.IsTrue(output1.Equals("First") || output1.Equals("Fizzy") || output1.Equals("Fuzzy"));
            Assert.IsTrue(output2.Equals("First") || output2.Equals("Fizzy") || output2.Equals("Fuzzy"));
        }

        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockWithRepeatTest2()
        {
            var builder = new PatternBuilder().Optional(new List<string> { "First", "Fizzy", "Fuzzy" }).Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 10);

            var output1 = output.Substring(0, 5);
            var output2 = output.Substring(5);
            Assert.IsTrue(output1.Equals("First") || output1.Equals("Fizzy") || output1.Equals("Fuzzy"));
            Assert.IsTrue(output2.Equals("First") || output2.Equals("Fizzy") || output2.Equals("Fuzzy"));
        }

        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockWithRepeatTest3()
        {
            var builder = new PatternBuilder().Optional("First", "Fizzy", "Fuzzy").Repeat(2,3);
            var generator = new Generator().UseBuilder(builder);
            var test = true;
            var doubleSeen = false;
            var tripleSeen = false;
            var count = 0;  //To prevent the loop from running indefinitely
            while (test)
            {
                count++;
                var output = generator.GetString();

                Assert.IsTrue(output.Length == 10 || output.Length == 15);

                var output1 = output.Substring(0, 5);
                var output2 = output.Substring(5, 5);
                var output3 = string.Empty;

                if (output.Length == 10)
                {
                    doubleSeen = true;
                }
                else
                {
                    tripleSeen = true;
                    output3 = output.Substring(10, 5);
                    Assert.IsTrue(output3.Equals("First") || output3.Equals("Fizzy") || output3.Equals("Fuzzy"));
                }

                Assert.IsTrue(output1.Equals("First") || output1.Equals("Fizzy") || output1.Equals("Fuzzy"));
                Assert.IsTrue(output2.Equals("First") || output2.Equals("Fizzy") || output2.Equals("Fuzzy"));

                test = !((doubleSeen && tripleSeen) || count == 500);
            }

            Assert.IsTrue(doubleSeen);
            Assert.IsTrue(tripleSeen);
        }

        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockWithRepeatTest4()
        {
            var builder = new PatternBuilder().Optional(new List<string> { "First", "Fizzy", "Fuzzy" }).Repeat(2,3);
            var generator = new Generator().UseBuilder(builder);
            var test = true;
            var doubleSeen = false;
            var tripleSeen = false;
            var count = 0;  //To prevent the loop from running indefinitely
            while (test)
            {
                count++;
                var output = generator.GetString();

                Assert.IsTrue(output.Length == 10 || output.Length == 15);

                var output1 = output.Substring(0, 5);
                var output2 = output.Substring(5, 5);
                var output3 = string.Empty;

                if (output.Length == 10)
                {
                    doubleSeen = true;
                }
                else
                {
                    tripleSeen = true;
                    output3 = output.Substring(10, 5);
                    Assert.IsTrue(output3.Equals("First") || output3.Equals("Fizzy") || output3.Equals("Fuzzy"));
                }

                Assert.IsTrue(output1.Equals("First") || output1.Equals("Fizzy") || output1.Equals("Fuzzy"));
                Assert.IsTrue(output2.Equals("First") || output2.Equals("Fizzy") || output2.Equals("Fuzzy"));

                test = !((doubleSeen && tripleSeen) || count == 500);
            }

            Assert.IsTrue(doubleSeen);
            Assert.IsTrue(tripleSeen);
        }
        #endregion

        #region Ranges
        [TestMethod]
        [TestCategory("Ranges")]
        public void LowercaseLetterRangeTest()
        {
            var builder = new PatternBuilder().Range('a', 'z');
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void LowercaseLetterRangeRepeatTest()
        {
            var builder = new PatternBuilder().Range('a', 'z').Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLetter(output[1]));
            Assert.IsTrue(char.IsLower(output[0]));
            Assert.IsTrue(char.IsLower(output[1]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void UppercasecaseLetterRangeTest()
        {
            var builder = new PatternBuilder().Range('A', 'Z');
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsUpper(output[0]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void UppercaseLetterRangeRepeatTest()
        {
            var builder = new PatternBuilder().Range('A', 'Z').Repeat(2);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLetter(output[1]));
            Assert.IsTrue(char.IsUpper(output[0]));
            Assert.IsTrue(char.IsUpper(output[1]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void IndividualLettersRangeTest()
        {
            var builder = new PatternBuilder().Range('a', 'b');
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
            Assert.IsTrue(output[0].Equals('a') || output[0].Equals('b'));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void IndividualLettersRangeTest2()
        {
            var builder = new PatternBuilder().Range('a', 'b');
            var generator = new Generator().UseBuilder(builder);
            var test = true;
            var aSeen = false;
            var bSeen = false;
            var count = 0;  //To prevent the loop from running indefinitely
            while (test)
            {
                count++;
                var output = generator.GetString();

                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsLower(output[0]));

                if (output[0].Equals('a'))
                {
                    aSeen = true;
                }
                else if (output[0].Equals('b'))
                {
                    bSeen = true;
                }
                test = !((aSeen && bSeen) || count == 500);
            }

            Assert.IsTrue(aSeen);
            Assert.IsTrue(bSeen);
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void FullNumberRangeTest()
        {
            var builder = new PatternBuilder().Range('0', '9');
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void RestrictedNumberRangeTest()
        {
            var builder = new PatternBuilder().Range('1', '8');
            var generator = new Generator().UseBuilder(builder);

            foreach (var output in generator.GetStrings(10000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsNumber(output[0]));
                Assert.IsTrue(!output[0].Equals('0') && !output[0].Equals('9'));
            }
        }
        #endregion

        #region Format Block
        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatNumberLiteralAsHexStringTest()
        {
            var regex = new Regex("^[A-Fa-f0-9]*$");
            var builder = new PatternBuilder().Literal("255").Format("{0:X}");
            var output = new Generator().UseBuilder(builder).GetString();
            
            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(regex.IsMatch(output));
        }

        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatNumberAsHexStringTest()
        {
            var regex = new Regex("^[A-Fa-f0-9]*$");
            var builder = new PatternBuilder().Number().Repeat(2).Format("{0:X}");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1 || output.Length == 2);
            Assert.IsTrue(regex.IsMatch(output));
        }

        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatStringLiteralWithAlignmentTest()
        {
            var builder = new PatternBuilder().Literal("TEST").Format("{0,5}");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.Equals(" TEST"));
        }

        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatStringWithAlignmentTest()
        {
            var builder = new PatternBuilder().Letter().Repeat(3).Format("{0,5}");
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.StartsWith("  "));
        }
        #endregion

        #region Advanced Patterns
        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersTest()
        {
            var builder = new PatternBuilder().Letter().Number();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]) && char.IsNumber(output[1]));
            Assert.IsTrue(char.IsNumber(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersSymbolsTest()
        {
            var builder = new PatternBuilder().Letter().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void NumbersSymbolsTest()
        {
            var builder = new PatternBuilder().Number().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void NumbersSymbolsTest2()
        {
            var builder = new PatternBuilder().Number().NumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsNumber(output[0]));
            Assert.IsTrue(char.IsNumber(output[1]) || char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest()
        {
            var builder = new PatternBuilder().Letter().Number().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsNumber(output[1]));
            Assert.IsTrue(char.IsSymbol(output[2]) || char.IsPunctuation(output[2]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest2()
        {
            var builder = new PatternBuilder().Letter().Number().NumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsNumber(output[1]));
            Assert.IsTrue(char.IsNumber(output[1]) || char.IsSymbol(output[2]) || char.IsPunctuation(output[2]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest3()
        {
            var builder = new PatternBuilder().LetterNumberOrSymbol().LetterNumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            Assert.IsTrue(char.IsLetter(output[1]) || char.IsNumber(output[1]) || char.IsSymbol(output[1]) || char.IsPunctuation(output[1]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LettersNumbersSymbolsTest4()
        {
            var builder = new PatternBuilder().LetterNumberOrSymbol().Letter().LetterNumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            Assert.IsTrue(char.IsLetter(output[1]));
            Assert.IsTrue(char.IsLetter(output[2]) || char.IsNumber(output[2]) || char.IsSymbol(output[2]) || char.IsPunctuation(output[2]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersTest()
        {
            var builder = new PatternBuilder().Literal("Order-").Letter().Number();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]));
            Assert.IsTrue(char.IsNumber(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersSymbolsTest()
        {
            var builder = new PatternBuilder().Literal("Order-").Letter().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]));
            Assert.IsTrue(char.IsSymbol(output[7]) || char.IsPunctuation(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralNumbersSymbolsTest()
        {
            var builder = new PatternBuilder().Literal("Order-").Number().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsNumber(output[6]));
            Assert.IsTrue(char.IsSymbol(output[7]) || char.IsPunctuation(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersSymbolsTest()
        {
            var builder = new PatternBuilder().Literal("Order-").Letter().Number().Symbol();
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Literal("Order-").Letter().Number().NumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().Literal("Order-").LetterNumberOrSymbol().LetterNumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.AreEqual(output.Substring(0, 6), "Order-", false);
            Assert.IsTrue(char.IsLetter(output[6]) || char.IsNumber(output[6]) || char.IsSymbol(output[6]) || char.IsPunctuation(output[6]));
            Assert.IsTrue(char.IsLetter(output[7]) || char.IsNumber(output[7]) || char.IsSymbol(output[7]) || char.IsPunctuation(output[7]));
        }

        [TestMethod]
        [TestCategory("Advanced Patterns")]
        public void LiteralLettersNumbersSymbolsTest4()
        {
            var builder = new PatternBuilder().Literal("Order-").LetterNumberOrSymbol().Letter().LetterNumberOrSymbol();
            var output = new Generator().UseBuilder(builder).GetString();

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
            var builder = new PatternBuilder().AddGlobalExclusions('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M').Letter().UppercaseOnly();
            var generator = new Generator().UseBuilder(builder);
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsUpper(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalExclusionTest2()
        {
            var excluded = new List<char>() { 'a', '{', '}' };
            var builder = new PatternBuilder().AddGlobalExclusions('a', '{', '}').LetterOrSymbol().LowercaseOnly();
            var generator = new Generator().UseBuilder(builder);
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
                Assert.IsTrue(char.IsLetter(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
                if (char.IsLetter(output[0]))
                    Assert.IsTrue(char.IsLower(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void TokenExclusionTest()
        {
            var excluded = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
            var builder = new PatternBuilder().Letter().UppercaseOnly().Exclude('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M');
            var generator = new Generator().UseBuilder(builder);
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsUpper(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void TokenExclusionTest2()
        {
            var excluded = new List<char>() { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~' };
            var builder = new PatternBuilder().Symbol().Exclude('!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~');
            var generator = new Generator().UseBuilder(builder);
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
                Assert.IsTrue(char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalAndTokenExclusionTest()
        {
            var excluded = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'Z' };
            var builder = new PatternBuilder().AddGlobalExclusions('Z').Letter().UppercaseOnly().Exclude('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M');
            var generator = new Generator().UseBuilder(builder);

            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsUpper(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalAndTokenExclusionTest2()
        {
            var excluded = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'Z' };
            var builder = new PatternBuilder().Letter().UppercaseOnly().Exclude('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M').AddGlobalExclusions('Z');
            var generator = new Generator().UseBuilder(builder);

            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!excluded.Contains(output[0]));
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsUpper(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GeneralDateTimeStringTest()
        {
            var builder = new PatternBuilder().AddDateTime();
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GeneralDateTimeStringForceTest()
        {
            var builder = new PatternBuilder().AddDateTime(true);
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GeneralDateTimeStringRepeatTest()
        {
            var builder = new PatternBuilder().AddDateTime().Repeat(2);
            var output = new Generator().UseBuilder(builder).ToString();

            //Best way to test for two date/time string concated?
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateStringTest()
        {
            var builder = new PatternBuilder().AddDateTime("d");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void TimeStringTest()
        {
            var builder = new PatternBuilder().AddDateTime("t");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateTimeStringTest()
        {
            var builder = new PatternBuilder().AddDateTime("G");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomDateStringTest()
        {
            var builder = new PatternBuilder().AddDateTime("MMMM dd, yyyy");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomTimeStringTest2()
        {
            var builder = new PatternBuilder().AddDateTime("HH:mm:ss");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void CustomTimeStringRepeatTest()
        {
            var builder = new PatternBuilder().AddDateTime("HH:mm:ss").Repeat(2);
            var output = new Generator().UseBuilder(builder).ToString();

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
            var builder = new PatternBuilder().AddDateTime("MMM dd, yyyy HH:mm:ss zzz");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateTimeStringForceTest()
        {
            var builder = new PatternBuilder().AddDateTime("MM/dd/yyyy HH:mm:ss.ffffff", true);
            var generator = new Generator().UseBuilder(builder);
            var output1 = generator.ToString();
            var output2 = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output1, out var dt1));
            Assert.IsTrue(DateTime.TryParse(output2, out var dt2));
            Assert.AreEqual(dt1, dt2);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateTimeStringForceRepeatTest()
        {
            var builder = new PatternBuilder().AddDateTime("MM/dd/yyyy HH:mm:ss.ffffff", true).Repeat(2);
            var generator = new Generator().UseBuilder(builder);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 52);

            var date1 = output.Substring(0, 26);
            var date2 = output.Substring(26);

            Assert.IsTrue(DateTime.TryParse(date1, out var dt1));
            Assert.IsTrue(DateTime.TryParse(date2, out var dt2));
            Assert.AreEqual(dt1, dt2);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDTest()
        {
            var builder = new PatternBuilder().AddGuid();
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(Guid.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDTest2()
        {
            var builder = new PatternBuilder().AddGuid("N");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 32);
            Assert.IsTrue(Guid.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDTest3()
        {
            var builder = new PatternBuilder().AddGuid("D");
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 36);
            Assert.IsTrue(Guid.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDRepeatTest()
        {
            var builder = new PatternBuilder().AddGuid().Repeat(2);
            var output = new Generator().UseBuilder(builder).ToString();

            var guid1 = output.Substring(0, 36);
            var guid2 = output.Substring(36);

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDForceTest()
        {
            var builder = new PatternBuilder().AddGuid(true);
            var generator = new Generator().UseBuilder(builder);
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
            var builder = new PatternBuilder().AddGuid("D", true);
            var generator = new Generator().UseBuilder(builder);
            var guid1 = generator.ToString();
            var guid2 = generator.ToString();

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.AreEqual(guid1, guid2);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDForceRepeatTest()
        {
            var builder = new PatternBuilder().AddGuid("D", true).Repeat(2);
            var generator = new Generator().UseBuilder(builder);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 72);

            var guid1 = output.Substring(0, 36);
            var guid2 = output.Substring(36);

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.AreEqual(guid1, guid2);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDWithSymbolTest()
        {
            var builder = new PatternBuilder().AddGuid().Symbol();
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 37);

            var guid = output.Substring(0, 36);
            var symbol = output.Substring(36);

            Assert.IsTrue(Guid.TryParse(guid, out _));
            Assert.IsTrue(char.IsSymbol(symbol[0]) || char.IsPunctuation(symbol[0]));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDWithSymbolTest2()
        {
            var builder = new PatternBuilder().AddGuid("D").Symbol();
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 37);

            var guid = output.Substring(0, 36);
            var symbol = output.Substring(36);

            Assert.IsTrue(Guid.TryParse(guid, out _));
            Assert.IsTrue(char.IsSymbol(symbol[0]) || char.IsPunctuation(symbol[0]));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDWithSymbolTest3()
        {
            var builder = new PatternBuilder().AddGuid().Repeat(2).Symbol();
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 73);

            var guid1 = output.Substring(0, 36);
            var guid2 = output.Substring(36, 36);
            var symbol = output.Substring(72);

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.IsTrue(char.IsSymbol(symbol[0]) || char.IsPunctuation(symbol[0]));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDWithSymbolTest4()
        {
            var builder = new PatternBuilder().AddGuid(true).Repeat(2).Symbol();
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 73);

            var guid1 = output.Substring(0, 36);
            var guid2 = output.Substring(36, 36);
            var symbol = output.Substring(72);

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.AreEqual(guid1, guid2);
            Assert.IsTrue(char.IsSymbol(symbol[0]) || char.IsPunctuation(symbol[0]));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDWithSymbolTest5()
        {
            var builder = new PatternBuilder().AddGuid(true).Repeat(2).Symbol().Repeat(2);
            var output = new Generator().UseBuilder(builder).ToString();

            Assert.IsTrue(output.Length == 74);

            var guid1 = output.Substring(0, 36);
            var guid2 = output.Substring(36, 36);
            var symbol1 = output.Substring(72, 1);
            var symbol2 = output.Substring(73, 1);

            Assert.IsTrue(Guid.TryParse(guid1, out _));
            Assert.IsTrue(Guid.TryParse(guid2, out _));
            Assert.AreEqual(guid1, guid2);
            Assert.IsTrue(char.IsSymbol(symbol1[0]) || char.IsPunctuation(symbol1[0]));
            Assert.IsTrue(char.IsSymbol(symbol2[0]) || char.IsPunctuation(symbol2[0]));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionDelegateTest()
        {
            var builder = new PatternBuilder().AddUDF("My");
            var generator = new Generator().UseBuilder(builder);

            generator.AddFunction("My", () => { return "25"; });
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(output.Equals("25"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionDelegate2Test()
        {
            var builder = new PatternBuilder().AddUDF("My", ()=> { return "25"; });
            var generator = new Generator().UseBuilder(builder);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(output.Equals("25"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionDelegateRepeatTest()
        {
            var builder = new PatternBuilder().AddUDF("My").Repeat(2);
            var generator = new Generator().UseBuilder(builder);

            generator.AddFunction("My", () => { return "25"; });
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 4);
            Assert.IsTrue(output.Equals("2525"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionDelegateRepeat2Test()
        {
            var builder = new PatternBuilder().AddUDF("My", () => { return "25"; }).Repeat(2);
            var generator = new Generator().UseBuilder(builder);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 4);
            Assert.IsTrue(output.Equals("2525"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionTest()
        {
            var builder = new PatternBuilder().AddUDF("My");
            var generator = new Generator().UseBuilder(builder);

            generator.AddFunction("My", ReturnString);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(output.Equals("55"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunction2Test()
        {
            var builder = new PatternBuilder().AddUDF("My", ReturnString);
            var generator = new Generator().UseBuilder(builder);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(output.Equals("55"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionRepeatTest()
        {
            var builder = new PatternBuilder().AddUDF("My").Repeat(2);
            var generator = new Generator().UseBuilder(builder);

            generator.AddFunction("My", ReturnString);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 4);
            Assert.IsTrue(output.Equals("5555"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionRepeat2Test()
        {
            var builder = new PatternBuilder().AddUDF("My", ReturnString).Repeat(2);
            var generator = new Generator().UseBuilder(builder);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 4);
            Assert.IsTrue(output.Equals("5555"));
        }
        #endregion

        #region Real-World Formats
        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void SocialSecurityNumberTest()
        {
            var regex = new Regex("^\\d{3}-\\d{2}-\\d{4}$");
            var builder = new PatternBuilder().Number().Repeat(3).Literal("-").Number().Repeat(2).Literal("-").Number().Repeat(4);
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(regex.IsMatch(output));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void VisaCardTest()
        {
            var regex = new Regex("^4[0-9]{12}(?:[0-9]{3})?$");
            var builder = new PatternBuilder().Literal("4").Number().Repeat(12);
            var generator = new Generator().UseBuilder(builder);

            var oldCardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(oldCardNumber));

            builder = new PatternBuilder().Literal("4").Number().Repeat(15);
            generator.UseBuilder(builder);

            var newCardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(newCardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void MasterCardTest()
        {
            // MC has many variations for their card numbers.
            var regex = new Regex("^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
            var builder = new PatternBuilder().Literal("51").Number().Repeat(14);
            var generator = new Generator().UseBuilder(builder);
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("52").Number().Repeat(14);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("53").Number().Repeat(14);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("54").Number().Repeat(14);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("55").Number().Repeat(14);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            //MC card numbers can also start with values between 2221 through 2720 but will still be 16 characters long
            for (var start = 2221; start <= 2720; start++)
            {
                builder = new PatternBuilder().Literal(start.ToString()).Number().Repeat(12);
                generator.UseBuilder(builder);
                cardNumber = generator.GetString();
                Assert.IsTrue(regex.IsMatch(cardNumber));
            }
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void AMEXTest()
        {
            var regex = new Regex("^3[47][0-9]{13}$");
            var builder = new PatternBuilder().Literal("34").Number().Repeat(13);
            var generator = new Generator().UseBuilder(builder);
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("37").Number().Repeat(13);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void DinersClubCardText()
        {
            var regex = new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
            var builder = new PatternBuilder().Literal("36").Number().Repeat(12);
            var generator = new Generator().UseBuilder(builder);
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("38").Number().Repeat(12);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            for (var start = 300; start <= 305; start++)
            {
                builder = new PatternBuilder().Literal(start.ToString()).Number().Repeat(11);
                generator.UseBuilder(builder);
                cardNumber = generator.GetString();
                Assert.IsTrue(regex.IsMatch(cardNumber));
            }
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void DiscoverCardTest()
        {
            var regex = new Regex("^6(?:011|5[0-9]{2})[0-9]{12}$");
            var builder = new PatternBuilder().Literal("6011").Number().Repeat(12);
            var generator = new Generator().UseBuilder(builder);
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("65").Number().Repeat(14);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void JCBCardTest()
        {
            var regex = new Regex("^(?:2131|1800|35\\d{3})\\d{11}$");
            var builder = new PatternBuilder().Literal("2131").Number().Repeat(11);
            var generator = new Generator().UseBuilder(builder);
            var cardNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("1800").Number().Repeat(11);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));

            builder = new PatternBuilder().Literal("35").Number().Repeat(14);
            generator.UseBuilder(builder);
            cardNumber = generator.GetString();
            Assert.IsTrue(regex.IsMatch(cardNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void PhoneNumberTest()
        {

            var regex = new Regex("^((\\(\\d{3}\\) ?)|(\\d{3}-))?\\d{3}-\\d{4}$");
            var builder = new PatternBuilder()
                .Literal("(").NumberExceptZero().Number().Repeat(2).Literal(") ")
                .NumberExceptZero().Number().Repeat(2)
                .Literal("-").Number().Repeat(4);
            var generator = new Generator().UseBuilder(builder);
            var phoneNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(phoneNumber));
        }

        [TestMethod]
        [TestCategory("Real-World Patterns")]
        public void PhoneNumberFromMultipleBuildersTest()
        {

            var regex = new Regex("^((\\(\\d{3}\\) ?)|(\\d{3}-))?\\d{3}-\\d{4}$");
            var areaBuilder = new PatternBuilder().Literal("(").NumberExceptZero().Number().Repeat(2).Literal(") ");
            var exchangeBuilder = new PatternBuilder().NumberExceptZero().Number().Repeat(2);
            var numberBuilder = new PatternBuilder().Number().Repeat(4);

            var generator = new Generator().UseBuilder(areaBuilder + exchangeBuilder + new PatternBuilder().Literal("-") + numberBuilder);
            var phoneNumber = generator.GetString();

            Assert.IsTrue(regex.IsMatch(phoneNumber));
        }
        #endregion

        #region Other
        [TestMethod]
        [TestCategory("Other")]
        public void PatternPropertyTest()
        {
            //TODO: Need to generate the pattern from the token list for the pattern builder
            //var pattern = "a(2,3)";
            //var generator = new Generator(pattern);

            //Assert.AreEqual(pattern, generator.Pattern, false);
        }

        [TestMethod]
        [TestCategory("Other")]
        public void IEnumerableTest()
        {
            var count = 500;
            var builder = new PatternBuilder().LetterOrNumber();
            var generator = new Generator().UseBuilder(builder);
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
            var builder = new PatternBuilder().Letter();
            var output = new Generator().UseBuilder(builder).GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
        }

        [TestMethod]
        [TestCategory("Other")]
        public void UsingTest()
        {
            var builder = new PatternBuilder().Letter();
            using (var generator = new Generator().UseBuilder(builder))
            {
                var output = generator.ToString();

                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]));
            }
        }
        #endregion

        #region Helper Methods (NOT TESTS)
        private string ReturnString()
        {
            return "55";
        }
        #endregion

    }
}
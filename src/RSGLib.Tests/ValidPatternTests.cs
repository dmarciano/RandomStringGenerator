using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;
using System;
using System.Collections.Generic;
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

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void StringInterpolationTest()
        {
            var pattern = $"[{"AAA".ToString()}]@";
            var generator = new Generator(pattern);
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
            var pattern = $"[{"AAA".ToString()}](2)@";
            var generator = new Generator(pattern);
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

        [TestMethod]
        [TestCategory("Modifier")]
        public void MultipleModifiersTest()
        {
            var generator = new Generator(".^~");

            foreach(var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]));
                if (char.IsLetter(output[0]))
                {
                    Assert.IsTrue(char.IsUpper(output[0]));
                }
                else
                {
                    Assert.IsTrue(!output[0].Equals('0'));
                }
            }
        }

        [TestMethod]
        [TestCategory("Modifier")]
        public void MultipleModifiersTest2()
        {
            var generator = new Generator(".!~");

            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]));
                if (char.IsLetter(output[0]))
                {
                    Assert.IsTrue(char.IsLower(output[0]));
                }
                else
                {
                    Assert.IsTrue(!output[0].Equals('0'));
                }
            }
        }

        [TestMethod]
        [TestCategory("Modifier")]
        public void MultipleModifiersTest3()
        {
            var generator = new Generator("*^~");

            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
                if (char.IsLetter(output[0]))
                {
                    Assert.IsTrue(char.IsUpper(output[0]));
                }
                else
                {
                    Assert.IsTrue(!output[0].Equals('0'));
                }
            }
        }

        [TestMethod]
        [TestCategory("Modifier")]
        public void MultipleModifiersTest4()
        {
            var generator = new Generator("*!~");

            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]) || char.IsNumber(output[0]) || char.IsSymbol(output[0]) || char.IsPunctuation(output[0]));
                if (char.IsLetter(output[0]))
                {
                    Assert.IsTrue(char.IsLower(output[0]));
                }
                else
                {
                    Assert.IsTrue(!output[0].Equals('0'));
                }
            }
        }
        #endregion

        #region Order of Modifiers
        [TestMethod]
        [TestCategory("Order")]
        public void OrderTest()
        {
            var generator = new Generator("a^(2)>{0,5}<");
            foreach(var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 5);
                Assert.IsTrue(char.IsLetter(output[3]));
                Assert.IsTrue(char.IsUpper(output[3]));
                Assert.IsTrue(char.IsLetter(output[4]));
                Assert.IsTrue(char.IsUpper(output[4]));
            }
        }

        [TestMethod]
        [TestCategory("Order")]
        public void OrderTest2()
        {
            var generator = new Generator("a^>{0,5}<(2)");
            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 5);
                Assert.IsTrue(char.IsLetter(output[3]));
                Assert.IsTrue(char.IsUpper(output[3]));
                Assert.IsTrue(char.IsLetter(output[4]));
                Assert.IsTrue(char.IsUpper(output[4]));
            }
        }

        [TestMethod]
        [TestCategory("Order")]
        public void OrderTest3()
        {
            var generator = new Generator("a(2)^>{0,5}<");
            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 5);
                Assert.IsTrue(char.IsLetter(output[3]));
                Assert.IsTrue(char.IsUpper(output[3]));
                Assert.IsTrue(char.IsLetter(output[4]));
                Assert.IsTrue(char.IsUpper(output[4]));
            }
        }

        [TestMethod]
        [TestCategory("Order")]
        public void OrderTest4()
        {
            var generator = new Generator("a(2)>{0,5}<^");
            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 5);
                Assert.IsTrue(char.IsLetter(output[3]));
                Assert.IsTrue(char.IsUpper(output[3]));
                Assert.IsTrue(char.IsLetter(output[4]));
                Assert.IsTrue(char.IsUpper(output[4]));
            }
        }

        [TestMethod]
        [TestCategory("Order")]
        public void OrderTest5()
        {
            var generator = new Generator("a>{0,5}<^(2)");
            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 5);
                Assert.IsTrue(char.IsLetter(output[3]));
                Assert.IsTrue(char.IsUpper(output[3]));
                Assert.IsTrue(char.IsLetter(output[4]));
                Assert.IsTrue(char.IsUpper(output[4]));
            }
        }

        [TestMethod]
        [TestCategory("Order")]
        public void OrderTest6()
        {
            var generator = new Generator("a>{0,5}<(2)^");
            foreach (var output in generator.GetStrings(1000))
            {
                Assert.IsTrue(output.Length == 5);
                Assert.IsTrue(char.IsLetter(output[3]));
                Assert.IsTrue(char.IsUpper(output[3]));
                Assert.IsTrue(char.IsLetter(output[4]));
                Assert.IsTrue(char.IsUpper(output[4]));
            }
        }
        #endregion

        #region Escape Sequences
        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void NewLineInPatternTest()
        {
            var generator = new Generator("a\\n0");
            var output = generator.GetString();

            // Environment.NewLine.Length is used below to account for different OS's new line
            // i.e. some are \n and Windows is \r\n
            Assert.IsTrue(output.Length == 2 + Environment.NewLine.Length);
            Assert.IsTrue(output.Contains(Environment.NewLine));
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsNumber(output[2 + Environment.NewLine.Length - 1]));
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void TabInPatternTest()
        {
            var generator = new Generator("a\\t0");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 3);
            Assert.IsTrue(output.Contains("\t"));
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsNumber(output[2]));
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void LiteralClosingBracketEscapeTest()
        {
            var generator = new Generator("[\\]]");
            var output = generator.GetString();

            Assert.AreEqual(output, "]", false);
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void LiteralNewLineEscapeTest()
        {
            var generator = new Generator("[a\\nb]");
            var output = generator.GetString();

            Assert.IsTrue(output.Contains(Environment.NewLine));
            Assert.AreEqual(output, $"a{Environment.NewLine}b", false);
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void LiteralTabEscapeTest()
        {
            var generator = new Generator("[a\\tb]");
            var output = generator.GetString();
            Assert.IsTrue(output.Contains("\t"));
            Assert.AreEqual(output, "a\tb", false);
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void LiteralBackSlashEscapeTest()
        {
            var generator = new Generator("[a\\\\b]");
            var output = generator.GetString();
            Assert.IsTrue(output.Contains("\\"));
            Assert.AreEqual(output, "a\\b", false);
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void ClosingBraceGlobalExclusionTest()
        {
            var generator = new Generator("{-\\}}@");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!output.Equals("}"));
            }
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void BackSlashGlobalExclusionTest()
        {
            var generator = new Generator("{-\\\\}@");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!output.Equals("\\"));
            }
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void HypenGlobalExclusionTest()
        {
            var generator = new Generator("{-\\-}@");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!output.Equals("-"));
            }
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void ClosingBraceLocalExclusionTest()
        {
            var generator = new Generator("@{-\\}}");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!output.Equals("}"));
            }
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void BackSlashLocalExclusionTest()
        {
            var generator = new Generator("@{-\\\\}");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!output.Equals("\\"));
            }
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void HypenLocalExclusionTest()
        {
            var generator = new Generator("@{-\\-}");
            foreach (var output in generator.GetStrings(500))
            {
                Assert.IsTrue(!output.Equals("-"));
            }
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void OptionalHashEscapeTest()
        {
            var generator = new Generator("#Suite \\#1,Suite \\#2,Suite \\#3#");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.IsTrue(output.Equals("Suite #1") || output.Equals("Suite #2") || output.Equals("Suite #3"));
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void OptionalCommaEscapeTest()
        {
            var generator = new Generator("#Suite 1\\,,Suite 2\\,,Suite 3\\,#");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.IsTrue(output.Equals("Suite 1,") || output.Equals("Suite 2,") || output.Equals("Suite 3,"));
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void OptionalBackSlashEscapeTest()
        {
            var generator = new Generator("#Suite \\\\1,Suite \\\\2,Suite \\\\3#");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 8);
            Assert.IsTrue(output.Equals("Suite \\1") || output.Equals("Suite \\2") || output.Equals("Suite \\3"));
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void OptionalNewLineEscapeTest()
        {
            var generator = new Generator("#Suite \\n1,Suite \\n2,Suite \\n3#");
            var output = generator.GetString();

            Assert.IsTrue(output.Equals($"Suite {Environment.NewLine}1") || output.Equals($"Suite {Environment.NewLine}2") || output.Equals($"Suite {Environment.NewLine}3"));
        }

        [TestMethod]
        [TestCategory("Escape Sequence")]
        public void OptionalTabEscapeTest()
        {
            var generator = new Generator("#Suite \\t1,Suite \\t2,Suite \\t3#");
            var output = generator.GetString();

            Assert.IsTrue(output.Equals("Suite \t1") || output.Equals("Suite \t2") || output.Equals("Suite \t3"));
        }
        #endregion

        #region Optionals
        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockTest()
        {
            var generator = new Generator("#First,Fizzy,Fuzzy#");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.Equals("First") || output.Equals("Fizzy") || output.Equals("Fuzzy"));
        }

        [TestMethod]
        [TestCategory("Optional Blocks")]
        public void OptionalBlockWithRepeatTest()
        {
            var generator = new Generator("#First,Fizzy,Fuzzy#(2)");
            var output = generator.GetString();

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
            var generator = new Generator("#First,Fizzy,Fuzzy#(2,3)");
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
            var generator = new Generator("<a-z>");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void LowercaseLetterRangeRepeatTest()
        {
            var generator = new Generator("<a-z>(2)");
            var output = generator.GetString();

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
            var generator = new Generator("<A-Z>");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsUpper(output[0]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void UppercaseLetterRangeRepeatTest()
        {
            var generator = new Generator("<A-Z>(2)");
            var output = generator.GetString();

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
            var generator = new Generator("<ab>");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
            Assert.IsTrue(output[0].Equals('a') || output[0].Equals('b'));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void IndividualLettersRangeTest2()
        {
            var generator = new Generator("<ab>");
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
            var generator = new Generator("<0-9>");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsNumber(output[0]));
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void RestrictedNumberRangeTest()
        {
            var generator = new Generator("<1-8>");

            foreach (var output in generator.GetStrings(10000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsNumber(output[0]));
                Assert.IsTrue(!output[0].Equals('0') && !output[0].Equals('9'));
            }
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void LetterNumberRangeTest()
        {
            var generator = new Generator("<a1-8>");
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetterOrDigit(output[0]));

                if (char.IsLetter(output[0]))
                    Assert.IsTrue(output[0].Equals('a'));
                else
                    Assert.IsTrue(!output[0].Equals('0') && !output[0].Equals('9'));

            }
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void LetterNumberRangeRepeatTest()
        {
            var generator = new Generator("<a1-8>(2)");
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 2);

                var output1 = output[0];
                var output2 = output[1];

                Assert.IsTrue(char.IsLetterOrDigit(output1));
                Assert.IsTrue(char.IsLetterOrDigit(output2));

                if (char.IsLetter(output1))
                    Assert.IsTrue(output1.Equals('a'));
                else
                    Assert.IsTrue(!output1.Equals('0') && !output1.Equals('9'));

                if (char.IsLetter(output2))
                    Assert.IsTrue(output2.Equals('a'));
                else
                    Assert.IsTrue(!output2.Equals('0') && !output2.Equals('9'));

            }
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void MultipleRangesTest()
        {
            var generator = new Generator("<a-cx-z>");
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsLower(output[0]));
                Assert.IsTrue(output[0].Equals('a') || output[0].Equals('b') || output[0].Equals('c') || output[0].Equals('x') || output[0].Equals('y') || output[0].Equals('z'));
            }
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void MultipleRangesTest2()
        {
            var generator = new Generator("<b-d3-5>");
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetterOrDigit(output[0]));
                Assert.IsTrue(output[0].Equals('b') || output[0].Equals('c') || output[0].Equals('d') || output[0].Equals('3') || output[0].Equals('4') || output[0].Equals('5'));
            }
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void MultipleRangesTest3()
        {
            var generator = new Generator("<bd379>");
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetterOrDigit(output[0]));
                Assert.IsTrue(output[0].Equals('b') || output[0].Equals('d') || output[0].Equals('3') || output[0].Equals('7') || output[0].Equals('9'));
            }
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void MultipleRangesTest3Modified()
        {
            var generator = new Generator("<bd379>");
            var bSeen = false;
            var dSeen = false;
            var threeSeen = false;
            var sevenSeen = false;
            var nineSeen = false;
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 1);
                Assert.IsTrue(char.IsLetterOrDigit(output[0]));
                Assert.IsTrue(output[0].Equals('b') || output[0].Equals('d') || output[0].Equals('3') || output[0].Equals('7') || output[0].Equals('9'));

                switch (output[0])
                {
                    case 'b':
                        bSeen = true;
                        break;
                    case 'd':
                        dSeen = true;
                        break;
                    case '3':
                        threeSeen = true;
                        break;
                    case '7':
                        sevenSeen = true;
                        break;
                    case '9':
                        nineSeen = true;
                        break;
                }
            }

            Assert.IsTrue(bSeen);
            Assert.IsTrue(dSeen);
            Assert.IsTrue(threeSeen);
            Assert.IsTrue(sevenSeen);
            Assert.IsTrue(nineSeen);
        }

        [TestMethod]
        [TestCategory("Ranges")]
        public void MultipleRangesRepeatTest()
        {
            var generator = new Generator("<bd379>(2)");
            var bSeen1 = 0;
            var dSeen1 = 0;
            var threeSeen1 = 0;
            var sevenSeen1 = 0;
            var nineSeen1 = 0;

            var bSeen2 = 0;
            var dSeen2 = 0;
            var threeSeen2 = 0;
            var sevenSeen2 = 0;
            var nineSeen2 = 0;
            foreach (var output in generator.GetStrings(100000))
            {
                Assert.IsTrue(output.Length == 2);

                var output1 = output[0];
                var output2 = output[1];
                Assert.IsTrue(char.IsLetterOrDigit(output1));
                Assert.IsTrue(char.IsLetterOrDigit(output2));
                Assert.IsTrue(output1.Equals('b') || output1.Equals('d') || output1.Equals('3') || output1.Equals('7') || output1.Equals('9'));
                Assert.IsTrue(output2.Equals('b') || output2.Equals('d') || output2.Equals('3') || output2.Equals('7') || output2.Equals('9'));

                switch (output1)
                {
                    case 'b':
                        bSeen1++;
                        break;
                    case 'd':
                        dSeen1++;
                        break;
                    case '3':
                        threeSeen1++;
                        break;
                    case '7':
                        sevenSeen1++;
                        break;
                    case '9':
                        nineSeen1++;
                        break;
                }

                switch (output2)
                {
                    case 'b':
                        bSeen2++;
                        break;
                    case 'd':
                        dSeen2++;
                        break;
                    case '3':
                        threeSeen2++;
                        break;
                    case '7':
                        sevenSeen2++;
                        break;
                    case '9':
                        nineSeen2++;
                        break;
                }
            }

            Assert.IsTrue(bSeen1 >= 2);
            Assert.IsTrue(dSeen1 >= 2);
            Assert.IsTrue(threeSeen1 >= 2);
            Assert.IsTrue(sevenSeen1 >= 2);
            Assert.IsTrue(nineSeen1 >= 2);

            Assert.IsTrue(bSeen2 >= 2);
            Assert.IsTrue(dSeen2 >= 2);
            Assert.IsTrue(threeSeen2 >= 2);
            Assert.IsTrue(sevenSeen2 >= 2);
            Assert.IsTrue(nineSeen2 >= 2);
        }
        #endregion

        #region Format Block
        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatNumberLiteralAsHexStringTest()
        {
            var regex = new Regex("^[A-Fa-f0-9]*$");
            var generator = new Generator("[255]>{0:X}<");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(regex.IsMatch(output));
        }

        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatNumberAsHexStringTest()
        {
            var regex = new Regex("^[A-Fa-f0-9]*$");
            var generator = new Generator("0(2)>{0:X}<");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(regex.IsMatch(output));
        }

        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatStringLiteralWithAlignmentTest()
        {
            var generator = new Generator("[TEST]>{0,5}<");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.Equals(" TEST"));
        }

        [TestMethod]
        [TestCategory("Format Block")]
        public void FormatStringWithAlignmentTest()
        {
            var generator = new Generator("a(3)>{0,5}<");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 5);
            Assert.IsTrue(output.StartsWith("  "));
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
                Assert.IsTrue(char.IsLetter(output[0]));
                Assert.IsTrue(char.IsUpper(output[0]));
            }
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GlobalExclusionTest2()
        {
            var excluded = new List<char>() { 'a', '{', '}' };
            var generator = new Generator("{-a{\\}}+!");
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
            var generator = new Generator("a^{-ABCDEFGHIJKLM}");
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
            var generator = new Generator("@{-!\"#$%&'()*+,-.\\\\/:;<>?@[]^_`{|\\}~}");
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
            var generator = new Generator("{-Z}a^{-ABCDEFGHIJKLM}");
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
            var pattern = "{T}";
            var generator = new Generator(pattern);
            var output = generator.ToString();

            Assert.IsTrue(DateTime.TryParse(output, out _));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GeneralDateTimeStringForceTest()
        {
            var pattern = "{T?}";
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
            Assert.AreEqual(dt1, dt2);
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void DateTimeStringForceRepeatTest()
        {
            var pattern = "{T:MM/dd/yyyy HH:mm:ss.ffffff?}(2)";
            var generator = new Generator(pattern);
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

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void GUIDForceRepeatTest()
        {
            var pattern = "{G:D?}(2)";
            var generator = new Generator(pattern);
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
            var pattern = "{G}@";
            var generator = new Generator(pattern);
            var output = generator.ToString();

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
            var pattern = "{G:D}@";
            var generator = new Generator(pattern);
            var output = generator.ToString();

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
            var pattern = "{G}(2)@";
            var generator = new Generator(pattern);
            var output = generator.ToString();

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
            var pattern = "{G?}(2)@";
            var generator = new Generator(pattern);
            var output = generator.ToString();

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
            var pattern = "{G?}(2)@(2)";
            var generator = new Generator(pattern);
            var output = generator.ToString();

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
            var generator = new Generator("{My}");
            generator.AddFunction("My", () => { return "25"; });
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(output.Equals("25"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionDelegateRepeatTest()
        {
            var generator = new Generator("{My}(2)");
            generator.AddFunction("My", () => { return "25"; });
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 4);
            Assert.IsTrue(output.Equals("2525"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionTest()
        {
            var generator = new Generator("{My}");
            generator.AddFunction("My", ReturnString);
            var output = generator.ToString();

            Assert.IsTrue(output.Length == 2);
            Assert.IsTrue(output.Equals("55"));
        }

        [TestMethod]
        [TestCategory("Control Blocks")]
        public void UserDefinedFunctionRepeatTest()
        {
            var generator = new Generator("{My}(2)");
            generator.AddFunction("My", ReturnString);
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

        #region Helper Methods (NOT TESTS)
        private string ReturnString()
        {
            return "55";
        }
        #endregion

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;
using System;

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
            Assert.IsTrue(char.IsSymbol(output[0]));
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

            Assert.IsTrue(output.Length >= 1 && output.Length<=5);
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

            Assert.IsTrue(output.Length >= 0 && output.Length<= 2);
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
        public void Advanced()
        {

        }
        #endregion
    }
}
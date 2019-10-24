using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;
using System;

namespace RSGLib.Tests
{
    [TestClass]
    public class ValidPatternTests
    {
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
        public void SingleUppercaseLetterTest()
        {
            var generator = new Generator("a^");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsUpper(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void SingleLowercaseLetterTest()
        {
            var generator = new Generator("a!");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 1);
            Assert.IsTrue(char.IsLetter(output[0]));
            Assert.IsTrue(char.IsLower(output[0]));
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
        public void MultipleCharacterTest()
        {
            var generator = new Generator("a(2)");
            var output = generator.GetString();

            Assert.IsTrue(output.Length == 2);
            foreach(var c in output.ToCharArray())
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
                Assert.IsTrue(char.IsSymbol(c));
            }
        }

        [TestMethod]
        [TestCategory("Basic Pattern")]
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
        [TestCategory("Basic Pattern")]
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
    }
}
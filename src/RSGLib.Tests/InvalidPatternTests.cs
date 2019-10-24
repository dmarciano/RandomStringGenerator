using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;

namespace RSGLib.Tests
{
    [TestClass]
    public class InvalidPatternTests
    {
        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(NoPatternException))]
        public void ConstructorWhitespaceStringTest()
        {
            var generator = new Generator(" ");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(NoPatternException))]
        public void NullStringTest()
        {
            var generator = new Generator();
            string str = null;
            generator.SetPattern(str);
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(NoPatternException))]
        public void EmptyStringTest()
        {
            var generator = new Generator();
            string str = string.Empty;
            generator.SetPattern(str);
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(NoPatternException))]
        public void WhitespaceStringTest()
        {
            var generator = new Generator();
            string str = " ";
            generator.SetPattern(str);
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void ZeroRepeatTest()
        {
            var generator = new Generator("a(0)");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidExceptZeroModifierTest()
        {
            var generator = new Generator("a~");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidExceptZeroModifierTest2()
        {
            var generator = new Generator("@~");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidExceptZeroModifierTest3()
        {
            var generator = new Generator("+~");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidCaseModifierTest()
        {
            var generator = new Generator("0^");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidCaseModifierTest2()
        {
            var generator = new Generator("0!");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidCaseModifierTest3()
        {
            var generator = new Generator("9^");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidModifierException))]
        public void InvalidCaseModifierTest4()
        {
            var generator = new Generator("9!");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void NoClosingParentesisTest()
        {
            var generator = new Generator("a(2,");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void NoClosingParentesisTest2()
        {
            var generator = new Generator("a(2,3");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void NoClosingParentesisTest4()
        {
            var generator = new Generator("a(2,3%");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(DuplicateModifierException))]
        public void DuplicateModifierTest()
        {
            var generator = new Generator("a^^");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(DuplicateModifierException))]
        public void DuplicateModifierTest2()
        {
            var generator = new Generator("a!!");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(DuplicateModifierException))]
        public void DuplicateModifierTest3()
        {
            var generator = new Generator("0~~");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void TooManyCommasTest()
        {
            var generator = new Generator("a(2,,3)");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void TooManyCommasTest2()
        {
            var generator = new Generator("a(2,3,)");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidCountTest()
        {
            var generator = new Generator("a(2-3)");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidTokenTest()
        {
            var generator = new Generator("a(2,3),");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidTokenTest2()
        {
            var generator = new Generator("a(2,3)v");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidTokenTest3()
        {
            var generator = new Generator("av");
        }

        //Too many commas detected
        //No closing count parenthesis
        //Duplicate modifiers
    }
}

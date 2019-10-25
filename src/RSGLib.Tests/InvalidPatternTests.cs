﻿using System;
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
        public void TooManyCommasTest3()
        {
            var generator = new Generator("a(2,3,4)");
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
            var generator = new Generator("v");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidTokenTest2()
        {
            var generator = new Generator("a(2,3),");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidTokenTest3()
        {
            var generator = new Generator("a(2,3)v");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidTokenTest4()
        {
            var generator = new Generator("av");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void MaximumLessThanMinimumTest()
        {
            var generator = new Generator("a(3,2)");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void NoClosingLiteralTest()
        {
            var generator = new Generator("[a");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void NoClosingLiteralTest2()
        {
            var generator = new Generator("[[a]");
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void NoPatternSetTest()
        {
            var generator = new Generator();
            generator.GetString();
        }

        [TestMethod]
        [TestCategory("Invalid Pattern")]
        [ExpectedException(typeof(InvalidPatternException))]
        public void InvalidPatternSetTest()
        {
            var generator = new Generator();
            generator.SetPattern("v");
        }

        //Control block on closed
        //Control block missing formatter
        //No function specified
        //Unknown function
        //Duplicate globals
    }
}
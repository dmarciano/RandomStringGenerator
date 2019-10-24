using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;

namespace RSGLib.Tests
{
    [TestClass]
    public class InvalidPatternTests
    {
        [TestMethod]
        [ExpectedException(typeof(NoPatternException))]
        public void ConstructorWhitespaceStringTest()
        {
            var generator = new Generator(" ");
        }

        [TestMethod]
        [ExpectedException(typeof(NoPatternException))]
        public void NullStringTest()
        {
            var generator = new Generator();
            string str = null;
            generator.SetPattern(str);
        }

        [TestMethod]
        [ExpectedException(typeof(NoPatternException))]
        public void EmptyStringTest()
        {
            var generator = new Generator();
            string str = string.Empty;
            generator.SetPattern(str);
        }

        [TestMethod]
        [ExpectedException(typeof(NoPatternException))]
        public void WhitespaceStringTest()
        {
            var generator = new Generator();
            string str = " ";
            generator.SetPattern(str);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPatternException))]
        public void ZeroRepeatTest()
        {
            var generator = new Generator("a(0)");
        }

        //Too many commas detected
        //No closing count parenthesis
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMC.Utilities.RSG;

namespace RSGLib.Tests
{
    [TestClass]
    public class ValidPatternTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var generator = new Generator();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ConstructorEmptyStringTest()
        {
            var generator = new Generator(string.Empty);

            Assert.IsTrue(true);
        }
    }
}

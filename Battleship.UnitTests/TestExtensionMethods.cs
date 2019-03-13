using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship.Utilities;

namespace Battleship.UnitTests
{
    [TestClass]
    public class TestExtensionMethods
    {
        [TestMethod]
        public void TestWordWrap1()
        {
            var lines = "very long string that has to be word wrapped or it will kill us all".WordWrap(20, 3);
            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual("very long string", lines[0]);
            Assert.AreEqual("that has to be word", lines[1]);
            Assert.AreEqual("wrapped or it will", lines[2]);
        }

        [TestMethod]
        public void TestWordWrap2()
        {
            var lines = "extremelylongwordthatwillnotwrap but after that it will be ok".WordWrap(20, 6);
            Assert.AreEqual(4, lines.Count);
            Assert.AreEqual("extremelylongwordtha", lines[0]);
            Assert.AreEqual("twillnotwrap but", lines[1]);
            Assert.AreEqual("after that it will", lines[2]);
            Assert.AreEqual("be ok", lines[3]);
        }
    }
}

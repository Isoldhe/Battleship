using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Battleship.Utilities;
using System.Text;

namespace Battleship.UnitTests
{
    [TestClass]
    public class TestExtensionMethods
    {
        [TestMethod]
        public void TestEmptyString()
        {
            var lines = "".WordWrap(20, 3);
            Assert.AreEqual(1, lines.Count);
            Assert.AreEqual("", lines[0]);
        }

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

        [TestMethod]
        public void TestWordWrapWithNewLine()
        {
            var sb = new StringBuilder();
            sb.AppendLine("wazzaaaaa this is real pro stuff man");
            sb.AppendLine("like yo");
            sb.AppendLine("for actual ");
            sb.AppendLine(" stuffy");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("stuffs inda house like mad yoooo");
            var lines = sb.ToString().WordWrap(20, 15);
            Assert.AreEqual(10, lines.Count);
            Assert.AreEqual("wazzaaaaa this is", lines[0]);
            Assert.AreEqual("real pro stuff man", lines[1]);
            Assert.AreEqual("like yo", lines[2]);
            Assert.AreEqual("for actual", lines[3]);
            Assert.AreEqual("stuffy", lines[4]);
            Assert.AreEqual("", lines[5]);
            Assert.AreEqual("", lines[6]);
            Assert.AreEqual("stuffs inda house", lines[7]);
            Assert.AreEqual("like mad yoooo", lines[8]);
            Assert.AreEqual("", lines[9]);
        }
    }
}

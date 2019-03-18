using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.UnitTests
{
    [TestClass]
    public class TestRandomThings
    {
        [TestMethod]
        public void TestForLoop()
        {
            var buffer = new char[] { ' ', ' ', ' ' };
            int lastColumn;
            for (lastColumn = buffer.Length - 1; lastColumn > 0 && char.IsWhiteSpace(buffer[lastColumn]); lastColumn--) ;
            Assert.AreEqual(0, lastColumn);
        }
    }
}

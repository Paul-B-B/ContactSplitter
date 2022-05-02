using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("Frau Sandra Berger")]
        [DataRow("Herr Dr. Sandro Gutmensch")]
        [DataRow("Professor Heinrich Freiherr vom Wald")]
        public void TestMethod1()
        {
        }
    }
}
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Backend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class UnitTest1
    {

        private readonly KontaktParser _Parser = new KontaktParser(pathToData: "../../../../ContactSplitter/Backend/Data");

        [TestMethod]
        //[DataRow("Peter Lustig")]
        public void TestMethod1()
        {
            var req = new SplitContactRequest();
            req.UserInput = "Peter Lustig";

            var res = new SplitContactResponse();

            _Parser.SplitName(ref req, ref res);
        }
    }
}

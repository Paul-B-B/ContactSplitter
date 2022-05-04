using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Backend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class KontaktParserTest
    {

        private readonly KontaktParser _Parser = new KontaktParser(pathToData: "../../../../ContactSplitter/Backend/Data");

        [TestMethod]
        [DataRow("Peter Lustig")]
        [DataRow("Herr Peter Lustig")]
        [DataRow("Herr Dr. Peter Lustig")]
        [DataRow("Frau Professorin Peter Lustig")]
        public void TestMethod1(string userInput)
        {
            // Daten vorbereiten
            var req = new SplitContactRequest()
            {
                UserInput = userInput
            };

            // Zu testende Methode ausf�hren

            var parsedContact = _Parser.ParseKontakt(req);

            // Ergebnisse �berpr�fen 
            Assert.IsNotNull(parsedContact);
        }
    }
}

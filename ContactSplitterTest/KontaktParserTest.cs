using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ContactSplitterTest
{
    [TestClass]
    public class KontaktParserTest
    {

        private readonly KontaktParser _Parser = new KontaktParser(pathToData: "../../../../ContactSplitter/Backend/Data");

        [TestMethod]
        [DataRow("Peter Lustig", "Peter", "Lustig", Geschlecht.unbekannt, Sprache.Unbekannt, "", "Guten Tag Peter Lustig")]
        [DataRow("Herr Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Deutsch, "", "Sehr geehrter Herr Peter Lustig")]
        [DataRow("Herr Dr. Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Deutsch, "Dr. ", "Sehr geehrter Herr Dr. Peter Lustig")]
        [DataRow("Frau Professorin Petra Witzig", "Petra", "Witzig", Geschlecht.w, Sprache.Deutsch, "Prof. ", "Sehr geehrte Frau Prof. Petra Witzig")]
        [DataRow("Frau Prof. Dr. Dr. Petra Witzig", "Petra", "Witzig", Geschlecht.w, Sprache.Deutsch, "Prof. Dr. Dr. ", "Sehr geehrte Frau Prof. Dr. Dr. Petra Witzig")]
        [DataRow("Professor Doktor Dr. Peter Lustig", "Peter", "Lustig", Geschlecht.unbekannt, Sprache.Unbekannt, "Prof. Dr. Dr. ", "Guten Tag Prof. Dr. Dr. Peter Lustig")]
        [DataRow("Mr. Doktor Dr. Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Englisch,"Dr. Dr. ", "Dear Mr. Dr. Dr. Peter Lustig")]
        public void ParseKontaktTest(string userInput, string vorname, string nachname, Geschlecht geschlecht, Sprache sprache, string alleTitel, string briefanrede)
        {
            var req = new SplitContactRequest()
            {
                UserInput = userInput
            };

            var parsedContact = _Parser.ParseKontakt(req);

            Assert.IsNotNull(parsedContact);
            Assert.AreEqual(vorname, parsedContact.Vorname );
            Assert.AreEqual(nachname, parsedContact.Nachname);
            Assert.AreEqual(geschlecht, parsedContact.Geschlecht);
            Assert.AreEqual(sprache, parsedContact.Sprache);
            Assert.AreEqual(alleTitel, parsedContact.AlleTitel);
            Assert.AreEqual(briefanrede, parsedContact.Briefanrede);

        }
    }
}

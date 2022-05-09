using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class KontaktParserTest
    {

        private readonly KontaktParser _Parser = new KontaktParser();


        /// <summary>
        /// Test des KontaktParsers.
        /// Dieser deckt den gesamten Code des KontaktParsers ab.
        /// Jeder Titel sowie jede Zusammenstellung eines m�glichen Kontakts werden hiermit abgedeckt und �berpr�ft
        /// </summary>
        [TestMethod]
        [DataRow("Peter Lustig", "Peter", "Lustig", Geschlecht.unbekannt, Sprache.unbekannt, "", "Guten Tag Peter Lustig")]
        [DataRow("Herr Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Deutsch, "", "Sehr geehrter Herr Peter Lustig")]
        [DataRow("Frau Petra Witzig", "Petra", "Witzig", Geschlecht.w, Sprache.Deutsch, "", "Sehr geehrte Frau Petra Witzig")]
        [DataRow("Mr. Stanley Funny", "Stanley", "Funny", Geschlecht.m, Sprache.Englisch, "", "Dear Mr. Stanley Funny")]
        [DataRow("Mrs. Karen Silly", "Karen", "Silly", Geschlecht.w, Sprache.Englisch, "", "Dear Mrs. Karen Silly")]
        [DataRow("Ms. Karen Silly", "Karen", "Silly", Geschlecht.w, Sprache.Englisch, "", "Dear Ms. Karen Silly")]
        [DataRow("Herr Professor Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Deutsch, "Prof. ", "Sehr geehrter Herr Prof. Peter Lustig")]
        [DataRow("Frau Professorin Petra Witzig", "Petra", "Witzig", Geschlecht.w, Sprache.Deutsch, "Prof. ", "Sehr geehrte Frau Prof. Petra Witzig")]
        [DataRow("Frau Prof. Petra Witzig", "Petra", "Witzig", Geschlecht.w, Sprache.Deutsch, "Prof. ", "Sehr geehrte Frau Prof. Petra Witzig")]
        [DataRow("Mrs. Dr. rer. nat. Petra Funny", "Petra", "Funny", Geschlecht.w, Sprache.Englisch, "Dr. ", "Dear Dr. Petra Funny")]
        [DataRow("Herr Dr. Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Deutsch, "Dr. ", "Sehr geehrter Herr Dr. Peter Lustig")]
        [DataRow("Mr. Doktor Doctor Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Englisch, "Dr. Dr. ", "Dear Dr. Peter Lustig")]
        [DataRow("Frau Dr. Dr. M. Sc. Prof. Petra Witzig", "Petra", "Witzig", Geschlecht.w, Sprache.Deutsch, "Prof. Dr. Dr. M. Sc. ", "Sehr geehrte Frau Prof. Dr. Dr. Petra Witzig")]
        [DataRow("Diplom-Ingenieur B. Sc. Peter Lustig", "Peter", "Lustig", Geschlecht.unbekannt, Sprache.unbekannt, "Diplom-Ingenieur B. Sc. ", "Guten Tag Peter Lustig")]
        [DataRow("Herr M.Sc. B.Sc. Peter Lustig", "Peter", "Lustig", Geschlecht.m, Sprache.Deutsch, "M.Sc. B.Sc. ", "Sehr geehrter Herr Peter Lustig")]
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

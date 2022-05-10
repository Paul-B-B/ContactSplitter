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
        [DataRow("Peter Lustig", "Peter", "Lustig", Gender.unknown, Language.Unknown, "", "Guten Tag Peter Lustig")]
        [DataRow("Lustig, Peter", "Peter", "Lustig", Gender.unknown, Language.Unknown, "", "Guten Tag Peter Lustig")]
        [DataRow("Peter-Erich Lustig", "Peter-Erich", "Lustig", Gender.unknown, Language.Unknown, "", "Guten Tag Peter-Erich Lustig")]
        [DataRow("Lustig, Peter-Erich", "Peter-Erich", "Lustig", Gender.unknown, Language.Unknown, "", "Guten Tag Peter-Erich Lustig")]
        [DataRow("Peter Lustig-Hans", "Peter", "Lustig-Hans", Gender.unknown, Language.Unknown, "", "Guten Tag Peter Lustig-Hans")]
        [DataRow("Herr Peter Lustig", "Peter", "Lustig", Gender.m, Language.German, "", "Sehr geehrter Herr Peter Lustig")]
        [DataRow("Frau Petra Witzig", "Petra", "Witzig", Gender.f, Language.German, "", "Sehr geehrte Frau Petra Witzig")]
        [DataRow("Mr. Stanley Funny", "Stanley", "Funny", Gender.m, Language.English, "", "Dear Mr. Stanley Funny")]
        [DataRow("Mrs. Karen Silly", "Karen", "Silly", Gender.f, Language.English, "", "Dear Ms. Karen Silly")]
        [DataRow("Ms. Karen Silly", "Karen", "Silly", Gender.f, Language.English, "", "Dear Ms. Karen Silly")]
        [DataRow("Herr Professor Peter Lustig", "Peter", "Lustig", Gender.m, Language.German, "Prof. ", "Sehr geehrter Herr Prof. Peter Lustig")]
        [DataRow("Frau Professorin Petra Witzig", "Petra", "Witzig", Gender.f, Language.German, "Prof. ", "Sehr geehrte Frau Prof. Petra Witzig")]
        [DataRow("Frau Prof. Petra Witzig", "Petra", "Witzig", Gender.f, Language.German, "Prof. ", "Sehr geehrte Frau Prof. Petra Witzig")]
        [DataRow("Mrs. Dr. rer. nat. Petra Funny", "Petra", "Funny", Gender.f, Language.English, "Dr. ", "Dear Dr. Petra Funny")]
        [DataRow("Herr Dr. Peter Lustig", "Peter", "Lustig", Gender.m, Language.German, "Dr. ", "Sehr geehrter Herr Dr. Peter Lustig")]
        [DataRow("Mr. Doktor Doctor Peter Lustig", "Peter", "Lustig", Gender.m, Language.English, "Dr. Dr. ", "Dear Dr. Peter Lustig")]
        [DataRow("Frau Dr. Dr. M. Sc. Prof. Petra Witzig", "Petra", "Witzig", Gender.f, Language.German, "Prof. Dr. Dr. M. Sc. ", "Sehr geehrte Frau Prof. Dr. Dr. Petra Witzig")]
        [DataRow("Diplom-Ingenieur B. Sc. Peter Lustig", "Peter", "Lustig", Gender.unknown, Language.Unknown, "Diplom-Ingenieur B. Sc. ", "Guten Tag Peter Lustig")]
        [DataRow("Herr M.Sc. B.Sc. Peter Lustig", "Peter", "Lustig", Gender.m, Language.German, "M.Sc. B.Sc. ", "Sehr geehrter Herr Peter Lustig")]
        public void ParseKontaktTest(string userInput, string firstName, string lastName, Gender gender, Language language, string allTitles, string letterSalutation)
        {
            var req = userInput;

            var parsedContact = _Parser.ParseKontakt(req);

            Assert.IsNotNull(parsedContact);
            Assert.AreEqual(firstName, parsedContact.FirstName );
            Assert.AreEqual(lastName, parsedContact.LastName);
            Assert.AreEqual(gender, parsedContact.Gender);
            Assert.AreEqual(language, parsedContact.Language);
            Assert.AreEqual(allTitles, parsedContact.AllTitles);
            Assert.AreEqual(letterSalutation, parsedContact.LetterSalutation);

        }
    }
}

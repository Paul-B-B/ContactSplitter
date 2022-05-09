using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class BackendTest
    {

        private readonly TitelHandler _TitelHandler = new TitelHandler();
        private readonly KontaktParser _KontaktParser = new KontaktParser();

        /// <summary>
        /// Test zur gemeinsamen Verwenden des TitelHandlers und KontaktParsers.
        /// Es wird ein bisher nicht vorhandener Titel hinzugefügt und anschließend muss dieser korrekt erkannt werden
        /// </summary>
        [TestMethod]
        [DataRow("Dr. Med.", "Dr. Med.", "Dr. Med. Peter Lustig", "Dr. Med. ", "Guten Tag Dr. Med. Peter Lustig")] //Formell ist die Briefanrede mit "Dr. Med." nicht korrekt, dies dient lediglich dem Test
        [DataRow("Dipl.-Bw.", "", "Dipl.-Bw. Peter Lustig", "Dipl.-Bw. ", "Guten Tag Peter Lustig")]
        public void AddeUndVerwendeTitelTest(string newTitle, string newSalutation, string newContact, string allTitles, string letterSalutation)
        {
            var newTitleSalutation = new TitleSalutation() { Salutation = newSalutation,  Title = newTitle };
            var request = newContact;

            _TitelHandler.AddTitle(newTitleSalutation);
            _KontaktParser.ReadJsonFiles();
            var parsedContact = _KontaktParser.ParseKontakt(request);

            Assert.IsNotNull(parsedContact);
            Assert.AreEqual(allTitles, parsedContact.AllTitles);
            Assert.AreEqual(letterSalutation, parsedContact.LetterSalutation);

        }

        /// <summary>
        /// Test zur Kombination zweier hinzugefügter Titel, diese müssen korrekt hinzugefügt, erkannt und der Briefanrede korrekt hinzugefügt werden
        /// </summary>
        [TestMethod]
        [DataRow("Dipl.-Bw.", "", "Dr. Med.", "Dr. Med.", "Dr. Med. Dipl.-Bw. Peter Lustig", "Dr. Med. Dipl.-Bw. ", "Guten Tag Dr. Med. Peter Lustig")]
        public void KombiniereHinzugefügteTitel(string newTitle1, string newSalutation1, string newTitle2, string newSalutation2, string newContact, string allTitles, string letterSalutation)
        {
            var newTitleSalutation1 = new TitleSalutation() { Salutation = newSalutation1, Title = newTitle1 };
            var newTitleSalutation2 = new TitleSalutation() { Salutation = newSalutation2, Title = newTitle2 };
            var request = newContact;

            _TitelHandler.AddTitle(newTitleSalutation1);
            _TitelHandler.AddTitle(newTitleSalutation2);
            _KontaktParser.ReadJsonFiles();
            var parsedContact = _KontaktParser.ParseKontakt(request);

            Assert.IsNotNull(parsedContact);
            Assert.AreEqual(allTitles, parsedContact.AllTitles);
            Assert.AreEqual(letterSalutation, parsedContact.LetterSalutation);
        }
    }
}

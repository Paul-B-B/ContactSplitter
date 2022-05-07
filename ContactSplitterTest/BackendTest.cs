using ContactSplitter.Backend.Model.Requests;
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
        public void AddeUndVerwendeTitelTest(string neuerTitel, string neueAnrede, string neuerKontakt, string alleTitel, string briefanrede)
        {
            var neueTitelAnrede = new TitelAnrede() { Anrede = neueAnrede,  Titel = neuerTitel };
            var request = new SplitContactRequest() { UserInput = neuerKontakt };

            _TitelHandler.AddTitel(neueTitelAnrede);
            _KontaktParser.LeseJsonDateien();
            var parsedContact = _KontaktParser.ParseKontakt(request);

            Assert.IsNotNull(parsedContact);
            Assert.AreEqual(alleTitel, parsedContact.AlleTitel);
            Assert.AreEqual(briefanrede, parsedContact.Briefanrede);

        }

        /// <summary>
        /// Test zur Kombination zweier hinzugefügter Titel, diese müssen korrekt hinzugefügt, erkannt und der Briefanrede korrekt hinzugefügt werden
        /// </summary>
        [TestMethod]
        [DataRow("Dipl.-Bw.", "", "Dr. Med.", "Dr. Med.", "Dr. Med. Dipl.-Bw. Peter Lustig", "Dr. Med. Dipl.-Bw. ", "Guten Tag Dr. Med. Peter Lustig")]
        public void KombiniereHinzugefügteTitel(string neuerTitel1, string neueAnrede1, string neuerTitel2, string neueAnrede2, string neuerKontakt, string alleTitel, string briefanrede)
        {
            var neueTitelAnrede1 = new TitelAnrede() { Anrede = neueAnrede1, Titel = neuerTitel1 };
            var neueTitelAnrede2 = new TitelAnrede() { Anrede = neueAnrede2, Titel = neuerTitel2 };
            var request = new SplitContactRequest() { UserInput = neuerKontakt };

            _TitelHandler.AddTitel(neueTitelAnrede1);
            _TitelHandler.AddTitel(neueTitelAnrede2);
            _KontaktParser.LeseJsonDateien();
            var parsedContact = _KontaktParser.ParseKontakt(request);

            Assert.IsNotNull(parsedContact);
            Assert.AreEqual(alleTitel, parsedContact.AlleTitel);
            Assert.AreEqual(briefanrede, parsedContact.Briefanrede);
        }
    }
}

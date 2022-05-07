using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class TitelHandlerTest
    {
        private readonly TitelHandler _TitelHandler = new TitelHandler();

        /// <summary>
        /// Test zum Hinzufügen eines Titels
        /// </summary>
        [TestMethod]
        [DataRow("Dr. Med.", "Dr.")]
        public void AddTitelTest(string titel, string anrede)
        {
            var neueTitelAnrede = new TitelAnrede() { Anrede = anrede, Titel = titel };

            var success = _TitelHandler.AddTitel(neueTitelAnrede);

            Assert.IsTrue(success);
        }
    } 
}


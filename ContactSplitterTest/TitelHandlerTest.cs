using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class TitelHandlerTest
    {
        private readonly TitelHandler titelHinzufuegen = new TitelHandler();


        [TestMethod]
        [DataRow("Dr. Med.", "Dr.")]
        public void AddTitelTest(string titel, string anrede)
        {
            var neueTitelAnrede = new TitelAnrede() { Anrede = anrede, Titel = titel };

            var success = titelHinzufuegen.AddTitel(neueTitelAnrede);

            Assert.IsTrue(success);
        }
    } 
}


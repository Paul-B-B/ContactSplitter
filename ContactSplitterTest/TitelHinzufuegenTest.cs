using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactSplitterTest
{
    [TestClass]
    public class TitelHinzufuegenTest
    {
        private TitelAnrede titelAnrede = new TitelAnrede();
        private readonly TitelHandler titelHinzufuegen = new TitelHandler();

        [TestMethod]
        public void TestMethod1()
        {
            titelAnrede.Titel = "Herrr";
            titelAnrede.Anrede = "Hrrr";
            titelHinzufuegen.AddTitel(titelAnrede);

        }
    } 
}


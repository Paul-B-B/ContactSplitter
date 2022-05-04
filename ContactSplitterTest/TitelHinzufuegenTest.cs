using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Backend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContactSplitter.Backend.Model;
using System.Collections.Generic;

namespace ContactSplitterTest
{
    [TestClass]
    public class TitelHinzufuegenTest
    {
        private TitelAnrede titelAnrede = new TitelAnrede();
        private readonly TitelHinzufuegen titelHinzufuegen = new TitelHinzufuegen();

        [TestMethod]
        public void TestMethod1()
        {
            titelAnrede.Titel = "Herrr";
            titelAnrede.Anrede = "Hrrr";
            titelHinzufuegen.AddTitel(titelAnrede);

        }
    } 
}


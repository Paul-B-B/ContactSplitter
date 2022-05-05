using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Modellierung einer Anrede mit zugehörigem Geschlecht und zugehöriger Sprache
    /// </summary>
    public class GeschlechtAnrede
    {
        /// <summary>
        /// Die Anrede des Kontakts, Bsp.: Herr, Frau, Mr, Mrs
        /// </summary>
        public string Anrede { get; set; }

        /// <summary>
        /// Geschlecht des Kontakts
        /// </summary>
        public Geschlecht Geschlecht { get; set; }

        /// <summary>
        /// Sprache des Kontakts
        /// </summary>
        public Sprache Sprache { get; set; }
    }
}

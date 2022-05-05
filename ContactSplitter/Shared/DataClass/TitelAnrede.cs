using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Modell eines Titels eines Kontakts
    /// </summary>
    public class TitelAnrede
    {
        /// <summary>
        /// Die Schreibweise des Titels
        /// </summary>
        public string Titel { get; set; }

        /// <summary>
        /// Die in der Briefanrede zu verwendende Schreibweise
        /// </summary>
        public string Anrede { get; set; }
    }
}

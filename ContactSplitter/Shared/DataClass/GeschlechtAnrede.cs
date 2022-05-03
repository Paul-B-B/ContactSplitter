using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    public class GeschlechtAnrede
    {
        public string Anrede { get; set; }

        public Geschlecht Geschlecht { get; set; }

        public Sprache Sprache { get; set; }
    }
}

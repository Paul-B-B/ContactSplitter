using ContactSplitter.Shared.DataClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model.Responses
{
    public class SplitContactResponse
    {
        public string? Anrede { get; set; } //Herr / Frau

        public string? Briefanrede { get; set; }

        public string? Titel { get; set; } //Prof. / Dr.

        public Geschlecht Geschlecht { get; set; } 

        public string? Vorname { get; set; }

        public string? Nachname { get; set; }

        public string? RawInput { get; set; } //was der Nutzer eingegeben hatte

        public Sprache Sprache { get; set; }
    }
}

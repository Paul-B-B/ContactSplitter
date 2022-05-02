using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model.Responses
{
    public class SplitContactResponse
    {
        public string? Anrede { get; set; }

        public string? Briefanrede { get; set; }

        public string? Titel { get; set; }

        public string? Geschlecht { get; set; }

        public string? Vorname { get; set; }

        public string? Nachname { get; set; }

        public string? RawInput { get; set; }
    }
}

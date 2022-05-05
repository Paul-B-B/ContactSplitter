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

        public Geschlecht Geschlecht { get; set; }

        public string? Vorname { get; set; }

        public string? Nachname { get; set; }

        public string? RawInput { get; set; } //was der Nutzer eingegeben hatte

        public Sprache Sprache { get; set; }

        public List<TitelAnrede>? ListeAllerTitel { get; set; }

        public string? AlleTitel
        {
            get
            {
                var _AlleTitel = string.Empty;
                ListeAllerTitel.ForEach(titel => _AlleTitel += $"{titel.Anrede} ");
                return _AlleTitel;
            }
        }

        public string? BriefTitel
        {
            get
            {
                var _BriefTitel = string.Empty;
                for (int i = 0; i < (ListeAllerTitel.Count >= 3 ? 3 : ListeAllerTitel.Count); i++)
                {
                    _BriefTitel += $"{ListeAllerTitel[i].Anrede} ";
                }
                return _BriefTitel;
            }
        }
    }
}

using ContactSplitter.Shared.DataClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model.Responses
{
    /// <summary>
    /// Response zur Aufteilung eines Kontakts
    /// </summary>
    public class SplitContactResponse
    {
        /// <summary>
        /// Gefundene Anrede des Kontakts, Bsp: Herr oder Frau
        /// </summary>
        public string Anrede { get; set; } = string.Empty;

        /// <summary>
        /// Zusammengestellt Briefanrede des Kontakts, Bsp: "Sehr geehrter Herr Dr. Peter Lustig
        /// Ist nie null
        /// </summary>
        public string Briefanrede { get; set; } = string.Empty;

        /// <summary>
        /// Aus der Anrede gefundenes Geschlecht des Kontakts, Möglichkeiten siehe Enum "Geschlecht"
        /// Default: Unbekannt
        /// </summary>
        public Geschlecht Geschlecht { get; set; } = Geschlecht.unbekannt;

        /// <summary>
        /// Gefundener Vorname des Kontakts
        /// </summary>
        public string Vorname { get; set; } = string.Empty;

        /// <summary>
        /// Gefundener Nachname des Kontakts
        /// </summary>
        public string Nachname { get; set; } = string.Empty;

        /// <summary>
        /// Ursprüngliche Nutzereingabe
        /// </summary>
        public string RawInput { get; set; } = string.Empty;

        /// <summary>
        /// Aus der Anrede gefundene Sprache des Kontakts, Möglichkeiten siehe Enum "Sprache"
        /// Default: Unbekannt
        /// </summary>
        public Sprache Sprache { get; set; } = Sprache.Unbekannt;

        /// <summary>
        /// Liste aller einzelnen Titel
        /// </summary>
        public List<TitelAnrede>? ListeAllerTitel { get; set; } = new List<TitelAnrede>();

        /// <summary>
        /// String aller Titel des Kontakts
        /// </summary>
        public string AlleTitel
        {
            get
            {
                var _AlleTitel = string.Empty;
                ListeAllerTitel.ForEach(titel => _AlleTitel += $"{titel.Anrede} ");
                return _AlleTitel;
            }
        }

        /// <summary>
        /// String der ersten drei Titel des Kontakts
        /// </summary>
        public string BriefTitel
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

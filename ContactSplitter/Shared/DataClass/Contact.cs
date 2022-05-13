using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Response zur Aufteilung eines Kontakts
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gefundene Salutation des Kontakts, Bsp: Herr oder Frau
        /// </summary>
        public string Salutation { get; set; } = string.Empty;

        /// <summary>
        /// Zusammengestellt BriefSalutation des Kontakts, Bsp: "Sehr geehrter Herr Dr. Peter Lustig
        /// Ist nie null
        /// </summary>
        public string LetterSalutation { get; set; } = string.Empty;

        /// <summary>
        /// Aus der Salutation gefundenes Geschlecht des Kontakts, Möglichkeiten siehe Enum "Geschlecht"
        /// Default: Unbekannt
        /// </summary>
        public Gender Gender { get; set; } = Gender.unknown;

        /// <summary>
        /// Gefundener Vorname des Kontakts
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gefundener Nachname des Kontakts
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Ursprüngliche Nutzereingabe
        /// </summary>
        public string RawInput { get; set; } = string.Empty;

        /// <summary>
        /// Aus der Salutation gefundene Sprache des Kontakts, Möglichkeiten siehe Enum "Sprache"
        /// Default: Unbekannt
        /// </summary>
        public Language Language { get; set; } = Language.Unknown;

        /// <summary>
        /// Liste aller einzelnen Titel
        /// </summary>
        public List<TitleSalutation>? TitleList { get; set; } = new List<TitleSalutation>();

        /// <summary>
        /// String aller Titel des Kontakts
        /// </summary>
        public string AllTitles
        {
            get
            {
                var allTitles = string.Empty;
                TitleList.ForEach(titel => allTitles += string.IsNullOrEmpty(titel.Salutation) ? $"{titel.Title} " : $"{titel.Salutation} ");
                return allTitles;
            }
        }

        /// <summary>
        /// String der ersten drei Titel des Kontakts
        /// </summary>
        public string LetterTitle
        {
            get
            {
                var letterTitle = string.Empty;
                if (Language == Language.English)
                {
                    letterTitle = (TitleList.Count != 0 && !string.IsNullOrEmpty(TitleList[0].Salutation)) ? $"{TitleList[0].Salutation} " : string.Empty;
                }
                else
                {
                    for (int i = 0; i < (TitleList.Count >= 3 ? 3 : TitleList.Count); i++)
                    {
                        letterTitle += string.IsNullOrEmpty(TitleList[i].Salutation) ? string.Empty : $"{TitleList[i].Salutation} ";
                    }

                }

                return letterTitle;
            }
        }
    }
}

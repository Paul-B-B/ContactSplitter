using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Modell einer Telefunnumer zur internen Kommunikation
    /// </summary>
    public class PhoneNumber
    {
        /// <summary>
        /// Ursprüngliche Nutzereingae und String, auf dem gearbeitet wird beim Parsen
        /// </summary>
        public string RawNumberInput { get; set; }

        /// <summary>
        /// Die final nach DIN 5008
        /// </summary>
        public string FormattedNumber { get; set; }

        /// <summary>
        /// Land der Nummer, erkannt aus der Ländervorwahl
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// Erkannte Vorwahl der Telefonnummer
        /// </summary>
        public string AreaCode { get; set; } 

        /// <summary>
        /// Erkannte Durchwahl der Telefonnummer
        /// </summary>
        public string DirectDial { get; set; }

        /// <summary>
        /// Erkannte Hauptnummer der Telefonnummer
        /// </summary>
        public string MainNumber { get; set; }

        /// <summary>
        /// Flag, ob eine Durchwahl vorhanden ist
        /// </summary>
        public bool HasDirectDial { get; set; }

    }
}

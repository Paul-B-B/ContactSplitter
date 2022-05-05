using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    /// <summary>
    /// Modellierung eines Landes zur Erkennung der Telefonnummern
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Name des Landes
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Landesvorwahl
        /// </summary>
        public string PhoneCode { get; set; }

        /// <summary>
        /// Abkürzung des Landes
        /// </summary>
        public string Abbrevation { get; set; }
    }
}

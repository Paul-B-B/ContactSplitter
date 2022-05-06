using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;

namespace ContactSplitter.Backend.Services
{
    public class PhoneNumberSplitterModel : IPhoneNumberSplitter
    {

        private readonly PhoneNumberParser _phoneNumberParser;

        /// <summary>
        /// Erstellt ein neues Objekt der PhoneNumberSplitter Klasse und gibt dieses zurück.
        /// Der PhoneNumberSplitter ist für das Einlesen und auftrennen von Telefonnummern zuständig
        /// </summary>
        public PhoneNumberSplitterModel()
        {
            _phoneNumberParser = new PhoneNumberParser();
        }

        /// <summary>
        /// Formatiert eine Telefonnumer in ihre einzelnen Bestandteile und gibt diese als PhoneNumber Objekt zurück
        /// </summary>
        /// <param name="userInput">Die zu parsende Telefonnummer</param>
        /// <returns>Die geparste Telefonnummer</returns>
        public PhoneNumber GetFormattedNumber(string userInput)
        {
            if(userInput == null) return null;
            userInput.Trim(); //Um abzufangen, wenn Nutzer zu Beginn oder Ende Leerzeichen einfügen

            return _phoneNumberParser.ParseNumber(userInput);

        }

    }
}

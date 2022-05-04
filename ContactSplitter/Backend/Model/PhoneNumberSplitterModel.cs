using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;

namespace ContactSplitter.Backend.Model
{
    public class PhoneNumberSplitterModel : IPhoneNumberSplitterModel
    {

        private readonly PhoneNumberParser _phoneNumberParser;

        public PhoneNumberSplitterModel()
        {
            _phoneNumberParser = new PhoneNumberParser();
        }

        public PhoneNumber GetFormattedNumber(string userInput)
        {
            if(userInput == null) return null;
            userInput.Trim(); //Um abzufangen, wenn Nutzer zu Beginn oder Ende Leerzeichen einfügen

            return _phoneNumberParser.ParseNumber(userInput);

        }

    }
}

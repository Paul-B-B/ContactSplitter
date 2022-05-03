using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model
{
    public class PhoneNumberModel : IPhoneNumberModel
    {

        private readonly PhoneNumberParser _phoneNumberParser;

        public PhoneNumberModel()
        {
            _phoneNumberParser = new PhoneNumberParser();
        }

        public PhoneNumber GetFormattedNumber(string userInput)
        {
            userInput.Trim(); //Um abzufangen, wenn Nutzer zu Beginn oder Ende Leerzeichen einfügen

            return _phoneNumberParser.ParseNumber(userInput);

        }

    }
}

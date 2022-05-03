using ContactSplitter.Shared.DataClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model.Interfaces
{
    internal interface IPhoneNumberModel
    {
        PhoneNumber GetFormattedNumber(string userInput);

    }
}

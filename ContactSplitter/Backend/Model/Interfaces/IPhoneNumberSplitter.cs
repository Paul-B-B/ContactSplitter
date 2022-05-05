using ContactSplitter.Shared.DataClass;

namespace ContactSplitter.Backend.Model.Interfaces
{
    internal interface IPhoneNumberSplitter
    {
        PhoneNumber GetFormattedNumber(string userInput);

    }
}

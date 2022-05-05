using ContactSplitter.Shared.DataClass;

namespace ContactSplitter.Backend.Model.Interfaces
{
    internal interface IPhoneNumberSplitterModel
    {
        PhoneNumber GetFormattedNumber(string userInput);

    }
}

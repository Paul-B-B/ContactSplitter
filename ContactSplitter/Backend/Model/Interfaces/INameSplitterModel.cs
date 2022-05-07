using ContactSplitter.Backend.Model.Responses;

namespace ContactSplitter.Backend.Model.Interfaces
{
    internal interface INameSplitterModel
    {
        SplitContactResponse GetSplitContact(string input);
    }
}

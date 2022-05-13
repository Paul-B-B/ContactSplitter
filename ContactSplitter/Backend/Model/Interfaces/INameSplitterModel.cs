using ContactSplitter.Shared.DataClass;

namespace ContactSplitter.Backend.Model.Interfaces
{
    internal interface INameSplitterModel
    {
        Contact GetSplitContact(string input);

        string GetLetterSalutation(Contact contact);

        void AddTitle(TitleSalutation input);

    }
}

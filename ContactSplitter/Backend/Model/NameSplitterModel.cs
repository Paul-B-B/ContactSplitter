using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Services;
using ContactSplitter.Shared.DataClass;

namespace ContactSplitter.Backend.Model
{
    class NameSplitterModel : INameSplitterModel
    {
        private KontaktParser _kontaktParser;

        private TitelHandler _titelHandler;

        public NameSplitterModel()
        {
            _kontaktParser = new KontaktParser();
            _titelHandler = new TitelHandler();
        }

        public Contact GetSplitContact(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var output = _kontaktParser.ParseKontakt(input);
            return output;
        }

        public string GetLetterSalutation(Contact contact)
        {
            if (contact == null) return null;
            var output = this._kontaktParser.CreateLetterSalutation(contact);
            return output;
        }

        public void AddTitle(TitleSalutation titleSalutation)
        {
            if (titleSalutation == null) return;

            _titelHandler.AddTitle(titleSalutation);
            this._kontaktParser.ReadJsonFiles();
        }
    }
}

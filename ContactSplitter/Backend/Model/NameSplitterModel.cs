using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
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

        public SplitContactResponse GetSplitContact(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var output = _kontaktParser.ParseKontakt(new SplitContactRequest() { UserInput = input});
            return output;
        }

        public void AddTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) return;

            var titleAnrede = new TitelAnrede() { Anrede = "", Titel = title };
            _titelHandler.AddTitel(titleAnrede);
            this._kontaktParser.LeseJsonDateien();
        }
    }
}

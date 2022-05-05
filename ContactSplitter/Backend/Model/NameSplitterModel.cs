using ContactSplitter.Backend.Model.Interfaces;
using ContactSplitter.Backend.Model.Requests;
using ContactSplitter.Backend.Model.Responses;
using ContactSplitter.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model
{
    class NameSplitterModel : INameSplitterModel
    {
        private KontaktParser _kontaktParser;

        public NameSplitterModel()
        {
            _kontaktParser = new KontaktParser(@"Backend\Data\");
        }

        public SplitContactResponse GetSplitContact(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var output = _kontaktParser.ParseKontakt(new SplitContactRequest(input));
            return output;
        }
    }
}

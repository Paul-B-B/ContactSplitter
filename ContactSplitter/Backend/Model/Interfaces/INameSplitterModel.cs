using ContactSplitter.Backend.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model.Interfaces
{
    internal interface INameSplitterModel
    {
        SplitContactResponse GetSplitContact(string input);
    }
}

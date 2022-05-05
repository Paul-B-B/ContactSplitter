using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Backend.Model.Requests
{
    public class SplitContactRequest
    {
        public string UserInput { get; set; }

        public SplitContactRequest(string userInput)
        {
            this.UserInput = userInput;
        }
    }
}

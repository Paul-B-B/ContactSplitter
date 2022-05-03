using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactSplitter.Shared.DataClass
{
    public class PhoneNumber
    {
        public string RawNumberInput { get; set; }

        public string FormattedNumber { get; set; }

        public Country Country { get; set; }

        public string AreaCode { get; set; } //Vorwahl

        public string DirectDial { get; set; } //Durchwahl

        public string MainNumber { get; set; }

        public bool HasDirectDial { get; set; }

    }
}

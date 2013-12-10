using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Integer : ResponseBase
    {
        private int defaultInteger;

        private string defaultIntegerString;
        public string DefaultIntegerString
        {
            get { return defaultIntegerString; }

            set
            {
                bool parseOK;
                parseOK = Int32.TryParse(value, out defaultInteger);

                

            }
        }

        public Integer()
        {
            defaultInteger = Int32.MinValue;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class SampleCode : ResponseBase
    {
        public override string ResponseXMLType
        {
            get
            {
                return "SampleCode";
            }
        }

        public override ResponseBase Copy()
        {
            return new SampleCode();
        }

    }
}

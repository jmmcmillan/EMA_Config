using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class FillIn : ResponseBase
    {
        public override string ResponseXMLType
        {
            get
            {
                return "FillIn";
            }
        }

        public FillIn() : base() { }

        public override ResponseBase Copy()
        {
            return new FillIn();
        }
    }
}

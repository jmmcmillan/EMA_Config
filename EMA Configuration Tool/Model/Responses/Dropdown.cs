using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Dropdown : StringChoice
    {
        public override string ResponseXMLType
        { get { return "Dropdown"; } }

        public Dropdown()
            : base()
        {
        }

        public override ResponseBase Copy()
        {
            Dropdown dd = new Dropdown();
            dd.Responses = Responses;
            return dd;
        }
    }
}

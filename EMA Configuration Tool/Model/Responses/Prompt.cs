using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Prompt : ResponseBase
    {
        public override string ResponseXMLType
        {
            get
            {
                return "Prompt";
            }
        }

    }
}

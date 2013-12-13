using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class SingleChoiceList : StringChoice
    {
        public override string ResponseXMLType
        { 
            get 
            { 
                if (Responses.IsZeroBased)
                    return "SingleChoiceList_0Index";
                else return "SingleChoiceList_1Index"; 
            } 
        }


        public SingleChoiceList() : base()
        {
            Description = "Single Choice List";

            
        }
    }
}

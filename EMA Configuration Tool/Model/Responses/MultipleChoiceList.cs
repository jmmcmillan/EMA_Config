using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class MultipleChoiceList : StringChoice    
    {
        public override string ResponseXMLType
        { 
            get 
            {
                if (Responses.IsZeroBased)
                    return "MultipleChoiceList_0Index";
                else return "MultipleChoiceList_1Index"; 
            } 
        }

        public MultipleChoiceList()
            : base()
        {
        }

        public override ResponseBase Copy()
        {
            MultipleChoiceList mcl = new MultipleChoiceList();
            mcl.Responses = Responses;
            return mcl;
        }
    }
}

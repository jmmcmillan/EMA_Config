using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class SelectedResponsesFrom : BasedOnQuestions
    {
        public override string ResponseXMLType
        { get { return "SelectedResponsesFrom"; } }

        
        public SelectedResponsesFrom() 
            : base()
        {
            
        }

        public override ResponseBase Copy()
        {
            SelectedResponsesFrom srf = new SelectedResponsesFrom();
            srf.ReferenceQuestions = base.CopyReferenceQuestions();

            return srf;
        }
    }
}

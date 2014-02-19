using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class DynamicGroup : BasedOnQuestions
    {
        public override string ResponseXMLType
        { get { return "DynamicGroup"; } }


        public DynamicGroup()
            : base()
        {
        }

        public override ResponseBase Copy()
        {
            DynamicGroup dg = new DynamicGroup();
            dg.ReferenceQuestions = base.CopyReferenceQuestions();

            return dg;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class PeopleNamesList : GeneratedChoice
    {
        public override string ResponseXMLType
        {
            get { return "PeopleNamesList"; }
        }

        public PeopleNamesList() : base()
        {
            Description = "List of People's Names";
        }

       
        public override ResponseBase Copy()
        {
            PeopleNamesList names = new PeopleNamesList();
            names.Responses = Responses;
            return names;
        }
    }
}

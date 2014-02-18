using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class SocialGroupsList : GeneratedChoice
    {
        public override string ResponseXMLType
        {
            get { return "SocialGroupsList"; }
        }

        public SocialGroupsList() : base()
        {
            Description = "List of Default Social Groups";
        }

       
        public override ResponseBase Copy()
        {
            SocialGroupsList names = new SocialGroupsList();
            names.Responses = Responses;
            return names;
        }
    }
}

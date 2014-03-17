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

            base.Responses = SocialNetwork.TopLevelSocialGroupsResponseSet;
        }        

        public override ResponseBase Copy()
        {
            SocialGroupsList names = new SocialGroupsList();
            names.Responses = Responses;
            return names;
        }
    }

    public class SecondLevelSocialGroupsList : SocialGroupsList
    {
         public override string ResponseXMLType
        {
            get { return "SecondLevelSocialGroupsList"; }
        }

        public SecondLevelSocialGroupsList() : base()
        {
            Description = "List of Default Second Level Social Groups";

            base.Responses = SocialNetwork.SecondLevelSocialGroupsResponseSet;
        }        

        public override ResponseBase Copy()
        {
            SecondLevelSocialGroupsList names = new SecondLevelSocialGroupsList();
            names.Responses = Responses;
            return names;
        }
    }

    public class CustomSocialGroupsList : SocialGroupsList
    {
        public override string ResponseXMLType
        {
            get { return "CustomSocialGroupsList"; }
        }

        public CustomSocialGroupsList()
            : base()
        {
            Description = "List of Custom Social Groups";

            base.Responses = SocialNetwork.CustomSocialGroupsResponseSet;
        }

        public override ResponseBase Copy()
        {
            CustomSocialGroupsList names = new CustomSocialGroupsList();
            names.Responses = Responses;
            return names;
        }
    }
}

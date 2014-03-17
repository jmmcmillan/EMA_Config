using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using EMA_Configuration_Tool.Model.Groups;

namespace EMA_Configuration_Tool.Model
{
    
    public class SocialNetwork
    {

        [XmlArray(ElementName = "groups")]
        [XmlArrayItem("group")]
        public List<Group> Groups { get; set; }


        [XmlIgnore]
        public static StringResponseSet TopLevelSocialGroupsResponseSet;

        [XmlIgnore]
        public static StringResponseSet SecondLevelSocialGroupsResponseSet;

        [XmlIgnore]
        public static StringResponseSet CustomSocialGroupsResponseSet;


        [XmlIgnore]
        public static List<Group> SocialGroups;

        [XmlIgnore]
        public ObservableCollection<Group> customGroups;
        [XmlIgnore]
        public ObservableCollection<Group> CustomSocialGroups
        {
            get
            {
                if (customGroups == null)
                    customGroups = new ObservableCollection<Group>();
                return customGroups;
            }
            set
            {
                customGroups = value;
            }
        }


        [XmlIgnore]
        public ObservableCollection<Person> People { get; set; }

        public static string[] TopLevelSocialGroupNames = new string[] { "Spouse or partner", "Child", "Grandchild", "Parent", "In-law", "Other relative", "Coworker", "Neighbor", "Classmate", "Close friend", "Group member", "Service Professional", "Service Recipient (Client, Student)", "Other acquaintance", "Stranger" };
        public static string[] SecondLevelGroups = new string[] { "Boss", "Employee", "Other coworker" };


        public SocialNetwork()
        {
            People = new ObservableCollection<Person>();

            SocialGroups = new List<Group>();
            TopLevelSocialGroupsResponseSet = new StringResponseSet();
            SecondLevelSocialGroupsResponseSet = new StringResponseSet();

            foreach (string groupLabel in TopLevelSocialGroupNames)
            {
                TopLevelSocialGroupsResponseSet.StringResponses.Add(groupLabel);

                if (groupLabel.Equals("Coworker"))
                {
                    foreach (string s in SecondLevelGroups)
                    {
                        SocialGroups.Add(new Group(s));
                        SecondLevelSocialGroupsResponseSet.StringResponses.Add(s);

                    }
                }
                else SocialGroups.Add(new Group(groupLabel));
            }

            CustomSocialGroupsResponseSet = new StringResponseSet();

        }


        public void RefreshCustomGroupResponseSet()
        {
            CustomSocialGroupsResponseSet.StringResponses.Clear();

            foreach (Group group in customGroups)
            {
                CustomSocialGroupsResponseSet.StringResponses.Add(group.GroupName);
            }
        }

        public void ConstructGroups()
        {
            Groups = new List<Group>();

            IEnumerable<Group> groupsForXML = SocialGroups.Concat(CustomSocialGroups);

            foreach (Group group in groupsForXML)
            {
                group.Names.Clear();

                foreach (Person p in People)
                {
                    if (p.MyGroups.Contains(group))
                    {
                        group.Names.Add(p.Name);
                    }
                }

                Groups.Add(group);
            }
        }

        public void RecoverFromSerialization()
        {
            GeneratePeople();
        }

        public void PrepareForSerialization()
        {
            ConstructGroups();
        }

        public void GeneratePeople()
        {
            if (Groups.Count < 1)
                return;

            SocialGroups = new List<Group>();
            CustomSocialGroups = new ObservableCollection<Group>();

            foreach (Group group in Groups)
            {
                if (TopLevelSocialGroupNames.Contains(group.GroupName))
                {
                    SocialGroups.Add(group);
                }
                else if (SecondLevelGroups.Contains(group.GroupName))
                {
                    SocialGroups.Add(group);
                }
                else CustomSocialGroups.Add(group);

                foreach (string personName in group.Names)
                {
                    Person thisPerson = People.Where(p => p.Name == personName).FirstOrDefault();

                    if (thisPerson == null)
                    {
                        thisPerson = new Person() { Name = personName };
                        thisPerson.MyGroups.Add(group);
                        People.Add(thisPerson);
                    }
                    else
                    {
                        thisPerson.MyGroups.Add(group);
                    }
                }
            }

            RefreshCustomGroupResponseSet();
        }
    }
}

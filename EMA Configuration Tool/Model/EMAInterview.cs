using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using EMA_Configuration_Tool.Model.Constraints;
using EMA_Configuration_Tool.Model.Responses;
using EMA_Configuration_Tool.Model.Groups;

namespace EMA_Configuration_Tool.Model
{
    [XmlRoot("interview")]
    public class EMAInterview : PropertyChangedBase
    {
        #region settings

        [XmlAttribute("timeout")]
        public int Timeout { get; set; }

        [XmlAttribute("startMessage")]
        public string StartMessage { get; set; }

        [XmlAttribute("canCancel")]
        public string XMLCanCancel 
        {
            get
            {
                if (CanCancel)
                    return "true";
                else return "false";
            }

            set
            {
                if (value.Contains("true"))
                    CanCancel = true;
                else CanCancel = false;
            }
        }

        [XmlIgnore]
        public bool CanCancel { get; set; }

        [XmlAttribute("canDelay")]
        public string XMLCanDelay
        {
            get
            {
                if (CanDelay)
                    return "true";
                else return "false";
            }

            set
            {
                if (value.Contains("true"))
                    CanDelay = true;
                else CanDelay = false;
            }
        }

        [XmlIgnore]
        public bool CanDelay { get; set; }

        [XmlIgnore]
        public bool OutputSalivaScreens { get; set; }

        #endregion


        [XmlArray(ElementName = "groups")]
        [XmlArrayItem("group")]
        public List<Group> Groups { get; set; }

       

        public void ConstructGroups()
        {
            Groups = new List<Group>();

            for (int i = 0; i < SocialGroupNames.Count(); i++)
            {
                Group thisGroup = new Group() { GroupName = SocialGroupNames[i] };

                foreach (Person p in People)
                {
                    if (p.GroupMembership[i] == true)
                    {
                        thisGroup.Names.Add(p.Name);
                    }
                }

                Groups.Add(thisGroup);
            }
        }

        [XmlIgnore]
        public ObservableCollection<Person> People { get; set; }

        [XmlIgnore]
        public static string[] SocialGroupNames { get; set; }
            

        [XmlIgnore]
        private ObservableCollection<StringResponseSet> stringResponseSets;


       
        [XmlArray(ElementName = "questions")]
        [XmlArrayItem("question")]        
        public List<Question> Questions { get; set; }

        [XmlArray(ElementName = "responses")]
        [XmlArrayItem("response")]
        public ObservableCollection<StringResponseSet> StringResponseSets
        {
            get { return stringResponseSets; }
            set
            {
                stringResponseSets = value;
                NotifyOfPropertyChange(() => StringResponseSets);
            }
        }
        

        [XmlIgnore]
        private ObservableCollection<object> constraints;
        [XmlIgnore]
        public ObservableCollection<object> Constraints
        {
            get { return constraints; }
            set
            {
                constraints = value;
                NotifyOfPropertyChange(() => Constraints);
            }
        }


        public void SynchronizeIndices()
        {
            setResponseSetIndices();
            setQuestionIndices();
        }

        public void GenerateConstraints()
        {
            foreach (Question question in Questions)
            {
                if (question.FollowupFor != -1)
                {
                    EMA_Configuration_Tool.Services.ConstraintService.GenerateConstraint(question);
                }
            }
        }

        private void GenerateResponses()
        {
            foreach (Question question in Questions)
            {
                if (question.Response == null)
                {
                    question.Response = EMA_Configuration_Tool.Services.ResponseService.GetMeOneOfThese(question.ResponseType);

                    if (question.Response is StringChoice)
                    {
                        (question.Response as StringChoice).Responses = StringResponseSets.ElementAt(question.ResponseIndex);
                    }

                    if (question.Response is Integer || question.Response is Time)
                    {
                        question.Response.ResponseXMLDefaults = question.Defaults;
                    }

                    if (question.Response is DynamicGroup)
                    {
                        (question.Response as DynamicGroup).ReferenceQuestion = Questions.ElementAt(question.GroupsFromQuestion);                     
                    }
                }

            }

        }

        [XmlIgnore]
        public Dictionary<Guid, int> QuestionsToIndexes;
        private void setQuestionIndices()
        {
            QuestionsToIndexes = new Dictionary<Guid, int>();

            int i = 0;
            foreach (Question q in Questions)
            {
                QuestionsToIndexes.Add(q.ID, i);
                q.Index = i;

                i++;
            }
        }

        [XmlIgnore]
        public Dictionary<Guid, int> ResponseSetsToIndexes;
        private void setResponseSetIndices()
        {
            ResponseSetsToIndexes = new Dictionary<Guid, int>();

            int i = 0;
            foreach (StringResponseSet sts in StringResponseSets)
            {
                ResponseSetsToIndexes.Add(sts.ID, i);
                sts.Index = i;

                i++;
            }
        }

        public void PrepareForSerialization()
        {
            SynchronizeIndices();
            ConstructGroups();
        }

        public void RecoverFromSerialization()
        {
            SynchronizeIndices();
            GenerateConstraints();
            GenerateResponses();
            GeneratePeople();
        }

        public void GeneratePeople()
        {
            foreach (Group group in Groups)
            {
                int thisIndex = 0;
                foreach (string s in SocialGroupNames)
                {
                    if (s.Equals(group.GroupName))
                    {
                        break;
                    }
                    thisIndex++;
                }

                foreach (string personName in group.Names)
                {
                    Person thisPerson = People.Where(p => p.Name == personName).FirstOrDefault();

                    if (thisPerson == null)
                    {
                        thisPerson = new Person() { Name = personName };
                        thisPerson.GroupMembership[thisIndex] = true;
                        People.Add(thisPerson);
                    }
                    else
                    {
                        thisPerson.GroupMembership[thisIndex] = true;
                    }
                    


                }
            }
        }

        public EMAInterview()
        {
            Questions = new List<Question>();
            StringResponseSets = new ObservableCollection<StringResponseSet>();
            People = new ObservableCollection<Person>();

            Constraints = new ObservableCollection<object>();
            Constraints.Add("None (This question always appears.)");
            
            SocialGroupNames = new string[] { "Spouse/partner", "Child", "Parent", "In-law", "Other relative", "Coworker", "Neighbor", "Classmate", "Church/temple/religious group", "Volunteer work group", "Other group", "Service professional", "Friend", "Stranger" };

            OutputSalivaScreens = true;
            Timeout = 900000;
        }
    }
}

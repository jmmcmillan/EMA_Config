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

        [XmlAttribute("participantID")]
        public string ParticipantID { get; set; }

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

            IEnumerable<Group> groupsForXML = SocialGroups.Concat(CustomSocialGroups);

            foreach (Group group in groupsForXML)
            {
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

        [XmlIgnore]
        public ObservableCollection<Person> People { get; set; }

        [XmlIgnore]
        public static string[] TopLevelSocialGroupNames = new string[] { "Spouse/partner", "Child", "Parent", "In-law", "Other relative", "Coworker", "Neighbor", "Classmate", "Church/temple/religious group", "Volunteer work group", "Other group", "Service professional", "Friend", "Stranger" };
        public static string[] SecondLevelGroups = new string[] { "Boss", "Employee", "Other coworker" };

        [XmlIgnore]
        private ObservableCollection<StringResponseSet> stringResponseSets;

        [XmlIgnore]
        public static List<Group> SocialGroups;

        [XmlIgnore]
        public static List<Group> CustomSocialGroups;
       
        [XmlArray(ElementName = "questions")]
        [XmlArrayItem("question")]        
        public ObservableCollection<Question> Questions { get; set; }



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

                    if (question.Response is GeneratedChoice)
                    {
                        (question.Response as GeneratedChoice).Responses = StringResponseSets.ElementAt(question.ResponseIndex);
                    }

                    if (question.Response is Integer || question.Response is Time)
                    {
                        question.Response.ResponseXMLDefaults = question.Defaults;
                    }

                    if (question.Response is BasedOnQuestions)
                    {
                        List<Guid> relevantQuestions = new List<Guid>();
                        foreach (int i in question.BasedOnQuestions)
                        {
                            relevantQuestions.Add(Questions.ElementAt(i).ID);
                        }

                        //have to do it this way because can't modify the collection while iterating through it
                        List<ReferenceQuestion> newReferenceQuestions = new List<ReferenceQuestion>();                        
                        foreach (ReferenceQuestion rq in (question.Response as BasedOnQuestions).ReferenceQuestions)
                        {
                            if (relevantQuestions.Contains(rq.Question.ID))
                                newReferenceQuestions.Add(new ReferenceQuestion(rq.Question, true));
                            else newReferenceQuestions.Add(new ReferenceQuestion(rq.Question, false));
                        }

                        (question.Response as BasedOnQuestions).ReferenceQuestions = newReferenceQuestions;
                     
                    }
                }
            }

            //remove generated string response sets
            Question removeNames = Questions.Where(q => q.Response is PeopleNamesList).FirstOrDefault();
            if (removeNames != null)
                StringResponseSets.Remove((removeNames.Response as PeopleNamesList).Responses);

            Question removeSocialGroups = Questions.Where(q => q.Response is SocialGroupsList).FirstOrDefault();
            if (removeSocialGroups != null)
                StringResponseSets.Remove((removeSocialGroups.Response as SocialGroupsList).Responses);

            //convert exclusive indices back into strings
            foreach (StringResponseSet sts in StringResponseSets)
            {
                if (sts.xmlExclusiveOption > -1)
                {
                    if (sts.xmlExclusiveOption < sts.StringResponses.Count)
                        sts.ExclusiveOption = sts.StringResponses.ElementAt(sts.xmlExclusiveOption);
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
            AddContentForGeneratedTypes();

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


        private void AddContentForGeneratedTypes()
        {
            StringResponseSet namesSTS = null;
            StringResponseSet groupsSTS = null;
            StringResponseSet customGroupsSTS = null;

            foreach (Question q in Questions)
            {
                if (q.Response is PeopleNamesList)
                {
                    if (namesSTS == null)
                    {
                        string nan = "None of the above";

                        List<string> allNames = People.Select(p => p.Name).OrderBy(p => p).ToList();
                        allNames.Add(nan);

                        namesSTS = new StringResponseSet(Guid.NewGuid(), allNames);
                        namesSTS.ExclusiveOption = nan;

                        StringResponseSets.Add(namesSTS);
                    }

                    (q.Response as PeopleNamesList).Responses = namesSTS;
                }

                if (q.Response is SocialGroupsList)
                {
                    if (groupsSTS == null)
                    {   
                        groupsSTS = new StringResponseSet(Guid.NewGuid(), TopLevelSocialGroupNames.ToList());
                        StringResponseSets.Add(groupsSTS);
                    }

                    (q.Response as SocialGroupsList).Responses = groupsSTS;
                }
            }
        }

        public void GeneratePeople()
        {
            SocialGroups = new List<Group>();
            CustomSocialGroups = new List<Group>();

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
        }

        public EMAInterview()
        {
            Questions = new ObservableCollection<Question>();
            StringResponseSets = new ObservableCollection<StringResponseSet>();
            People = new ObservableCollection<Person>();

            Constraints = new ObservableCollection<object>();
            Constraints.Add("None (This question always appears.)");
            
            SocialGroups = new List<Group>();
            foreach (string groupLabel in TopLevelSocialGroupNames)
            {
                if (groupLabel.Equals("Coworker"))
                {
                    foreach (string s in SecondLevelGroups)
                        SocialGroups.Add(new Group(s));
                }
                else SocialGroups.Add(new Group(groupLabel));
            }

            CustomSocialGroups = new List<Group>();

            OutputSalivaScreens = true;
            Timeout = 900000;
        }
    }
}

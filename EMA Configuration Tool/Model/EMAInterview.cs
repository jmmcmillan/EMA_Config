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

        [XmlIgnore]
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

        [XmlAttribute("type")]
        public InterviewType EMAType { get; set; }

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

        //[XmlIgnore]
        //public bool OutputSalivaScreens { get; set; }

        #endregion



        

        [XmlIgnore]
        private ObservableCollection<StringResponseSet> stringResponseSets;


       
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
                        BasedOnQuestions boq = question.Response as BasedOnQuestions;

                        foreach (int i in question.BasedOnQuestions)
                        {
                            boq.ReferenceQuestions.Add(Questions.ElementAt(i));
                        }
                    }

                    if (question.Response is SocialGroupsList)
                    {
                        (question.Response as SocialGroupsList).Responses = SocialNetwork.TopLevelSocialGroupsResponseSet;
                    }

                    if (question.Response is CustomSocialGroupsList)
                    {
                        (question.Response as CustomSocialGroupsList).Responses = SocialNetwork.CustomSocialGroupsResponseSet;
                    }

                    if (question.Response is SecondLevelSocialGroupsList)
                    {
                        (question.Response as SecondLevelSocialGroupsList).Responses = SocialNetwork.SecondLevelSocialGroupsResponseSet;
                    }
                }
            }

            //remove generated string response sets
            Question removeNames = Questions.Where(q => q.Response is PeopleNamesList).FirstOrDefault();
            if (removeNames != null)
                StringResponseSets.Remove((removeNames.Response as PeopleNamesList).Responses);

            List<Question> removeSocialGroups = Questions.Where(q => q.Response is SocialGroupsList).ToList();
            foreach (Question sg in removeSocialGroups)
            {
                StringResponseSets.Remove((sg.Response as SocialGroupsList).Responses);
            }

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
            
        }

        public void RecoverFromSerialization()
        {
            SynchronizeIndices();
            GenerateConstraints();
            GenerateResponses();
           
        }


        private void AddContentForGeneratedTypes()
        {
            StringResponseSet namesSTS = null;
            //StringResponseSet groupsSTS = null;
            //StringResponseSet customGroupsSTS = null;
            

            foreach (Question q in Questions)
            {
                if (q.Response is PeopleNamesList)
                {
                    if (namesSTS == null)
                    {
                        string nan = "None of the above";

                        List<string> allNames = App.Network.People.Select(p => p.Name).OrderBy(p => p).ToList();
                        allNames.Add(nan);

                        namesSTS = new StringResponseSet(Guid.NewGuid(), allNames);
                        namesSTS.ExclusiveOption = nan;

                        StringResponseSets.Add(namesSTS);
                    }

                    (q.Response as PeopleNamesList).Responses = namesSTS;
                }

                if (q.Response is SocialGroupsList)
                {
                    if (StringResponseSets.Contains((q.Response as SocialGroupsList).Responses))
                        continue;
                    else StringResponseSets.Add((q.Response as SocialGroupsList).Responses);
                }
            }
        }

        

        public EMAInterview()
        {
            Questions = new ObservableCollection<Question>();
            StringResponseSets = new ObservableCollection<StringResponseSet>();
            
            Constraints = new ObservableCollection<object>();
            Constraints.Add("None (This question always appears.)");
           

            

            OutputSalivaScreens = true;
            Timeout = 900000;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Responses;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model.Constraints;
using EMA_Configuration_Tool.Model.Groups;

namespace EMA_Configuration_Tool.Model
{
    public class Question : PropertyChangedBase
    {
        
        [XmlText]
        public string Text { get; set; }

        [XmlAttribute("mapping")]
        public int XMLResponseIndex
        {
            get
            {
                if (Response != null)
                    return Response.ResponseXMLIndex;
                else return -1;
            }
            set 
            { 
                ResponseIndex = value; 
            }
        }


        [XmlIgnore]
        private int responseIndex;
        [XmlIgnore]
        public int ResponseIndex
        {
            get
            {
                return responseIndex;
            }
            set { responseIndex = value; }
        }

        #region default


        [XmlIgnore]
        public string Defaults { get; set; }

        [XmlAttribute("default")]
        public string XMLDefaults
        {
            get
            {
                if (Response != null)
                    return Response.ResponseXMLDefaults;
                else return "";
            }
            set
            {
                Defaults = value;
            }
        }

        public bool ShouldSerializeXMLDefaults()
        {
            return !String.IsNullOrEmpty(XMLDefaults);
        }

        #endregion

        #region type
        [XmlAttribute("type")]
        public string XMLResponseType
        {
            get
            {
                if (Response != null)
                    return Response.ResponseXMLType;
                else return "Prompt";
            }
            set
            {  
                ResponseType = value;
            }
        }

        [XmlIgnore]
        private string responseType;
        [XmlIgnore]
        public string ResponseType
        {
            get
            {
                return responseType;
            }
            set 
            { 
                responseType = value;
            }
        }

        #endregion

        [XmlIgnore]
        private string xmlLabel;
        [XmlAttribute("label")]
        public string XMLLabel
        {
            set 
            {
                xmlLabel = value;

                if (String.IsNullOrEmpty(Label)) //returning from serialization
                {
                    int underscore = xmlLabel.IndexOf('_');

                    if (underscore > 0)
                        Label = xmlLabel.Substring(0, underscore);
                    else Label = xmlLabel;
                }
            }

            get
            {
                if (Response is MultipleChoiceList)
                {
                    string result = "";
                    foreach (string s in (Response as MultipleChoiceList).Responses.StringResponses)
                    {
                        result += String.Format("{0}_{1},", Label, Regex.Replace(s, @"\s+", ""));
                    }

                    result.Remove(result.Length - 1);
                    return result;
                }

                if (Response is DynamicGroup)
                {
                    string result = "";
                    foreach (Person p in App.Interview.People)
                    {
                        result += String.Format("{0}_{1},", Label, Regex.Replace(p.Name, @"\s+", ""));
                    }

                    if (result.Length > 0)
                        result = result.Remove(result.Length - 1);

                    return result;
                }


                else return Label;
            }
        }

        [XmlIgnore]
        public string Label { get; set; }

        [XmlIgnore]
        public Guid ID { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlIgnore]
        private ResponseBase response;
        [XmlIgnore]
        public ResponseBase Response
        {
            get { return response; }
            set
            {
                response = value;
                NotifyOfPropertyChange(() => Response);
            }
        }

        public void RefreshResponses()
        {
            NotifyOfPropertyChange(() => Response);
        }

        #region dynamic groups

        [XmlIgnore]
        public int GroupsFromQuestion { get; set; }

        [XmlAttribute("groupsFromQuestion")]
        public int XMLgroupsFromQuestion
        {
            get
            {
                if (Response is DynamicGroup)
                {
                    return App.Interview.QuestionsToIndexes[(Response as DynamicGroup).ReferenceQuestion.ID];
                }
                else return -1;
            }
            set
            {
                GroupsFromQuestion = value;
            }
        }

        public bool ShouldSerializeXMLgroupsFromQuestion()
        {
            return XMLgroupsFromQuestion != -1;
        }


        #endregion

        #region followup


        [XmlIgnore]
        public int FollowupFor { get; set; }

        [XmlAttribute("followup")]
        public int XMLFollowupFor
        {
            get
            {
                Constraint first = Constraints.FirstOrDefault();

                if (first != null)
                {
                    return App.Interview.QuestionsToIndexes[first.FollowupForGuid];
                }
                else return -1;
            }
            set
            {
                FollowupFor = value;
            }
        }

        public bool ShouldSerializeXMLFollowupFor()
        {
            return XMLFollowupFor != -1;
        }

        #endregion

        #region followupVal

        [XmlIgnore]
        public string FollowupForValue { get; set; }

        [XmlAttribute("followupVal")]
        public string XMLFollowupForValue
        {
            get
            {
                Constraint first = Constraints.FirstOrDefault();

                if (first != null)
                {
                    return (first as StringConstraint).FollowupValueIndexesAsString;                 
                }
                else return "";
            }
            set
            {
                FollowupForValue = value;
            }
        }

        public bool ShouldSerializeXMLFollowupForValue()
        {
            return !String.IsNullOrEmpty(XMLFollowupForValue);
        }

        #endregion
        
        [XmlIgnore]
        public List<Constraint> Constraints { get; set; }

        public Question()
        {
            ID = Guid.NewGuid();
            Constraints = new List<Constraint>();

            FollowupFor = -1;
            FollowupForValue = "";
        }

        [XmlIgnore]
        public bool HasConstraints
        {
            get
            {
                if (Constraints != null)
                    return Constraints.Count > 0;

                return false;
            }
        }
    }
}

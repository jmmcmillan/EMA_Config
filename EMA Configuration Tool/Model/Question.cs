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

        public string PreviewPaneText
        {
            get
            {
                string oneLine = Text.Replace('\n', ' ');
                return oneLine;
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

                    result = result.Remove(result.Length - 1);

                    return result;
                }

                if (Response is DynamicGroup)
                {
                    string result = String.Format("{0}_{1},{0}_{2},{0}_{3}", Label, "1", "2", "3");
                    
                    return result;
                }

                if (Response is SelectedResponsesFrom)
                {
                    string result = "";

                    List<string> labels = new List<string>();

                    foreach (ReferenceQuestion rq in (Response as SelectedResponsesFrom).ReferenceQuestions.Where(r => r.IsReferenced))
                    {
                        if (rq.Question.Response is ChoiceBase)
                        {
                            ChoiceBase sc = rq.Question.Response as ChoiceBase;

                            foreach (string s in sc.Responses.StringResponses)
                            {
                                string label = String.Format("{0}_{1}", Label, s);
                                if (labels.Contains(label))
                                    continue;
                                else labels.Add(label);

                            }
                        }
                    }

                    result = string.Join(",", labels);

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

        
        #region dynamic groups and selected responses from question

        [XmlIgnore]
        public List<int> BasedOnQuestions { get; set; }

        [XmlAttribute("basedOnQuestions")]
        public string XMLbasedOnQuestions
        {
            get
            {
                if (Response is BasedOnQuestions)
                {
                    BasedOnQuestions thisResponse = Response as BasedOnQuestions;

                    string result = "";

                    foreach (ReferenceQuestion rq in thisResponse.ReferenceQuestions)
                    {
                        if (rq.IsReferenced)
                        {
                            if (App.Interview.QuestionsToIndexes.Keys.Contains(rq.Question.ID))
                                result += String.Format("{0},", App.Interview.QuestionsToIndexes[rq.Question.ID]);
                        }
                    }
                    
                    if (result.Length > 0)
                        result = result.Remove(result.Length - 1);

                    return result;
                }
                else return "";
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;

                BasedOnQuestions = value.ToString().Split(',').Select(t => Int32.Parse(t)).ToList();
            }
        }

        public bool ShouldSerializeXMLbasedOnQuestions()
        {
            return !String.IsNullOrEmpty(XMLbasedOnQuestions);
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
            Text = String.Empty;
            Label = String.Empty;

            FollowupFor = -1;
            FollowupForValue = "";

            BasedOnQuestions = new List<int>();
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

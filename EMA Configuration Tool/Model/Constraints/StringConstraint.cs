using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Responses;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model.Constraints
{
    public class StringConstraint : Constraint
    {
        [XmlIgnore]
        public string FollowupValueLabels
        {
            get
            {
                string result = String.Empty;

                int i = 0;

                foreach (string s in (relevantQuestion.Response as StringChoice).Responses.StringResponses)
                {
                    if (FollowupValueIndexes.Contains(i))
                        result += String.Format("{0}, ", s);

                    i++;
                }

                result.Remove(result.LastIndexOf(','));

                return result;
            }
        }

        [XmlIgnore]
        private string xmlContent;
        [XmlIgnore]
        public string XMLContent
        {
            set
            {
                xmlContent = value;

                if (FollowupValueIndexes == null) //returning from serialization
                {
                    
                }
            }
            get
            {
                xmlContent = IndexValuesToString();
                return xmlContent;
            }
        }

        [XmlIgnore]
        private List<int> followupValueIndexes;
        public List<int> FollowupValueIndexes 
        {
            get { return followupValueIndexes; }
            set
            {
                followupValueIndexes = value;
                FollowupValueIndexesAsString = IndexValuesToString();
            }
        }
        
        public string FollowupValueIndexesAsString
        {
            get;
            set;
        }

        private List<int> IndexStringsToInts(string content)
        {
            List<int> indices = new List<int>();

            foreach (string s in content.Split(','))
                indices.Add(Int32.Parse(s));

            return indices;
        }

        private string IndexValuesToString()
        {
            string result = String.Empty;

            int i = 0;
            int responseCount = FollowupValueIndexes.Count;

            foreach (int j in FollowupValueIndexes)
            {
                result += j.ToString();

                if (i < responseCount - 1)
                    result += ",";

                i++;
            }

            return result;
        }

        public StringConstraint() : base()
        {
        }

        public StringConstraint(Guid id, List<int> indices) : this()
        {
            FollowupValueIndexes = indices;
            FollowupForGuid = id;
        }

        public StringConstraint(Guid id, string indices)
            : this()
        {
            FollowupForGuid = id;
            FollowupValueIndexes = IndexStringsToInts(indices);

        }
    }
}

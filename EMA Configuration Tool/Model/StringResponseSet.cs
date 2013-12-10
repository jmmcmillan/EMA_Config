using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model
{
   
    public class StringResponseSet
    {
        [XmlIgnore]
        public Guid ResponseSetID { get; set; }

        [XmlIgnore]
        public List<string> Responses { get; set; }

        [XmlIgnore]
        public bool IsZeroBased { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }

        public StringResponseSet() : base()
        {
            ResponseSetID = Guid.NewGuid();
        }

        public StringResponseSet(Guid id, List<string> responses) : this()
        {
            ResponseSetID = id;
            Responses = responses;
        }

        [XmlIgnore]
        public string xmlContent;

        [XmlText]
        public string XMLContent
        {
            set 
            { 
                xmlContent = value;

                if (Responses == null) //returning from serialization
                {
                    foreach (string s in xmlContent.Split('\n'))
                        Responses.Add(s);
                }
            }
            get
            {
                xmlContent = this.ToString();
                return xmlContent;
            }
        }

        [XmlIgnore]
        public string Description
        {
            get
            {
                string result = String.Empty;

                int score = (IsZeroBased) ? 0 : 1;  

                foreach (string s in Responses)
                {
                    result += String.Format("{0} ({1}), ", s, score);
                    score++;
                }

                result.Remove(result.Length - 3,2); //strip last comma and space

                return result;
            }
        }

        public override string ToString()
        {
            string result = String.Empty;

            int i = 0;
            int responseCount = Responses.Count;

            foreach (string s in Responses)
            {
                result += s;

                if (i < responseCount - 1)
                    result += ",";

                i++;
            }

            return result;
        }
    }
}

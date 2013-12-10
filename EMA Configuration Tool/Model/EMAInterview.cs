using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model
{
    [XmlRoot("interview")]
    public class EMAInterview : PropertyChangedBase
    {
        [XmlIgnore]
        private ObservableCollection<StringResponseSet> stringResponseSets;


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
        public List<Constraint> Constraints { get; set; }
        
        [XmlIgnore]
        public List<Question> Questions { get; set; }

        [XmlIgnore]
        public Dictionary<Guid, int> ResponseSetsToIndexes;

        public void PrepareForSerialization()
        {
            setResponseSetIndices();
        }

        private void setResponseSetIndices()
        {
            ResponseSetsToIndexes = new Dictionary<Guid, int>();

            int i = 0;
            foreach (StringResponseSet sts in StringResponseSets)
            {
                ResponseSetsToIndexes.Add(sts.ResponseSetID, i);
                sts.Index = i;

                i++;
            }
        }


        public EMAInterview()
        {
            Questions = new List<Question>();            
            Constraints = new List<Constraint>();
            StringResponseSets = new ObservableCollection<StringResponseSet>();
        }
    }
}

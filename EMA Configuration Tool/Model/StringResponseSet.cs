﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model
{
   
    public class StringResponseSet : PropertyChangedBase
    {
        [XmlIgnore]
        public Guid ID { get; set; }

        [XmlIgnore]
        private ObservableCollection<string> stringResponses;
        [XmlIgnore]
        public ObservableCollection<string> StringResponses
        {
            get { return stringResponses; }
            set
            {
                stringResponses = value;
                NotifyOfPropertyChange(() => stringResponses);
            }
        }

       

        [XmlAttribute("startsWithZero")]
        public bool IsZeroBased { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }

        public StringResponseSet() : base()
        {
            ID = Guid.NewGuid();
        }

        public StringResponseSet(Guid id, List<string> responses) : this()
        {
            ID = id;

            StringResponses = new ObservableCollection<string>();
            foreach (String s in responses)
                StringResponses.Add(s);
        }

        [XmlIgnore]
        public string xmlContent;

        [XmlText]
        public string XMLContent
        {
            set 
            { 
                xmlContent = value;

                if (StringResponses == null) //returning from serialization
                {
                    StringResponses = new ObservableCollection<string>();

                    foreach (string s in xmlContent.Split('|'))
                        StringResponses.Add(s);

                }
            }
            get
            {
                string xmlContent = String.Empty;

                foreach (string s in StringResponses)
                {
                    xmlContent += String.Format("{0}|", s);
                }

                if (StringResponses.Count() > 0)
                    xmlContent = xmlContent.Remove(xmlContent.Length - 1);  //strip out last pipe

               
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

                foreach (string s in StringResponses)
                {
                    result += String.Format("{0} ({1}), ", s, score);
                    score++;
                }

                result = result.Remove(result.Length - 2); //strip last comma and space

                return result;
            }
        }

        public override string ToString()
        {
            string result = String.Empty;

            int i = 0;
            int responseCount = StringResponses.Count;

            foreach (string s in StringResponses)
            {
                result += String.Format("{0}", s);

                if (i < responseCount - 2)
                    result += ", ";

                i++;
            }

            return result;
        }
    }
}

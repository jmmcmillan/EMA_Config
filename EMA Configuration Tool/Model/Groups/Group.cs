using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model.Groups
{
    public class Group
    {
        [XmlAttribute("name")]
        public string GroupName { get; set; }

        
        [XmlText]
        public string XMLNames
        {
            get
            {
                string result = string.Join("|", Names);
                return result;
            }

            set
            {
                Names = value.Split('|').ToList();

                if (Names.Count == 1)
                    Names = value.Split(',').ToList();
            }
        }



        [XmlIgnore]
        public List<string> Names { get; set; }

        public Group()
        {
            Names = new List<string>();
        }

        public Group(string name)
            : this()
        {
            GroupName = name;
        }
    }
}

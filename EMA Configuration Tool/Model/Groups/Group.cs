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

        [XmlAttribute("isCustom")]
        public bool IsCustom { get; set; }

        [XmlText]
        public string XMLNames
        {
            get
            {
                string result = "";

                foreach (string name in Names)
                {
                    result += String.Format("{0},", name);
                }

                int comma = result.IndexOf(',');
                if (comma > 0)
                    result = result.Remove(result.Length - 1);

                return result;
            }

            set
            {
                Names = value.Split(',').ToList();
            }
        }



        [XmlIgnore]
        public List<string> Names { get; set; }

        [XmlIgnore]
        public string Label { get; set; }

        public Group()
        {
            Names = new List<string>();
            IsCustom = false;
        }

        public Group(string name)
            : this()
        {
            Label = name;
        }
    }
}

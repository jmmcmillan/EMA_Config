using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace EMA_Configuration_Tool.Model.Help
{
    [XmlRoot("helpContent")]
    public class HelpContent
    {
       // [XmlArray(ElementName = "helpContent")]
        [XmlElement("page")]        
        public ObservableCollection<HelpPage> HelpPages { get; set; }

        public HelpContent()
        {
            HelpPages = new ObservableCollection<HelpPage>();
        }
        
    }
}

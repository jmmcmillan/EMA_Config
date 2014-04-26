using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model.Help
{
    [XmlRoot("page")]
    public class HelpPage : PropertyChangedBase
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlText]
        public string Text { get; set; }

        //[XmlIgnore]
        //private string text;
        //[XmlText]
        //public string Text {
        //    get { return text; }
        //    set
        //    {
        //        text = value;
        //        NotifyOfPropertyChange(() => Text);
        //    }
        //}


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;
using System.Xml.Serialization;
using System.IO;

namespace EMA_Configuration_Tool
{
    public class RootViewModel : Conductor<object>
    {
        public RootViewModel()
        {
            this.DisplayName = "EMA Configuration Tool";
        }

        public void NewInterview()
        {
            App.Interview = new EMAInterview();
            ActivateItem(new ContentViews.ContentShellViewModel());
        }

        public void SaveInterview()
        {
            App.Interview.PrepareForSerialization();

            XmlSerializer serializer = new XmlSerializer(typeof(EMAInterview));
            TextWriter textWriter = new StreamWriter(@"C:\Projects\EMA Configuration Tool\EMA Configuration Tool\test\test.xml");
            serializer.Serialize(textWriter, App.Interview);
            textWriter.Close();
        }

        public void LoadInterview()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(EMAInterview));
            TextReader textReader = new StreamReader(@"C:\Projects\EMA Configuration Tool\EMA Configuration Tool\test.xml");
            App.Interview = (EMAInterview)deserializer.Deserialize(textReader);
            textReader.Close();

        }


    }
}

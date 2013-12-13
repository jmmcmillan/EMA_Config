using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;
using System.Xml.Serialization;
using System.IO;
using EMA_Configuration_Tool.ContentViews;
using EMA_Configuration_Tool.Groups;
using Microsoft.Win32;
using EMA_Configuration_Tool.GroupViews;

namespace EMA_Configuration_Tool
{
    public class RootViewModel : Conductor<object>
    {
       

        public ContentShellViewModel ContentDisplay { get; set; }
        public GroupShellViewModel PeopleDisplay { get; set; }
        public bool HasContent { get; set; }

        public RootViewModel()
        {
            this.DisplayName = "EMA Configuration Tool";
            HasContent = false;
           
        }


        public void Exit()
        {
            App.Current.Shutdown();

        }

        

        public void NewInterview()
        {
            App.Interview = new EMAInterview();

            initAll();
         
        }

        public void SaveInterview()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            if ((bool)sfd.ShowDialog())
            {

                App.Interview.PrepareForSerialization();

                try
                {  
                    XmlSerializer serializer = new XmlSerializer(typeof(EMAInterview));
                    TextWriter textWriter = new StreamWriter(sfd.FileName);
                    serializer.Serialize(textWriter, App.Interview);
                    textWriter.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void LoadInterview()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if ((bool)ofd.ShowDialog())
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(EMAInterview));
                TextReader textReader = new StreamReader(ofd.FileName);
                App.Interview = (EMAInterview)deserializer.Deserialize(textReader);
                textReader.Close();

                App.Interview.RecoverFromSerialization();

                initAll();
            }
        }

        private void initAll()
        {
            HasContent = true;
            NotifyOfPropertyChange(() => HasContent);

            ContentDisplay = new ContentShellViewModel();
            NotifyOfPropertyChange(() => ContentDisplay);

            PeopleDisplay = new GroupShellViewModel();
            NotifyOfPropertyChange(() => PeopleDisplay);
        }


    }
}

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
using EMA_Configuration_Tool.SettingViews;
using EMA_Configuration_Tool.Model.Adapters;

namespace EMA_Configuration_Tool
{
    public class RootViewModel : Conductor<object>
    {
       

        public ContentShellViewModel ContentDisplay { get; set; }
        public GroupShellViewModel PeopleDisplay { get; set; }
        public SettingsViewModel SettingsDisplay { get; set; }


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
            sfd.DefaultExt = ".xml";
            sfd.Filter = "EMA Interview (*.xml)|*.xml";
            sfd.Title = "Save EMA Interview";

            if ((bool)sfd.ShowDialog())
            {
                App.SerializeInterview(sfd.FileName);

                if (App.Interview.OutputSalivaScreens)
                    SaveInterviewWithAdapter(sfd.FileName);
            }
        }

        private void SaveInterviewWithAdapter(string originalFileName)
        {
            SalivaContentAdapter salivaQuestions = new SalivaContentAdapter();

            salivaQuestions.AdaptInterview();

            string fileName = Path.GetFileNameWithoutExtension(originalFileName);
            string directory = Path.GetDirectoryName(originalFileName);
            string newFileName = Path.Combine(directory, String.Format("{0}_saliva.xml", fileName));

            App.SerializeInterview(newFileName);

            salivaQuestions.RevertInterview();

        }

        public void LoadInterview()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Filter = "EMA Interview (*.xml)|*.xml";
            ofd.Title = "Open Existing EMA Interview";

            if ((bool)ofd.ShowDialog())
            {
                App.DeserializeInterview(ofd.FileName);
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

            SettingsDisplay = new SettingsViewModel();
            NotifyOfPropertyChange(() => SettingsDisplay);
        }


    }
}

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

            if ((bool)sfd.ShowDialog())
            {
                App.SerializeInterview(sfd.FileName);                
            }
        }

        public void LoadInterview()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";

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

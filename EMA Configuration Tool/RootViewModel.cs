﻿using System;
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
using Avalon.Windows.Dialogs;
using System.Windows;

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
            if (MessageBox.Show("Do you want to exit the configuration tool? All unsaved changes will be lost.", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            App.Current.Shutdown();

        }

        

        public void NewInterview()
        {
            App.Interview = new EMAInterview();

            initAll();
         
        }

        private bool canSave()
        {
            if (String.IsNullOrEmpty(App.Interview.ParticipantID))
            {
                string id = Microsoft.VisualBasic.Interaction.InputBox("Please enter a Participant ID", "Participant ID", string.Empty);

                if (String.IsNullOrEmpty(id))
                    return false;

                else
                {
                    App.Interview.ParticipantID = id;
                    App.Interview.Refresh();
                }
            }

            return true;
        }

        private bool successfullyAccessedDirectory(string participantFolder)
        {
            try
            {
                if (!Directory.Exists(participantFolder))
                    Directory.CreateDirectory(participantFolder);

                string participantBackupConfigFolder = Path.Combine(participantFolder, "backup");
                if (!Directory.Exists(participantBackupConfigFolder))
                    Directory.CreateDirectory(participantBackupConfigFolder);

                //create a blank file to try deleting in case there aren't any files in this directory yet
                string path = Path.Combine(participantFolder, "test.txt");
                FileStream fs = File.Create(path);
                fs.Close();

                DirectoryInfo di = new DirectoryInfo(participantFolder);
                foreach (FileInfo oldContent in di.GetFiles())
                {
                    //delete the test file
                    if (oldContent.Extension == ".txt")
                        oldContent.Delete();

                    //move previous xml files into the backup folder
                    else
                    {
                        string newFileName = String.Format("{0} {1:MM dd yy - hh mm ss}.xml", oldContent.Name, DateTime.Now);
                        oldContent.MoveTo(Path.Combine(participantBackupConfigFolder, newFileName));
                    }

                }             
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not access this directory. Please choose another place to save the configuration files.",
                    "Couldn't acccess directory", MessageBoxButton.OK, MessageBoxImage.Warning);

                Console.Write(ex.ToString());

                return false;
            }

            return true;
        }

        private string getConfigDirectory()
        {
            FolderBrowserDialog getFolder = new FolderBrowserDialog();
            getFolder.Title = "Save EMA Configuration Location";

            string fileFolder = String.Empty;

            if (getFolder.ShowDialog() == true)
            {
                fileFolder = getFolder.SelectedPath;
                string participantFolder = Path.Combine(fileFolder, App.Interview.ParticipantID + "_config");

                if (Directory.Exists(participantFolder))
                {
                    if (MessageBox.Show(String.Format("This will overwrite previous configuration information for participant {0}. Do you want to continue?", App.Interview.ParticipantID),
                        "Overwrite Previous Configuration", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return String.Empty;
                    }                    
                }

                if (successfullyAccessedDirectory(participantFolder))
                    return participantFolder;
            }

            return String.Empty;

        }

        public void SaveInterview()
        {
            if (!canSave())
                return;

            string saveDirectory = getConfigDirectory();

            if (String.IsNullOrEmpty(saveDirectory))
                return;

            string fileName = String.Format("{0}_hourly.xml", App.Interview.ParticipantID);
            string fullPath = Path.Combine(saveDirectory, fileName);

            App.SerializeInterview(fullPath);

            if (App.Interview.OutputSalivaScreens)
                SaveInterviewWithAdapter(fullPath);
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

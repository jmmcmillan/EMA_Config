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
            App.Network = new SocialNetwork();

            initAll();
         
        }
        

        private bool canSave()
        {
            if (App.Interview == null)
                return false;

            string id = Microsoft.VisualBasic.Interaction.InputBox("Please confirm the Participant ID", "Participant ID", App.Interview.ParticipantID);

            if (String.IsNullOrEmpty(App.Interview.EMAType.ToString()))
            {
                MessageBox.Show("The interview must have a type specified. Please select the InterviewType on the Settings tab.", "Set Interview Type", MessageBoxButton.OK);
                return false;
            }

            if (String.IsNullOrEmpty(id))
            {
                MessageBox.Show("The Participant ID cannot be blank.", "Enter Participant ID", MessageBoxButton.OK);
                return false;
            }

            else
            {
                App.Interview.ParticipantID = id;
                App.Interview.Refresh();
            }

            return true;
        }

        private bool successfullyAccessedDirectory(string participantFolder)
        {
            try
            {
                if (!Directory.Exists(participantFolder))
                    Directory.CreateDirectory(participantFolder);

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

                string configFolder = Path.Combine(fileFolder, App.Interview.ParticipantID + "_config");

                if (Directory.Exists(configFolder))
                {
                    if (MessageBox.Show(String.Format("This could overwrite previous configuration information for participant {0}. Do you want to continue?", App.Interview.ParticipantID),
                        "Overwrite Previous Configuration", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        return String.Empty;
                    }                    
                }

                if (successfullyAccessedDirectory(configFolder))
                    return configFolder;
            }

            return String.Empty;

        }

        private bool backedUpOldFiles(string participantFolder, string fileName)
        {
            try
            {
                string backupDirectory = Path.Combine(participantFolder, "backup");
                if (!Directory.Exists(backupDirectory))
                    Directory.CreateDirectory(backupDirectory);

                string interviewType = fileName.Substring(0, fileName.IndexOf(".xml"));

                DirectoryInfo di = new DirectoryInfo(participantFolder);
                foreach (FileInfo oldContent in di.GetFiles())
                {
                    if (oldContent.Name.StartsWith(interviewType))  //want to remove any adapted interviews in addition to the base interview (e.g., BOD_saliva.xml as well as BOD.xml)
                    {
                        string newFileName = String.Format("{0} {1:MM-dd-yy hh-mm-ss}.xml", oldContent.Name, DateTime.Now);
                        oldContent.MoveTo(Path.Combine(backupDirectory, newFileName));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Participant folder: {0}, File name: {1}", participantFolder, fileName));
                Console.Write(ex.ToString());
                return false;
            }

        }

        public void SaveInterview()
        {
            if (!canSave())
                return;

            string configDirectory = getConfigDirectory();

            if (String.IsNullOrEmpty(configDirectory))
                return;
                        
            string interviewFileName = String.Format("{0}.xml", App.Interview.EMAType);
            string interviewFullPath = Path.Combine(configDirectory, interviewFileName);

            if (!backedUpOldFiles(configDirectory, interviewFileName))
            {
                MessageBox.Show("Could not back up old files - NOT writing new content.", "Failed to back up old files", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            App.AdapterKnowledge = new PreexistingKnowledge();

            //save interview content
            App.SerializeInterview(interviewFullPath);

            //save interview with adapters
            SaveInterviewWithAdapters(interviewFullPath);

            //save social network             
            string socialNetworkFullPath = Path.Combine(configDirectory, "SocialNetwork.xml");

            App.SerializeSocialNetwork(socialNetworkFullPath);
        }


        private void SaveInterviewWithAdapters(string originalFilePath)
        {
            string directory = Path.GetDirectoryName(originalFilePath);
            string originalFileName = Path.GetFileNameWithoutExtension(originalFilePath);

            List<List<AdapterBase>> adapaterLists = App.AdapterKnowledge.GetAdapterListsFor(App.Interview.EMAType);

            foreach (List<AdapterBase> adapters in adapaterLists)
            {
                string newFileName = originalFileName;

                //add content from the adapters
                foreach (AdapterBase newContent in adapters)
                {
                    newContent.AdaptInterview();
                    newFileName += newContent.FileSuffix;
                }

                //save adapted interview
                string newInterviewPath = Path.Combine(directory, String.Format("{0}.xml", newFileName));
                App.SerializeInterview(newInterviewPath);

                //remove content from the adapaters
                foreach (AdapterBase newContent in adapters)
                {
                    newContent.RevertInterview();                    
                }
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
                if (!App.DeserializeSocialNetwork(ofd.FileName))
                {
                    App.Network = new SocialNetwork();
                }

                if (App.DeserializeInterview(ofd.FileName))
                {
                    //get participant ID from parent folder
                    DirectoryInfo di = new DirectoryInfo(ofd.FileName).Parent;

                    int underscoreIndex = di.Name.LastIndexOf('_');
                    if (underscoreIndex > -1)
                    {
                        App.Interview.ParticipantID = di.Name.Substring(0, underscoreIndex);
                        Console.WriteLine("Setting Particiapnt ID to " + App.Interview.ParticipantID);
                    }
                    else App.Interview.ParticipantID = String.Empty;

                    initAll();
                }
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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using EMA_Configuration_Tool.Model.Adapters;

namespace EMA_Configuration_Tool
{
    public enum InterviewType { Hourly, BOD, EOD, HalfHour };

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EMAInterview Interview {get; set;}
        public static SocialNetwork Network { get; set; }
        
        public App()
            : base()
        {
           
          
        }

        private static EventAggregator eventAggregator;
        public static EventAggregator EventAggregator
        {
            get
            {
                if (eventAggregator == null)
                    eventAggregator = new EventAggregator();

                return eventAggregator;
            }
        }

        private static PreexistingKnowledge adapterKnowledge;
        public static PreexistingKnowledge AdapterKnowledge
        {
            get
            {
                if (adapterKnowledge == null)
                    adapterKnowledge = new PreexistingKnowledge();

                return adapterKnowledge;
            }
            set { adapterKnowledge = value; }
        }

        public static bool DeserializeInterview(string fileName)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(EMAInterview));
                TextReader textReader = new StreamReader(fileName);
                App.Interview = (EMAInterview)deserializer.Deserialize(textReader);
                textReader.Close();

                Interview.RecoverFromSerialization();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("There was a problem opening the interview. Check that the correct file was selected.", "Error Opening Interview", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public static void SerializeInterview(string fileName)
        {
            Interview.PrepareForSerialization();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EMAInterview));
                TextWriter textWriter = new StreamWriter(fileName);
                serializer.Serialize(textWriter, Interview);
                textWriter.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SerializeSocialNetwork(string fileName)
        {
            Network.PrepareForSerialization();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SocialNetwork));
                TextWriter textWriter = new StreamWriter(fileName);
                serializer.Serialize(textWriter, Network);
                textWriter.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public static bool DeserializeSocialNetwork(string fileName)
        {
            string selectedFile = Path.GetFileName(fileName);
            string selectedFolder = Path.GetDirectoryName(fileName);

            //check whether we have a social network file for this participant            
            string socialNetworkPath = Path.Combine(selectedFolder, "SocialNetwork.xml");
            if (!File.Exists(socialNetworkPath))
                return false;

            //deserialize the file
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(SocialNetwork));
                TextReader textReader = new StreamReader(socialNetworkPath);
                App.Network = (SocialNetwork)deserializer.Deserialize(textReader);
                textReader.Close();

                Network.RecoverFromSerialization();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("There was a problem opening the social network. Check that the correct interview file was selected.", "Error Opening Social Network", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }

            return true;
        }

    }
}

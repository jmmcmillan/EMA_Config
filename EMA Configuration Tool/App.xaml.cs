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

namespace EMA_Configuration_Tool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EMAInterview Interview {get; set;}
        
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
     
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;

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

     
    }
}

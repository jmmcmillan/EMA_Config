using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.SettingViews
{
    public class SettingsViewModel
    {
        private string timeoutInMinutes;
        public string TimeoutInMinutes 
        {
            get { return timeoutInMinutes; }
            set
            {
                int to;
                bool parseOK = Int32.TryParse(value, out to);

                if (parseOK)
                {
                    timeoutInMinutes = value;
                    App.Interview.Timeout = to * 60 * 1000;
                }
                else
                {
                    timeoutInMinutes = "";
                    //System.Windows.MessageBox.Show(String.Format("{0} is not a valid timeout value. Please enter an integer.", value));
                }
            }
        }

        public List<string> InterviewTypes { get; set; }
        public int InterviewTypeSelectedIndex { get; set; }

        public SettingsViewModel()
        {
            timeoutInMinutes = String.Format("{0}", (App.Interview.Timeout / 1000) / 60);

            InterviewTypes = new List<string>() { "Hourly", "BOD", "EOD"} ;

            InterviewTypeSelectedIndex = InterviewTypes.IndexOf(App.Interview.InterviewType);
        }

        
        

    }
}

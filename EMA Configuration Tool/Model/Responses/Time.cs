using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Time : ResponseBase
    {
        public override string ResponseXMLType
        { get { return "Time"; } }


        public override string ResponseXMLDefaults
        {
            get
            {
                return TimeStringToIntString(DefaultTime);
            }
            set
            {
                int totalMinutes;
                bool parseOK = Int32.TryParse(value, out totalMinutes);

                if (parseOK)
                    DefaultTime = TimeIntToString(totalMinutes);
            }
        }

        public static string TimeIntToString(int i)
        {
            int hour = i / 60;
            int minutes = i - (hour * 60);
            string suffix = "am";

            if (hour > 12)
            {
                hour = hour - 12;
                suffix = "pm";
            }

            return String.Format("{0}:{1:d2} {2}", hour, minutes, suffix);
        }

        public static string TimeStringToIntString(string dt)
        {
                DateTime datetime;
                bool parseOK;
                parseOK = DateTime.TryParse(dt, out datetime);

                if (!parseOK)
                    return "";
                else return String.Format("{0}", (datetime.Hour * 60) + datetime.Minute);

         
        }

        public override bool DefaultIsValid
        {
            get
            {
                DateTime datetime;
                return DateTime.TryParse(DefaultTime, out datetime);
            }
        }
      

        public string DefaultTime { get; set; }


        public Time()
            : base()
        {
            DefaultTime = "";
        }

    }
}

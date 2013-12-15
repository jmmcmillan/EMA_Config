using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model.Groups
{
    public class Person : PropertyChangedBase
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public bool[] GroupMembership
        {
            get;
            set;
        }

        public string Groups
        {
            get
            {
                string result = "";

                int i = 0;
                foreach (bool isMember in GroupMembership)
                {
                    if (isMember)
                        result += String.Format("{0},",EMAInterview.SocialGroupNames[i]);

                    i++;
                }

                if (result.Length > 0)
                    result = result.Remove(result.Length - 1);

                return result;
            }
        }

        public Person()
        {
            ID = Guid.NewGuid();

            GroupMembership = new bool[EMAInterview.SocialGroupNames.Count()];
           
            //foreach (string s in EMAInterview.SocialGroupNames)
            //{
            //    GroupMembership[i] = false;
            //    i++;
            //}
            
        }
    }
}


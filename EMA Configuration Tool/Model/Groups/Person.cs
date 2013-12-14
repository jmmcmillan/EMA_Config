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
        public string Name { get; set; }

     
  
        public ObservableCollection<PersonGroup> GroupsForBinding { get; set; }

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
            GroupMembership = new bool[EMAInterview.SocialGroupNames.Count()];

            GroupsForBinding = new ObservableCollection<PersonGroup>();

            int i = 0;
            foreach (string s in EMAInterview.SocialGroupNames)
            {
                GroupMembership[i] = false;
                GroupsForBinding.Add(new PersonGroup() { IsMember = false });

                i++;
            }
               

            GroupMembership[5] = true;
            
        }
    }
}


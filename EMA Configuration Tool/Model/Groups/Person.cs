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

        public List<Group> MyGroups { get; set; }

        public string GroupsString
        {
            get
            {
                string result = string.Join(", ", MyGroups.Select(g => g.GroupName));
                return result;
            }
        }

        public Person()
        {
            ID = Guid.NewGuid();
            MyGroups = new List<Group>();
        }
    }
}


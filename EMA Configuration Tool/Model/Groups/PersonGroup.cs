using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model.Groups
{
    public class PersonGroup : PropertyChangedBase
    {

        public string GroupName { get; set; }
        public bool IsMember { get; set; }

    }
        
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model.Groups
{
    public class BindableBool : PropertyChangedBase
    {
        public bool Value { get; set; }

        public BindableBool(bool b)
        {
            Value = b;
        }
    }
}

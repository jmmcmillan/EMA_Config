using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Time : ResponseBase
    {
        public TimeSpan DefaultTime { get; set; }
        public bool IsPM { get; set; }

    }
}

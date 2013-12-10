using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public abstract class StringChoice : ResponseBase
    {
        public StringResponseSet Responses { get; set; }
        public string Description { get; set; }

    }
}

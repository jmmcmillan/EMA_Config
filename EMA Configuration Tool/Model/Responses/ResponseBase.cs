using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public abstract class ResponseBase
    {
        public bool HasDefaults { get; set; }
        public bool IsExclusive { get; set; }
    }
}

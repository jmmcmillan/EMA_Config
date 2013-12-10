using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Responses;

namespace EMA_Configuration_Tool.Model
{
    public class Question
    {
        public string Text { get; set; }
        public string ID { get; set; }
        public ResponseBase Response { get; set; }
        public List<Constraint> Constraints { get; set; }
    }
}

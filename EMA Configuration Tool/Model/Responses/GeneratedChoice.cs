using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public abstract class GeneratedChoice : ChoiceBase
    {
        

        public GeneratedChoice()
        {
            Responses = new StringResponseSet();
        }
    }
}

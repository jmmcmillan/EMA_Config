using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public abstract class StringChoice : ResponseBase
    {
        
        
        private StringResponseSet responses { get; set; }
        public StringResponseSet Responses
        {
            get { return responses; }
            set
            {
                responses = value;
                NotifyOfPropertyChange(() => Responses);
            }
        }

        
        
        
        public string Description { get; set; }

        public StringChoice()
        {
            Responses = new StringResponseSet();
        }

    }
}

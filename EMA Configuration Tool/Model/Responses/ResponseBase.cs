using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model.Responses
{
    public abstract class ResponseBase : PropertyChangedBase
    {
        
        public virtual ResponseBase Copy() 
        {
            return null;
        }

        public virtual string ResponseXMLType { get; set; }
        public virtual string ResponseXMLDefaults { get; set; }

        public virtual string ExclusiveOption { get; set; }
        public virtual int XMLExclusiveOption { get; set; }
        
        public int ResponseXMLIndex
        {
            get
            {
                if (this is StringChoice)
                {
                    if (App.Interview.ResponseSetsToIndexes.Keys.Contains((this as StringChoice).Responses.ID))
                        return App.Interview.ResponseSetsToIndexes[(this as StringChoice).Responses.ID];
                    else return -1;
                }

                else return -1;
            }
        }
    }
}

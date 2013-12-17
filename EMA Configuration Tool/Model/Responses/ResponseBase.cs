using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model.Responses
{
    public abstract class ResponseBase : PropertyChangedBase
    {
        public virtual ResponseBase Copy() { return null; }
        public virtual string ResponseXMLType { get; set; }
        public virtual string ResponseXMLDefaults { get; set; }
        public bool IsExclusive { get; set; }

        //private bool HasDefaults { get; set; }
        //public virtual bool DefaultIsValid { get { return true; } }

        public int ResponseXMLIndex
        {
            get
            {
                if (this is StringChoice)
                {
                    return App.Interview.ResponseSetsToIndexes[(this as StringChoice).Responses.ID];
                }

                else return -1;
            }
        }
    }
}

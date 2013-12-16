using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class Integer : ResponseBase
    {
        public override string ResponseXMLType
        { get { return "Integer"; } }

        
        public override string ResponseXMLDefaults
        { 
            get
            {
                int integer;
                bool parseOK;
                parseOK = Int32.TryParse(DefaultInteger, out integer);

                if (!parseOK)
                    return "";
                else return DefaultInteger;
                
            }
            set
            {
                DefaultInteger = value; 
            }
        }

        public override bool DefaultIsValid
        {
            get
            {
                int integer;
                return Int32.TryParse(DefaultInteger, out integer);
            }
        }
        public string DefaultInteger { get; set; }

        public Integer() : base()
        {
            DefaultInteger = "";
        }

        public override ResponseBase Copy()
        {
            Integer ir = new Integer();
            ir.DefaultInteger = DefaultInteger;
            return ir;
        }
    }
}

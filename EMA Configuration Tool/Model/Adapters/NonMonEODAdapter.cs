using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class NonMonEODAdapter : TailAdapterBase
    {

        public override string FileSuffix
        {
            get { return "_nm"; }
        }

        

        public override string FriendlyName
        {
            get {  return "Non-Monitoring End of Day Interview";  }
        }

        public override string FinalPromptLabel { get { return "nmeodk1we"; } }

      
        public NonMonEODAdapter() : base()
        {   
            FinalPrompt.Text = "1. Take off Actical from your belt. Press the * button.\n\n2. Please keep the Actiwatch on.\n\n3. Please plug the ED in NOW so that it may charge overnight!";
           
        }
    }
}

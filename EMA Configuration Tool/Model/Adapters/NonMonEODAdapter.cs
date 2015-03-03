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
            FinalPrompt.Text = "1. Please plug in the ED phone NOW so that it may charge overnight!! Place ED near your bed so you can hear the morning alarm.\n\n2. After the morning alarm, follow the instructions on ED for tomorrow's schedule.\n\n3. Please keep the Actiwatch on.";
           
        }
    }
}

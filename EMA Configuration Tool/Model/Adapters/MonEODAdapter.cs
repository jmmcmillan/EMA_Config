using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class MonEODAdapter : TailAdapterBase
    {
        public override string FileSuffix
        {
            get { return "_m"; }
        }

       

        public override string FriendlyName
        {
            get { return "Monitoring End of Day Interview"; }
        }

        public override string FinalPromptLabel { get { return "meodk1we"; } }

        

        public MonEODAdapter() : base()
        {
            FinalPrompt.Text = "1. Please plug in the ED phone NOW so that it may charge overnight!!\n\n2. Take the belt with the Actical off.\n\n3. Please keep the BP monitor on.\n\n4. Please keep the Actiwatch on.";
        
        }
    }
}

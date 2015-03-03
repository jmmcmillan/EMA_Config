using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class NonMonBODAdapter : TailAdapterBase
    {
        public override string FileSuffix
        {
            get { return "_nm"; }
        }

        public override string FriendlyName
        {
            get { return "Non-Monitoring Beginning of Day Interview"; }
        }

        public override string FinalPromptLabel { get { return "nmbodk1we"; } }

        public NonMonBODAdapter() : base()
        {
            FinalPrompt.Text = "Interview completed. Thank you!\n\nReminder: Please don't turn off the ED phone!!";
        }
    }
}

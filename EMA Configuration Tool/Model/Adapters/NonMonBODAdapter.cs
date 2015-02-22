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
            FinalPrompt.Text = "Today is not a full monitoring day- do not wear your Oscar or take your ED with you today.\n\nPlease continue to wear your Actical and Actiwatch.\n\nPlease place your ED by your bed as a reminder to complete an End of Day questionnaire tonight.";
        }
    }
}

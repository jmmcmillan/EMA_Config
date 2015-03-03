using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class FinalDayAdapter : TailAdapterBase
    {
        public override string FileSuffix
        {
            get { return "_final"; }
        }

        public override string FriendlyName
        {
            get { return "Final Interview"; }
        }

        public override string FinalPromptLabel { get { return "finalk1we"; } }

        public FinalDayAdapter() : base()
        {
            FinalPrompt.Text = "Interview completed. Thank you!\n\nReminder: Please don't turn off the ED phone!!";
        }
    }
}

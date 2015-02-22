using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Responses;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public abstract class TailAdapterBase : AdapterBase
    {
        public abstract override string FileSuffix { get; }

        public abstract string FriendlyName { get; }

        public abstract string FinalPromptLabel { get; }

        public Question FinalPrompt { get; set; }

        protected TailAdapterBase() : base()
        {
            FinalPrompt = new Question();
            FinalPrompt.Label = FinalPromptLabel;

            PostQuestions.Add(FinalPrompt);
        }
    }
}

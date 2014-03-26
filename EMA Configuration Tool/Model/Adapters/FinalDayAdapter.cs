using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class FinalDayAdapter : AdapterBase
    {
        public override string FileSuffix
        {
            get { return "_final"; }
        }


        public FinalDayAdapter()
        {
            Question finalPrompt = new Question();
            finalPrompt.Label = "p";
            finalPrompt.Text = "Thank you! You have completed the study.";
            finalPrompt.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            PostQuestions.Add(finalPrompt);
        }
    }
}

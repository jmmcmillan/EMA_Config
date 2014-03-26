using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class NonMonBODAdapter : AdapterBase
    {
        public override string FileSuffix
        {
            get { return "_nm"; }
        }


        public NonMonBODAdapter()
        {
            Question finalPrompt = new Question();
            finalPrompt.Label = "p";
            finalPrompt.Text = "Today is not a full monitoring day- do not wear your Oscar or take your ED with you today.\n\nPlease continue to wear your Actical and Actiwatch.\n\nPlease place your ED by your bed as a reminder to complete an End of Day questionnaire tonight.";
            finalPrompt.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            PostQuestions.Add(finalPrompt);
        }
    }
}

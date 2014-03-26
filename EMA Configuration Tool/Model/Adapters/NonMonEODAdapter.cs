using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class NonMonEODAdapter : AdapterBase
    {
        public override string FileSuffix
        {
            get { return "_nm"; }
        }


        public NonMonEODAdapter()
        {
            Question finalPrompt = new Question();
            finalPrompt.Label = "p";
            finalPrompt.Text = "1. Take off Actical from your belt. Press the * button.\n\n2. Please press the Actiwatch button when you are trying to go to sleep.\n\n3. Please plug the ED in NOW so that it may charge overnight!";
            finalPrompt.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            PostQuestions.Add(finalPrompt);
        }
    }
}

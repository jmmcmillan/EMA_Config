using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Responses;
using System.Collections.ObjectModel;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class SalivaContentAdapter : AdapterBase
    {
        public override string FileSuffix
        {
            get { return "_saliva"; }
        }
       
        public SalivaContentAdapter() : base()
        {
            Question salivettePrompt = new Question();
            salivettePrompt.Label = "p";
            salivettePrompt.Text = "Please place a salivette in your mouth and gently chew.";
            salivettePrompt.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            PreQuestions.Add(salivettePrompt);

            Question timerAnchor = new Question();
            timerAnchor.Label = "p";
            timerAnchor.Text = "Please wait...";
            timerAnchor.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            Question returnSalivettePrompt = new Question();
            returnSalivettePrompt.Label = "p";
            returnSalivettePrompt.Text = "Please return salivette to the tube.";
            returnSalivettePrompt.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            Question salivetteLabel = new Question();
            salivetteLabel.Label = "sample code";
            salivetteLabel.Text = "Record the following number on the salivette tube:";
            salivetteLabel.Response = new SampleCode();

            Question salivetteQ = new Question();
            salivetteQ.Label = "saliva";
            salivetteQ.Text = "Did you complete your saliva sampling?";
            SingleChoiceList choice = new SingleChoiceList();
            choice.Responses = EMA_Configuration_Tool.Services.ResponseService.GetStringResponseSet(new List<string>() { "No", "Yes"});
            salivetteQ.Response = choice;

            PostQuestions.Add(timerAnchor);
            PostQuestions.Add(returnSalivettePrompt);
            PostQuestions.Add(salivetteLabel);
            PostQuestions.Add(salivetteQ);
        }

        
    }
}

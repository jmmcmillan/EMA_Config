using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Responses;
using System.Collections.ObjectModel;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class SalivaContentAdapter
    {
        List<Question> PreQuestions = new List<Question>();
        List<Question> PostQuestions = new List<Question>();

        public SalivaContentAdapter()
        {
            Question salivettePrompt = new Question();
            salivettePrompt.Label = "p";
            salivettePrompt.Text = "Please place a salivette in your mouth and gently chew.";
            salivettePrompt.Response = new EMA_Configuration_Tool.Model.Responses.Prompt();

            PreQuestions.Add(salivettePrompt);

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

            PostQuestions.Add(returnSalivettePrompt);
            PostQuestions.Add(salivetteLabel);
            PostQuestions.Add(salivetteQ);
        }

        public void AdaptInterview()
        {
            //was using the perfectly nice List.InsertRange method until switching to observableCollection 
            //for the up/down buttons on CV.xaml -- find replacement?
            ObservableCollection<Question> newQuestions = new ObservableCollection<Question>();

            foreach (Question q in PreQuestions)
                newQuestions.Add(q);

            foreach (Question q in App.Interview.Questions)
                newQuestions.Add(q);

            foreach (Question q in PostQuestions)
                newQuestions.Add(q);

            App.Interview.Questions = newQuestions;
        }

        public void RevertInterview()
        {
            IEnumerable<Question> allQuestions = PreQuestions.Concat(PostQuestions);

            foreach (Question q in allQuestions)
            {
                App.Interview.Questions.Remove(q);
            }
        }
    }
}

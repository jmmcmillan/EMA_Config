using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ContentViewModel : PropertyChangedBase
    {
        public ContentViewModel() 
        { 
        }
        
        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            set { 
                selectedQuestion = value;
                NotifyOfPropertyChange(() => SelectedQuestion);                
            }
            get
            {
                return selectedQuestion;
            }
        }

        public void AddQuestion()
        {
            App.EventAggregator.Publish(new QuestionViewModel());
        }

        public void EditQuestion(object view)
        {
            if (view != null)
            {
                ContentView cv = (view as ContentView);
                object question = cv.QuestionList.SelectedItem;

                if (question is Question)
                    App.EventAggregator.Publish(new QuestionViewModel(question as Question));
            }

        }

        public void DeleteQuestion()
        {
            App.Interview.Questions.Remove(SelectedQuestion);
        }

        public void SaveInterview()
        {
        }

        public void SwitchSelectedQuestion(object sender)
        {
            if (sender is Question)
            {
                SelectedQuestion = sender as Question;
            }
        }

        public void AddQuestionBefore()
        {
            int thisIndex = App.Interview.Questions.FindIndex(q => q.ID == SelectedQuestion.ID);

            App.EventAggregator.Publish(new QuestionViewModel(thisIndex));

        }

        public void AddQuestionAfter()
        {
            int thisIndex = App.Interview.Questions.FindIndex(q => q.ID == SelectedQuestion.ID);

            App.EventAggregator.Publish(new QuestionViewModel(thisIndex + 1));

        }
    }
}

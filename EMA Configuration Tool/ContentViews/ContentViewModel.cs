using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ContentViewModel 
    {
        public ContentViewModel() { }

        public Question SelectedQuestion { get; set; }

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
    }
}

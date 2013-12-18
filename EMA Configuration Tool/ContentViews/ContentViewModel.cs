using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;
using System.Windows;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ContentViewModel : PropertyChangedBase
    {
        WindowManager windowManager;

        public ContentViewModel() 
        {
            windowManager = new WindowManager();
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
            if (SelectedQuestion != null)
                App.EventAggregator.Publish(new QuestionViewModel(SelectedQuestion));

        }

        public void DeleteQuestion()
        {
            if (SelectedQuestion == null)
                return;

            if (MessageBox.Show("Are you sure you want to delete this question?", "Delete Question", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            List<Question> dependencies = EMA_Configuration_Tool.Services.ConstraintService.ThisQuestionUsedBy(SelectedQuestion);

            if (dependencies.Count > 0)
            {
                DeleteHelperViewModel deleteHelp = new DeleteHelperViewModel(SelectedQuestion, dependencies);

                windowManager.ShowWindow(deleteHelp);
            }
            else App.Interview.Questions.Remove(SelectedQuestion);
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

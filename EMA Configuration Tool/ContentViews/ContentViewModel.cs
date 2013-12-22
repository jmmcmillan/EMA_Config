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

            if (MessageBox.Show("Are you sure you want to delete this question?", "Delete Question", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                return;

            List<Question> dependencies = EMA_Configuration_Tool.Services.ConstraintService.ThisQuestionUsedBy(SelectedQuestion);

            if (dependencies.Count > 0)
            {
                DeleteHelperViewModel deleteHelp = new DeleteHelperViewModel(SelectedQuestion, dependencies);
                windowManager.ShowWindow(deleteHelp);
            }
            else
            {   
                EMA_Configuration_Tool.Services.ConstraintService.DeleteConstraintsReferencing(SelectedQuestion);
                App.Interview.Questions.Remove(SelectedQuestion);

                App.Interview.Refresh();
            }
        }


        public void AddQuestionBefore()
        {
            int thisIndex = 0;

            if (SelectedQuestion != null)
                thisIndex = App.Interview.Questions.IndexOf(SelectedQuestion);

            App.EventAggregator.Publish(new QuestionViewModel(thisIndex));

        }

        public void AddQuestionAfter()
        {
            int thisIndex = App.Interview.Questions.IndexOf(SelectedQuestion);

            App.EventAggregator.Publish(new QuestionViewModel(thisIndex + 1));

        }

        public void MoveQuestionUp()
        {
            if (SelectedQuestion != null)
            {   
                int index = App.Interview.Questions.IndexOf(SelectedQuestion);

                if (index < 1)
                    return;

                App.Interview.Questions.Insert(index - 1, SelectedQuestion);
                App.Interview.Questions.RemoveAt(index + 1);

                SelectedQuestion = App.Interview.Questions.ElementAt(index - 1);
                
            }
        }

        public void MoveQuestionDown()
        {
            if (SelectedQuestion != null)
            {
                int index = App.Interview.Questions.IndexOf(SelectedQuestion);

                if (index >= App.Interview.Questions.Count - 1)
                    return;

                if (index + 2 > App.Interview.Questions.Count)
                {
                    App.Interview.Questions.Add(SelectedQuestion);  //add to the end
                }
                else  App.Interview.Questions.Insert(index + 2, SelectedQuestion);
                App.Interview.Questions.RemoveAt(index);

                SelectedQuestion = App.Interview.Questions.ElementAt(index + 1);

            }

        }
    }
}

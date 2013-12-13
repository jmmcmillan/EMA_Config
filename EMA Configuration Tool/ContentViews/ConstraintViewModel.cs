using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;
using System.Windows.Controls;
using System.Windows;
using EMA_Configuration_Tool.Model.Responses;
using EMA_Configuration_Tool.Model.Constraints;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ConstraintViewModel : Screen
    {
        public ConstraintViewModel()
        {
            this.DisplayName = "Add/Edit Constraint";

        }
        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get {return selectedQuestion;}
            set
            {
                selectedQuestion = value;
                NotifyOfPropertyChange(() => SelectedQuestion);
            }
        }

        private List<string> CheckedItems = new List<string>();

        public void SwitchSelectedQuestion(object q)
        {
            if (q is Question)
                SelectedQuestion = q as Question;

        }

        public void Save(object view)
        {
            if (!(view is Window))
                return;

            ConstraintView cView = (view as Window).Content as ConstraintView;

            Question question = cView.QuestionList.SelectedItem as Question;

            List<int> indexList = new List<int>();
            int i = 0;
            foreach (string s in (question.Response as StringChoice).Responses.StringResponses)
            {
                if (CheckedItems.Contains(s))
                    indexList.Add(i);

                i++;
            }

            StringConstraint sc = new StringConstraint(question.ID, indexList);
            App.Interview.Constraints.Add(sc);

            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }

        public void Unchecked(object chkbox)
        {
            if (!(chkbox is CheckBox))
                return;

            CheckBox cb = chkbox as CheckBox;
            string response = (cb.Content as TextBlock).Text.ToString();

            CheckedItems.RemoveAll(s => s.Equals(response));
        }

        public void Checked(object chkbox)
        {
            if (!(chkbox is CheckBox))
                return;

            CheckBox cb = chkbox as CheckBox;
            string response = (cb.Content as TextBlock).Text.ToString();

            CheckedItems.Add(response);
        }
    }
}

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
using System.Collections.ObjectModel;
using EMA_Configuration_Tool.Model.Groups;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ConstraintViewModel : Screen
    {
        public List<Question> StringChoiceQuestions { get; set; }
        
        private Constraint unchangedConstraint;

        public ObservableCollection<BindableBool> SelectedResponses { get; set; }
        public List<string> ResponseStrings { get; set; }

        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                selectedQuestion = value;
                NotifyOfPropertyChange(() => SelectedQuestion);

                SelectedResponses = new ObservableCollection<BindableBool>();
                ResponseStrings = new List<string>();

                if (selectedQuestion != null && selectedQuestion.Response is ChoiceBase)
                {
                    ResponseStrings = (selectedQuestion.Response as ChoiceBase).Responses.StringResponses;
                    initResponseStringData();
                }

                NotifyOfPropertyChange(() => ResponseStrings);
                NotifyOfPropertyChange(() => SelectedResponses);

            }
        }

        public bool ShowIfSkipped { get; set; }

        public ConstraintViewModel(Constraint constraint)
            :this()
        {
            unchangedConstraint = constraint;

            SelectedQuestion = App.Interview.Questions.Where(q => q.ID == unchangedConstraint.FollowupForGuid).FirstOrDefault();

            ShowIfSkipped = (constraint as StringConstraint).FollowupValueIndexes.Contains(-1);
        }

        private void initResponseStringData()
        {
            for (int i = 0; i < ResponseStrings.Count; i++)
            {
                if (unchangedConstraint != null)
                {
                    if ((unchangedConstraint as StringConstraint).FollowupValueIndexes.Contains(i))
                    {
                        SelectedResponses.Add(new BindableBool(true));
                    }
                    else SelectedResponses.Add(new BindableBool(false));
                }
                else SelectedResponses.Add(new BindableBool(false));
            }

            
        }

        public ConstraintViewModel()
            : base()
        {
            this.DisplayName = "Add/Edit Constraint";

            SelectedResponses = new ObservableCollection<BindableBool>();
            ResponseStrings = new List<string>();

            StringChoiceQuestions = new List<Question>();
            foreach (Question q in App.Interview.Questions)
            {
                if (q.Response is ChoiceBase)
                    StringChoiceQuestions.Add(q);
            }

            SelectedQuestion = StringChoiceQuestions.FirstOrDefault();

        }

        public Constraint configuredConstraint;

        private bool canSave()
        {
            if (SelectedQuestion == null)
            {
                MessageBox.Show("Please select a question.", "No Question Selected", MessageBoxButton.OK);
                return false;
            }

            if (SelectedResponses.Where(b => b.Value).Count() < 1)
            {
                MessageBox.Show("No response options are selected. Please select 1 or more conditions for this constraint.", "No Conditions Selected", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        public void Save()
        {
            if (!canSave())
                return;

            List<int> indexList = new List<int>();
            int i = 0;
            foreach (BindableBool b in SelectedResponses)
            {
                if (b.Value)
                    indexList.Add(i);

                i++;
            }

            if (ShowIfSkipped)
                indexList.Insert(0, -1);

            if (unchangedConstraint != null)
            {
                (unchangedConstraint as StringConstraint).FollowupForGuid = SelectedQuestion.ID;
                (unchangedConstraint as StringConstraint).FollowupValueIndexes = indexList;

                unchangedConstraint.Refresh();

                configuredConstraint = unchangedConstraint;
            }

            else
            {
                StringConstraint sc = new StringConstraint(SelectedQuestion.ID, indexList);
                configuredConstraint = sc;
                App.Interview.Constraints.Add(sc);
            }

            TryClose();
        }

     
        public void Cancel()
        {
            TryClose();
        }

    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model.Responses;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using EMA_Configuration_Tool.Model.Constraints;
using System.Windows;

namespace EMA_Configuration_Tool.ContentViews
{
    
    public class QuestionViewModel : Screen
    {  

        public Constraint SelectedConstraint { get; set; }
        public Question Question { get; set; }
        public Type SelectedResponseType { get; set; }

        private int requestedIndex = -1;
        private Question unchangedQuestion = null;

        public bool ConstraintCBVisible
        {
            get
            {
                return App.Interview.Constraints.Count > 0;
            }
        }

        public QuestionViewModel(Question q) : this()
        {
            
            //keep this question untouched in case they want to cancel, and make a copy for modification
            unchangedQuestion = q;

            Question.Text = q.Text;
            Question.Label = q.Label;
            Question.Response = q.Response.Copy();
            
            foreach (Constraint c in q.Constraints)
                Question.Constraints.Add(c);

            SelectedConstraint = Question.Constraints.FirstOrDefault();

            if (Question.Response != null)
            {
                SelectedResponseType = Question.Response.GetType();
            }
        }

        public QuestionViewModel()
        {
            Question = new Question();

            this.DisplayName = "Add/Edit Question";
            
        }

        public QuestionViewModel(int index) : base()
        {
            requestedIndex = index;
            Question = new Question();           

        }

        private bool okayToSave()
        {
            if (Question.Label.Contains('_'))
            {
                System.Windows.MessageBox.Show("Question labels can't contain underscores. Please enter a different label.");
                return false;
            }

            if (Question.Response is IHaveDefault)
            {
                IHaveDefault def = Question.Response as IHaveDefault;

                if (!def.DefaultIsValid())
                {
                    System.Windows.MessageBox.Show(def.GetBadDefaultMessage());
                    return false;
                }
            }

            return true;
        }

        public void SaveQuestion()
        {
            if (!okayToSave())
                return;

            //context menu "Insert Before" or "Insert After" was used to get here
            if (requestedIndex > -1)
            {
                App.Interview.Questions.Insert(requestedIndex, Question);
            }

            //editing an existing question
            else if (unchangedQuestion != null)
            {
                int indexOf = App.Interview.Questions.IndexOf(unchangedQuestion);
                App.Interview.Questions.RemoveAt(indexOf);
                App.Interview.Questions.Insert(indexOf, Question);
            }

            //else tack it on at the end
            else App.Interview.Questions.Add(Question); 
            

            App.EventAggregator.Publish(new ContentViewModel());
        }

        public void Cancel()
        {
            App.EventAggregator.Publish(new ContentViewModel());
        }

        public void SwitchResponseType(object rt)
        {
            Question.Response = (ResponseBase)Activator.CreateInstance(rt as Type);
        }

        public void SwitchStringResponseSet(object srs)
        {
            if (Question.Response is StringChoice)
            {
                (Question.Response as StringChoice).Responses = srs as StringResponseSet;
            }
        }

        public void AddResponseSet()
        {   
            //bad form, but accessing the global one requires jumping through some DI hoops I haven't figured out yet
            WindowManager windowManager = new WindowManager();
            windowManager.ShowDialog(new ResponseSetViewModel());
        }

        public void EditResponseSet(object dataContext)
        {
            if (dataContext is StringChoice)
            {
                StringChoice sc = dataContext as StringChoice;

                //bad form, but accessing the global one requires jumping through some DI hoops I haven't figured out yet
                WindowManager windowManager = new WindowManager();
                //windowManager.ShowDialog(new ResponseSetViewModel(sc.Responses));
                windowManager.ShowDialog(new ResponseSetViewModel(sc));
                
            }
        }

        public void DeleteResponseSet(object dataContext)
        {
            if (!(dataContext is StringChoice))
                return;

            if (MessageBox.Show("Are you sure you want to delete this set of responses?", "Delete Response Set", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App.Interview.StringResponseSets.Remove((dataContext as StringChoice).Responses);
            }
        }

        public void SelectedReferenceQuestionChanged(object question)
        {
            if (question is Question)
            {
                (Question.Response as DynamicGroup).ReferenceQuestion = question as Question;
            }
        }


        public void SelectedConstraintChanged(object constraint)
        {
            if (constraint is Constraint)
            {
                SelectedConstraint = constraint as Constraint;

                //for the moment, only 1 constraint per question
                Question.Constraints = new List<Constraint>();
                Question.Constraints.Add(constraint as Constraint);
            }

            else
            {
                //they picked the "No constraints" string
                Question.Constraints = new List<Constraint>();
            }
        }

        public void AddConstraint()
        {
            //bad form, but accessing the global one requires jumping through some DI hoops I haven't figured out yet
            WindowManager windowManager = new WindowManager();
            windowManager.ShowDialog(new ConstraintViewModel());

            NotifyOfPropertyChange(() => ConstraintCBVisible);
        }

        public void EditConstraint(object dataContext)
        {
           

        }

        public void DeleteConstraint(object dataContext)
        {
            if (SelectedConstraint == null)
                return;

            if (MessageBox.Show("Are you sure you want to delete this skipping pattern?", "Delete Skipping Pattern", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App.Interview.Constraints.Remove(SelectedConstraint);
            }


        }

    
    }
}

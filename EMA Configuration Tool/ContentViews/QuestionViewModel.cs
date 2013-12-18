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
        //bad form, but accessing the global one requires jumping through some DI hoops I haven't figured out yet
        private WindowManager windowManager;

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

        
        public QuestionViewModel() : base()
        {
            windowManager = new WindowManager();

            Question = new Question();

            this.DisplayName = "Add/Edit Question";
            
        }

        public QuestionViewModel(int index) : this()
        {
            requestedIndex = index;
        }

        #region Save Question

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
                Question.ID = unchangedQuestion.ID; // so the constraints can still find this question

                int indexOf = App.Interview.Questions.IndexOf(unchangedQuestion);
                App.Interview.Questions.RemoveAt(indexOf);
                App.Interview.Questions.Insert(indexOf, Question);
            }

            //else tack it on at the end
            else App.Interview.Questions.Add(Question); 
            

            App.EventAggregator.Publish(new ContentViewModel());
        }

        #endregion

        public void Cancel()
        {
            App.EventAggregator.Publish(new ContentViewModel());
        }

        public void SwitchResponseType(object rt)
        {
            Question.Response = (ResponseBase)Activator.CreateInstance(rt as Type);
        }

        #region String Response Sets

        public void AddResponseSet()
        {
            windowManager.ShowDialog(new ResponseSetViewModel(Question));
        }

        public void EditResponseSet(object dataContext)
        {
            if (dataContext is StringChoice)
            {
                StringChoice sc = dataContext as StringChoice;
                windowManager.ShowDialog(new ResponseSetViewModel(sc.Responses, Question));
                
            }
        }

        public void DeleteResponseSet(object dataContext)
        {
            StringResponseSet srs;

            if (!(dataContext is StringChoice))
                return;
            else  srs = (dataContext as StringChoice).Responses;

            if (MessageBox.Show("Are you sure you want to delete this set of responses?", "Delete Response Set", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            List<Question> dependencies = EMA_Configuration_Tool.Services.ResponseService.ReponseSetAlsoUsedIn(srs, Question);

            if (dependencies.Count > 0)
            {
                DeleteHelperViewModel deleteHelp = new DeleteHelperViewModel(srs, dependencies);

                windowManager.ShowWindow(deleteHelp);
            }

            else App.Interview.StringResponseSets.Remove((dataContext as StringChoice).Responses);
        }

        #endregion


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

            if (MessageBox.Show("Are you sure you want to delete this constraint?", "Delete Constraint", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            List<Question> dependencies = EMA_Configuration_Tool.Services.ConstraintService.ConstraintIsAlsoUsedBy(SelectedConstraint, Question);

            if (dependencies.Count < 1)
            {
                App.Interview.Constraints.Remove(SelectedConstraint);
                
                Question.Constraints = new List<Constraint>();
            }

            else
            {  
                DeleteHelperViewModel deleteHelp = new DeleteHelperViewModel(SelectedConstraint, dependencies);

                windowManager.ShowWindow(deleteHelp);

            }

        }

    
    }
}

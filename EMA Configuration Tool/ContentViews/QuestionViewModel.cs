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
using System.Windows.Threading;
using System.Windows.Input;

namespace EMA_Configuration_Tool.ContentViews
{
    
    public class QuestionViewModel : Screen
    {
        //bad form, but accessing the global one requires jumping through some DI hoops I haven't figured out yet
        private WindowManager windowManager;

        public object SelectedConstraint { get; set; }
        //public Constraint SelectedConstraint { get; set; }
        public Question Question { get; set; }
        public Type SelectedResponseType { get; set; }

        private int requestedIndex = -1;
        private Question unchangedQuestion = null;


        public List<ReferenceQuestion> QuestionsForReference { get; set; }

        //public bool ConstraintCBVisible
        //{
        //    get
        //    {
        //        return App.Interview.Constraints.Count > 0;
        //    }
        //}

        public QuestionViewModel(Question q) : this()
        {   
            //keep this question untouched in case they want to cancel, and make a copy for modification
            unchangedQuestion = q;

            Question.Text = q.Text;
            Question.Label = q.Label;
            Question.Response = q.Response.Copy();
            
            //check for constraints
            foreach (Constraint c in q.Constraints)
                Question.Constraints.Add(c);

            if (Question.Constraints.Count() > 0)
                SelectedConstraint = Question.Constraints.FirstOrDefault();
            else SelectedConstraint = App.Interview.Constraints.FirstOrDefault();

            //check for response
            if (Question.Response != null)
            {
                SelectedResponseType = Question.Response.GetType();
            }

            //check for question references
            if (Question.Response is BasedOnQuestions)
            {
                BasedOnQuestions boq = Question.Response as BasedOnQuestions;
                if (boq.HasReferences)
                {
                    foreach (Question rq in boq.ReferenceQuestions)
                    {
                        QuestionsForReference.Where(t => t.Question.ID.Equals(rq.ID)).First().IsReferenced = true;
                    }
                }
            }
        }

        
        public QuestionViewModel() : base()
        {
            this.DisplayName = "Add/Edit Question"; 
            windowManager = new WindowManager();

            Question = new Question();

            SelectedResponseType = typeof(Prompt);
            NotifyOfPropertyChange(() => SelectedResponseType);

            Question.Response = new Prompt();
            Question.Refresh();

            SelectedConstraint = App.Interview.Constraints.FirstOrDefault();

            QuestionsForReference = new List<ReferenceQuestion>();
            foreach (Question q in App.Interview.Questions)
                QuestionsForReference.Add(new ReferenceQuestion(q, false));
            
        }

        public QuestionViewModel(int index) : this()
        {
            requestedIndex = index;
        }

        #region Save Question

        private bool okayToSave()
        {
            if (String.IsNullOrEmpty(Question.Label))
            {
                System.Windows.MessageBox.Show("Questions must have a label. Please enter a label.", "No Label for Question", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //if (Question.Label.Contains('_'))
            //{
            //    System.Windows.MessageBox.Show("Question labels can't contain underscores ('_'). Please enter a different label.", "No Underscores in Question Label", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

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

            if (MessageBox.Show("Are you sure you want to delete this set of responses?", "Delete Response Set", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                return;

            Question exceptThis = (unchangedQuestion == null) ? Question : unchangedQuestion;
            List<Question> dependencies = EMA_Configuration_Tool.Services.ResponseService.ReponseSetAlsoUsedIn(srs, exceptThis);

            if (dependencies.Count > 0)
            {
                DeleteHelperViewModel deleteHelp = new DeleteHelperViewModel(srs, dependencies);

                windowManager.ShowWindow(deleteHelp);
            }

            else
            {
                App.Interview.StringResponseSets.Remove((dataContext as StringChoice).Responses);

                SelectedResponseType = typeof(Prompt);
                NotifyOfPropertyChange(() => SelectedResponseType);

                Question.Response = new Prompt();
                Question.Refresh();
            }

        }

        #endregion


        #region Constraints
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

            Question.Refresh();
        }

        public void AddConstraint()
        {
            ConstraintViewModel cvm = new ConstraintViewModel();
            windowManager.ShowDialog(cvm);

            SelectedConstraint = cvm.configuredConstraint;
            NotifyOfPropertyChange(() => SelectedConstraint);

            //picks up after dialog is closed to autoselect the most recent constraint
            SelectedConstraintChanged(cvm.configuredConstraint);

         
        }

        public void EditConstraint()
        {
            if (SelectedConstraint != null && (SelectedConstraint is Constraint))
                windowManager.ShowDialog(new ConstraintViewModel(SelectedConstraint as Constraint));

            else
            {
                MessageBox.Show("Please select a constraint to edit.", "No Constraint Selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void DeleteConstraint()
        {
            if (SelectedConstraint == null || !(SelectedConstraint is Constraint))
            {
                MessageBox.Show("Please select a constraint to delete.", "No Constraint Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this constraint?", "Delete Constraint", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                return;

            Question exceptThis = (unchangedQuestion == null) ? Question : unchangedQuestion;
            List<Question> dependencies = EMA_Configuration_Tool.Services.ConstraintService.ConstraintIsAlsoUsedBy(SelectedConstraint as Constraint, exceptThis);

            if (dependencies.Count < 1)
            {
                App.Interview.Constraints.Remove(SelectedConstraint);
                
                Question.Constraints = new List<Constraint>();
            }

            else
            {  
                DeleteHelperViewModel deleteHelp = new DeleteHelperViewModel(SelectedConstraint as Constraint, dependencies);

                windowManager.ShowWindow(deleteHelp);

            }

        }

        #endregion


        #region reference questions


        public void ReferenceQuestionChange(object dataContext)
        {
            ReferenceQuestion referenceQuestion = dataContext as ReferenceQuestion;

            if (referenceQuestion.IsReferenced)
            {
                if (!(Question.Response as BasedOnQuestions).ReferenceQuestions.Contains(referenceQuestion.Question))
                    (Question.Response as BasedOnQuestions).ReferenceQuestions.Add(referenceQuestion.Question);
            }
            else (Question.Response as BasedOnQuestions).ReferenceQuestions.Remove(referenceQuestion.Question);
        }


        #endregion

    }

    
}

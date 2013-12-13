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

namespace EMA_Configuration_Tool.ContentViews
{
    
    public class QuestionViewModel : Screen
    {  

        public Constraint SelectedConstraint { get; set; }
        public Question Question { get; set; }
        public Type SelectedResponseType { get; set; }


        public bool ConstraintCBVisible
        {
            get
            {
                return App.Interview.Constraints.Count > 0;
            }
        }

        public QuestionViewModel(Question q) : this()
        {
            Question = q;

            SelectedConstraint = q.Constraints.FirstOrDefault();

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

        public void SaveQuestion()
        {
            bool inserted = false;

            for (int i = 0; i < App.Interview.Questions.Count; i++)
            {                
                if (App.Interview.Questions[i].ID == Question.ID)
                {
                    App.Interview.Questions.RemoveAt(i);
                    App.Interview.Questions.Insert(i, Question);

                    inserted = true;
                    break;
                }
            }

            if (!inserted)
                App.Interview.Questions.Add(Question); 
                        

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
        }

        public void AddConstraint()
        {
            //bad form, but accessing the global one requires jumping through some DI hoops I haven't figured out yet
            WindowManager windowManager = new WindowManager();
            windowManager.ShowDialog(new ConstraintViewModel());

            NotifyOfPropertyChange(() => ConstraintCBVisible);
        }

        public void EditConstraint(object view)
        {
           

        }

        public void DeleteConstraint()
        {
        }

    
    }
}

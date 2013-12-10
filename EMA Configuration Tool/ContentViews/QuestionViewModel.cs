using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model.Responses;
using System.ComponentModel.Composition;

namespace EMA_Configuration_Tool.ContentViews
{
    
    public class QuestionViewModel : PropertyChangedBase
    {
        public Question Question { get; set; }

        private ResponseBase response;
        public ResponseBase Response 
        {
            get { return response; }
            set
            {
                response = value;
                NotifyOfPropertyChange(() => Response);
            }
        }

       

        public QuestionViewModel(Question q)
        {
            Question = q;
        }

        public QuestionViewModel()
        {
            Question = new Question();
        }

        public void SaveQuestion()
        {
            Question.Response = this.Response;

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
            Response = (ResponseBase)Activator.CreateInstance(rt as Type);
        }

        public void SwitchStringResponseSet(object srs)
        {
            if (Response is StringChoice)
            {
                (Response as StringChoice).Responses = srs as StringResponseSet;
            }
        }

        public void AddResponseSet()
        {
            WindowManager windowManager = new WindowManager();

            windowManager.ShowDialog(new ResponseSetViewModel());
        }
    }
}

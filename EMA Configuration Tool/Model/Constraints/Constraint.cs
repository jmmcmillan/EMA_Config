using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EMA_Configuration_Tool.Model.Constraints;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model
{
    
    public abstract class Constraint : PropertyChangedBase
    {   
       
        public Guid ID { get; set; }

        
        protected Question relevantQuestion
        {
            get
            {
                if (followupForGuid != null)
                {
                    Question question = App.Interview.Questions.Where(q => q.ID == FollowupForGuid).FirstOrDefault();

                    if (question != null)
                        return question;

                    else
                    {
                        Console.Write("Constraint reference question not found", followupForGuid);
                        return new Question();
                    }

                }
                else return new Question();
            }
        }

       
        public string FollowUpForLabel
        {
            get { return relevantQuestion.Label; }
        }

        
        public string FollowUpForText
        {
            get { return relevantQuestion.PreviewPaneText; }
        }

       
        private Guid followupForGuid;       
        public Guid FollowupForGuid 
        {
            get
            {
                return followupForGuid;
            }
            set
            {
                followupForGuid = value;                
            }
        }


        public Constraint()
        {
            ID = Guid.NewGuid();
        }



    }
}

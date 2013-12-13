using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EMA_Configuration_Tool.Model.Constraints;

namespace EMA_Configuration_Tool.Model
{
    
    public abstract class Constraint
    {   
       
        public Guid ID { get; set; }

        
        protected Question relevantQuestion
        {
            get
            {
                if (followupForGuid != null)
                    return App.Interview.Questions.Where(q => q.ID == FollowupForGuid).First();
                else return new Question();
            }
        }

       
        public string FollowUpForLabel
        {
            get { return relevantQuestion.Label; }
        }

        
        public string FollowUpForText
        {
            get { return relevantQuestion.Text; }
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

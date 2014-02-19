using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EMA_Configuration_Tool.Model.Responses
{
    public class ReferenceQuestion
    {
        public Question Question { get; set; }
        public bool IsReferenced { get; set; }

        public ReferenceQuestion(Question q, bool i)
        {
            Question = q;
            IsReferenced = i;
        }
    }

    public abstract class BasedOnQuestions : ResponseBase
    {
        [XmlIgnore]
        public List<ReferenceQuestion> ReferenceQuestions { get; set; }

        public BasedOnQuestions()
            : base()
        {
            ReferenceQuestions = new List<ReferenceQuestion>();

            foreach (Question q in App.Interview.Questions)
            {
                ReferenceQuestions.Add(new ReferenceQuestion(q, false));
            }
        }

        protected List<ReferenceQuestion> CopyReferenceQuestions()
        {
            List<ReferenceQuestion> newReferences = new List<ReferenceQuestion>();

            foreach (ReferenceQuestion rq in ReferenceQuestions)
            {
                newReferences.Add(new ReferenceQuestion(rq.Question, rq.IsReferenced));
            }

            return newReferences;
        }
    }
}

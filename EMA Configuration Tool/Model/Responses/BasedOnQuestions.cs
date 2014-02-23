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
        public List<Question> ReferenceQuestions { get; set; }

        [XmlIgnore]
        public List<Tuple<string, string>> ReferenceQuestionsForDisplay
        {
            get
            {
                List<Tuple<string, string>> questions = new List<Tuple<string, string>>();

                foreach (Question rq in ReferenceQuestions)
                {
                    questions.Add(new Tuple<string,string>(rq.Label, rq.PreviewPaneText));
                }

                return questions;
            }
        }

        public BasedOnQuestions()
            : base()
        {
            ReferenceQuestions = new List<Question>();           
        }

        public bool HasReferences { get { return ReferenceQuestions.Count > 0; } }

        protected List<Question> CopyReferenceQuestions()
        {
            List<Question> newReferences = new List<Question>();

            foreach (Question rq in ReferenceQuestions)
            {
                newReferences.Add(rq);
            }

            return newReferences;
        }
    }
}

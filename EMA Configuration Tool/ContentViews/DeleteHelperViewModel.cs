using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.ContentViews
{
    public class DeleteHelperViewModel : Screen
    {

        public string Instructions { get; set; }
        public List<Question> Questions { get; set; }

        
        public DeleteHelperViewModel(Question question, List<Question> affectedQuestions)
            : this(affectedQuestions)
        {
            Instructions = "This question is being used by one or more constraints. Please change or remove the following questions before deleting this question:";
        }


        public DeleteHelperViewModel(StringResponseSet srs, List<Question> affectedQuestions)
            : this(affectedQuestions)
        {
            Instructions = "This response set is being used by other questions and/or constraints. Please change or remove the following questions before deleting this response set:";
        }


        public DeleteHelperViewModel(Constraint constraint, List<Question> affectedQuestions)
            : this(affectedQuestions)
        {
            Instructions = "This constraint is being used by other questions. Please change or remove the following questions before deleting this constraint:";
        }

        public DeleteHelperViewModel(List<Question> affectedQuestions)
        {   
            Questions = affectedQuestions;
            this.DisplayName = "Delete Helper";

        }

        public void Close()
        {
            TryClose();
        }

    }
}

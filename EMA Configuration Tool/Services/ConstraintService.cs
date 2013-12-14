using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using EMA_Configuration_Tool.Model.Constraints;

namespace EMA_Configuration_Tool.Services
{
    public static class ConstraintService
    {
        //coming back from XML, we only have the follow-up question index and the numerical index values
        //this method translates those numbers back to into objects and adds them to the question and, if
        //necessary, the global list of available constraints
        public static void GenerateConstraint(Question question)
        {
            Question followupForQuestion = App.Interview.Questions.ElementAt(question.FollowupFor);

            if (followupForQuestion == null)
                return;

            bool done = false;
            foreach (object obj in App.Interview.Constraints)
            {
                if (!(obj is Constraint))
                    continue;

                Constraint c = obj as Constraint;

                if (c.FollowupForGuid == followupForQuestion.ID)  //references same ID
                {
                    if ((c as StringConstraint).FollowupValueIndexesAsString == question.FollowupForValue)
                    {
                        question.Constraints.Add(c);
                        done = true;
                        break;
                    }
                }

            }

            if (!done)
            {
                StringConstraint sc = new StringConstraint(followupForQuestion.ID, question.FollowupForValue);
                question.Constraints.Add(sc);

                App.Interview.Constraints.Add(sc);
            }
        }
        

    }
}

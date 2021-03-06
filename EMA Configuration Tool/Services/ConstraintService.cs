﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using EMA_Configuration_Tool.Model.Constraints;
using EMA_Configuration_Tool.Model.Responses;

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

        public static List<Question> ConstraintIsAlsoUsedBy(Constraint constraint, Question question)
        {
            List<Question> questions = new List<Question>();

            foreach (Question q in App.Interview.Questions)
            {
                if (q.ID == question.ID)
                    continue;

                if (q.HasConstraints)
                {
                    foreach (Constraint c in q.Constraints)
                    {
                        if (c.ID == constraint.ID)
                        {
                            questions.Add(q);
                            continue;
                        }

                    }
                }
            }

            return questions;

        }

        public static void DeleteConstraintsReferencing(Question question)
        {
            List<Constraint> toRemove = new List<Constraint>();

            foreach (object obj in App.Interview.Constraints)
            {
                if (!(obj is Constraint))
                    continue;

                Constraint c = obj as Constraint;

                if (c is StringConstraint)
                {
                    if (c.FollowupForGuid == question.ID)
                        toRemove.Add(c);
                }
            }

            foreach (Constraint c in toRemove)
                App.Interview.Constraints.Remove(c);
        }

        public static List<Question> ThisQuestionUsedBy(Question question)
        {
            List<Question> questions = new List<Question>();

            foreach (Question q in App.Interview.Questions)
            {   
                if (q.HasConstraints)
                {
                    foreach (Constraint c in q.Constraints)
                    {
                        if (c.FollowupForGuid == question.ID)
                        {
                            questions.Add(q);
                            continue;
                        }

                    }
                }
            }

            return questions;

        }

        public static List<Constraint> UsesResponseSet(StringResponseSet srs)
        {
            List<Constraint> cons = new List<Constraint>();

            foreach (object obj in App.Interview.Constraints)
            {
                if (!(obj is Constraint))
                    continue;

                Constraint c = obj as Constraint;

                if (c is StringConstraint)
                {
                    Question question = App.Interview.Questions.Where(q => q.ID == c.FollowupForGuid).FirstOrDefault();

                    if (question == null)
                        continue;

                    else if (question.Response is StringChoice)
                    {
                        if ((question.Response as StringChoice).Responses == srs)
                            cons.Add(c);
                    }
                }
            }

            return cons;

        }

    }
}

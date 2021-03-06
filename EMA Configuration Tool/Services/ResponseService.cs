﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using EMA_Configuration_Tool.Model.Responses;
using EMA_Configuration_Tool.Model;

namespace EMA_Configuration_Tool.Services
{
    public static class ResponseService
    {
        private static List<Type> responseTypes;
        public static List<Type> ResponseTypes
        {
            set { responseTypes = value; }
            get
            {
                if (responseTypes == null)
                    responseTypes = GetResponseTypes();

                return responseTypes;
            }
        }

        private static List<Type> GetResponseTypes()
        {
            List<Type> responseTypes = new List<Type>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(ResponseBase)))
                {
                    if (!type.IsAbstract)
                        responseTypes.Add(type);
                }
            }

            return responseTypes;
        }

        public static ResponseBase GetMeOneOfThese(string response)
        {
            int underscore = response.IndexOf('_');
            string typeName = (underscore > 0) ? response.Substring(0, underscore) : response;


            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof(ResponseBase)))
                {
                    if (type.Name.Equals(typeName))
                        return (ResponseBase)Activator.CreateInstance(type);
                }
            }

            return null;
        }

        public static List<Question> ReponseSetAlsoUsedIn(StringResponseSet srs, Question question)
        {
            List<Constraint> constraints = ConstraintService.UsesResponseSet(srs);

            List<Question> Questions = new List<Question>();

            foreach (Question q in App.Interview.Questions)
            {
                if (q.ID == question.ID)
                    continue;

                if (q.Response is StringChoice)
                {
                    if ((q.Response as StringChoice).Responses == srs)
                    {
                        Questions.Add(q);
                        continue;
                    }
                }

                if (q.Constraints.Intersect(constraints).Count() > 0)
                    Questions.Add(q);
            }

            return Questions;
        }

        public static StringResponseSet GetStringResponseSet(List<string> responseLabels)
        {
            
            foreach (StringResponseSet sts in App.Interview.StringResponseSets)
            {

                if (sts.StringResponses.Count == responseLabels.Count)
                {
                    bool foundIt = true;

                    for (int i = 0; i < responseLabels.Count; i++)
                    {
                        foundIt = foundIt && sts.StringResponses[i] == responseLabels[i];
                    }

                    if (foundIt)
                        return sts;
                }
            }

            StringResponseSet newSTS = new StringResponseSet();
            newSTS.StringResponses = newSTS.StringResponses.Concat(responseLabels).ToList();
            App.Interview.StringResponseSets.Add(newSTS);
            return newSTS;
        }
    }
}

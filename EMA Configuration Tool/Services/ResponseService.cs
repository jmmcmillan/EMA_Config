using System;
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



        public static StringResponseSet GetStringResponseSet(List<string> responseLabels)
        {
            foreach (StringResponseSet sts in App.Interview.StringResponseSets)
            {
                bool foundIt = true;

                foreach (string s in responseLabels)
                {
                    foundIt = foundIt && sts.StringResponses.Contains(s);
                }

                if (foundIt)
                    return sts;
            }

            StringResponseSet newSTS = new StringResponseSet();
            newSTS.StringResponses.Concat(responseLabels);
            return newSTS;
        }
    }
}

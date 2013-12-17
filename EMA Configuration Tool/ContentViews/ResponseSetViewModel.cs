using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using EMA_Configuration_Tool.Model.Responses;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ResponseSetViewModel : Screen
    {
        private StringResponseSet unchangedSTS;
        private Question question;
      
        public string Responses { get; set; }
        public bool StartsWithZero { get; set; }
        public bool StartsWithOne { get; set; }

        public ResponseSetViewModel(StringResponseSet sts, Question q)
            : this(q)
        {
            unchangedSTS = sts;           

            foreach (string s in sts.StringResponses)
            {
                Responses += String.Format("{0}\n", s);
            }

            StartsWithZero = sts.IsZeroBased;
            StartsWithOne = !StartsWithZero;
        }



        public ResponseSetViewModel(Question q)
            : base()
        {
            question = q;

            Responses = "";

            StartsWithZero = true;
        }

        public void Save(object sender)
        {
            List<string> responseList = Responses.Trim().Split('\n').Select(s => s.Trim()).ToList();
            
            //editing existing
            if (unchangedSTS != null)
            {
                unchangedSTS.StringResponses = responseList;
                unchangedSTS.IsZeroBased = StartsWithZero;
               
                unchangedSTS.Refresh();  //to update existing Question View listbox
            }

            //creating new
            else
            {
                StringResponseSet responseSet = new StringResponseSet();
                responseSet.StringResponses = responseList;
                responseSet.IsZeroBased = StartsWithZero;

                App.Interview.StringResponseSets.Add(responseSet);

                (question.Response as StringChoice).Responses = responseSet;

                
            }

            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}

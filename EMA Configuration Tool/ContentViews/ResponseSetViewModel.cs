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
        //private StringResponseSet unchangedSTS;

        private StringChoice unchangedSC;

        public string Responses { get; set; }
        public bool StartsWithZero { get; set; }
        public bool StartsWithOne { get; set; }

        //public ResponseSetViewModel(StringResponseSet sts) : this()
        //{
        //    unchangedSTS = sts;

        //    foreach (string s in sts.StringResponses)
        //    {
        //        Responses += String.Format("{0}\n", s);
        //    }

        //    StartsWithZero = sts.IsZeroBased;
        //    StartsWithOne = !sts.IsZeroBased;
        //}

        public ResponseSetViewModel(StringChoice sc)
            : this()
        {

            unchangedSC = sc;

            foreach (string s in sc.Responses.StringResponses)
            {
                Responses += String.Format("{0}\n", s);
            }

            StartsWithZero = sc.Responses.IsZeroBased;
            StartsWithOne = sc.Responses.IsZeroBased;
        }

        public ResponseSetViewModel() : base()
        {
            Responses = "";

            StartsWithZero = true;
        }

        public void Save(object sender)
        {
            ObservableCollection<string> responseList = new ObservableCollection<string>();

            foreach (string s in Responses.Trim().Split('\n').Select(s => s.Trim()).ToList())
                responseList.Add(s);
            
            //editing existing
            if (unchangedSC != null)
            {
                unchangedSC.Responses.StringResponses = responseList;
                unchangedSC.Responses.IsZeroBased = StartsWithZero;
            }

            else
            {
                StringResponseSet responseSet = new StringResponseSet();
                responseSet.StringResponses = responseList;
                responseSet.IsZeroBased = StartsWithZero;

                App.Interview.StringResponseSets.Add(responseSet);
            }

            TryClose();
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}

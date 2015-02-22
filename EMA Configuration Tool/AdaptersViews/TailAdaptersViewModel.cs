using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Adapters;

namespace EMA_Configuration_Tool.AdaptersViews
{
    public class TailAdaptersViewModel
    {
        public List<TailAdapterBase> TailAdapters { get; set; }

        public TailAdaptersViewModel()
        {
            InterviewType interviewType = App.Interview.EMAType;

            if (!App.AdapterKnowledge.AdapterLists.ContainsKey(interviewType))
                return;

            TailAdapters = App.AdapterKnowledge.AdapterLists[App.Interview.EMAType].SelectMany(l => l).OfType<TailAdapterBase>().ToList();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public class PreexistingKnowledge
    {

        public Dictionary<InterviewType, List<List<AdapterBase>>> AdapterLists;

        public List<TailAdapterBase> TailAdapters;

        public PreexistingKnowledge()
        {
            SalivaContentAdapter saliva = new SalivaContentAdapter();
            NonMonBODAdapter nonMonBOD = new NonMonBODAdapter();
            FinalDayAdapter final = new FinalDayAdapter();
            NonMonEODAdapter nmEOD = new NonMonEODAdapter();
            MonEODAdapter mEOD = new MonEODAdapter();

            //refactor so this doesn't need to be updated manually
            TailAdapters = new List<TailAdapterBase>() { nonMonBOD, final, nmEOD, mEOD };

            AdapterLists = new Dictionary<InterviewType, List<List<AdapterBase>>>();

            //BOD
            List<List<AdapterBase>> bodLists = new List<List<AdapterBase>>()
            {
                new List<AdapterBase> { saliva },
                new List<AdapterBase> { nonMonBOD },
                new List<AdapterBase> { final }
            };
            AdapterLists.Add(InterviewType.BOD, bodLists);

            
            //EOD
            List<List<AdapterBase>> eodLists = new List<List<AdapterBase>>()
            {
                new List<AdapterBase> { saliva, mEOD },                
                new List<AdapterBase> { nmEOD }
            };
            AdapterLists.Add(InterviewType.EOD, eodLists);

            
            //Hourly
            List<List<AdapterBase>> hrlyLists = new List<List<AdapterBase>>()
            {
                new List<AdapterBase> { saliva },                
            };
            AdapterLists.Add(InterviewType.Hourly, hrlyLists);


            //30 minute post BOD 
            List<List<AdapterBase>> halfHrLists = new List<List<AdapterBase>>()
            {
                new List<AdapterBase> { saliva },                
            };
            AdapterLists.Add(InterviewType.HalfHour, halfHrLists);

            
        }

        public List<List<AdapterBase>> GetAdapterListsFor(InterviewType interviewType)
        {
            if (AdapterLists.ContainsKey(interviewType))
            {
                return AdapterLists[interviewType];
            }
            else return new List<List<AdapterBase>>();
        }

    }
}

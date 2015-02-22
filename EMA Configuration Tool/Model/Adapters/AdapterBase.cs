using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.Model.Adapters
{
    public abstract class AdapterBase : PropertyChangedBase
    {
        public List<Question> PreQuestions {get; set;} 
        public virtual List<Question> PostQuestions {get; set;}

        public abstract string FileSuffix { get; }

        public AdapterBase()
        {
            PreQuestions = new List<Question>();
            PostQuestions = new List<Question>();
        }

        public void AdaptInterview()
        {
            //was using the perfectly nice List.InsertRange method until switching to observableCollection 
            //for the up/down buttons on CV.xaml -- find replacement?
            ObservableCollection<Question> newQuestions = new ObservableCollection<Question>();

            foreach (Question q in PreQuestions)
                newQuestions.Add(q);

            foreach (Question q in App.Interview.Questions)
                newQuestions.Add(q);

            foreach (Question q in PostQuestions)
                newQuestions.Add(q);

            App.Interview.Questions = newQuestions;
        }

        public void RevertInterview()
        {
            IEnumerable<Question> allQuestions = PreQuestions.Concat(PostQuestions);

            foreach (Question q in allQuestions)
            {
                App.Interview.Questions.Remove(q);
            }
        }
    }
}

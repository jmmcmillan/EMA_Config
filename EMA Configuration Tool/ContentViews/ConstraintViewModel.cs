﻿using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;
using System.Windows.Controls;
using System.Windows;
using EMA_Configuration_Tool.Model.Responses;
using EMA_Configuration_Tool.Model.Constraints;
using System.Collections.ObjectModel;
using EMA_Configuration_Tool.Model.Groups;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ConstraintViewModel : Screen
    {
        private Constraint unchangedConstraint;

        public ObservableCollection<BindableBool> SelectedResponses { get; set; }
        public List<string> ResponseStrings { get; set; }

        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                selectedQuestion = value;
                NotifyOfPropertyChange(() => SelectedQuestion);

                if (selectedQuestion.Response is StringChoice)
                {
                    ResponseStrings = (selectedQuestion.Response as StringChoice).Responses.StringResponses;
                }

                initData();

                NotifyOfPropertyChange(() => ResponseStrings);
                NotifyOfPropertyChange(() => SelectedResponses);

            }
        }

        public bool ShowIfSkipped { get; set; }

        public ConstraintViewModel(Constraint constraint)
            :this()
        {
            unchangedConstraint = constraint;

            SelectedQuestion = App.Interview.Questions.Where(q => q.ID == unchangedConstraint.FollowupForGuid).FirstOrDefault();

            ShowIfSkipped = (constraint as StringConstraint).FollowupValueIndexes.Contains(-1);
                   
        }

        private void initData()
        {
            for (int i = 0; i < (SelectedQuestion.Response as StringChoice).Responses.StringResponses.Count; i++)
            {
                if (unchangedConstraint != null)
                {
                    if ((unchangedConstraint as StringConstraint).FollowupValueIndexes.Contains(i))
                    {
                        SelectedResponses.Add(new BindableBool(true));
                    }
                    else SelectedResponses.Add(new BindableBool(false));
                }
                else SelectedResponses.Add(new BindableBool(false));
            }

            
        }

        public ConstraintViewModel()
            : base()
        {
            this.DisplayName = "Add/Edit Constraint";

            SelectedResponses = new ObservableCollection<BindableBool>();
            ResponseStrings = new List<string>();

        }
     
        
        public void Save()
        {  
            List<int> indexList = new List<int>();
            int i = 0;
            foreach (BindableBool b in SelectedResponses)
            {
                if (b.Value)
                    indexList.Add(i);

                i++;
            }

            if (ShowIfSkipped)
                indexList.Insert(0, -1);

            if (unchangedConstraint != null)
            {
                (unchangedConstraint as StringConstraint).FollowupForGuid = SelectedQuestion.ID;
                (unchangedConstraint as StringConstraint).FollowupValueIndexes = indexList;
            }

            else
            {
                StringConstraint sc = new StringConstraint(SelectedQuestion.ID, indexList);
                App.Interview.Constraints.Add(sc);
            }

            TryClose();
        }

     
        public void Cancel()
        {
            TryClose();
        }

    
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Groups;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace EMA_Configuration_Tool.Groups
{
    public class PersonViewModel : PropertyChangedBase
    {
        public Person Person { get; set; }

        public ObservableCollection<BindableBool> GroupsForBinding { get; set; }

        private string previousPersonName = "";

        public PersonViewModel()
        {
            Person = new Person();
            initGroupData();
        }


        public PersonViewModel(Person p)
        {
            Person = p;
            previousPersonName = p.Name;
            initGroupData();
        }

        private void initGroupData()
        {
            GroupsForBinding = new ObservableCollection<BindableBool>();

            foreach (bool b in Person.GroupMembership)
            {
                GroupsForBinding.Add(new BindableBool(b));
            }

            

        }

        private bool okayToSave()
        {
            if (String.IsNullOrEmpty(Person.Name))
            {
                MessageBox.Show(String.Format("Please enter a name for the person."));
                return false;
            }

            foreach (Person p in App.Interview.People)
            {
                if (p.Name == Person.Name)
                {
                    if (p.ID != Person.ID)
                    {
                        MessageBox.Show(String.Format("A person named {0} already exists. Please enter a different name.", Person.Name));
                        return false;
                    }
                }
            }

            if (GroupsForBinding.Where(b => b.Value).Count() < 1)
            {
                MessageBox.Show("No groups are selected. Please select 1 or more groups for this person.", "No Groups Selected", MessageBoxButton.OK);
                return false;
            }


            return true;

        }

        public void SavePerson()
        {
            if (!okayToSave())
                return;

            bool[] newMemberships = new bool[EMAInterview.TopLevelSocialGroupNames.Count()];

            int i = 0;
            foreach (BindableBool b in GroupsForBinding)
            {
                newMemberships[i] = b.Value;
                i++;
            }

            bool found = false;                       
            foreach (Person p in App.Interview.People)
            {
                if (p.Name == Person.Name)
                {
                    p.GroupMembership = newMemberships;
                    found = true;
                    break;
                }

            }

            if (!found)
            {
                Person.GroupMembership = newMemberships;
                App.Interview.People.Insert(0, Person);
            }

            App.EventAggregator.Publish(new PeopleViewModel());
        }

       

        public void Cancel()
        {
            //restore previous person name
            Person.Name = previousPersonName;

            App.EventAggregator.Publish(new PeopleViewModel());
        }

    }
}

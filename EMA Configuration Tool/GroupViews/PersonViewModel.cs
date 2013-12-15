using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Groups;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;
using System.Collections.ObjectModel;

namespace EMA_Configuration_Tool.Groups
{
    public class PersonViewModel : PropertyChangedBase
    {
        public Person Person { get; set; }

        public ObservableCollection<BindableBool> GroupsForBinding { get; set; }

        public PersonViewModel()
        {
            Person = new Person();
            initGroupData();
        }


        public PersonViewModel(Person p)
        {
            Person = p;
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
                System.Windows.MessageBox.Show(String.Format("Please enter a name for the person."));
                return false;
            }

            foreach (Person p in App.Interview.People)
            {
                if (p.Name == Person.Name)
                {
                    if (p.ID != Person.ID)
                    {
                        System.Windows.MessageBox.Show(String.Format("Another person with the name {0} already exists. Please choose a different name.", Person.Name));
                        return false;
                    }
                }
            }

            return true;

        }

        public void SavePerson()
        {
            if (!okayToSave())
                return;

            bool[] newMemberships = new bool[EMAInterview.SocialGroupNames.Count()];

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

            Cancel();
        }

       

        public void Cancel()
        {
            App.EventAggregator.Publish(new PeopleViewModel());
        }

    }
}

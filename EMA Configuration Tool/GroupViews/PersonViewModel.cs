using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Groups;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;

namespace EMA_Configuration_Tool.Groups
{
    public class PersonViewModel
    {
        public Person Person { get; set; }

        public PersonViewModel()
        {
            Person = new Person();
        }


        public PersonViewModel(Person p)
        {
            Person = p;
        }

        public void SavePerson()
        {
            bool[] newMemberships = new bool[EMAInterview.SocialGroupNames.Count()];

            int i = 0;
            foreach (PersonGroup pg in Person.GroupsForBinding)
            {
                if (pg.IsMember)
                {
                    newMemberships[i] = true;
                }

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

        public void Checked(object source)
        {
            if (source is PersonView)
            {
                PersonView pv = source as PersonView;

                foreach (object o in pv.GroupMemberships.Items)
                {

                }

            }

        }

        public void Cancel()
        {
            App.EventAggregator.Publish(new PeopleViewModel());
        }

    }
}

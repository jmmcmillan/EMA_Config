using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Groups;
using Caliburn.Micro;
using EMA_Configuration_Tool.Model;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.VisualBasic;

namespace EMA_Configuration_Tool.Groups
{
    public class PersonViewModel : PropertyChangedBase
    {
        public Person Person { get; set; }

        public ObservableCollection<PersonGroup> DefaultGroups { get; set; }
        public ObservableCollection<PersonGroup> CustomGroups { get; set; }

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
            DefaultGroups = new ObservableCollection<PersonGroup>();

            foreach (Group g in SocialNetwork.SocialGroups)
            {
                if (g.Names.Equals("Group member"))
                    continue;

                if (Person.MyGroups.Contains(g))
                    DefaultGroups.Add(new PersonGroup(g, true));
                else DefaultGroups.Add(new PersonGroup(g, false));
            }

            CustomGroups = new ObservableCollection<PersonGroup>();

            foreach (Group g in App.Network.CustomSocialGroups)
            {
                if (Person.MyGroups.Contains(g))
                    CustomGroups.Add(new PersonGroup(g, true));
                else CustomGroups.Add(new PersonGroup(g, false));
            }
            

        }

        private bool okayToSave()
        {
            if (String.IsNullOrEmpty(Person.Name))
            {
                MessageBox.Show(String.Format("Please enter a name for the person."));
                return false;
            }

            foreach (Person p in App.Network.People)
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

            if (NoGroupsSelected)
            {
                MessageBox.Show("No groups are selected. Please select 1 or more groups for this person.", "No Groups Selected", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        private bool NoGroupsSelected
        {
            get
            {
                foreach (PersonGroup pg in DefaultGroups)
                {
                    if (pg.IsMember)
                        return false;
                }

                foreach (PersonGroup pg in CustomGroups)
                {
                    if (pg.IsMember)
                        return false;
                }

                return true;
            }
        }

        public void SavePerson()
        {
            if (!okayToSave())
                return;

            SetGroupMembership(Person);

            if (App.Network.People.Where(p => p.ID == Person.ID).Count() < 1)
                App.Network.People.Insert(0, Person);

            App.EventAggregator.Publish(new PeopleViewModel());
        }

       private void SetGroupMembership(Person p)
       {
           p.MyGroups = new List<Group>();

           foreach (PersonGroup pg in DefaultGroups)
           {
               if (pg.IsMember)
                   p.MyGroups.Add(pg.Group);
           }

           foreach (PersonGroup pg in CustomGroups)
           {
               if (pg.IsMember)
                   p.MyGroups.Add(pg.Group);
           }
       }

        public void Cancel()
        {
            //restore previous person name; without calling SetGroupMembership the person's original groups haven't been modified
            Person.Name = previousPersonName;

            App.EventAggregator.Publish(new PeopleViewModel());
        }

        public void AddCustomGroup()
        {
            string description = Interaction.InputBox("Enter the name of the custom social group");

            if (description != String.Empty)
            {
                Group newGroup = new Group(description);

                App.Network.CustomSocialGroups.Add(newGroup);
                App.Network.RefreshCustomGroupResponseSet();

                CustomGroups.Add(new PersonGroup(newGroup, false));
                
            }
        }

        public void DeleteCustomGroup(object dataContext)
        {
            PersonGroup pg = dataContext as PersonGroup;

            if (MessageBox.Show(String.Format("Are you sure you want to delete the group {0}?", pg.Group.GroupName), "Delete Group", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                return;

            foreach (Person p in App.Network.People)
                p.MyGroups.Remove(pg.Group);

            App.Network.CustomSocialGroups.Remove(pg.Group);

            CustomGroups.Remove(pg);

            
        }

    }
}

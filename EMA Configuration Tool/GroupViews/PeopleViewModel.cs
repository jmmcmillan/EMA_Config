using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Groups;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Windows;

namespace EMA_Configuration_Tool.Groups
{
    public class PeopleViewModel : PropertyChangedBase
    {

        public Person SelectedPerson { get; set; }

        public PeopleViewModel()
        {
            

            
        }

        public void AddPerson()
        {
            App.EventAggregator.Publish(new PersonViewModel());
        }

        public void EditPerson(object view)
        {
            if (view != null)
            {
                PeopleView pv = (view as PeopleView);
                object person = pv.PersonDataGrid.SelectedItem;

                if (person is Person)
                    App.EventAggregator.Publish(new PersonViewModel(person as Person));
            }

        }

        public void DeletePerson()
        {
            if (SelectedPerson != null)
            {
                
                if (MessageBox.Show(String.Format("Are you sure you want to delete {0}?", SelectedPerson.Name), "Delete Person", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    App.Interview.People.Remove(SelectedPerson);
                }
                
            }
        }


    }
}

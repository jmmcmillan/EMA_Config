using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Groups;
using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace EMA_Configuration_Tool.Groups
{
    public class PeopleViewModel : PropertyChangedBase
    {
        

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
        }


    }
}

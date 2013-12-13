using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using EMA_Configuration_Tool.Groups;

namespace EMA_Configuration_Tool.GroupViews
{
    public class GroupShellViewModel : Conductor<object>, IHandle<PeopleViewModel>, IHandle<PersonViewModel>
    {
        public GroupShellViewModel()
        {
            App.EventAggregator.Subscribe(this);

            Handle(new PeopleViewModel());
        }

        public void Handle(PeopleViewModel pvm)
        {
            ActivateItem(pvm);
        }

        public void Handle(PersonViewModel pvm)
        {
            ActivateItem(pvm);
        }
    }
}

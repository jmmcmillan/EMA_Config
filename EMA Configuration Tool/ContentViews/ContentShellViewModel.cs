using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ContentShellViewModel : Conductor<object>, IHandle<QuestionViewModel>, IHandle<ContentViewModel>
    {
        public string TabName
        {
            get { return "Content"; }
        }

        public ContentShellViewModel()
        {
            App.EventAggregator.Subscribe(this);

            Handle(new ContentViewModel());

            
        }

        public void Handle(ContentViewModel cvm)
        {
            ActivateItem(cvm);
        }

        public void Handle(QuestionViewModel qvm)
        {
            ActivateItem(qvm);
        }


       

    }
}

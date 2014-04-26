using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model.Help;
using System.Windows;

namespace EMA_Configuration_Tool
{
    public class HelpContentViewModel
    {
        public void AddPage()
        {
            string pageTitle = Microsoft.VisualBasic.Interaction.InputBox("Please enter the page title", "Page Title");
            pageTitle = pageTitle.Trim();

            //blank title
            if (String.IsNullOrEmpty(pageTitle))
                return;

            //duplicate title
            if (App.HelpContents.HelpPages.Where(hp => hp.Title.Equals(pageTitle)).Count() > 0)
            {
                MessageBox.Show(String.Format("A page with the title {0} already exists.", pageTitle),"Duplicate Page Title");
                return;
            }

            HelpPage newPage = new HelpPage() { Title = pageTitle };

            App.HelpContents.HelpPages.Add(newPage);
        }

        public void DeletePage(object dataContext)
        {   
            if (dataContext is HelpPage)
            {
                HelpPage page = dataContext as HelpPage;
                if (MessageBox.Show(String.Format("Are you sure you want to delete the {0} help page?", page.Title ), "Delete Help Page", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                    return;

                else App.HelpContents.HelpPages.Remove(page);
            }
        }

    }
}

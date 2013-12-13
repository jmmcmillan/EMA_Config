﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMA_Configuration_Tool.Model;
using Caliburn.Micro;

namespace EMA_Configuration_Tool.ContentViews
{
    public class ResponseSetViewModel : Screen
    {   
        public ResponseSetViewModel()
        {
     
        }

        public void Save(object sender)
        {
            object senderContent = (sender as System.Windows.Window).Content;

            if (senderContent is ResponseSetView)
            {
                ResponseSetView view = senderContent as ResponseSetView;

                    StringResponseSet responseSet = new StringResponseSet();

                    responseSet.StringResponses = view.ResponseLines.Text.Trim().Split('\n').Select(s => s.Trim()).ToList();

                    responseSet.IsZeroBased = (bool)(view.StartZero.IsChecked) ? true : false;

                    App.Interview.StringResponseSets.Add(responseSet);

                    TryClose();
                
            }
            
        }

        public void Cancel()
        {
            TryClose();
        }
    }
}
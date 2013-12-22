﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EMA_Configuration_Tool.ContentViews
{
    /// <summary>
    /// Interaction logic for QuestionView.xaml
    /// </summary>
    public partial class QuestionView : UserControl
    {
        public QuestionView()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(QuestionView_Loaded);
        }

        void QuestionView_Loaded(object sender, RoutedEventArgs e)
        {
            qlabelTextBox.Focus();
        }

        
    }
}

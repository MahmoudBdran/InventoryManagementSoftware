﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvntoryManagementSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for MyTextBox.xaml
    /// </summary>
    public partial class MyTextBox : UserControl
    {
        public MyTextBox()
        {
            InitializeComponent();
        }
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }

        public static  DependencyProperty HintProperty = DependencyProperty.Register("Hint", typeof(string), typeof(MyTextBox));

        public string CustomText
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }
        public static DependencyProperty CustomTextProperty = DependencyProperty.Register("CustomText", typeof(string), typeof(MyTextBox));





        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(MyTextBox));
        public string ErrorCaption
        {
            get { return (string)GetValue(ErrorCaptionProperty); }
            set { SetValue(ErrorCaptionProperty, value); }
        }

        public static DependencyProperty ErrorCaptionProperty = DependencyProperty.Register("ErrorCaption", typeof(string), typeof(MyTextBox));

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            textBox.SelectAll();
        }
    }
}
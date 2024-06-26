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
using System.Windows.Shapes;

namespace Mailer_For_Business.Windows.Dash
{
    /// <summary>
    /// Interaction logic for SendMessageBox.xaml
    /// </summary>
    public partial class SendMessageBox : Window
    {
        public SendMessageBox()
        {
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle OK button click
            DialogResult = true; // Set dialog result to true
            this.Close();
        }
        public void Settext(String title, String msg)
        {
            titletext.Text = title;
            massagetext.Text = msg;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle cancel button click
            DialogResult = false; // Set dialog result to false
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Interaction logic for UrlInputWindow.xaml
    /// </summary>
    public partial class UrlInputWindow : Window
    {

        public string EnteredUrl { get; private set; }
        public UrlInputWindow()
        {
            InitializeComponent();
        }
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            EnteredUrl = urlTextBox.Text;
            bool isValid = IsValidUrl(EnteredUrl);
            if (isValid) { DialogResult = true; } else { errortxt.Content = "Invailed URL"; }
            // Close the window with a true result
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Close the window with a false result
        }
        public static bool IsValidUrl(string url)
        {
            // Check if the URL is well-formed
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                // Check if the URL is a valid HTTP or HTTPS URL
                Uri uri = new Uri(url);
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }
            else
            {
                return false;
            }
        }

        private void urlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnteredUrl = urlTextBox.Text;
            bool isValid = IsValidUrl(EnteredUrl);
            if (isValid) { 
                    BitmapImage bitmapImage = new BitmapImage(new Uri(EnteredUrl));

                // Set the source of the Image control to the BitmapImage
                prvimagebox.Source = bitmapImage;


            } else {  }
        }
    }
}

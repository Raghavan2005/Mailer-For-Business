using System;
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

namespace Mailer_For_Business.Windows.Dash.controls
{
    /// <summary>
    /// Interaction logic for designselector.xaml
    /// </summary>
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class designselector : UserControl




#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        bool tempsel;
        List<string> headerItems = new List<string>
               {
    "Blue Header (Light Mode)",
    "Blue Header (Dark Mode)",
    "Red Header (Light Mode)"
                   }; 

        public designselector()
        {
            InitializeComponent();


            // Assign the list as the ItemsSource of the ComboBox
            tepsel.ItemsSource = headerItems;
            tepsel.SelectedIndex = 0;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
       
            if (tempsel == true)
            {
                tepsel.IsEnabled = true;
                tempsel = true;
            }
            else
            {
                tepsel.IsEnabled = false;
                tempsel = false;
            }
        }

        public bool getusetemp()
        {
            return tempsel;
        }
        private void tepsel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectimage();
        }

       public string setcurrentslectiondata()
        {
            if(tempsel == true) {

                return tepsel.SelectedValue.ToString();

            } else



            {

                return "none";

            }


        }
        void selectimage()
        {
            
            if ("Blue Header (Light Mode)" == tepsel.SelectedValue.ToString())
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(@"\Windows\Dash\controls\header\bluelight.jpg", UriKind.RelativeOrAbsolute));
                selectedimage.Source = bitmapImage;
            }
            else if ("Blue Header (Dark Mode)" == tepsel.SelectedValue.ToString())
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(@"\Windows\Dash\controls\header\bluedark.jpg", UriKind.RelativeOrAbsolute));
                selectedimage.Source = bitmapImage;
            }
            else if ("Red Header (Light Mode)" == tepsel.SelectedValue.ToString())
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(@"\Windows\Dash\controls\header\red.jpg", UriKind.RelativeOrAbsolute));
                selectedimage.Source = bitmapImage;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(@"\Windows\Dash\preview.png", UriKind.RelativeOrAbsolute));
                selectedimage.Source = bitmapImage;
            }

        }

       
    }
}

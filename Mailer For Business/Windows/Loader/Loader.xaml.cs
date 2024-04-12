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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using Mailer_For_Business.Windows.Dash;
namespace Mailer_For_Business.Windows
{
    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    ///
  
    public partial class Loader : Window
    {
        Dashboard dashboard;
        int loadingtime = 4;
        public Loader()
        {
            InitializeComponent();
             dashboard = new Dashboard();  
            appversion.Text = "v1.0-Beta";
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, loadingtime);
            dispatcherTimer.Start();

        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dashboard.Show();
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Runtime.InteropServices.JavaScript;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using Microsoft.Win32;
using System.Diagnostics;

namespace Mailer_For_Business.Windows.Dash
{
    /// <summary>
    /// Interaction logic for settings.xaml
    /// </summary>
    public partial class settings : Window
    {
        CustomMessageBox messageBox;
        string hostname, username, password, serndername, frommail;
        int? port;
        bool scren;
        loading ld;
        public settings()
        {
            InitializeComponent();
            messageBox = new CustomMessageBox();
         
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            portbox.Items.Add(25);
            portbox.Items.Add(587);
            portbox.Items.Add(465);
            portbox.Items.Add(2525);
            portbox.SelectedIndex = 0;
            configfoundandload();

         
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(hostname) && port != null && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)
                && hostname != "Ex : smtp.example.in" && username != "Ex : Example@gmail.com" && password != "Ex : Your Password" && !string.IsNullOrEmpty(serndername) && serndername != "Ex : Shailesh or Mati Solutions" && !string.IsNullOrEmpty(frommail) && frommail != "Ex : Example@gmail.com")
            {
                allsaveconfig(hostname, port.Value, scren, username, password, serndername);
            }
            this.Close();
          
        }
     
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
             hostname = smtphost.Text.Trim();
             port = portbox.SelectedItem as int?;
             username = smtpusername.Text.Trim();
             password = smtppassword.Text;
             serndername = smtpsendername.Text.ToString();
             scren = securecheck.IsEnabled;
         

            if (!string.IsNullOrEmpty(hostname) && port != null && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)
                && hostname != "Ex : smtp.example.in" && username != "Ex : Example@gmail.com" && password != "Ex : Your Password" && !string.IsNullOrEmpty(serndername) && serndername != "Ex : Shailesh or Mati Solutions" )
            {
                smtpfrommail.Content = username;
                if (ValidateSMTPServer(hostname, port.Value, username, password))
                {
                  //  ld?.Close();
                    // Check if the messageBox is null or closed 
                    if (messageBox == null || !messageBox.IsVisible)
                    {
                        messageBox = new CustomMessageBox();
                    }

                    allsaveconfig(hostname, port.Value, scren, username, password, serndername);
                    messageBox.Settext("Success", "SMTP server is valid. Click Ok To Restart The Application");
                    
                    sendtestmail.IsEnabled = true;
                    testmailid.IsEnabled = true;
                    bool? result = messageBox.ShowDialog();
                  if (result == true)
                    {
                        RestartApplication();
                    }
                }
            }
            else
            {
              //  ld?.Close();
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }

                messageBox.Settext("Input Not Found", "Invalid");
                messageBox.ShowDialog();
            }
        }

        private void RestartApplication()
        {
            // Get the path to the current executable
            string appPath = Process.GetCurrentProcess().MainModule.FileName;

            // Start a new instance of the application
            Process.Start(appPath);

            // Close the current instance
            Application.Current.Shutdown();
        }


        private void sendtestmail_Click(object sender, RoutedEventArgs e)
        {
            sendtxtmail();


        }
        ///
        private bool ValidateSMTPServer(string smtpServer, int port, string username, string password)
        {
            try
            {
                // Setup SmtpClient
                SmtpClient SmtpServer = new SmtpClient(smtpServer);
                SmtpServer.Port = port;
                SmtpServer.Credentials = new NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;

                // Create MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username);

                // Validate email recipient
                if (!username.Contains("@"))
                {
                    // Display error message if email address is invalid
                    if (messageBox == null || !messageBox.IsVisible)
                    {
                        messageBox = new CustomMessageBox();
                    }
                    messageBox.Settext("Error", "Please enter an actual email address");
                    messageBox.ShowDialog();
                    return false;
                }

                mail.To.Add(username);


                // Set mail subject and body
                mail.Subject = "Mailer Testing and Account Validation Mail";
                mail.IsBodyHtml = true;
                string htmlBody;
                string htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "testmail.html");

                using (StreamReader reader = new StreamReader(htmlFilePath))
                {
                    htmlBody = reader.ReadToEnd();
                }
                htmlBody = htmlBody.Replace("{IP_ADDRESS}", getipaddress());

                //
                mail.Body = htmlBody;
                messageBox.Settext("Connection Status", "Sending...\nPlease close this and wait for sent confirmation.");
                messageBox.ShowDialog();
                SmtpServer.Send(mail);
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                //  ld.Close();
                return true;
            }
            catch (FormatException ex)
            {
               // ld?.Close();
                // Display error message for invalid email format
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                messageBox.Settext("Invalid Email Format", "Please check the email address format");
                messageBox.ShowDialog();
                return false;
            }
            catch (SmtpException ex)
            {
                ld?.Close();
                // Display error message for SMTP exception
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                messageBox.Settext("SMTP Error", "Invalid port or hostname. Try using other ports as some servers may block it");
                messageBox.ShowDialog();
                return false;
            }
            catch (Exception ex)
            {
                ld?.Close();
                // Display generic error message
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                messageBox.Settext("Error", "An error occurred. Please contact support for assistance.\n\n" + ex.ToString());
                messageBox.ShowDialog();
                return false;
            }
        }



        private void smtphostclearbtn(object sender, RoutedEventArgs e)
        {
            smtphost.Clear();
        }

        private void smtpusernameclearbtn(object sender, RoutedEventArgs e)
        {
            smtpusername.Clear();
        }

        private void smtpsendernameclearbtn(object sender, RoutedEventArgs e)
        {
            smtpsendername.Clear();
        }

        

        private void smtppasswordclearbtn(object sender, RoutedEventArgs e)
        {
            smtppassword.Clear();
        }

        ///

        void saveconfig(string Key,string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[Key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
        void allsaveconfig(string smtpServer, int port,bool secure,string username, string password,string sendername)
        {
            saveconfig("hostname", smtpServer);
            saveconfig("port", port.ToString());
            saveconfig("secure", secure.ToString());
            saveconfig("username", username);
            saveconfig("password", password);
            saveconfig("sendername", sendername);
            

        }
        bool configfoundandload()
        {
            string hostname = ConfigurationManager.AppSettings["hostname"];
            if (hostname != "null") {

                string port = ConfigurationManager.AppSettings["port"];
                string secure = ConfigurationManager.AppSettings["secure"];
                string username = ConfigurationManager.AppSettings["username"];
                string password = ConfigurationManager.AppSettings["password"];
                string sendername = ConfigurationManager.AppSettings["sendername"];
                string maildelay = ConfigurationManager.AppSettings["maildelay"];
                string mailcolumn = ConfigurationManager.AppSettings["mailcolumn"];
                smtphost.Text = hostname;
                this.hostname = hostname;
                smtpfrommail.Content= username;
                this.username = username;
                portbox.SelectedItem = Convert.ToInt32(port);
                this.port = Convert.ToInt32(port);
                securecheck.IsChecked = Convert.ToBoolean(secure);
                this.scren= Convert.ToBoolean(secure);
                smtpusername.Text = username;
                smtppassword.Text = password;
                smtpsendername.Text = sendername;
                delay.Text = maildelay;
                columnindex.Text = mailcolumn;
                sendtestmail.IsEnabled = true;
                testmailid.IsEnabled = true;
                return true;
            }
            else
            {
                return false;
            }

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(hostname) && port != null && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password)
                && hostname != "Ex : smtp.example.in" && username != "Ex : Example@gmail.com" && password != "Ex : Your Password" && !string.IsNullOrEmpty(serndername) && serndername != "Ex : Shailesh or Mati Solutions" && !string.IsNullOrEmpty(frommail) && frommail != "Ex : Example@gmail.com")
            {
                allsaveconfig(hostname, port.Value, scren, username, password, serndername);
            }
            
        }

        private void delay_TextChanged(object sender, TextChangedEventArgs e)
        {
            string delats = delay.Text.ToString();
          
            saveconfig("maildelay", delats);
           
        }

        private void columnindex_TextChanged(object sender, TextChangedEventArgs e)
        {
            string columnx = columnindex.Text.ToString();
            saveconfig("mailcolumn", columnx);
        }
        void sendtxtmail()
        {
  
            string tomail = testmailid.Text.ToString();
            
            if(tomail != "Enter the Test Mail" && !string.IsNullOrEmpty(tomail)) {
            
            try
            {
                //setup
                SmtpClient SmtpServer = new SmtpClient(this.hostname);
                SmtpServer.Port = port.Value;
                SmtpServer.Credentials = new System.Net.NetworkCredential(this.username, this.password);
                SmtpServer.EnableSsl = true;

                MailMessage mail = new MailMessage();
                //default from email
                mail.From = new MailAddress(this.username);

                //email recipient
                string addresses = tomail;
                if (!addresses.Contains("@"))
                {
                    // Display error message if email address is invalid
                    if (messageBox == null || !messageBox.IsVisible)
                    {
                        messageBox = new CustomMessageBox();
                    }
                    messageBox.Settext("Error", "Please enter an actual email address");
                    messageBox.ShowDialog();
                 
                }

                mail.To.Add(addresses);

                //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

                //email subject
                mail.Subject = "Mailer Testing and Account Validation Mail";

                //email attachments
                //if (string.IsNullOrEmpty(fileName.Text) == false)
                //{
                //    mail.Attachments.Add(new Attachment(fileName.Text));
                //}
                mail.IsBodyHtml = true;
                //email body
                //  string htmlBody = "<html><body style ='background-color: #f0f0f0; padding-left: 40px; ' ><br><h1 style = 'color: #333333; text-align: center; margin-top: 10px; padding-left: 80px;'> Mailer For Business</h1><br><p> This is a Test mail from Mailer For Business to verify SMTP information.</p>" +
                //   "<br><p> If you did not initiate this action, please follow these steps:</p><br><ol><li> Change your SMTP server password immediately.</li>"+
                //    "<li> Check your account settings for any suspicious activities.</li><li> Enable two - factor authentication for additional security.</li><li> Contact support if you need further assistance.</li></ol><p style='color:red;'> Sender public IP address: <b>" + getipaddress() + "</b></p><br><br></body></html>";
                //testing site
                string htmlBody;
                string htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/"+"htmltemp", "testmail.html");

                using (StreamReader reader = new StreamReader(htmlFilePath))
                {
                    htmlBody = reader.ReadToEnd();
                }
                htmlBody = htmlBody.Replace("{IP_ADDRESS}", getipaddress());

                //
                mail.Body = htmlBody;

                //send email
                messageBox.Settext("Connection Status","Sending...\nPlease close this and Check Your Mail.");
                messageBox.ShowDialog();
                SmtpServer.Send(mail);
           
            }
            catch (FormatException ex)
            {
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                messageBox.Settext("Invailed Email Format", "Pls Check The Mail id.\n\n");
                messageBox.ShowDialog();
     
              
            }
            catch (SmtpException ex)
            {

                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                messageBox.Settext("Invailed Port Or Hostname", "Invailed Port Try Other Ports Some Server Blocked it.\n\n"+ex.ToString());
                messageBox.ShowDialog();
               
              
            }
            catch (Exception ex)
            {
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                messageBox.Settext("Error", "An error occurred. Please contact support for assistance.\n\n" + ex.Message.ToString());
                messageBox.ShowDialog();
             
               
            }

            }
            else
            {
                if (messageBox == null || !messageBox.IsVisible)
                {
                    messageBox = new CustomMessageBox();
                }
                testmailid.Clear();
                messageBox.Settext("Invailed Input", "Please Enter the Vailded Input.\n\n");
                messageBox.ShowDialog();
            }

        }

        string getipaddress()
        {
            try
            {
                // Create a WebClient instance to make an HTTP request
                using (WebClient client = new WebClient())
                {
                    // Make an HTTP request to get the public IP address
                    string publicIPAddress = client.DownloadString("https://api.ipify.org");

                    // Return the public IP address
                    return publicIPAddress;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the request
                Console.WriteLine("Error getting public IP address: " + ex.Message);
                return "notFound";
            }
        }



    }
}

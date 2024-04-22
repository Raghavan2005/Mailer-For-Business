using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Windows;

namespace Mailer_For_Business.Windows.Dash
{
    internal class mailsender
    {
        private string hostname, username, password, sendername, frommail, maildelay, mailcolumn;
        private int port;
        private bool secure;
        SmtpClient SmtpServer;
        public mailsender()
        {
            gettheconfigfile();
            SmtpServer = new SmtpClient(hostname);
        }

        public void sendthemail(string subjectinp,string body,string imageurl , string selectedtep)
        {
            try
            {
                if (selectedtep == "none") { 
                SmtpServer.Port = port;
                SmtpServer.Credentials = new NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;

                // Create MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username);

                // Validate email recipient
                string addresses = username;
                foreach (var address in addresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!address.Contains("@"))
                    {
                       //invailed email here 
                    }
                    mail.To.Add(address);
                }

                // Set mail subject and body
                mail.Subject = subjectinp;
                mail.IsBodyHtml = true;
                string htmlBody = body;
                mail.Body = htmlBody;
              
          
                SmtpServer.Send(mail);
                }

            }
            
            catch (Exception ex)
            {
                //invailed are cant proccess mail id are here add this mailid in error
            }

        }



        private void gettheconfigfile()
        {
            hostname = ConfigurationManager.AppSettings["hostname"];
            if (hostname != "null")
            {
                if (int.TryParse(ConfigurationManager.AppSettings["port"], out int parsedPort))
                {
                    port = parsedPort;
                }
                else
                {
                    // Handle invalid port value
                }

                if (bool.TryParse(ConfigurationManager.AppSettings["secure"], out bool parsedSecure))
                {
                    secure = parsedSecure;
                }
                else
                {
                    // Handle invalid secure value
                }

                username = ConfigurationManager.AppSettings["username"];
                password = ConfigurationManager.AppSettings["password"];
                sendername = ConfigurationManager.AppSettings["sendername"];
                frommail = ConfigurationManager.AppSettings["frommail"];
                maildelay = ConfigurationManager.AppSettings["maildelay"];
                mailcolumn = ConfigurationManager.AppSettings["mailcolumn"];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.IO;

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

        public bool sendthemail(string subjectinp,string body,string imageurl , string selectedtep,string sendmailid,string bus,string footer)
        {
            try
            {
                if (selectedtep != "none") { 
                SmtpServer.Port = port;
                SmtpServer.Credentials = new NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;

                // Create MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username);

                // Validate email recipient
                string addresses = sendmailid;
                    if (!addresses.Contains("@"))
                    {
                       return false;//add on invaild mail id
                    }

                    mail.To.Add(addresses);

                    // Set mail subject and body
                    mail.Subject = subjectinp;
                mail.IsBodyHtml = true;
                string htmlBody = "<html><body>"+ body+ "</body></html>";
                mail.Body = htmlBody;
              
          
                SmtpServer.Send(mail);
                    return true;//success mail
                }
                else
                {
                    //useing temp
                    SmtpServer.Port = port;
                    SmtpServer.Credentials = new NetworkCredential(username, password);
                    SmtpServer.EnableSsl = true;

                    // Create MailMessage
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(username);

                    // Validate email recipient
                    string addresses = sendmailid;
                    if (!addresses.Contains("@"))
                    {
                        return false;//add on invaild mail id
                    }

                    mail.To.Add(addresses);

                    // Set mail subject and body
                    mail.Subject = subjectinp;
                    mail.IsBodyHtml = true;
                    string htmlBody = returnthetempstring(selectedtep, bus,footer,imageurl,body);
                    mail.Body = htmlBody;


                    SmtpServer.Send(mail);
                    return true;//success mail
                }

            }
            
            catch (Exception ex)
            {
                return false;//invaild mail added
            }

        }

        //    "Blue Header (Light Mode)",
  //  "Blue Header (Dark Mode)",
    //"Red Header (Light Mode)"

        string returnthetempstring(string currenttemp,string bname , string footername , string logoimage,string body)
        {
            string htmlBody, htmlFilePath;
            
                if (currenttemp == "Blue Header (Light Mode)")
                {
                    htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "blueheaderlightmode.html");
                }
                else if (currenttemp == "Blue Header (Dark Mode)")
                {
                    htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "blueheaderdarkmode.html");
                }
                else if (currenttemp == "Red Header (Light Mode)")
                {
                     htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "redheaderlightmode.html");
                }
                else if (currenttemp == "Red Header (Dark Mode)")
                {
                    htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "redheaderdarkmode.html");
                }
                else if (currenttemp == "Green Header (Light Mode)")
                {
                    htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "greenheaderlightmode.html");
                }
                else if (currenttemp == "Green Header (Dark Mode)")
                {
                    htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "greenheaderdarkmode.html");
                }
                else
                {
                     htmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Windows/Dash/" + "htmltemp", "testmail.html");
                }

                using (StreamReader reader = new StreamReader(htmlFilePath))
                {
                    htmlBody = reader.ReadToEnd();
                }
                htmlBody = htmlBody.Replace("{body}", body);
              htmlBody = htmlBody.Replace("{bname}", bname);
                htmlBody = htmlBody.Replace("{footername}", footername);
                htmlBody = htmlBody.Replace("{logoimage}", logoimage);

 return htmlBody;

           
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

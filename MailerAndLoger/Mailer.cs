using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailerAndLoger
{
    public class Mailer
    {
        public static void SendEmail(string bodyOneLine, List<string> body, string subject = "?", string emailReceiver = "?")
        {
            string ERROR_FOLDER = "";
            string ERROR_FILE = "";
            
            try
            {
                string EmailSenderEmail = Environment.GetEnvironmentVariable("EmailSenderEmail", EnvironmentVariableTarget.User);
                string EmailSenderPassword = Environment.GetEnvironmentVariable("EmailSenderPassword", EnvironmentVariableTarget.User);

                string EmailReceiverEmail = "";
                if (emailReceiver.Equals("?"))
                    EmailReceiverEmail = Environment.GetEnvironmentVariable("EmailReceiverEmail", EnvironmentVariableTarget.User);
                else
                    EmailReceiverEmail = emailReceiver;


                string method_caller = new StackTrace().GetFrame(1).GetMethod().DeclaringType.FullName.ToString();
                ERROR_FOLDER = Strings.ERROR_FOLDER_AND_FILE(method_caller)[0];
                ERROR_FILE = Strings.ERROR_FOLDER_AND_FILE(method_caller)[1];

                MailAddress to = new MailAddress(EmailReceiverEmail);
                MailAddress from = new MailAddress(EmailReceiverEmail);
                MailMessage mail = new MailMessage(from, to);

                if (subject.Equals("?"))
                    mail.Subject = Strings.SOURCE_PROJECT + method_caller;
                else
                    mail.Subject = subject;

                mail.Body = Strings.SOURCE_PROJECT + method_caller + Environment.NewLine + "\n";
                if (body != null)
                {
                    foreach (string el in body)
                    {
                        mail.Body += el + Environment.NewLine + "\n";
                    }
                }
                else
                    mail.Body += bodyOneLine;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;

                smtp.Credentials = new NetworkCredential(EmailSenderEmail, EmailSenderPassword);
                smtp.EnableSsl = true;
                Console.WriteLine("Sendind an e-mail.");
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {
                string error = "Sendind an e-mail with exception: SmtpException";
                Console.WriteLine(error);
                Loger.PrintInFile(DateTime.Now + " : " + error + " --- " + ex.ToString(), null, Strings.ERROR_PATH_DISK + ERROR_FOLDER + ERROR_FILE, false);
            }
            catch (Exception ex)
            {
                string error = "General exception in method SendEmail.";
                Console.WriteLine(error);
                Loger.PrintInFile(DateTime.Now + " : " + error + " --- " + ex.ToString(), null, Strings.ERROR_PATH_DISK + ERROR_FOLDER + ERROR_FILE, false);
            }
        }
    }
}

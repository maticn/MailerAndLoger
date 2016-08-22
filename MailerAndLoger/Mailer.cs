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
        public static void SendEmail(string bodyOneLine, List<string> body, string subject = "?")
        {
            string ERROR_FOLDER = "";
            string ERROR_FILE = "";

            try
            {
                string method_caller = new StackTrace().GetFrame(1).GetMethod().DeclaringType.FullName.ToString();
                ERROR_FOLDER = Strings.ERROR_FOLDER_AND_FILE(method_caller)[0];
                ERROR_FILE = Strings.ERROR_FOLDER_AND_FILE(method_caller)[1];

                MailAddress to = new MailAddress("maticnova@gmail.com");
                MailAddress from = new MailAddress("maticnova@gmail.com");
                MailMessage mail = new MailMessage(from, to);

                if (subject.Equals("?"))
                    mail.Subject = Strings.OBVESTILO + method_caller;
                else
                    mail.Subject = subject;

                mail.Body = Strings.OBVESTILO + method_caller + Environment.NewLine + "\n";
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

                smtp.Credentials = new NetworkCredential("matic@vsegrad.si", "prpoTest15");
                smtp.EnableSsl = true;
                Console.WriteLine("Poslal bom e-mail.");
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {
                string error = "Posiljanje emaila exc: SmtpException";
                Console.WriteLine(error);
                Loger.PrintInFile(DateTime.Now + " : " + error + " --- " + ex.ToString(), null, Strings.ERROR_PATH_DISK + ERROR_FOLDER + ERROR_FILE, false);
            }
            catch (Exception ex)
            {
                string error = "Generalni exception v metodi sendEmail.";
                Console.WriteLine(error);
                Loger.PrintInFile(DateTime.Now + " : " + error + " --- " + ex.ToString(), null, Strings.ERROR_PATH_DISK + ERROR_FOLDER + ERROR_FILE, false);
            }
        }
    }
}

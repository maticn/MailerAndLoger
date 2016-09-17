using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MailerAndLoger
{
    public class FTPconnect
    {
        public static bool UploadFileToServer(XmlDocument doc, string FTP_PATH, string FTP_FILE, string UPLOAD_FILE_ERROR_MESSAGE, string FTP_USERNAME, string FTP_PASS)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTP_PATH + FTP_FILE);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UsePassive = false;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASS);

                //Get raw access to the request stream
                using (Stream s = request.GetRequestStream())
                {
                    //Save the XML doc to it
                    doc.Save(s);
                }

                // Ce bi bral datoteko na disku in jo pisal na streznik.
                /*
                // Copy the contents of the file to the request stream.
                StreamReader sourceStream = new StreamReader("datoteka_iz_katere_berem");
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                */

                //Push the request to the server and await its response
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //We should get a 226 status code back from the server if everything worked out ok
                if (response.StatusCode == FtpStatusCode.ClosingData)
                {
                    response.Close();
                    return true;
                }
                else {
                    Console.WriteLine("Error uploading file:" + response.StatusDescription);
                    Mailer.SendEmail(UPLOAD_FILE_ERROR_MESSAGE + "\n" + response.StatusDescription, null);
                    response.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Mailer.SendEmail(UPLOAD_FILE_ERROR_MESSAGE + "\n" + ex.ToString(), null);
                return false;
            }
        }
    }
}

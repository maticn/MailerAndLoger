using System;
using System.IO;
using System.Net;
using System.Xml;

namespace MailerAndLoger
{
  public class FTPconnect
  {
    public static bool UploadXmlToServer(XmlDocument doc, string FTP_PATH, string FTP_FILE, string UPLOAD_FILE_ERROR_MESSAGE, string FTP_USERNAME, string FTP_PASS)
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
          //Save the XML doc to it.
          doc.Save(s);
        }

        // If you want to read a file from the disk and write it to the server.
        /*
        // Copy the contents of the file to the request stream.
        StreamReader sourceStream = new StreamReader("file_you_want_to_read");
        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
        sourceStream.Close();
        request.ContentLength = fileContents.Length;

        Stream requestStream = request.GetRequestStream();
        requestStream.Write(fileContents, 0, fileContents.Length);
        requestStream.Close();
        */

        // Push the request to the server and await its response.
        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        // We should get a 226 status code back from the server if everything worked out ok.
        if (response.StatusCode == FtpStatusCode.ClosingData)
        {
          response.Close();
          return true;
        }
        else
        {
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

    public static int DownloadImageAndUploadItToFtpServer(string SourceFilePath, string FTP_PATH, string FTP_FILE, string DOWNLOAD_FILE_ERROR_MESSAGE, string UPLOAD_FILE_ERROR_MESSAGE, string FTP_USERNAME, string FTP_PASS)
    {
      try
      {
        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FTP_PATH + FTP_FILE);
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.UsePassive = false;
        request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASS);

        // Download an image from web.
        try
        {
          WebClient img_request = new WebClient();
          byte[] fileData = img_request.DownloadData(SourceFilePath);
          request.ContentLength = fileData.Length;

          Stream requestStream = request.GetRequestStream();
          requestStream.Write(fileData, 0, fileData.Length);
          requestStream.Close();
        }
        catch (Exception ex)
        {
          //Mailer.SendEmail(DOWNLOAD_FILE_ERROR_MESSAGE + "\n" + ex.ToString(), null);
          return -2;
        }

        // Push the request to the server and await its response.
        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        // We should get a 226 status code back from the server if everything worked out ok.
        if (response.StatusCode == FtpStatusCode.ClosingData)
        {
          response.Close();
          return 1;
        }
        else
        {
          Console.WriteLine("Error uploading file:" + response.StatusDescription);
          Mailer.SendEmail(UPLOAD_FILE_ERROR_MESSAGE + "\n" + response.StatusDescription, null);
          response.Close();
          return -1;
        }
      }
      catch (Exception ex)
      {
        Mailer.SendEmail(UPLOAD_FILE_ERROR_MESSAGE + "\n" + ex.ToString(), null);
        return -1;
      }
    }
  }
}

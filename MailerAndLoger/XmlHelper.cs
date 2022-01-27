using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace MailerAndLoger
{
  public static class XmlHelper
  {
    public static string GetContentFromOnlineXmlFile(string xmlPath)
    {
      String content = string.Empty;
      WebClient client = new WebClient();
      Stream stream = client.OpenRead(xmlPath);
      if (stream != null)
      {
        StreamReader streamReader = new StreamReader(stream, Encoding.Default, true);
        content = streamReader.ReadToEnd();
      }

      return content;
    }

    public static T DeserializeXmlContentToObject<T>(string xmlContent)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      T result;
      using (StringReader reader = new StringReader(xmlContent))
      {
        result = (T)serializer.Deserialize(reader);
      }

      return result;
    }

    public static T GetDeserializedObjectFromOnlineXmlFile<T>(string xmlPath)
    {
      string content = GetContentFromOnlineXmlFile(xmlPath);
      return DeserializeXmlContentToObject<T>(content);
    }
    
    public static string SerializeToXml(this object input)
    {
      XmlSerializer ser = new XmlSerializer(input.GetType());
      string result = string.Empty;

      using(MemoryStream memStm = new MemoryStream())
      {
        ser.Serialize(memStm, input);

        memStm.Position = 0;
        result = new StreamReader(memStm).ReadToEnd();
      } 

      return result;
    }
    
    public static XmlDocument SerializeToXmlDocument(this object input)
    {
      XmlSerializer ser = new XmlSerializer(input.GetType());
      XmlDocument xd = null;
      using(MemoryStream memStm = new MemoryStream())
      {
        ser.Serialize(memStm, input);
        memStm.Position = 0;

        XmlReaderSettings settings = new XmlReaderSettings
        {
          IgnoreWhitespace = true
        };

        using(var xtr = XmlReader.Create(memStm, settings))
        {  
          xd = new XmlDocument();
          xd.Load(xtr);
        }
      }

      return xd;
    }

    public static XmlDocument SerializeToXmlDocumentTest(this object input)
    {
      XmlDocument doc = new XmlDocument();
      XPathNavigator nav = doc.CreateNavigator();
      using (XmlWriter w = nav.AppendChild())
      {
        XmlSerializer ser = new XmlSerializer(input.GetType());
        ser.Serialize(w, input);
      }

      return doc;
    }
  }
}
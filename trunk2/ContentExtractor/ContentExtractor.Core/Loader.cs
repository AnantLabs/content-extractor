//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 12:32
//

using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Threading;

namespace ContentExtractor.Core
{
  /// <summary>
  /// Description of Loader.
  /// </summary>
  public sealed class Loader
  {
    private static Loader instance = new Loader();
    
    public static Loader Instance {
      get {
        return instance;
      }
    }
    
    private Loader()
    {
    }
    
    public static string LoadContentSync(DocPosition pos)
    {
      try
      {
        if (pos.Url == DocPosition.Empty.Url)
          return string.Empty;
        
        WebRequest request = WebRequest.Create(pos.Url);
        request.Proxy = WebRequest.DefaultWebProxy;
        // ToDo: Consider more complex proxy set up
        using(WebResponse response = request.GetResponse())
        {
          return ReadResponse(response);
        }
      }
      catch(Exception exc)
      {
        throw new Exception("Could not process request to "
                            + pos.ToString(),
                            exc);
      }
    }

    public XmlDocument LoadXmlSync(DocPosition pos)
    {
      string content = LoadContentSync(pos);
      return Utils.HtmlParse(content);
    }

    /// <summary>
    /// Warning! callback is called in other thread
    /// </summary>
    /// <param name="uri">Url to get content and parse</param>
    /// <param name="callback">This callback will be called from another thread
    /// than parsing is done.</param>
    public void LoadXmlAsync(DocPosition pos, Callback<XmlDocument> callback)
    {
      Thread loadThread = new Thread(new ThreadStart(
        delegate
        {
          XmlDocument result = LoadXmlSync(pos);
          callback(result);
        }));
      loadThread.SetApartmentState(ApartmentState.STA);
      loadThread.IsBackground = true;
      loadThread.Start();
    }

    public bool IsWorking
    {
      get
      {
        return false;
      }
    }


    
    private static string ReadResponse(WebResponse response)
    {
      string documentText = string.Empty;
      Encoding encoding = Encoding.Default;
      HttpWebResponse httpResponse = response as HttpWebResponse;
      if (httpResponse != null)
      {
        try
        {
          encoding = Encoding.GetEncoding(httpResponse.CharacterSet);
        }
        catch(ArgumentException exc)
        {
          // TODO: Need to log into INFO
          Console.WriteLine(exc);
        }
      }

      using (MemoryStream memory = new MemoryStream())
      {
        using (Stream responseStream = response.GetResponseStream())
        {
          byte[] buffer = new byte[1024];
          int lenRead;
          do
          {
            lenRead = responseStream.Read(buffer, 0, buffer.Length);
            memory.Write(buffer, 0, lenRead);
          } while (lenRead > 0);
        }

        documentText = ReadStreamUsingEncoding(encoding, memory);
        Regex contentCode = new Regex(@"<meta[^>]*content=[""'][^'"">]*charset=(?<coding>[^""'>]*)[""']",
          RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match match = contentCode.Match(documentText);
        if (match.Success)
        {
          encoding = Encoding.GetEncoding(match.Groups["coding"].Value);
          documentText = ReadStreamUsingEncoding(encoding, memory);
        }
      }
      return documentText;
    }
    
    private static string ReadStreamUsingEncoding(Encoding encoding, MemoryStream memory)
    {
      memory.Position = 0;
      return encoding.GetString(memory.ToArray());
    }


  }
}

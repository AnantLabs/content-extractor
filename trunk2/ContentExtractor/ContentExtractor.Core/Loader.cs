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
    
    public string LoadSync(Uri uri)
    {
      try
      {
        WebRequest request = WebRequest.Create(uri);
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
                            + uri.ToString(),
                            exc);
      }
    }
    
    private string ReadResponse(WebResponse response)
    {
      string documentText = string.Empty;
      Encoding encoding = Encoding.Default;
      if (response is HttpWebResponse)
      {
        try
        {
          encoding = Encoding.GetEncoding(((HttpWebResponse)response).CharacterSet);
        }
        catch(Exception exc)
        {
          // TODO: Need to log into INFO
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using MetaTech.Library;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ContentExtractor.Core
{
  /// <summary>
  /// Servise for asynchronic loading html pages 
  /// </summary>
  public static class AsyncLoader
  {
    private static Dictionary<WebPosition.PersistStruct, string> dataDict = new Dictionary<WebPosition.PersistStruct, string>();
    private static Dictionary<WebPosition.PersistStruct, bool> requestsDict = new Dictionary<WebPosition.PersistStruct, bool>();

    static AsyncLoader()
    {
      dataDict[WebPosition.EmptyPersist] = string.Empty;
      ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
    }

    /// <summary>
    /// Servise function for accepting remote certificates
    /// </summary>
    private static bool RemoteCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      return true;
    }

    /// <summary>
    /// Is loader working on something
    /// </summary>
    public static bool HasWork
    {
      get
      {
        lock (requestsDict)
        {
          return requestsDict.Count > 0;
        }
      }
    }

    internal static string GetDocumentCode(WebPosition position)
    {
      lock (dataDict)
      {
        if (dataDict.ContainsKey(position.Persist))
          return dataDict[position.Persist];
      }
      lock (requestsDict)
      {
        if (!requestsDict.ContainsKey(position.Persist))
        {
          requestsDict[position.Persist] = true;
          StartGetting(position.Persist);
        }
        return "";
      }
    }

    internal class RequestState
    {
      // This class stores the state of the request.
      public WebRequest Request = null;
      public WebPosition.PersistStruct Position;
      public RequestState(WebPosition.PersistStruct pos, WebRequest request)
      {
        this.Request = request;
        this.Position = pos;
      }
    }

    private static IWebProxy DefaultProxy
    {
      get
      {
        WebProxy result;
        CredentialCache cache = new CredentialCache();
        switch (proxyStatus)
        {
          case ProxyLocatingStatus.Default:
            return WebRequest.DefaultWebProxy;
          case ProxyLocatingStatus.GlobalProxySelection:
            return GlobalProxySelection.Select;
          case ProxyLocatingStatus.WithDefaultCredentials:
            result = (WebProxy)(GlobalProxySelection.Select ?? WebProxy.GetDefaultProxy());
            NetworkCredential defaultCred = CredentialCache.DefaultCredentials as NetworkCredential;
            if (defaultCred != null)
            {
              cache.Add(result.Address, "Digest", defaultCred);
              cache.Add(result.Address, "Negotiate", defaultCred);
              cache.Add(result.Address, "NTLM", defaultCred);
              cache.Add(result.Address, "Kerberos", defaultCred);
            }
            result.Credentials = cache;
            return result;
          case ProxyLocatingStatus.WithNetworkCredentials:
            result = (WebProxy)(GlobalProxySelection.Select ?? WebProxy.GetDefaultProxy());
            cache.Add(result.Address, "Digest", CredentialCache.DefaultNetworkCredentials);
            cache.Add(result.Address, "Negotiate", CredentialCache.DefaultNetworkCredentials);
            cache.Add(result.Address, "NTLM", CredentialCache.DefaultNetworkCredentials);
            cache.Add(result.Address, "Kerberos", CredentialCache.DefaultNetworkCredentials);
            result.Credentials = cache;
            return result;
          case ProxyLocatingStatus.Failed:
            return null;
        }
        return null;
      }
    }

    public static bool ProxyFailed
    {
      get { return proxyStatus == ProxyLocatingStatus.Failed; }
    }

    private static void MarkProxyAsFailed()
    {
      if (proxyStatus == ProxyLocatingStatus.Default)
      {
        TraceHlp2.AddMessage("Switched to GlobalProxySelection proxy mode");
        proxyStatus = ProxyLocatingStatus.GlobalProxySelection;
      }
      else if (proxyStatus == ProxyLocatingStatus.GlobalProxySelection)
      {
        TraceHlp2.AddMessage("Switched to WithNetworkCredentials proxy mode");
        proxyStatus = ProxyLocatingStatus.WithNetworkCredentials;
      }
      else if (proxyStatus == ProxyLocatingStatus.WithNetworkCredentials)
      {
        TraceHlp2.AddMessage("Switched to WithDefaultCredentials proxy mode");
        proxyStatus = ProxyLocatingStatus.WithDefaultCredentials;
      }
      else
      {
        TraceHlp2.AddMessage("Switched to Failed proxy mode");
        proxyStatus = ProxyLocatingStatus.Failed;
      }
    }
    private static ProxyLocatingStatus proxyStatus = ProxyLocatingStatus.Default;

    private enum ProxyLocatingStatus
    {
      Default,
      GlobalProxySelection,
      WithDefaultCredentials,
      WithNetworkCredentials,
      Failed
    }


    private static void StartGetting(WebPosition.PersistStruct position)
    {
      try
      {
        WebRequest request = WebRequest.Create(position.Url);
        request.Proxy = DefaultProxy;
        //if (!proxySaved)
        //{
        //  Uri yandex = new Uri("http://www.yandex.ru");
        //  IWebProxy p = request.Proxy;
        //  Type type = p.GetType();
        //  System.Reflection.PropertyInfo prop = type.GetProperty("WebProxy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //  WebProxy proxy = prop.GetValue(p, null) as WebProxy;
        //  if (proxy != null)
        //  {
        //    TraceHlp2.AddMessage("Url: {0}", proxy.Address);
        //    TraceHlp2.AddMessage(string.Join("; ", proxy.BypassList));
        //    TraceHlp2.AddMessage("BypassProxyOnLocal: {0}", proxy.BypassProxyOnLocal);
        //    TraceHlp2.AddMessage("UseDefaultCredentials: {0}", proxy.UseDefaultCredentials);
        //    TraceHlp2.AddMessage("Credentials: {0}", proxy.Credentials);
        //  }
        //  TraceHlp2.AddMessage("Bypassed {0}", p.IsBypassed(yandex));
        //  TraceHlp2.AddMessage("Yandex :{0}", p.GetProxy(yandex));
        //  TraceHlp2.AddMessage("Credentails :{0}", p.Credentials);
        //  proxySaved = true;
        //}
        ////request.Proxy = Proxy;
        //Console.WriteLine(request.Proxy);

        request.BeginGetResponse(new AsyncCallback(CallBack), new RequestState(position, request));
      }
      catch (Exception exc)
      {
        lock (requestsDict)
          requestsDict.Remove(position);
        TraceHlp2.WriteException(exc);
      }
    }

    private static void CallBack(IAsyncResult asyncResult)
    {
      RequestState state = (RequestState)asyncResult.AsyncState;
      try
      {
        string documentText = string.Empty;

        using (WebResponse response = state.Request.EndGetResponse(asyncResult))
        {
          Encoding encoding = Encoding.Default;
          if (response is HttpWebResponse)
          {
            FlowHlp.SafeBlock("", delegate { encoding = Encoding.GetEncoding(((HttpWebResponse)response).CharacterSet); });
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
        }
        lock (dataDict)
          dataDict[state.Position] = documentText;
      }
      catch (WebException exc)
      {
        //StringBuilder message = new StringBuilder();
        //message.AppendFormat("WebExceptionStatus {0}\r\n", exc.Status);
        //message.AppendFormat("Status {0}\r\n", exc.);

        TraceHlp2.WriteException(exc);
        if (IsProxyFailed(exc))
          MarkProxyAsFailed();
        //TraceHlp2.AddMessage(message.ToString());
      }
      catch (Exception exc)
      { TraceHlp2.WriteException(exc); }
      finally
      {
        lock (requestsDict)
          requestsDict.Remove(state.Position);
      }
    }

    private static bool IsProxyFailed(WebException exc)
    {
      HttpWebResponse webReponse = exc.Response as HttpWebResponse;
      return exc.Status == WebExceptionStatus.ProtocolError && webReponse != null && webReponse.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
             exc.Status == WebExceptionStatus.RequestProhibitedByProxy ||
             exc.Status == WebExceptionStatus.ProxyNameResolutionFailure;
    }

    private static string ReadStreamUsingEncoding(Encoding encoding, MemoryStream memory)
    {
      memory.Position = 0;
      return encoding.GetString(memory.GetBuffer());
      //using (StreamReader reader = new StreamReader(memory, encoding, true))
      //  return reader.ReadToEnd();
    }

  }
}

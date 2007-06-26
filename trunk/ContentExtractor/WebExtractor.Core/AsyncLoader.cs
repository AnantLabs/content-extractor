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
  public class AsyncLoader
  {
    public static AsyncLoader Instance
    {
      get
      {
        if(_instance == null)
          lock(typeof(AsyncLoader))
            if(_instance == null)
            {
              _instance = new AsyncLoader();
            }
        return _instance;
      }
    }
    private static AsyncLoader _instance = null;
    
    private AsyncLoader()
    {
      dataDict[WebPosition.EmptyPersist] = string.Empty;
      ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidation;
    }

    /// <summary>
    /// Service function for accepting remote certificates
    /// </summary>
    private bool RemoteCertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
      return true;
    }

    /// <summary>
    /// Is loader working on something?
    /// </summary>
    public bool HasWork
    {
      get
      {
        lock (requestsDict)
        {
          return requestsDict.Count > 0;
        }
      }
    }

    public string GetDocumentCode(WebPosition position)
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

    public string Load(WebPosition position)
    {
      try
      {
        WebRequest request = WebRequest.Create(position.Url);
        request.Proxy = DefaultProxy;
        using(WebResponse response = request.GetResponse())
        {
          return ReadResponse(response);
        }
      }
      catch(Exception exc)
      {
        throw new Exception("Could not process request to " 
                            + position.Url.ToString(),
                            exc);
      }
    }
    
    private string ReadResponse(WebResponse response)
    {
      string documentText = string.Empty;
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
      return documentText;
    }
    
    private void CallBack(IAsyncResult asyncResult)
    {
      RequestState state = (RequestState)asyncResult.AsyncState;
      try
      {
        string documentText;
        using (WebResponse response = state.Request.EndGetResponse(asyncResult))
        {
          documentText = ReadResponse(response);
        }
        lock (dataDict)
          dataDict[state.Position] = documentText;
      }
      catch (WebException exc)
      {
        TraceHlp2.WriteException(exc);
        if (IsProxyFailed(exc))
          MarkProxyAsFailed();
      }
      catch (Exception exc)
      { TraceHlp2.WriteException(exc); }
      finally
      {
        lock (requestsDict)
          requestsDict.Remove(state.Position);
      }
    }

    private void StartGetting(WebPosition.PersistStruct position)
    {
      try
      {
        WebRequest request = WebRequest.Create(position.Url);
        request.Proxy = DefaultProxy;
        request.BeginGetResponse(new AsyncCallback(CallBack), new RequestState(position, request));
      }
      catch (Exception exc)
      {
        lock (requestsDict)
          requestsDict.Remove(position);
        TraceHlp2.WriteException(exc);
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

    #region Proxy-related functions
    private IWebProxy DefaultProxy
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

    public bool ProxyFailed
    {
      get { return proxyStatus == ProxyLocatingStatus.Failed; }
    }

    private void MarkProxyAsFailed()
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
    private ProxyLocatingStatus proxyStatus = ProxyLocatingStatus.Default;

    private enum ProxyLocatingStatus
    {
      Default,
      GlobalProxySelection,
      WithDefaultCredentials,
      WithNetworkCredentials,
      Failed
    }

    private bool IsProxyFailed(WebException exc)
    {
      HttpWebResponse webReponse = exc.Response as HttpWebResponse;
      return exc.Status == WebExceptionStatus.ProtocolError && webReponse != null && webReponse.StatusCode == HttpStatusCode.ProxyAuthenticationRequired ||
             exc.Status == WebExceptionStatus.RequestProhibitedByProxy ||
             exc.Status == WebExceptionStatus.ProxyNameResolutionFailure;
    }
    #endregion

    private static string ReadStreamUsingEncoding(Encoding encoding, MemoryStream memory)
    {
      memory.Position = 0;
      return encoding.GetString(memory.ToArray());
    }

    private Dictionary<WebPosition.PersistStruct, string> dataDict = new Dictionary<WebPosition.PersistStruct, string>();
    private Dictionary<WebPosition.PersistStruct, bool> requestsDict = new Dictionary<WebPosition.PersistStruct, bool>();
  }
}

//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 14:36
//

using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace ContentExtractor.Core
{
  /// <summary>
  /// Different useful functions
  /// </summary>
  public static class Utils
  {
    public static List<T> CastList<T>(IEnumerable collection)
    {
      List<T> result = new List<T>();
      foreach(object item in collection)
        result.Add((T)item);
      return result;
    }
    
    public static Uri ParseUrl(string address)
    {
      Uri uri = null;
      if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
        Uri.TryCreate(Uri.UriSchemeHttp + "://" + address,
                      UriKind.Absolute, out uri);

      return uri;
    }
    
    public static XmlDocument HtmlParse(string content)
    {
      using( ExtendedWebBrowser browser = new ExtendedWebBrowser())
      {
        browser.DocumentText = content;
        browser.IsWebBrowserContextMenuEnabled = false;
        // WARNING!! this can cause problems when certificate approval needed!
        browser.ScriptErrorsSuppressed = true;
        System.Windows.Forms.Application.DoEvents();
        while (browser.IsBusy)
          System.Windows.Forms.Application.DoEvents();
        return DOMTreeToXml(browser.Document);
      }
    }
    
    private static XmlDocument DOMTreeToXml(HtmlDocument htmlDoc)
    {
      XmlDocument result = new XmlDocument();
      if(htmlDoc != null &&
         htmlDoc.Body != null &&
         htmlDoc.Body.Parent != null)
      {
        HtmlElement topHtml = htmlDoc.Body.Parent;
        using (StringReader sReader = new StringReader(topHtml.OuterHtml))
        {
          using (StringWriter errorLog = new StringWriter())
          {
            Sgml.SgmlReader reader = new Sgml.SgmlReader();
            reader.ErrorLog = errorLog;
            reader.InputStream = sReader;
            reader.DocType = "HTML";
            
            using (StringReader dtdReader = new StringReader(Encoding.UTF8.GetString(Resources.weak))) 
              //Resources.WeakDtd
              reader.Dtd = Sgml.SgmlDtd.Parse(null, "HTML", null, dtdReader, null, null, reader.NameTable);
            
            result.Load(reader);
            // TODO: log to INFO
            //errorLog.Flush();
            //Console.WriteLine(errorLog.ToString());
          }
        }
      }
      return result;
    }
  }
}

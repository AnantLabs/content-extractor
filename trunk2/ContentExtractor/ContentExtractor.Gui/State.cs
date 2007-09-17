//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 06.07.2007
// Time: 19:15
//

using System;
using ContentExtractor.Core;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Generic;

namespace ContentExtractor.Gui
{
  /// <summary>
  /// Description of State.
  /// </summary>
  public class State
  {
    public State()
    {
    }

    [XmlIgnore]
    public DocPosition BrowserPosition
    {
      get
      {
        return browserPos_ ?? DocPosition.Empty;
      }
      set
      {
        if (!Uri.Equals(value, browserPos_))
        {
          browserPos_ = value;
          if (BrowserUriChanged != null)
            BrowserUriChanged(this, EventArgs.Empty);
        }
      }
    }

    private DocPosition browserPos_ = null;

    public ScrapingProject Project = new ScrapingProject();

    public bool IsParseMode = false;

    public event EventHandler BrowserUriChanged;

    internal XmlDocument GetXmlAsync(DocPosition pos)
    {
      if (pos == null)
      {
        return new XmlDocument();
      }
      bool shouldGet = false;
      lock (docCache)
      {
        shouldGet = !docCache.ContainsKey(pos);
        // The stub added
        if (shouldGet)
          docCache[pos] = new XmlDocument();
      }
      if (shouldGet)
        Loader.Instance.LoadXmlAsync(pos,
          delegate(XmlDocument doc)
          {
            lock (docCache)
              docCache[pos] = doc;
          });
      lock (docCache)
        return docCache[pos];
    }

    private Dictionary<DocPosition, XmlDocument> docCache = new Dictionary<DocPosition, XmlDocument>();

    [XmlIgnore]
    public string SelectedNodeXPath
    {
      get
      {
        return selectedNodePath;
      }
      set
      {
        if (value != selectedNodePath)
        {
          selectedNodePath = value;
          if (SelectedNodeChanged != null)
            SelectedNodeChanged(this, EventArgs.Empty);
        }
      }
    }
    private string selectedNodePath = null;

    public event EventHandler SelectedNodeChanged;

  }
}

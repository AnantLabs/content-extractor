//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 14:58
//

using System;
using System.Xml.Serialization;

namespace ContentExtractor.Core
{
  /// <summary>
  /// Stores information necessary for scraping.
  /// Used for serialization
  /// </summary>
  public class ScrapingProject
  {
    public ScrapingProject()
    {
    }

    public Template Template;

    [XmlElement("Url")]
    public string[] XmlUrls
    {
      get
      {
        return Array.ConvertAll<Uri, string>(
          SourceUrls,
          delegate(Uri u) { return u.AbsoluteUri; } );
      }
      set
      {
        SourceUrls = Array.ConvertAll<string, Uri>(
          value,
          delegate(string s) { return new Uri(s);});
      }
    }
    
    [XmlIgnore]
    public Uri[] SourceUrls;
  }
}

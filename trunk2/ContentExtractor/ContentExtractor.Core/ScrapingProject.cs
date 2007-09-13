//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 14:58
//

using System;
using System.Xml.Serialization;
using System.Collections.Generic;

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

    public Template Template = new Template();

    [XmlElement("Url")]
    public string[] XmlUrls
    {
      get
      {
        return SourceUrls.ConvertAll<string>(
          delegate(Uri u) { return u.AbsoluteUri; } ).ToArray();
      }
      set
      {
        SourceUrls = new List<Uri>(value.Length);
        foreach(string uri in value)
          SourceUrls.Add(Utils.ParseUrl(uri));
      }
    }
    
    [XmlIgnore]
    public List<Uri> SourceUrls = new List<Uri>();

    public static void SaveProject(string filename, ScrapingProject project)
    {
      XmlUtils.Serialize(filename, project);
    }

    public static Uri EmptyUri
    {
      get
      {
        return new Uri("about:blank");
      }
    }

  }
}

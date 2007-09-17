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

    private List<DocPosition> _sourceUrls = new List<DocPosition>();

    [XmlElement("Url")]
    public List<DocPosition> SourcePositions
    {
      get { return _sourceUrls; }
      set { _sourceUrls = value; }
    }

    public static void SaveProject(string filename, ScrapingProject project)
    {
      XmlUtils.Serialize(filename, project);
    }

    public static ScrapingProject Load(string filename)
    {
      return XmlUtils.Deserialize<ScrapingProject>(filename);
    }

  }
}

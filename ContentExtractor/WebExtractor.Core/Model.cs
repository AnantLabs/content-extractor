using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using MetaTech.Library;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using System.ComponentModel;

namespace ContentExtractor.Core
{
//  [LicenseProvider(typeof(CoreLicenseProvider))]
  public class Model : ICloneable
  {
//    static Model()
//    {
//      CoreLicenseContext.InitContext();
//    }

    public Model()
    {
    }

    public enum WorkMode
    {
      Browse,
      Parse
    }
    public WorkMode Mode = WorkMode.Browse;

    public List<WebPosition> CurrentPositions
    {
      get
      {
        if (Mode == WorkMode.Parse)
        {
          List<WebPosition.PersistStruct> positions = PositionsList.ConvertAll<WebPosition.PersistStruct>(WebPosition.GetPersist);
          for (int i = currentPositions.Count - 1; i >= 0; i--)
          {
            if (!object.Equals(currentPositions[i].Persist, WebPosition.EmptyPersist) && !positions.Contains(currentPositions[i].Persist))
              currentPositions.RemoveAt(i);
          }
        }
        if (currentPositions.Count == 0)
        {
          currentPositions.Add(WebPosition.EmptyPosition);
        }
        return currentPositions;
      }
    }
    private List<WebPosition> currentPositions = new List<WebPosition>(new WebPosition[] { WebPosition.EmptyPosition });

    private Dictionary<WebPosition.PersistStruct, string> selectedNodes = new Dictionary<WebPosition.PersistStruct, string>();
    public Dictionary<WebPosition.PersistStruct, string> SelectedNodes
    {
      get
      {
        foreach (WebPosition.PersistStruct key in _.From<WebPosition.PersistStruct>(selectedNodes.Keys))
        {
          if (CurrentPositions.TrueForAll(delegate(WebPosition pos) { return !object.Equals(pos.Persist, key); }))
            selectedNodes.Remove(key);
        }
        foreach (WebPosition pos in CurrentPositions)
        {
          if (!selectedNodes.ContainsKey(pos.Persist))
            selectedNodes[pos.Persist] = string.Empty;
        }
        return selectedNodes;
      }
    }

    public string GetSelectedNode(WebPosition position)
    {
      return DictionaryHlp.GetValueOrDefault(SelectedNodes, position.Persist, "");
    }

    public WebPosition ActivePosition
    {
      get
      {
        return CurrentPositions[ActiveIndex];
      }
      set
      {
        CurrentPositions[ActiveIndex].Set(value);
      }
    }

    private int activeIndex = 0;
    public int ActiveIndex
    {
      get { return Math.Max(0, Math.Min(activeIndex, CurrentPositions.Count - 1)); }
      set { activeIndex = Math.Max(0, Math.Min(value, CurrentPositions.Count - 1)); }
    }

    private string cachedCurrentPositionText = string.Empty;

    public DataTemplate Template
    {
      get
      {
        if (template == null)
          template = new DataTemplate();
        return template;
      }
      set { template = value; }
    }
    private DataTemplate template = new DataTemplate();

    public XmlDocument Result
    {
      get
      {
        return Template.Apply(SourceTree);
      }
    }

    private WorkMode? cachedMode = null;

    public List<WebPosition> PositionsList
    {
      get
      {
        if (cachedMode != Mode)
        {
          cachedMode = Mode;
          if (Mode == WorkMode.Parse && currentPositions != null)
            foreach (WebPosition position in currentPositions)
              if (!object.Equals(position.Persist, WebPosition.EmptyPersist))
              {
                bool notFound = positionsList.TrueForAll(delegate(WebPosition pos) { return !object.Equals(pos.Persist, position.Persist); });
                if (notFound)
                  positionsList.Add(position);
              }
        }
        return positionsList;
      }
    }
    private List<WebPosition> positionsList = new List<WebPosition>();

    private List<string> sourceTreeTextCache = new List<string>();
    private XPathDocument cachedSourceTree = null;

    public IXPathNavigable SourceTree
    {
      get
      {
        bool equal = PositionsList.Count == sourceTreeTextCache.Count;
        if (equal)
        {
          for (int i = 0; i < PositionsList.Count; i++)
            if (PositionsList[i].DocumentText != sourceTreeTextCache[i])
            {
              equal = false;
              break;
            }
        }
        if (!equal || cachedSourceTree == null)
        {
          StringBuilder sb = new StringBuilder();
          sourceTreeTextCache = new List<string>(PositionsList.Count);
          XmlWriterSettings writerSettings = new XmlWriterSettings();
          writerSettings.ConformanceLevel = ConformanceLevel.Fragment;
          writerSettings.Indent = true;
          using (XmlWriter writer = XmlWriter.Create(sb, writerSettings))
          {
            try
            {
              foreach (WebPosition pos in PositionsList)
              {
                sourceTreeTextCache.Add(pos.DocumentText);
                writer.WriteStartElement("Page");
                writer.WriteAttributeString("url", pos.Url.AbsoluteUri);
                using (XmlReader reader = pos.XPathNavigable.CreateNavigator().ReadSubtree())
                {
                  reader.Read();
                  while (!reader.EOF)
                  {
                    if (reader.NodeType != XmlNodeType.Attribute || reader.Name != "xmlns")
                      writer.WriteNode(reader, false);
                    else
                      reader.Read();
                  }
                }
                writer.WriteEndElement();
              }
            }
            catch (Exception exc)
            {
              Console.WriteLine(exc);
            }
          }

          //XmlDocument cacheDoc = new XmlDocument();
          //cacheDoc.AppendChild(cacheDoc.CreateElement("Root"));
          //foreach (WebPosition pos in PositionsList)
          //{
          //  sourceTreeTextCache.Add(pos.DocumentText);

          //  XmlElement pageElement = cacheDoc.CreateElement("Page");
          //  cacheDoc.DocumentElement.AppendChild(pageElement);
          //  pageElement.SetAttribute("url", pos.Url.AbsoluteUri);
          //  //XmlDocument pageDocument = new XmlDocument();
          //  XmlNode pageNode = cacheDoc.ReadNode(pos.XPathNavigable.CreateNavigator().ReadSubtree());
          //  if (pageNode != null)
          //    pageElement.AppendChild(pageNode);
          //}
          //sb.Append(cacheDoc.DocumentElement.InnerXml);
          cachedSourceTree = null;
          FlowHlp.SafeBlock("Creating SourceTree XPathDocument",
            delegate
            {
              using (StringReader reader = new StringReader(sb.ToString()))
              {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.ValidationType = ValidationType.None;
                using (XmlReader xmlReader = XmlReader.Create(reader, settings))
                  cachedSourceTree = new XPathDocument(xmlReader);
              }
            });
        }
        return cachedSourceTree;
      }
    }

    public void Load(Model model)
    {
      Copy(model, this);
    }

    public void Load(ModelPersist persist)
    {
      this.Persist = persist;
    }

    public static T Clone<T>(T value)
      where T : ICloneable
    {
      if (value != null)
        return (T)value.Clone();
      else
        return value;
    }

    public string FileName = string.Empty;
    public bool HasFileName
    {
      get { return !string.IsNullOrEmpty(FileName); }
    }

    public object Clone()
    {
      Model result = new Model();
      Copy(this, result);
      return result;
    }

    private static void Copy(Model source, Model dest)
    {
      dest.Persist = source.Persist;
    }

    public override bool Equals(object obj)
    {
      if (obj is Model)
      {
        Model other = (Model)obj;

        return object.Equals(this.Persist, other.Persist);

        //if (!WebExtractorHlp.CompareList(this.CurrentPositions, other.CurrentPositions))
        //  return false;
        //else if (!DataTemplate.Equals(this.template, other.template))
        //  return false;
        //else if (!WebExtractorHlp.CompareList(_.From<string>(this.SelectedNodes.Values), _.From<string>(other.SelectedNodes.Values)))
        //  return false;
        //else if (!WebExtractorHlp.CompareList(this.PositionsList, other.PositionsList))
        //  return false;
        //else if (this.Mode != other.Mode)
        //  return false;
        //else
        //  return true;
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Persist.GetHashCode();
    }

    public void SaveToFile(string fileName)
    {
      XmlSerialization2.Save(Persist, fileName);
      this.FileName = fileName;
    }

    public void LoadFromFile(string fileName)
    {
      try
      {
        Persist = XmlSerialization2.Load<ModelPersist>(fileName);
        this.FileName = fileName;
      }
      catch (Exception exc)
      {
        TraceHlp2.WriteException(exc);
      }
    }

    public ModelPersist Persist
    {
      get
      {
        ModelPersist result = new ModelPersist();
        result.CurrentPositions = this.currentPositions.ConvertAll<WebPosition.PersistStruct>(WebPosition.GetPersist).ToArray();
        result.SelectedNodes = _.From<string>(this.SelectedNodes.Values).ToArray();
        result.PositoinsList = this.positionsList.ConvertAll<WebPosition.PersistStruct>(WebPosition.GetPersist).ToArray();
        result.Template = Clone<DataTemplate>(this.template);
        result.WorkMode = this.Mode;
        return result;
      }
      private set
      {
        this.currentPositions = new List<WebPosition>();
        if (value.CurrentPositions != null)
        {
          //this.currentPositions = new List<WebPosition>(value.CurrentPositions);
          for (int i = 0; i < value.CurrentPositions.Length; i++)
          {
            WebPosition pos = new WebPosition();
            pos.Persist = value.CurrentPositions[i];
            this.currentPositions.Add(pos);
            if (i < value.SelectedNodes.Length)
              SelectedNodes[pos.Persist] = value.SelectedNodes[i];
          }
        }
        this.positionsList = new List<WebPosition>();
        if (value.PositoinsList != null)
        {
          foreach (WebPosition.PersistStruct persist in value.PositoinsList)
          {
            WebPosition pos = new WebPosition();
            pos.Persist = persist;
            positionsList.Add(pos);
          }
        }
        this.template = Clone<DataTemplate>(value.Template);
        this.Mode = value.WorkMode;
      }
    }
  }

  public struct ModelPersist
  {
    public WebPosition.PersistStruct[] CurrentPositions;
    public string[] SelectedNodes;
    public DataTemplate Template;
    public WebPosition.PersistStruct[] PositoinsList;
    public Model.WorkMode WorkMode;

    public override bool Equals(object obj)
    {
      if (obj is ModelPersist)
      {
        ModelPersist other = (ModelPersist)obj;
        return WebExtractorHlp.CompareList(this.CurrentPositions, other.CurrentPositions) &&
          WebExtractorHlp.CompareList(this.PositoinsList, other.PositoinsList) &&
          WebExtractorHlp.CompareList(this.SelectedNodes, other.SelectedNodes) &&
          DataTemplate.Equals(this.Template, other.Template) &&
          this.WorkMode == other.WorkMode;
      }
      return false;
    }

    public override int GetHashCode()
    {
      //ToDo - переопределить, чтобы была правда
      return base.GetHashCode();
    }

  }
}

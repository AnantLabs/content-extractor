using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using MetaTech.Library;

namespace ContentExtractor.Core
{
  [NineRays.Obfuscator.NotObfuscate]
  public class XPathDataRowRule : IDataRowRule
  {
    public string RowsXPath;

    private string SelectPath
    {
      get
      {
        string result = XPathInfo.PathWithoutBracets(RowsXPath);
        if (result.StartsWith("/"))
          result = result.Substring(1);
        return result;
      }
    }

    public IDataRow[] GetRows(IXPathNavigable document)
    {
      if (document != null)
      {
        XPathNavigator navigator = document.CreateNavigator();

        List<XPathDataRow> result = new List<XPathDataRow>();
        int pageIndex = 0;
        int row = 0;
        foreach (XPathNavigator page in XmlHlp.Select(navigator, "/Page"))
        {
          Uri pageUrl = null;
          XPathNavigator url = XmlHlp.SelectSingleNode(page, "@url");
          if (url != null)
            pageUrl = new Uri(url.Value);
          XPathNodeIterator iterator = XmlHlp.Select(page, SelectPath);
          if (iterator != null)
            foreach (XPathNavigator nav in iterator)
            {
              result.Add(new XPathDataRow(this, nav.CreateNavigator(), pageUrl, row, pageIndex));
              row++;
            }
          pageIndex++;
        }
        return result.ToArray();
      }
      else
        return Array<IDataRow>.Empty;
    }

    public object Clone()
    {
      XPathDataRowRule result = new XPathDataRowRule();
      result.RowsXPath = this.RowsXPath;
      return result;
    }
    public override bool Equals(object obj)
    {
      if (obj is XPathDataRowRule)
      {
        XPathDataRowRule other = (XPathDataRowRule)obj;
        return this.RowsXPath == other.RowsXPath;
      }
      return false;
    }
    public override int GetHashCode()
    {
      return this.RowsXPath.GetHashCode();
    }
  }

  public class XPathDataRow : IDataRow
  {
    public XPathDataRow(XPathDataRowRule rule, IXPathNavigable row, Uri uri, int rowIndex, int pageIndex)
    {
      this.rule = rule;
      this.row = row;
      this.PageUri = uri;
      this.RowIndex = rowIndex;
      this.PageIndex = pageIndex;
    }
    private XPathDataRowRule rule;
    private IXPathNavigable row;

    public IDataRowRule ParentRule
    {
      get { return rule; }
    }

    public readonly Uri PageUri;
    public readonly int RowIndex;
    public readonly int PageIndex;

    public object Value
    {
      get { return row; }
    }
  }

  public class NamesDataRowRule : IDataRowRule
  {
    public IDataRow[] GetRows(IXPathNavigable document)
    {
      return new IDataRow[] { new NamesDataRow(this) };
    }

    public object Clone()
    {
      return new NamesDataRowRule();
    }

    public override bool Equals(object obj)
    {
      return obj is NamesDataRowRule;
    }
    public override int GetHashCode()
    {
      return 0;
    }
  }

  public class NamesDataRow : IDataRow
  {
    public NamesDataRow(NamesDataRowRule parent)
    {
      this.Parent = parent;
    }
    private NamesDataRowRule Parent;

    public IDataRowRule ParentRule
    {
      get { return Parent; }
    }

    public object Value
    {
      get { return "Names"; }
    }
  }
}

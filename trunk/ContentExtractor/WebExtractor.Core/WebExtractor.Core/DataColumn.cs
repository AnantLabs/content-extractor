using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.ComponentModel;

namespace ContentExtractor.Core
{
  internal class XPathValue
  {
    public readonly XPathNavigator Navigator;
    public int RowIndex
    { get { return row.RowIndex; } }
    public int PageIndex
    { get { return row.PageIndex; } }
    public Uri PageUri
    { get { return row.PageUri; } }

    private XPathDataRow row;
    public XPathValue(XPathDataRow row, XPathNavigator nav)
    {
      this.row = row;
      this.Navigator = nav;
    }
  }

  public class XPathDataColumn : IDataColumn //, IXmlSerializable
  {
    public XPathDataColumn() { }
    public XPathDataColumn(string xpath)
    {
      this.RelativeXPath = xpath;
    }

    [Category("Column properties")]
    [DisplayName("XPath")]
    public string RelativeXPath
    {
      get { return relativeXPath; }
      set { relativeXPath = value; }
    }

    private string relativeXPath = "";

    [Category("Column properties")]
    [DisplayName("Function applied")]
    [TypeConverter(typeof(FunctionTypeConverter))]
    public string Function
    {
      get { return function; }
      set { function = value; }
    }
    private string function = Functions.FToString;

    [Category("Column properties")]
    [Description("Name of the column")]
    public virtual string Name
    {
      get { return name; }
      set { name = value; }
    }
    string name = string.Empty;

    public object GetValue(IDataRow row)
    {
      if (row is XPathDataRow)
      {
        IXPathNavigable value = row.Value as IXPathNavigable;
        if (value != null)
        {
          XPathNavigator nav = XmlHlp.SelectSingleNode(value.CreateNavigator(), RelativeXPath);
          XPathValue cellValue = new XPathValue((XPathDataRow)row, nav);

          if (Functions.FunctionExist(Function))
            return Functions.Evaluate(Function, cellValue);
          else
            return string.Format("Error: unknown function {0}", Function);
        }
      }
      else if (row is NamesDataRow)
        return this.Name;
      return string.Empty;
    }

    public object Clone()
    {
      XPathDataColumn result = new XPathDataColumn();
      result.RelativeXPath = this.RelativeXPath;
      result.function = this.function;
      result.Name = this.Name;
      return result;
    }
    public override bool Equals(object obj)
    {
      if (obj is XPathDataColumn)
      {
        XPathDataColumn other = (XPathDataColumn)obj;
        return this.RelativeXPath == other.RelativeXPath && this.Function == other.Function && this.Name == other.Name;
      }
      return false;
    }
    public override int GetHashCode()
    {
      return this.RelativeXPath.GetHashCode() ^ this.Function.GetHashCode() ^ this.Name.GetHashCode();
    }

    //#region IXmlSerializable Members

    //public class Persist
    //{
    //  public string XPath;
    //  public string Function;
    //  public string Name;
    //}

    //XmlSchema IXmlSerializable.GetSchema()
    //{
    //  return new XmlSchema();
    //}

    //void IXmlSerializable.ReadXml(XmlReader reader)
    //{
    //  Persist persist = (Persist)PersistSer.Deserialize(reader);
    //  this.RelativeXPath = persist.XPath;
    //  this.Name = persist.Name;
    //  this.Function = persist.Function;

    //  //while (reader.MoveToNextAttribute())
    //  //{
    //  //  switch (reader.Name)
    //  //  {
    //  //    case "xpath":
    //  //      RelativeXPath = reader.Value;
    //  //      break;
    //  //    case "function":
    //  //      Function = reader.Value;
    //  //      break;
    //  //    case "name":
    //  //      Name = reader.Value;
    //  //      break;
    //  //  }
    //  //}
    //  //reader.Read();
    //  //if (reader.NodeType == XmlNodeType.Attribute)
    //  //{
    //  //  Console.WriteLine("attribute");
    //  //}
    //}

    //private static XmlSerializer _persistSer = null;
    //private static XmlSerializer PersistSer
    //{
    //  get
    //  {
    //    if (_persistSer == null)
    //    {
    //      _persistSer = new XmlSerializer(typeof(Persist), XmlRootAttribute);
    //    }
    //    return _persistSer;
    //  }
    //}

    //void IXmlSerializable.WriteXml(XmlWriter writer)
    //{
    //  Persist persist = new Persist();
    //  persist.XPath = this.RelativeXPath;
    //  persist.Name = this.Name;
    //  persist.Function = this.Function;
    //  PersistSer.Serialize(writer, persist);

    //  //writer.WriteAttributeString("name", Name);
    //  //writer.WriteAttributeString("xpath", RelativeXPath);
    //  //if (Function != Functions.FToString)
    //  //  writer.WriteAttributeString("function", Function);
    //}

    //#endregion

  }
}

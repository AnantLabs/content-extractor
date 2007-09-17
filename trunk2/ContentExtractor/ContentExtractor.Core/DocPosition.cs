using System;
using System.Collections.Generic;
using System.Text;

namespace ContentExtractor.Core
{
  public class DocPosition : System.Xml.Serialization.IXmlSerializable
  {
    public DocPosition()
    {
      Url = new Uri("about:blank");
    }
    public DocPosition(Uri url)
    {
      this.Url = url;
    }
    public DocPosition(string url_string)
    {
      this.Url = new Uri(url_string);
    }

    public override string ToString()
    {
      return Url.AbsoluteUri;
    }

    public Uri Url;

    public static readonly DocPosition Empty = new DocPosition("about:blank");

    public override bool Equals(object obj)
    {
      DocPosition other = obj as DocPosition;
      if (other != null)
      {
        return object.Equals(other.Url, this.Url);
      }
      return false;
    }


    #region IXmlSerializable Members

    System.Xml.Schema.XmlSchema System.Xml.Serialization.IXmlSerializable.GetSchema()
    {
      return null;
    }

    void System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
    {
      reader.MoveToContent();
      this.Url = new Uri(reader.ReadString());
      reader.Read();
    }

    void System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
    {
      writer.WriteString(Url.AbsoluteUri);
    }

    #endregion
  }
}

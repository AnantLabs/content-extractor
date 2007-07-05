//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 19:34
//

using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ContentExtractor.Core
{
	public static class XmlUtils
	{
    public static XmlDocument LoadXml(string content)
    {
      XmlDocument result = new XmlDocument();
      result.LoadXml(content);
      return result;
    }
    
    public static T Deserialize<T>(Stream stream)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      return (T)serializer.Deserialize(stream);
    }
    
    public static T Deserialize<T>(string filename)
    {
      using(Stream stream = File.OpenRead(filename))
        return Deserialize<T>(stream);
    }
    
    public static void Serialize<T>(string filename, T obj)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(T));
      using(Stream stream = File.Create(filename))
        serializer.Serialize(stream, obj);
    }
	}
}

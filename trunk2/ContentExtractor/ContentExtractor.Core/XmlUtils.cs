//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 19:34
//

using System;
using System.Xml;

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
	}
}

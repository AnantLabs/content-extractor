//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 29.06.2007
// Time: 14:36
//

using System;
using System.Collections.Generic;
using System.Collections;

namespace ContentExtractor.Core
{
	/// <summary>
	/// Different useful functions
	/// </summary>
	public static class Utils
	{
	  public static List<T> CastList<T>(IEnumerable collection)
	  {
	    List<T> result = new List<T>();
	    foreach(object item in collection)
	      result.Add((T)item);
	    return result;
	  }
	}
}

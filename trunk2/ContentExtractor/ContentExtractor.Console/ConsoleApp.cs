//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 05.07.2007
// Time: 14:38
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using ContentExtractor.Core;

namespace ContentExtractor.Console
{
  class ConsoleApp
  {
    private const string HelpString = @"
ContentExtractor.Console.exe <template file> <output>
Run specified template over URLs
	  ";//<output>

    [STAThread]
    public static int Main(string[] args)
    {
      if (args.Length == 0 || args.Length == 1 && args[0].ToLower() == "help")
        System.Console.WriteLine(HelpString.Trim());
      else if (args.Length == 2)
      {
        ScrapingProject project = LoadProject(args[0]);
        if(project == null) return 1;
        
        List<XmlDocument> input = new List<XmlDocument>();
        foreach (DocPosition pos in project.SourcePositions)
        {
          try
          {
            System.Console.WriteLine("Starting to process {0}", pos);
            input.Add(Utils.HtmlParse(Loader.LoadContentSync(pos)));
          }
          catch(Exception exc)
          {
            System.Console.Error.Write("Can't load given document: {0}\r\n{1}",
                                       pos,
                                       exc);
            return 2;
          }
        }
        
        XmlDocument result;
        try
        {
          result = project.Template.Transform(input);
        }
        catch(Exception exc)
        {
          System.Console.Error.Write(
            "Can't perform transformation using template: {0}\r\n{1}",
            args[0],
            exc);
          return 3;
        }
        
        try
        {
          result.Save(args[1]);
        }
        catch(Exception exc)
        {
          System.Console.Error.Write("Can't save result to file {0}\r\n{1}",
                                     args[1],
                                     exc);
          return 4;
        }
      }
      return 0;
    }
    
    private static ScrapingProject LoadProject(string filename)
    {
      ScrapingProject project = null;
      try
      {
        project = XmlUtils.Deserialize<ScrapingProject>(filename);
      }
      catch(Exception exc)
      {
        System.Console.Error.Write(
          "Error occured during loading of scrapping project {0}\r\n{1}",
          filename,
          exc);
      }
      return project;
    }
  }
}

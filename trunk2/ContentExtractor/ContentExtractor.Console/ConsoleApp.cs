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
using log4net;

namespace ContentExtractor.Console
{
  class ConsoleApp
  {
    [STAThread]
    public static int Main(string[] args)
    {
      Logger.Debug("------------------------------------------------------------------");
      Logger.DebugFormat("Command line arguments '{0}'", string.Join("', '", args));
      if (args.Length == 0 || args.Length == 1 && args[0].ToLower() == "help")
        System.Console.WriteLine(Properties.Resources.HelpString.Trim());
      else if (args.Length == 2 || args.Length == 3)
      {
        ScrapingProject project = LoadProject(args[0]);
        if (project == null)
        {
          PrintError(Properties.Resources.TemplateCannotBeParsedError, args[0]);
          return 1;
        }
        Logger.DebugFormat("Project {0} has been loaded", args[0]);

        List<DocPosition> positions = project.SourcePositions;
        if (args.Length == 3)
        {
          positions = ParsePositionsString(args[2]);
          if (positions == null)
          {
            PrintError(Properties.Resources.UrlsOrFilesCannotBeParsedError, args[2]);
            return 5;
          }
        }
        if (Logger.IsDebugEnabled)
        {
          Logger.DebugFormat("Positions has been parsed. Total number: {0}", positions.Count);
          foreach (DocPosition pos in positions)
            Logger.DebugFormat("  {0}", pos);
        }


        List<XmlDocument> input = new List<XmlDocument>();
        foreach (DocPosition pos in positions)
        {
          try
          {
            System.Console.WriteLine(Properties.Resources.StartProcessDocumentInfo, pos);
            input.Add(Utils.HtmlParse(Loader.LoadContentSync(pos)));
          }
          catch (Exception exc)
          {
            PrintError(Properties.Resources.CannotLoadDocumentError, pos, exc);
            return 2;
          }
        }

        XmlDocument result;
        try
        {
          result = project.Template.Transform(input);
        }
        catch (Exception exc)
        {
          PrintError(Properties.Resources.CannotPerformTransformationError, args[0], exc);
          return 3;
        }

        try
        {
          result.Save(args[1]);
        }
        catch (Exception exc)
        {
          PrintError(Properties.Resources.CannotSaveResult, args[1], exc);
          return 4;
        }
      }
      return 0;
    }

    private static void PrintError(string format, params object[] args)
    {
      System.Console.Error.WriteLine(format, args);
    }

    private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsoleApp));

    private static List<DocPosition> ParsePositionsString(string list)
    {
      try
      {
        List<DocPosition> result = new List<DocPosition>();
        foreach (string arg in list.Split(','))
        {
          int resultsCount = result.Count;
          if (Uri.IsWellFormedUriString(arg, UriKind.Absolute))
            result.Add(new DocPosition(arg));
          else
          {
            List<string> parts = new List<string>(
              arg.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            if (parts.Count > 1)
            {
              string start = parts[0];
              if (start.IndexOf(Path.VolumeSeparatorChar) != -1)
              {
                parts.RemoveAt(0);
                AddFiles(start + "\\", parts, result);
              }
              else
                AddFiles(Environment.CurrentDirectory, parts, result);
            }
          }
          if (result.Count == resultsCount)
            Logger.WarnFormat(Properties.Resources.CantParseDocumentAddressWarn, arg);
        }
        return result;
      }
      catch (Exception exc)
      {
        Logger.Error(exc);
      }
      return null;
    }

    private static void AddFiles(string dir, List<string> parts, List<DocPosition> result)
    {
      if (parts.Count > 1)
      {
        string start = parts[0];
        parts.RemoveAt(0);
        switch (start)
        {
          case ".":
            AddFiles(dir, parts, result);
            break;
          case "..":
            AddFiles(Directory.GetParent(dir).FullName, parts, result);
            break;
          default:
            foreach (string subdir in Directory.GetDirectories(dir, start))
              AddFiles(subdir, parts, result);
            break;
        }
      }
      else
      {
        foreach (string file in Directory.GetFiles(dir, parts[0]))
          result.Add(new DocPosition(file));
      }
    }

    private static ScrapingProject LoadProject(string filename)
    {
      ScrapingProject project = null;
      try
      {
        project = XmlUtils.Deserialize<ScrapingProject>(filename);
      }
      catch (Exception exc)
      {
        Logger.Error(exc);
      }
      return project;
    }
  }
}

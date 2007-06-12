using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ContentExtractor.Core;
using MetaTech.Library;
using System.Xml;
using System.Threading;

namespace ContentExtractor.Console
{
  class Program
  {
    static void Main(string[] args)
    {
      SoftTech.Diagnostics.Logger.EnableLogging();
      if (args.Length == 0)
      {
        System.Console.WriteLine(Properties.Resources.Help);
      }
      else
      {
        ArgsReader reader = ParseParameters(args);
        if (!reader.Error)
        {
          CollectData();
        }
      }
    }

    private static void CollectData()
    {
      Model model = new Model();
      try
      {
        model.LoadFromFile(templateFileName);
      }
      catch (Exception exc)
      {
        Messages.CantLoadTemplate(templateFileName, exc);
        return;
      }
      if (positions.Count > 0)
      {
        model.PositionsList.Clear();
        model.PositionsList.AddRange(positions);
      }
      System.Console.WriteLine(Properties.Resources.LoadingPages);
      XmlDocument result = model.Result;
      while (AsyncLoader.HasWork)
      {
        while (AsyncLoader.HasWork)
          Thread.Sleep(700);
        result = model.Result;
      }
      result = model.Result;

      if (exportFiles.ContainsKey("-html"))
        TrySaveToFile(exportFiles["-html"], DataExport.ExportToHtml(result));
      if (exportFiles.ContainsKey("-excel"))
        TrySaveToFile(exportFiles["-excel"], DataExport.ExportToExcelML(result));
      if (exportFiles.ContainsKey("-xml"))
        TrySaveToFile(exportFiles["-xml"], XmlHlp2.GetFormatedOuterXml(result.DocumentElement));
    }

    private static void TrySaveToFile(string filename, string content)
    {
      try
      {
        File.WriteAllText(filename, content, Encoding.UTF8);
        Messages.FileSaved(filename);
      }
      catch (Exception exc)
      {
        TraceHlp2.WriteException(exc);
        Messages.CantSaveFile(filename);
      }
    }

    static string templateFileName;
    private static ArgsReader ParseParameters(string[] args)
    {
      ArgsReader reader = new ArgsReader(args);
      reader.MoveNext();
      templateFileName = reader.Current;
      if (!File.Exists(templateFileName))
        Messages.TemplateFileNotFound(templateFileName);
      reader.MoveNext();
      while (!reader.EOF && !reader.Error)
      {
        switch (reader.Current.ToLower())
        {
          case "-input":
            LoadInputs(reader);
            break;
          case "-excel":
          case "-xml":
          case "-html":
            string method = reader.Current.ToLower();
            if (!reader.MoveNext() || reader.Current.StartsWith("-"))
            {
              Messages.NoExportName(method);
              reader.Error = true;
            }
            exportFiles[method] = reader.Current;
            reader.MoveNext();
            break;
          default:
            Messages.UnknownSpecifier(reader.Current);
            reader.Error = true;
            break;
        }
      }
      if (!reader.Error && exportFiles.Count == 0)
      {
        Messages.NoExportFilenames();
        reader.Error = true;
      }
      return reader;
    }
    private static Dictionary<string, string> exportFiles = new Dictionary<string, string>();

    private static void LoadInputs(ArgsReader reader)
    {
      while (reader.MoveNext() && IsExportSpec(reader.Current))
      {
        Uri uri;
        if (Uri.TryCreate(reader.Current, UriKind.Absolute, out uri) ||
          Uri.TryCreate("http://" + reader.Current, UriKind.Absolute, out uri))
        {
          positions.Add(new WebPosition(uri));
        }
        else
        {
          Messages.BadUri(reader.Current);
          reader.Error = true;
          break;
        }
      }
    }

    private static bool IsExportSpec(string str)
    {
      return Array.FindIndex(ExportNames, delegate(string s) { return "-" + s == str.ToLower(); }) == -1;
    }

    private static List<WebPosition> positions = new List<WebPosition>();

    private static readonly string[] ExportNames = new string[ ] { "excel", "html", "xml" };

  }

  class ArgsReader
  {
    public ArgsReader(IEnumerable<string> args)
    {
      queue = new Queue<string>(args);
    }

    public bool Error = false;

    private Queue<string> queue;

    public bool EOF
    {
      get
      {
        return queue.Count == 0 && current == null;
      }
    }

    public string Current
    {
      get
      {
        return current;
      }
    }

    public bool MoveNext()
    {
      if (queue.Count > 0)
      {
        current = queue.Dequeue();
        return true;
      }
      else
      {
        current = null;
        return false;
      }
    }

    private string current;

  }

  static class Messages
  {
    public static void TemplateFileNotFound(string fileName)
    {
      System.Console.WriteLine(Properties.Resources.TemplateDoesntExist, fileName);
    }

    public static void NoExportFilenames()
    {
      System.Console.WriteLine(Properties.Resources.NoExportFiles);
    }

    public static void BadUri(string code)
    {
      System.Console.WriteLine(Properties.Resources.BadUri, code);
    }

    public static void NoExportName(string method)
    {
      System.Console.WriteLine(Properties.Resources.NoExportFilename, method);
    }

    public static void UnknownSpecifier(string p)
    {
      System.Console.WriteLine(Properties.Resources.UnknownSpecifier, p);
    }

    public static void CantLoadTemplate(string templateFileName, Exception exc)
    {
      System.Console.WriteLine(Properties.Resources.CantLoadTemplate, templateFileName);
    }

    public static void CantSaveFile(string filename)
    {
      System.Console.WriteLine(Properties.Resources.CantSaveFile, filename);
    }

    public static void FileSaved(string filename)
    {
      System.Console.WriteLine(Properties.Resources.FileSaved, filename);
    }
  }
}

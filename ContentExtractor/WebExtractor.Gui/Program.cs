#define DEBUG

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      try
      {
        SoftTech.Diagnostics.Logger.EnableLogging();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Model model = new Model();
        MainForm form = new MainForm();
        form.InitModel(delegate { return model; });
        Application.Run(form);
      }
      finally
      {
        Properties.Settings.Default.Save();
      }
    }
  }
}
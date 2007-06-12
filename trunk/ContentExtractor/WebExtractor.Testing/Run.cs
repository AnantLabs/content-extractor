#define DEBUG

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebExtractor_Testing
{
  class Run
  {
    [STAThread]
    static void Main(string[] args)
    {
      SoftTech.Diagnostics.Logger.EnableLogging();


      //DocumentTreeFromWebBrowser tests = new DocumentTreeFromWebBrowser();
      //tests.SetUp();
      //tests.БраузерСтроитДеревоДляYandex();
      //tests.TeadDown();

      //DocumentTreeToTreeView tests = new DocumentTreeToTreeView();
      //tests.SetUp();
      //tests.СинхронизацияДерева();
      //tests.TearDown();

      //TemplateTests tests = new TemplateTests();
      //tests.SetUp();
      //tests.ШаблонДляСтрокиИзДвухЯчеек();
    }
  }
}

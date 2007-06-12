using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaTech.Library;
using System.IO;
using SoftTech.Diagnostics;
using System.Xml.Serialization;
using ContentExtractor.Core;
using SoftTech.Gui;
using System.Xml;
using System.Diagnostics;

namespace ContentExtractor.Gui
{
  public partial class MainForm : Form , IView
  {
    public MainForm()
    {
      InitializeComponent();
      stateManager = new ModelStateManager(LoadModelFromSettings, SaveModelToSettings);

      Splash splash = new Splash(this, 5);
      splash.TopMost = true;
      splash.Show();
    }

    Getter<Model> GetModel = delegate { return new Model(); };

    public void InitModel(Getter<Model> modelGetter)
    {
      this.GetModel = modelGetter;

      TreeViewWrapper treeViewWrapper = new TreeViewWrapper();
      Views.Add(treeViewWrapper);

      BrowsersView browserWrapper = new BrowsersView();
      Views.Add(browserWrapper);

      GridWrapper gridWrapper1 = new GridWrapper();
      Views.Add(gridWrapper1);

      SelectedNodeTextBox htmlBox = new SelectedNodeTextBox();
      htmlBox.TextTaker = delegate(XmlNode node) { return XmlHlp2.GetFormatedOuterXml(node); };
      Views.Add(htmlBox);

      SelectedNodeTextBox innerTextBox = new SelectedNodeTextBox();
      innerTextBox.TextTaker = delegate(XmlNode node) { return Functions.InnerText(node); };
      Views.Add(innerTextBox);

      PagesPanel pages = new PagesPanel();
      Views.Add(pages);

      Views.Add(stateManager);

      foreach (IView view in Views)
        view.InitModel(GetModel);

      //VS2005Style.Extender.SetSchema(panel1, VS2005Style.Extender.Schema.FromBase);

      dockManager = new DockContentManager(panel1, this, true,
       delegate
       {
         return typeof(Layout._Dummy).Assembly.GetManifestResourceStream(typeof(Layout._Dummy), "Layout.user.config");
       }, null);
      dockManager.AddControl(treeViewWrapper, "HtmlTreeView", "Html дерево");
      dockManager.AddControl(browserWrapper, "Browser", "Браузер");
      dockManager.AddControl(gridWrapper1, "ResultGrid", "Результат");
      dockManager.AddControl(htmlBox, "HtmlCodeBox", "Html");
      dockManager.AddControl(innerTextBox, "HtmlTextBox", "Текст");
      dockManager.AddControl(pages, "PagesPanel", "Страницы");
      menuStrip1.Items.Add(dockManager.GenerateMenu());

      controlsToHideInBrowse.AddRange(new Control[] { treeViewWrapper, gridWrapper1, htmlBox, innerTextBox });
      viewsTimer.Enabled = true;
    }

    List<Control> controlsToHideInBrowse = new List<Control>();

    DockContentManager dockManager;

    List<IView> Views = new List<IView>();
    private string fileName = string.Empty;
    private ModelStateManager stateManager;
    private Properties.Settings Settings = Properties.Settings.Default;// new ContentExtractor.Gui.Properties.Settings();

    private Model LoadModelFromSettings()
    {
      Model result = new Model();
      try
      {
        if (!Settings.HasUpgraded)
        {
          Settings.Upgrade();
          Settings.HasUpgraded = true;
          Settings.Save();
        }
        result.Load(Settings.SavedModel);
      }
      catch (Exception exc)
      {
        TraceHlp2.WriteException(exc);
      }
      return result;
    }

    private void SaveModelToSettings(Model model)
    {
      Settings.SavedModel = model.Persist;
      Settings.Save();
    }

    public void ForceSynchronize()
    {
      undoStripButton.Enabled = stateManager.UndoAvailable;
      undoToolStripMenuItem.Enabled = stateManager.UndoAvailable;
      redoStripButton.Enabled = stateManager.RedoAvailable;
      redoToolStripMenuItem.Enabled = stateManager.RedoAvailable;

      browserModeButton.Checked = GetModel().Mode == Model.WorkMode.Browse;
      parseModeButton.Checked = GetModel().Mode == Model.WorkMode.Parse;

      if (!cachedWorkMode.HasValue || cachedWorkMode.Value != GetModel().Mode)
      {
        cachedWorkMode = GetModel().Mode;
        if (cachedWorkMode.Value == Model.WorkMode.Browse)
          foreach (Control c in controlsToHideInBrowse)
            dockManager.Hide(c);
        else if (cachedWorkMode.Value == Model.WorkMode.Parse)
          foreach (Control c in controlsToHideInBrowse)
            dockManager.Show(c);
      }
    }
    private Model.WorkMode? cachedWorkMode = null;

    private void viewsTimer_Tick(object sender, EventArgs e)
    {
      foreach (IView view in Views)
        view.ForceSynchronize();
      this.ForceSynchronize();
    }


    private void LoadModel()
    {
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        GetModel().LoadFromFile(openFileDialog1.FileName);
      }
    }

    private void SaveModel()
    {
      if (!GetModel().HasFileName)
        SaveModelAs();
      else
        GetModel().SaveToFile(GetModel().FileName);
    }


    private void SaveModelAs()
    {
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        GetModel().SaveToFile(saveFileDialog1.FileName);
      }
    }


    private void UndoModel()
    {
      stateManager.Undo();
    }

    private void RedoModel()
    {
      stateManager.Redo();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LoadModel();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveModel();
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      UndoModel();
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RedoModel();
    }

    private void toolStripButton1_Click(object sender, EventArgs e)
    {
      LoadModel();
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      SaveModel();
    }

    private void toolStripButton3_Click(object sender, EventArgs e)
    {
      UndoModel();
    }

    private void toolStripButton4_Click(object sender, EventArgs e)
    {
      RedoModel();
    }

    private void browserModeButton_Click(object sender, EventArgs e)
    {
      GetModel().Mode = Model.WorkMode.Browse;
    }

    private void parseModeButton_Click(object sender, EventArgs e)
    {
      GetModel().Mode = Model.WorkMode.Parse;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SaveModelAs();
    }

    private void contentExtractorHomepageToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ProcessStartInfo startInfo = new ProcessStartInfo("http://www.contentextractor.com");
      startInfo.UseShellExecute = true;
      Process.Start(startInfo);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      //if (!LicenseManager.IsLicensed(typeof(Core.Model)))
      //{
      //  ShowNag();
      //}
    }
  }
}

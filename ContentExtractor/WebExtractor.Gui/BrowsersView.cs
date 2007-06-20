using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ContentExtractor.Core;
using MetaTech.Library;

namespace ContentExtractor.Gui
{
  public partial class BrowsersView : UserControl, IView
  {
    public BrowsersView()
    {
      InitializeComponent();
    }

    #region IView Members

    private Getter<Model> GetModel = delegate { return new Model(); };

    public void InitModel(Getter<Model> modelGetter)
    {
      this.GetModel = modelGetter;
    }

    private Model Model
    {
      get { return GetModel(); }
    }

    public void ForceSynchronize()
    {
      if (!WebExtractorHlp.CompareList(Positions, Model.CurrentPositions))
      {
        while (!SynchronizePages()) ;
      }
      for (int i = 0; i < Pages.Count; i++)
      {
        string shortTitle = ShortTitle(Pages[i].Title);
        if (tabControl1.TabPages[i].Text != shortTitle)
        {
          tabControl1.TabPages[i].Text = shortTitle;
          tabControl1.TabPages[i].ToolTipText = Pages[i].Title;
        }
        Pages[i].ForceSynchronize();
      }
      tabControl1.SelectedIndex = Model.ActiveIndex;
      //newTabToolStripMenuItem.Visible = Model.Mode == Model.WorkMode.Browse;
    }

    private static string ShortTitle(string longTitle)
    {
      string shortTitle = string.Empty;
      if (longTitle.Length > 25)
      {
        shortTitle = longTitle.Remove(22) + "...";
      }
      else
        shortTitle = longTitle;
      return shortTitle;
    }

    private bool SynchronizePages()
    {
      for (int index = 0; index < Model.CurrentPositions.Count; index++)
      {
        if (index >= Positions.Count || Positions[index] != Model.CurrentPositions[index])
        {
          int correspondent = Positions.FindIndex(index, delegate(WebPosition pos) { return pos == Model.CurrentPositions[index]; });
          if (correspondent != -1)
          {
            TabPage correspondentPage = tabControl1.TabPages[correspondent];
            tabControl1.TabPages.RemoveAt(correspondent);
            tabControl1.TabPages.Insert(index, correspondentPage);
            return false;
          }
          else
          {
            BrowserPage browserPage = new BrowserPage(Model.CurrentPositions[index]);
            browserPage.InitModel(GetModel);
            browserPage.Dock = DockStyle.Fill;
            TabPage newPage = new TabPage();
            newPage.Controls.Add(browserPage);
            if (index < tabControl1.TabPages.Count)
              tabControl1.TabPages.Insert(index, newPage);
            else
              tabControl1.TabPages.Add(newPage);
            return false;
          }
        }
      }
      if (Positions.Count > Model.CurrentPositions.Count)
      {
        for (int index = Model.CurrentPositions.Count; index < Positions.Count; index++)
          tabControl1.TabPages.RemoveAt(index);
      }
      return true;
    }

    #endregion

    private Getter<WebPosition> PositionGetter(int index)
    {
      return delegate
      {
        if (0 <= index && index < Model.CurrentPositions.Count)
          return Model.CurrentPositions[index];
        else
          return WebPosition.EmptyPosition;
      };
    }

    private Executter<WebPosition> PositionSetter(int index)
    {
      return delegate(WebPosition pos)
      {
        if (0 <= index && index < Model.CurrentPositions.Count)
          Model.CurrentPositions[index] = pos;
      };
    }

    private List<BrowserPage> Pages
    {
      get
      {
        List<BrowserPage> result = new List<BrowserPage>();
        foreach (TabPage page in tabControl1.TabPages)
        {
          BrowserPage browser = (BrowserPage)page.Controls[0];
          result.Add(browser);
        }
        return result;
      }
    }

    private List<WebPosition> Positions
    {
      get
      {
        return Pages.ConvertAll<WebPosition>(delegate(BrowserPage page) { return page.Position; });
      }
    }

    private void tabControl1_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        for (int i = 0; i < tabControl1.TabCount; i++)
        {
          if (tabControl1.GetTabRect(i).Contains(e.Location))
          {
            TabsContextMenu.Tag = tabControl1.TabPages[i].Controls[0];
            TabsContextMenu.Show(tabControl1.PointToScreen(e.Location));
            return;
          }
        }
      }
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      BrowserPage page = TabsContextMenu.Tag as BrowserPage;
      if (page != null)
      {
        Model.CurrentPositions.Remove(page.Position);
      }

    }

    private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Model.CurrentPositions.Insert(Model.ActiveIndex + 1, WebPosition.EmptyPosition);
      Model.ActiveIndex = Model.ActiveIndex + 1;
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      Model.ActiveIndex = tabControl1.SelectedIndex;
    }

  }
}

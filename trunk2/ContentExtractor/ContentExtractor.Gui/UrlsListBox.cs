//
// Created by Sergey Babenko
//  mc.vertix@gmail.com
//
// Date: 13.07.2007
// Time: 19:29
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  /// <summary>
  /// Description of UrlsListBox.
  /// </summary>
  public partial class UrlsListBox : UserControl
  {
    public UrlsListBox()
    {
      //
      // The InitializeComponent() call is required for Windows Forms designer support.
      //
      InitializeComponent();
    }

    private State state = null;

    public void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;
        UpdateListBox();
      }
      else
        throw new InvalidOperationException("Cannot set state twice");
    }

    public void UpdateListBox()
    {
      List<object> selected = Utils.CastList<object>(this.listBox1.SelectedItems);
      listBox1.Items.Clear();
      foreach (Uri uri in state.Project.SourceUrls)
      {
        Utils.CheckNotNull(uri);
        listBox1.Items.Add(uri);
      }
      foreach (object item in selected)
        if (listBox1.Items.Contains(item))
        {
          listBox1.SelectedItems.Add(item);
        }
    }

    public void AddUri(Uri uri)
    {
      try
      {
        Utils.CheckNotNull(uri);
        state.Project.SourceUrls.Add(uri);
        UpdateListBox();
      }
      catch (Exception exc)
      {
        string log = exc.ToString();
        Console.WriteLine(exc);
      }
    }

    public void SelectUri(int index)
    {
      //listBox1.SelectedIndex = index;
      state.BrowserUri = state.Project.SourceUrls[index];
    }

    void ListBox1DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = e.AllowedEffect;
    }

    void ListBox1DragDrop(object sender, DragEventArgs e)
    {
      string[] Formats = e.Data.GetFormats();
      List<string> links = new List<string>();
      object obData = e.Data.GetData(DataFormats.Text);
      if (obData != null && obData is string)
        links.Add((string)obData);
      else
      {
        obData = e.Data.GetData(DataFormats.FileDrop);
        if (obData is string[])
          links.AddRange((string[])obData);
      }

      foreach (string link in links)
      {
        Uri uri = Utils.ParseUrl(link);
        if (uri != null)
          AddUri(uri);
      }
    }

    void ListBox1SelectedIndexChanged(object sender, EventArgs e)
    {
      SelectUri(listBox1.SelectedIndex);
    }

    public void Delete(int index)
    {
      state.Project.SourceUrls.RemoveAt(index);
      UpdateListBox();
    }

    private void listBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete &&
          0 <= listBox1.SelectedIndex &&
          listBox1.SelectedIndex < listBox1.Items.Count)
      {
        Delete(listBox1.SelectedIndex);
      }
    }
  }
}

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
        SynchronizedCollection<DocPosition> positionsSynchro;

        positionsSynchro = new SynchronizedCollection<DocPosition>(
          delegate { return (IList<DocPosition>)this.state.Project.SourcePositions; });
        this.components.Add(positionsSynchro);

        bindingSource1.DataSource = positionsSynchro;
      }
      else
        throw new InvalidOperationException("Cannot set state twice");
    }

    //  public void UpdateListBox()
    //  {
    //    List<object> selected = Utils.CastList<object>(this.listBox1.SelectedItems);
    //    listBox1.Items.Clear();
    //    foreach (Uri uri in state.Project.SourceUrls)
    //    {
    //      Utils.CheckNotNull(uri);
    //      listBox1.Items.Add(uri);
    //    }
    //    foreach (object item in selected)
    //      if (listBox1.Items.Contains(item))
    //      {
    //        listBox1.SelectedItems.Add(item);
    //      }
    //  }

    //  public void AddUri(Uri uri)
    //  {
    //    try
    //    {
    //      Utils.CheckNotNull(uri);
    //      listBox1.Items.Add(uri);
    //      //state.Project.SourceUrls.Add(uri);
    //      //UpdateListBox();
    //    }
    //    catch (Exception exc)
    //    {
    //      string log = exc.ToString();
    //      Console.WriteLine(exc);
    //    }
    //  }

    //  public void SelectUri(int index)
    //  {
    //    if (0 <= index && index < state.Project.SourceUrls.Count)
    //    {
    //      state.BrowserUri = state.Project.SourceUrls[index];
    //    }
    //    else
    //    {
    //      state.BrowserUri = ScrapingProject.EmptyUri;
    //    }
    //    //listBox1.SelectedIndex = index;
    //  }

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
        DocPosition position = new DocPosition(Utils.ParseUrl(link));
        if (position != null)
          state.Project.SourcePositions.Add(position);
      }
    }

    public void Swap(int leftIndex, int rightIndex)
    {
      DocPosition old_position = state.Project.SourcePositions[leftIndex];
      state.Project.SourcePositions[leftIndex] = state.Project.SourcePositions[rightIndex];
      state.Project.SourcePositions[rightIndex] = old_position;
    }

    private void listBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete &&
        Utils.IsIndexOk(listBox1.SelectedIndex, state.Project.SourcePositions))
      {
        state.Project.SourcePositions.RemoveAt(listBox1.SelectedIndex);
      }
    }

    private void delButton_Click(object sender, EventArgs e)
    {
      if (Utils.IsIndexOk(listBox1.SelectedIndex, state.Project.SourcePositions))
        state.Project.SourcePositions.RemoveAt(listBox1.SelectedIndex);
    }

    private void upButton_Click(object sender, EventArgs e)
    {
      if (Utils.IsIndexOk(listBox1.SelectedIndex, listBox1.Items) &&
          listBox1.SelectedIndex > 0)
      {
        int old_index = listBox1.SelectedIndex;
        Swap(old_index, old_index - 1);
        listBox1.SelectedIndex = old_index - 1;
      }
    }

    private void downButton_Click(object sender, EventArgs e)
    {
      if (Utils.IsIndexOk(listBox1.SelectedIndex, listBox1.Items) &&
          listBox1.SelectedIndex < listBox1.Items.Count - 1)
      {
        int old_index = listBox1.SelectedIndex;
        Swap(old_index, old_index + 1);
        listBox1.SelectedIndex = old_index + 1;
      }
    }

    private void bindingSource1_CurrentItemChanged(object sender, EventArgs e)
    {
      if (Utils.IsIndexOk(listBox1.SelectedIndex, bindingSource1))
        state.BrowserPosition = (DocPosition)bindingSource1[listBox1.SelectedIndex];
    }
  }
}

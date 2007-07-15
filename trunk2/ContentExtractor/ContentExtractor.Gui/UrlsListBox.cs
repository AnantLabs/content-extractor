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
      
      //
      // TODO: Add constructor code after the InitializeComponent() call.
      //
    }
    
    private State state = null;
    
    public void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;
      }
      else
        throw new InvalidOperationException("Cannot set state twice");
    }
    
    public void AddUri(Uri uri)
    {
      try{
        Utils.CheckNotNull(uri);
        state.Project.SourceUrls.Add(uri);
        listBox1.Items.Add(uri.AbsoluteUri);
      } catch (Exception exc)
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
        if(uri != null)
          AddUri(uri);
      }
    }
		
		void ListBox1SelectedIndexChanged(object sender, EventArgs e)
		{
		  SelectUri(listBox1.SelectedIndex);
		}
  }
}

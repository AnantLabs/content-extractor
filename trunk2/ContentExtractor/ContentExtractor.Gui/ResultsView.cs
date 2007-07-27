using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ContentExtractor.Core;

namespace ContentExtractor.Gui
{
  public partial class ResultsView : UserControl
  {
    public ResultsView()
    {
      InitializeComponent();

      dataGrid.AutoGenerateColumns = true;
      //bindingSource.DataSource = new CustomList();
    }

    private State state;

    public void SetState(State state)
    {
      if (this.state == null)
      {
        this.state = state;
        rows = new ResultRows(state);
        bindingSource.DataSource = rows;
      }
      else
        //TODO: Should log to warning
        throw new InvalidOperationException("Cannot assign state twice");
    }
    private ResultRows rows;

    private void timer1_Tick(object sender, EventArgs e)
    {
      rows.Refresh();
    }

    private void toolStripButton2_Click(object sender, EventArgs e)
    {
      state.Project.Template = new Template();
    }
  }

  public class ResultRows : List<XmlNode>, ITypedList, IBindingList
  {
    public ResultRows(State state)
    {
      this.state = state;
    }
    private State state;

    internal void Refresh()
    {
      this.Clear();
      List<XmlDocument> documents = state.Project.SourceUrls.ConvertAll<XmlDocument>(
          delegate(Uri u) { return state.GetXmlAsync(u); });
      XmlDocument result = state.Project.Template.Transform(documents);
      XmlNamespaceManager nsManager = new XmlNamespaceManager(result.NameTable);
      nsManager.AddNamespace(Template.CEXPrefix, Template.CEXNamespace);
      string rowXPath = string.Format("/{0}:{1}/{0}:{2}",
                                      Template.CEXPrefix,
                                      Template.DocumentTag,
                                      Template.RowTag);
      foreach (XmlNode row in result.SelectNodes(rowXPath, nsManager))
        this.Add(row);
      if (ListChanged != null)
        ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, null));
    }

    #region ITypedList Members

    public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
    {
      PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
      int index = 1;
      foreach (string localXPath in state.Project.Template.Columns)
      {
        result.Add(new XPathPropertyDescriptor(localXPath,
          string.Format("{0}:{1}[{2}]", Template.CEXPrefix, Template.CellTag, index)));
        index++;
      }
      return result;
    }

    public string GetListName(PropertyDescriptor[] listAccessors)
    {
      return typeof(ResultRows).Name;
    }

    #endregion

    #region IBindingList Members

    public void AddIndex(PropertyDescriptor property)
    {
      throw new NotImplementedException();
    }

    public object AddNew()
    {
      throw new NotImplementedException();
    }

    public bool AllowEdit
    {
      get { return false; }
    }

    public bool AllowNew
    {
      get { return false; }
    }

    public bool AllowRemove
    {
      get { return false; }
    }

    public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
      throw new NotImplementedException();
    }

    public int Find(PropertyDescriptor property, object key)
    {
      throw new NotImplementedException();
    }

    public bool IsSorted
    {
      get { return false; }
    }

    public event ListChangedEventHandler ListChanged;

    public void RemoveIndex(PropertyDescriptor property)
    {
      throw new NotImplementedException();
    }

    public void RemoveSort()
    {
      throw new NotImplementedException();
    }

    public ListSortDirection SortDirection
    {
      get { throw new NotImplementedException(); }
    }

    public PropertyDescriptor SortProperty
    {
      get { throw new NotImplementedException(); }
    }

    public bool SupportsChangeNotification
    {
      get { return true; }
    }

    public bool SupportsSearching
    {
      get { return false; }
    }

    public bool SupportsSorting
    {
      get { return false; }
    }

    #endregion
  }

  public class XPathPropertyDescriptor : PropertyDescriptor
  {
    public XPathPropertyDescriptor(string name, string xpath)
      : base(name, null)
    {
      this.xpath = xpath;
    }
    string xpath;

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override Type ComponentType
    {
      get { return this.PropertyType; }
    }

    public override object GetValue(object component)
    {
      XmlNode node = component as XmlNode;
      if (node != null)
      {
        XmlNamespaceManager nsManager = new XmlNamespaceManager(node.OwnerDocument.NameTable);
        nsManager.AddNamespace(Template.CEXPrefix, Template.CEXNamespace);
        StringBuilder result = new StringBuilder();
        foreach (XmlNode cellMatch in node.SelectNodes(xpath, nsManager))
        {
          result.Append(cellMatch.InnerXml);
          result.Append(" ");
        }
        return result.ToString().Trim();
      }
      return null;
    }

    public override bool IsReadOnly
    {
      get { return true; }
    }

    public override Type PropertyType
    {
      get { return typeof(string); }
    }

    public override void ResetValue(object component)
    {
      throw new NotImplementedException();
    }

    public override void SetValue(object component, object value)
    {
      throw new NotImplementedException();
    }

    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }
  }
}

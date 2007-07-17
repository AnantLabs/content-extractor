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
      bindingSource.DataSource = new CustomList();
    }
  }

  public class CustomList : System.Collections.Generic.List<XmlNode>, ITypedList, IBindingList
  {
    public CustomList()
      :base()
    {
      this.doc = XmlUtils.LoadXml("<s><row><cell>123</cell><cell>345</cell></row></s>");
      foreach (XmlNode node in doc.SelectNodes("/s/row"))
        this.Add(node);
    }
    private XmlDocument doc;

    #region ITypedList Members

    PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
    {
      PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
      result.Add(new XPathPropertyDescriptor("cell[1]"));
      result.Add(new XPathPropertyDescriptor("cell[2]"));
      return result;
    }

    string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
    {
      return typeof(CustomList).Name;
    }

    #endregion

    //#region IBindingList Members

    //public void AddIndex(PropertyDescriptor property)
    //{
    //  throw new Exception("The method or operation is not implemented.");
    //}

    //public object AddNew()
    //{
    //  throw new Exception("The method or operation is not implemented.");
    //}

    //public bool AllowEdit
    //{
    //  get { return false; }
    //}

    //public bool AllowNew
    //{
    //  get { return false; }
    //}

    //public bool AllowRemove
    //{
    //  get { return false; }
    //}

    //public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
    //{
    //  throw new Exception("The method or operation is not implemented.");
    //}

    //public int Find(PropertyDescriptor property, object key)
    //{
    //  throw new Exception("The method or operation is not implemented.");
    //}

    //public bool IsSorted
    //{
    //  get { return false; }
    //}

    //public event ListChangedEventHandler ListChanged;

    //public void RemoveIndex(PropertyDescriptor property)
    //{
    //  throw new Exception("The method or operation is not implemented.");
    //}

    //public void RemoveSort()
    //{
    //  throw new Exception("The method or operation is not implemented.");
    //}

    //public ListSortDirection SortDirection
    //{
    //  get { throw new Exception("The method or operation is not implemented."); }
    //}

    //public PropertyDescriptor SortProperty
    //{
    //  get { throw new Exception("The method or operation is not implemented."); }
    //}

    //public bool SupportsChangeNotification
    //{
    //  get { return false; }
    //}

    //public bool SupportsSearching
    //{
    //  get { return false; }
    //}

    //public bool SupportsSorting
    //{
    //  get { return false; }
    //}

    //#endregion
  }

  public class XPathPropertyDescriptor : PropertyDescriptor
  {
    public XPathPropertyDescriptor(string xpath)
      : base(xpath, null)
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
        XmlNode cell = node.SelectSingleNode(xpath);
        return cell.InnerXml;
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

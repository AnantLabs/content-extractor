using System;
using System.Collections.Generic;
using System.Text;
using ContentExtractor.Core;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;

namespace ContentExtractor.Gui
{
  public class SynchronizedObject<T> : Component, INotifyPropertyChanged
  {
    public SynchronizedObject(Getter<T> getter, Callback<T> setter)
    {
      Utils.CheckNotNull(getter);
      this._getter = getter;
      this._setter = setter;
      _timer = new Timer();
      _timer.Tick += new EventHandler(timer_Tick);
      _timer.Interval = (int)DefaultInterval.TotalMilliseconds;
      _timer.Enabled = true;
    }

    private Getter<T> _getter;
    private Callback<T> _setter;
    private Timer _timer;

    public static TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(500);

    public T Value
    {
      get
      {
        return _getter();
      }
      set
      {
        if (_setter != null)
          _setter(value);
      }
    }

    bool cachedValueInited = false;
    T cachedValue;

    void timer_Tick(object sender, EventArgs e)
    {
      if (!cachedValueInited)
      {
        cachedValue = _getter();
        cachedValueInited = true;
      }
      else
      {
        T value = _getter();
        if (!object.Equals(cachedValue, value))
        {
          cachedValue = value;
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs("Value"));
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _timer.Dispose();
      }
      base.Dispose(disposing);
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }

  public class SynchronizedCollection<TValue> : Component, IBindingList
    where TValue : new()
  {
    public SynchronizedCollection(Getter<IList<TValue>> getter)
    {
      Utils.CheckNotNull(getter);
      this._getter = getter;
      _timer = new Timer();
      _timer.Tick += new EventHandler(timer_Tick);
      _timer.Interval = (int)DefaultInterval.TotalMilliseconds;
      _timer.Enabled = true;
    }

    private Getter<IList<TValue>> _getter;
    private Timer _timer;

    public static TimeSpan DefaultInterval = TimeSpan.FromMilliseconds(500);

    private IList<TValue> Value
    {
      get { return _getter(); }
    }

    bool cachedValueInited = false;
    List<TValue> cache = new List<TValue>();

    private void CacheCollection(IList<TValue> collection)
    {
      cache.Clear();
      cache.AddRange(collection);
      cachedValueInited = true;
    }

    private static bool IsCollectionsEqual(IList<TValue> left, IList<TValue> right)
    {
      bool result = left.Count == right.Count;
      if (result)
      {
        for (int i = 0; i < left.Count && result; i++)
          result &= object.Equals(left[i], right[i]);
      }
      return result;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      if (cachedValueInited)
      {
        IList<TValue> value = _getter();
        if (!IsCollectionsEqual(cache, value))
        {
          CacheCollection(value);
          IBindingList this_as_bindinglist = (IBindingList)this;
          if (_listChanged != null)
          {
            _listChanged(this, new ListChangedEventArgs(ListChangedType.Reset, -1));
          }
        }
      }
      else
        CacheCollection(Value);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _timer.Dispose();
      }
      base.Dispose(disposing);
    }


    #region IBindingList Members

    void IBindingList.AddIndex(PropertyDescriptor property)
    {
      throw new NotSupportedException();
    }

    object IBindingList.AddNew()
    {
      TValue new_element = new TValue();
      Value.Add(new_element);
      return new_element;
    }

    bool IBindingList.AllowEdit
    {
      get { return false; }
    }

    bool IBindingList.AllowNew
    {
      get { return true; }
    }

    bool IBindingList.AllowRemove
    {
      get { return true; }
    }

    void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
      throw new NotSupportedException();
    }

    int IBindingList.Find(PropertyDescriptor property, object key)
    {
      throw new NotSupportedException();
    }

    bool IBindingList.IsSorted
    {
      get { return false; }
    }

    private event ListChangedEventHandler _listChanged;

    event ListChangedEventHandler IBindingList.ListChanged
    {
      add { _listChanged += value; }
      remove { _listChanged -= value; }
    }

    void IBindingList.RemoveIndex(PropertyDescriptor property)
    {
      throw new NotSupportedException();
    }

    void IBindingList.RemoveSort()
    {
      throw new NotSupportedException();
    }

    ListSortDirection IBindingList.SortDirection
    {
      get { throw new NotSupportedException(); }
    }

    PropertyDescriptor IBindingList.SortProperty
    {
      get { throw new NotSupportedException(); }
    }

    bool IBindingList.SupportsChangeNotification
    {
      get { return true; }
    }

    bool IBindingList.SupportsSearching
    {
      get { return false; }
    }

    bool IBindingList.SupportsSorting
    {
      get { return false; }
    }

    #endregion

    #region IList Members

    int IList.Add(object value)
    {
      return ((IList)Value).Add((TValue)value);
    }

    void IList.Clear()
    {
      Value.Clear();
    }

    bool IList.Contains(object value)
    {
      return Value.Contains((TValue)value);
    }

    int IList.IndexOf(object value)
    {
      return Value.IndexOf((TValue)value);
    }

    void IList.Insert(int index, object value)
    {
      Value.Insert(index, (TValue)value);
    }

    bool IList.IsFixedSize
    {
      get { return ((IList)Value).IsFixedSize; }
    }

    bool IList.IsReadOnly
    {
      get { return ((IList)Value).IsReadOnly; }
    }

    void IList.Remove(object value)
    {
      Value.Remove((TValue)value);
    }

    void IList.RemoveAt(int index)
    {
      Value.RemoveAt(index);
    }

    object IList.this[int index]
    {
      get
      {
        return Value[index];
      }
      set
      {
        Value[index] = (TValue)value;
      }
    }

    #endregion

    #region ICollection Members

    void ICollection.CopyTo(Array array, int index)
    {
      ((ICollection)Value).CopyTo(array, index);
    }

    int ICollection.Count
    {
      get { return Value.Count; }
    }

    bool ICollection.IsSynchronized
    {
      get { return ((ICollection)Value).IsSynchronized; }
    }

    object ICollection.SyncRoot
    {
      get { return ((ICollection)Value).SyncRoot; }
    }

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)Value).GetEnumerator();
    }

    #endregion
  }
}

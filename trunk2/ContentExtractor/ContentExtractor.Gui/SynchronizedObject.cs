using System;
using System.Collections.Generic;
using System.Text;
using ContentExtractor.Core;
using System.Windows.Forms;
using System.ComponentModel;

namespace ContentExtractor.Gui
{
  internal class SynchronizedObject<T> : Component, INotifyPropertyChanged
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
}

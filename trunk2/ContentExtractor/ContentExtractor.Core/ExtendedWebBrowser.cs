using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ContentExtractor.Core
{
  public class ExtendedNavigatingEventArgs : CancelEventArgs
  {
    public readonly string Url;
    public readonly string Frame;

    public ExtendedNavigatingEventArgs(string url, string frame)
      : base()
    {
      Url = url;
      Frame = frame;
    }
  }

  [ComImport(), Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch),
    TypeLibType(TypeLibTypeFlags.FHidden)]
  public interface IDWebBrowserEvents2
  {
    [DispId(250)]
    void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL,
      [In] ref object flags, [In] ref object targetFrameName,
      [In] ref object postData, [In] ref object headers, [In, Out] ref bool cancel);
    [DispId(273)]
    void NewWindow3([In, Out] ref object pDisp, [In, Out] ref bool cancel,
      [In] ref object flags, [In] ref object URLContext, [In] ref object URL);
  }


  /// <summary>
  /// Additional wrapper ober WebBrowser control.
  /// Enables OnBeforeNewWindow and OnBeforeNavigate events.
  /// See also:
  /// http://forums.microsoft.com/msdn/showpost.aspx?postid=12559&siteid=1
  /// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=115195
  /// </summary>
  public class ExtendedWebBrowser : WebBrowser
  {
    AxHost.ConnectionPointCookie cookie;
    WebBrowserExtendedEvents events;

    protected override void CreateSink()
    {
      //MAKE SURE TO CALL THE BASE or the normal events won't fire
      base.CreateSink();
      events = new WebBrowserExtendedEvents(this);
      cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, events, typeof(IDWebBrowserEvents2));
    }

    protected override void DetachSink()
    {
      if (null != cookie)
      {
        cookie.Disconnect();
        cookie = null;
      }
      base.DetachSink();
    }

    public event EventHandler<ExtendedNavigatingEventArgs> BeforeNavigate;
    public event EventHandler<ExtendedNavigatingEventArgs> BeforeNewWindow;

    protected void OnBeforeNewWindow(string url, out bool cancel)
    {
      EventHandler<ExtendedNavigatingEventArgs> handler = BeforeNewWindow;
      ExtendedNavigatingEventArgs args = new ExtendedNavigatingEventArgs(url, null);
      if (null != handler)
      {
        handler(this, args);
      }
      cancel = args.Cancel;
    }

    protected void OnBeforeNavigate(string url, string frame, out bool cancel)
    {
      EventHandler<ExtendedNavigatingEventArgs> handler = BeforeNavigate;
      ExtendedNavigatingEventArgs args = new ExtendedNavigatingEventArgs(url, frame);
      if (null != handler)
      {
        handler(this, args);
      }
      cancel = args.Cancel;
    }

    //This class will capture events from the WebBrowser
    class WebBrowserExtendedEvents : StandardOleMarshalObject, IDWebBrowserEvents2
    {
      ExtendedWebBrowser _Browser;
      public WebBrowserExtendedEvents(ExtendedWebBrowser browser) { _Browser = browser; }

      //Implement whichever events you wish
      public void BeforeNavigate2(object pDisp, ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
      {
        _Browser.OnBeforeNavigate((string)URL, (string)targetFrameName, out cancel);
      }

      public void NewWindow3(ref object pDisp, ref bool cancel, ref object flags, ref object URLContext, ref object URL)
      {
        _Browser.OnBeforeNewWindow((string)URL, out cancel);
      }
    }

  }
}

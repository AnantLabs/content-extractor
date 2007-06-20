using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MetaTech.Library;

namespace ContentExtractor.Core
{
  public static class BrowserAsyncLoader
  {
    public static string GetDocumentCode(WebPosition position)
    {
      bool shouldRequest = false;
      lock (dataCache)
      {
        shouldRequest = !dataCache.ContainsKey(position.Persist);
        if (shouldRequest)
          dataCache[position.Persist] = string.Empty;
      }
      if (shouldRequest)
      {
        lock (workQueue)
        {
          workQueue.Enqueue(position.Persist);
          Monitor.Pulse(workQueue);
        }
      }
      lock (dataCache)
        return dataCache[position.Persist];
    }

    public static bool HasWork
    {
      get
      {
        lock (workQueue)
          return workQueue.Count > 0 || threadHasWork;
      }
    }

    public static void ClearCache()
    {
      lock(dataCache)
        dataCache.Clear();
    }

    
    static BrowserAsyncLoader()
    {
      workThread = new Thread(DoWork);
      workThread.IsBackground = true;
      workThread.SetApartmentState(ApartmentState.STA);
      workThread.Name = "Browser async loader";
      workThread.Priority = ThreadPriority.Lowest;
      workThread.Start();
    }
    private static Thread workThread;
    private static void DoWork()
    {
      while (true)
      {
        WebPosition.PersistStruct? task = null;
        lock (workQueue)
        {
          threadHasWork = workQueue.Count > 0;
          if (threadHasWork)
            task = workQueue.Dequeue();
          else
            Monitor.Wait(workQueue);
        }
        if (task.HasValue)
          if (!DoTask(task.Value))
          {
            Console.WriteLine("'{0}' loading failed", task.Value.Url);
            lock (workQueue)
              workQueue.Enqueue(task.Value);
          }
      }
    }
    private static volatile bool threadHasWork = false;

    private static bool DoTask(WebPosition.PersistStruct persistStruct)
    {
      using (ExtendedWebBrowser browser = new ExtendedWebBrowser())
      {
        try
        {
          browser.Navigate(persistStruct.Url);
          for (int i = 0; i < 30; i++)
            System.Windows.Forms.Application.DoEvents();
          do
          {
            System.Windows.Forms.Application.DoEvents();
          } while (browser.IsBusy);
        }
        catch (Exception exc)
        {
          TraceHlp2.WriteException(exc);
          return false;
        }
        if (browser.Document != null && browser.Document.Body != null && browser.Document.Body.Parent != null)
        {
          lock (dataCache)
          {
            dataCache[persistStruct] = browser.Document.Body.Parent.OuterHtml;
            return true;
          }
        }
        else
          return false;
      }
    }

    private static Dictionary<WebPosition.PersistStruct, string> dataCache = new Dictionary<WebPosition.PersistStruct, string>();
    private static Queue<WebPosition.PersistStruct> workQueue = new Queue<WebPosition.PersistStruct>();

  }
}

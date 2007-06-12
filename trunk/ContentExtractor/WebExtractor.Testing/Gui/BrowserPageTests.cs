using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ContentExtractor.Gui;
using ContentExtractor.Core;
using System.Windows.Forms;
using System.IO;
using MetaTech.Library;

namespace WebExtractor_Testing.Gui
{
  [TestFixture]
  public class BrowserPageTests
  {
    BrowserPage form;
    Model Model;

    [SetUp]
    public void SetUp()
    {
      this.Model = new Model();
      Model.Mode = Model.WorkMode.Browse;
      this.form = new BrowserPage(Model.CurrentPositions[0]);
      this.form.InitModel(delegate { return Model; });
      WaitBrowser();
    }
    [TearDown]
    public void TearDown()
    {
      if (_fileName != null)
      {
        File.Delete(_fileName);
        _fileName = null;
      }
    }

    private void WaitBrowser()
    {
      do
      {
        form.ForceSynchronize();
        Application.DoEvents();
      } while (form.IsBusy);
    }

    private string _fileName;
    private string FileName
    {
      get
      {
        if (_fileName == null)
        {
          _fileName = ApplicationHlp.MapPath("file.html");
          File.WriteAllText(_fileName, "<html><head><title>Title</title></head><body> <p>Here is some text to display</p></body></html>");
        }
        return _fileName;
      }
    }

    [Test]
    public void ChangeTextInUrlTextBox()
    {
      Model.Mode = Model.WorkMode.Browse;
      form.urlComboBox.Text = "asdf";
      WaitBrowser();
      Assert.AreEqual("asdf", form.urlComboBox.Text);
    }

    [Test]
    public void ChangingPositionNavigatesBrowser()
    {
      WebPosition position = WebPosition.Parse(FileName);
      Model.ActivePosition = position;
      WaitBrowser();
      Assert.AreEqual(position.Url.AbsoluteUri, form.Browser.Url.AbsoluteUri);
    }

    [Test]
    public void UrlTextBoxChangesAfterChangingPositon()
    {
      WebPosition position = WebPosition.Parse(FileName);
      Model.ActivePosition = position;
      WaitBrowser();
      Assert.AreEqual(form.urlComboBox.Text, form.Browser.Url.AbsoluteUri);
    }

    [Test]
    public void UrlTextBoxChangesAfterBrowserNavigation()
    {
      form.Browser.Navigate(FileName);
      WaitBrowser();
      WaitBrowser();
      Assert.AreEqual(form.urlComboBox.Text, form.Browser.Url.AbsoluteUri);
    }
    [Test]
    public void AtivePositionChangesAfterBrowserNavigation()
    {
      form.Browser.Navigate(FileName);
      WaitBrowser();
      WaitBrowser();
      Assert.AreEqual(Model.ActivePosition.Url.AbsoluteUri, form.Browser.Url.AbsoluteUri);
    }
    [Test]
    public void BrowserNavigatesAfterChangingActivePositonInParseMode()
    {
      Model.Mode = Model.WorkMode.Parse;
      Model.PositionsList.Add(WebPosition.Parse(FileName));
      WaitBrowser();
      //Hack используется непосредственно присвоение, т.к. в GUI страница будет пересоздана сызнова
      form.Position.Set(Model.PositionsList[0]);
      WaitBrowser();
      Assert.AreEqual(Model.ActivePosition.Url.AbsoluteUri, form.Browser.Url.AbsoluteUri);
    }
  }
}

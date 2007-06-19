using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ContentExtractor.Core;
using System.Xml;
using MetaTech.Library;

namespace ContentExtractor.Gui
{
  public partial class SelectedNodeTextBox : UserControl, IView
  {
    public SelectedNodeTextBox()
    {
      InitializeComponent();
    }

    public Converter<XmlNode, string> TextTaker = null;

    public void SetSelectedNode(string xpath)
    {
      XmlNode node = XmlHlp.SelectSingleNode(GetModel().ActivePosition.XmlDocument, xpath);
      if (node != null && TextTaker != null)
        richTextBox1.Text = TextTaker(node);
      else
        richTextBox1.Text = string.Empty;
    }

    Getter<Model> GetModel = delegate { return new Model(); };

    #region IView Members

    public void InitModel(Getter<Model> modelGetter)
    {
      this.GetModel = modelGetter;
    }

    string cachedSelected = string.Empty;
    WebPosition cachedPosition = WebPosition.EmptyPosition;

    public void ForceSynchronize()
    {
      this.Enabled = GetModel().Mode == Model.WorkMode.Parse;
      if (this.Enabled)
      {
        if (cachedPosition != GetModel().ActivePosition || cachedSelected != GetModel().GetSelectedNode(cachedPosition))
        {
          cachedPosition = GetModel().ActivePosition;
          cachedSelected = GetModel().GetSelectedNode(cachedPosition);
          SetSelectedNode(cachedSelected);
        }
        //cachedSelectedNode.Synchronize(GetModel().SelectedNodes[GetModel().ActivePosition]);
      }
      else
        this.richTextBox1.Text = string.Empty;
    }

    #endregion
  }
}

///*
//* 
//* An XmlReader implementation for loading SGML (including HTML) converting it
//* to well formed XML, by adding missing quotes, empty attribute values, ignoring
//* duplicate attributes, case folding on tag names, adding missing closing tags
//* based on SGML DTD information, and so on.
//*
//* Copyright (c) 2002 Microsoft Corporation. All rights reserved.
//*
//* Chris Lovett
//* 
//*/

using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Net;

namespace Sgml
{

  /// <summary>
  /// SGML is case insensitive, so here you can choose between converting
  /// to lower case or upper case tags.  "None" means that the case is left
  /// alone, except that end tags will be folded to match the start tags.
  /// </summary>
  enum CaseFolding
  {
    None,
    ToUpper,
    ToLower
  }

  /// <summary>
  /// This stack maintains a high water mark for allocated objects so the client
  /// can reuse the objects in the stack to reduce memory allocations, this is
  /// used to maintain current state of the parser for element stack, and attributes
  /// in each element.
  /// </summary>
  internal class HWStack
  {
    object[] items;
    int size;
    int count;
    int growth;

    public HWStack(int growth)
    {
      this.growth = growth;
    }

    public int Count
    {
      get { return this.count; }
      set { this.count = value; }
    }
    public int Size
    {
      get { return this.size; }
    }
    // returns the item at the requested index or null if index is out of bounds
    public object this[int i]
    {
      get { return (i >= 0 && i < this.size) ? items[i] : null; }
      set { this.items[i] = value; }
    }
    public object Pop()
    {
      this.count--;
      if (this.count > 0)
      {
        return items[this.count - 1];
      }
      return null;
    }
    // This method tries to reuse a slot, if it returns null then
    // the user has to call the other Push method.
    public object Push()
    {
      if (this.count == this.size)
      {
        int newsize = this.size + this.growth;
        object[] newarray = new object[newsize];
        if (this.items != null)
          Array.Copy(this.items, newarray, this.size);
        this.size = newsize;
        this.items = newarray;
      }
      return items[this.count++];
    }
    public void RemoveAt(int i)
    {
      this.items[i] = null;
      Array.Copy(this.items, i + 1, this.items, i, this.count - i - 1);
      this.count--;

    }
  }

  /// <summary>
  /// This class represents an attribute.  The AttDef is assigned
  /// from a validation process, and is used to provide default values.
  /// </summary>
  internal class Attribute
  {
    internal string Name;    // the atomized name (using XmlNameTable).
    internal AttDef DtdType; // the AttDef of the attribute from the SGML DTD.
    internal char QuoteChar; // the quote character used for the attribute value.
    internal string literalValue; // tha attribute value

    /// <summary>
    /// Attribute objects are reused during parsing to reduce memory allocations,
    /// hence the Reset method.
    /// </summary>
    public void Reset(string name, string value, char quote)
    {
      this.Name = name;
      this.literalValue = value;
      this.QuoteChar = quote;
      this.DtdType = null;
    }

    public string Value
    {
      get
      {
        if (this.literalValue != null)
          return this.literalValue;
        if (this.DtdType != null)
          return this.DtdType.Default;
        return null;
      }
      set
      {
        this.literalValue = value;
      }
    }

    public bool IsDefault
    {
      get
      {
        return (this.literalValue == null);
      }
    }
  }

  /// <summary>
  /// This class models an XML node, an array of elements in scope is maintained while parsing
  /// for validation purposes, and these Node objects are reused to reduce object allocation,
  /// hence the reset method.
  /// </summary>
  internal class Node
  {
    internal XmlNodeType NodeType;
    internal string Value;
    internal XmlSpace Space;
    internal string XmlLang;
    internal bool IsEmpty;
    internal string Name;
    internal ElementDecl DtdType; // the DTD type found via validation
    internal State CurrentState;
    internal bool Simulated; // tag was injected into result stream.
    HWStack attributes = new HWStack(10);

    /// <summary>
    /// Attribute objects are reused during parsing to reduce memory allocations,
    /// hence the Reset method.
    /// </summary>
    public void Reset(string name, XmlNodeType nt, string value)
    {
      this.Value = value;
      this.Name = name;
      this.NodeType = nt;
      this.Space = XmlSpace.None;
      this.XmlLang = null;
      this.IsEmpty = true;
      this.attributes.Count = 0;
      this.DtdType = null;
    }

    public Attribute AddAttribute(string name, string value, char quotechar, bool caseInsensitive)
    {
      Attribute a;
      // check for duplicates!
      for (int i = 0, n = this.attributes.Count; i < n; i++)
      {
        a = (Attribute)this.attributes[i];
        if (caseInsensitive && string.Compare(a.Name, name, true) == 0)
        {
          return null;
        }
        else if ((object)a.Name == (object)name)
        {
          return null;
        }
      }
      // This code makes use of the high water mark for attribute objects,
      // and reuses exisint Attribute objects to avoid memory allocation.
      a = (Attribute)this.attributes.Push();
      if (a == null)
      {
        a = new Attribute();
        this.attributes[this.attributes.Count - 1] = a;
      }
      a.Reset(name, value, quotechar);
      return a;
    }

    public void RemoveAttribute(string name)
    {
      for (int i = 0, n = this.attributes.Count; i < n; i++)
      {
        Attribute a = (Attribute)this.attributes[i];
        if (a.Name == name)
        {
          this.attributes.RemoveAt(i);
          return;
        }
      }
    }
    public void CopyAttributes(Node n)
    {
      for (int i = 0, len = n.attributes.Count; i < len; i++)
      {
        Attribute a = (Attribute)n.attributes[i];
        Attribute na = this.AddAttribute(a.Name, a.Value, a.QuoteChar, false);
        na.DtdType = a.DtdType;
      }
    }

    public int AttributeCount
    {
      get
      {
        return this.attributes.Count;
      }
    }

    public int GetAttribute(string name)
    {
      for (int i = 0, n = this.attributes.Count; i < n; i++)
      {
        Attribute a = (Attribute)this.attributes[i];
        if (a.Name == name)
        {
          return i;
        }
      }
      return -1;
    }

    public Attribute GetAttribute(int i)
    {
      if (i >= 0 && i < this.attributes.Count)
      {
        Attribute a = (Attribute)this.attributes[i];
        return a;
      }
      return null;
    }
  }

  // This enum is used to track the current state of te SgmlReader
  internal enum State
  {
    Initial,    // The initial state (Read has not been called yet)
    Markup,     // Expecting text or markup
    EndTag,     // Positioned on an end tag
    Attr,       // Positioned on an attribute
    AttrValue,  // Positioned in an attribute value
    Text,       // Positioned on a Text node.
    PartialTag, // Positioned on a text node, and we have hit a start tag
    AutoClose,  // We are auto-closing tags (this is like State.EndTag), but end tag was generated
    CData,      // We are on a CDATA type node, eg. <scipt> where we have special parsing rules.
    PartialText,
    PseudoStartTag, // we pushed a pseudo-start tag, need to continue with previous start tag.
    Eof
  }


  /// <summary>
  /// SgmlReader is an XmlReader API over any SGML document (including built in
  /// support for HTML).
  /// </summary>
  internal class SgmlReader : XmlReader
  {
    SgmlDtd dtd;
    Entity current;
    State state;
    XmlNameTable nametable;
    char partial;
    object endTag;
    HWStack stack;
    Node node; // current node (except for attributes)
    // Attributes are handled separately using these members.
    Attribute a;
    int apos; // which attribute are we positioned on in the collection.
    Uri baseUri;
    StringBuilder sb;
    StringBuilder name;
    TextWriter log;
    bool foundRoot;

    // autoclose support
    Node newnode;
    int poptodepth;
    int rootCount;
    bool isHtml;
    string rootElementName;

    string href;
    string errorLogFile;
    Entity lastError;
    string proxy;
    TextReader inputStream;
    string syslit;
    string pubid;
    string subset;
    string docType;
    WhitespaceHandling whitespaceHandling;
    CaseFolding folding = CaseFolding.None;
    bool stripDocType = true;
    string startTag;

    public SgmlReader()
    {
      Init();
      this.nametable = new NameTable();
    }

    /// <summary>
    /// Specify the SgmlDtd object directly.  This allows you to cache the Dtd and share
    /// it across multipl SgmlReaders.  To load a DTD from a URL use the SystemLiteral property.
    /// </summary>
    public SgmlDtd Dtd
    {
      get
      {
        LazyLoadDtd(this.baseUri);
        return this.dtd;
      }
      set { this.dtd = value; }
    }

    private void LazyLoadDtd(Uri baseUri)
    {
      if (this.dtd == null)
      {
        if (this.syslit == null || this.syslit == "")
        {
          if (this.docType != null && StringUtilities.EqualsIgnoreCase(this.docType, "html"))
          {
            Assembly a = typeof(SgmlReader).Assembly;
            string name = a.FullName.Split(',')[0] + ".Html.dtd";
            Stream stm = a.GetManifestResourceStream(name);
            if (stm != null)
            {
              StreamReader sr = new StreamReader(stm);
              this.dtd = SgmlDtd.Parse(baseUri, "HTML", null, sr, null, this.proxy, this.nametable);
            }
          }
        }
        else
        {
          if (baseUri != null)
          {
            baseUri = new Uri(baseUri, this.syslit);
          }
          else if (this.baseUri != null)
          {
            baseUri = new Uri(this.baseUri, this.syslit);
          }
          else
          {
            baseUri = new Uri(new Uri(Directory.GetCurrentDirectory() + "\\"), this.syslit);
          }
          this.dtd = SgmlDtd.Parse(baseUri, this.docType, this.pubid, baseUri.AbsoluteUri, this.subset, this.proxy, this.nametable);
        }

        if (this.dtd != null && this.dtd.Name != null)
        {
          switch (this.CaseFolding)
          {
            case CaseFolding.ToUpper:
              this.rootElementName = this.dtd.Name.ToUpper();
              break;
            case CaseFolding.ToLower:
              this.rootElementName = this.dtd.Name.ToLower();
              break;
            default:
              this.rootElementName = this.dtd.Name;
              break;
          }
          this.isHtml = StringUtilities.EqualsIgnoreCase(this.dtd.Name, "html");
        }

      }
    }

    /// <summary>
    /// The name of root element specified in the DOCTYPE tag.
    /// </summary>
    public string DocType
    {
      get { return this.docType; }
      set { this.docType = value; }
    }

    /// <summary>
    /// The PUBLIC identifier in the DOCTYPE tag
    /// </summary>
    public string PublicIdentifier
    {
      get { return this.pubid; }
      set { this.pubid = value; }
    }

    /// <summary>
    /// The SYSTEM literal in the DOCTYPE tag identifying the location of the DTD.
    /// </summary>
    public string SystemLiteral
    {
      get { return this.syslit; }
      set { this.syslit = value; }
    }

    /// <summary>
    /// The DTD internal subset in the DOCTYPE tag
    /// </summary>
    public string InternalSubset
    {
      get { return this.subset; }
      set { this.subset = value; }
    }

    /// <summary>
    /// The input stream containing SGML data to parse.
    /// You must specify this property or the Href property before calling Read().
    /// </summary>
    public TextReader InputStream
    {
      get { return this.inputStream; }
      set { this.inputStream = value; Init(); }
    }

    /// <summary>
    /// Sometimes you need to specify a proxy server in order to load data via HTTP
    /// from outside the firewall.  For example: "itgproxy:80".
    /// </summary>
    public string WebProxy
    {
      get { return this.proxy; }
      set { this.proxy = value; }
    }

    /// <summary>
    /// The base Uri is used to resolve relative Uri's like the SystemLiteral and
    /// Href properties.  This is a method because BaseURI is a read-only
    /// property on the base XmlReader class.
    /// </summary>
    public void SetBaseUri(string uri)
    {
      this.baseUri = new Uri(uri);
    }

    /// <summary>
    /// Specify the location of the input SGML document as a URL.
    /// </summary>
    public string Href
    {
      get { return this.href; }
      set
      {
        this.href = value;
        Init();
        if (this.baseUri == null)
        {
          if (this.href.IndexOf("://") > 0)
          {
            this.baseUri = new Uri(this.href);
          }
          else
          {
            this.baseUri = new Uri("file:///" + Directory.GetCurrentDirectory() + "//");
          }
        }
      }
    }

    /// <summary>
    /// Whether to strip out the DOCTYPE tag from the output (default true)
    /// </summary>
    public bool StripDocType
    {
      get { return this.stripDocType; }
      set { this.stripDocType = value; }
    }

    public CaseFolding CaseFolding
    {
      get { return this.folding; }
      set { this.folding = value; }
    }

    /// <summary>
    /// DTD validation errors are written to this stream.
    /// </summary>
    public TextWriter ErrorLog
    {
      get { return this.log; }
      set { this.log = value; }
    }

    /// <summary>
    /// DTD validation errors are written to this log file.
    /// </summary>
    public string ErrorLogFile
    {
      get { return this.errorLogFile; }
      set
      {
        this.errorLogFile = value;
        this.ErrorLog = new StreamWriter(value);
      }
    }

    void Log(string msg, params string[] args)
    {
      if (ErrorLog != null)
      {
        string err = String.Format(msg, args);
        if (this.lastError != this.current)
        {
          err = err + "    " + this.current.Context();
          this.lastError = this.current;
          ErrorLog.WriteLine("### Error:" + err);
        }
        else
        {
          string path = "";
          if (this.current.ResolvedUri != null)
          {
            path = this.current.ResolvedUri.AbsolutePath;
          }
          ErrorLog.WriteLine("### Error in " +
              path + "#" +
              this.current.Name +
              ", line " + this.current.Line + ", position " + this.current.LinePosition + ": " +
              err);
        }
      }
    }
    void Log(string msg, char ch)
    {
      Log(msg, ch.ToString());
    }


    void Init()
    {
      this.state = State.Initial;
      this.stack = new HWStack(10);
      this.node = Push(null, XmlNodeType.Document, null);
      this.node.IsEmpty = false;
      this.sb = new StringBuilder();
      this.name = new StringBuilder();
      this.poptodepth = 0;
      this.current = null;
      this.partial = '\0';
      this.endTag = null;
      this.a = null;
      this.apos = 0;
      this.newnode = null;
      this.rootCount = 0;
      this.foundRoot = false;
    }

    Node Push(string name, XmlNodeType nt, string value)
    {
      Node result = (Node)this.stack.Push();
      if (result == null)
      {
        result = new Node();
        this.stack[this.stack.Count - 1] = result;
      }
      result.Reset(name, nt, value);
      this.node = result;
      return result;
    }

    void SwapTopNodes()
    {
      int top = this.stack.Count - 1;
      if (top > 0)
      {
        Node n = (Node)this.stack[top - 1];
        this.stack[top - 1] = this.stack[top];
        this.stack[top] = n;
      }
    }

    Node Push(Node n)
    {
      // we have to do a deep clone of the Node object because
      // it is reused in the stack.
      Node n2 = Push(n.Name, n.NodeType, n.Value);
      n2.DtdType = n.DtdType;
      n2.IsEmpty = n.IsEmpty;
      n2.Space = n.Space;
      n2.XmlLang = n.XmlLang;
      n2.CurrentState = n.CurrentState;
      n2.CopyAttributes(n);
      this.node = n2;
      return n2;
    }

    void Pop()
    {
      if (this.stack.Count > 1)
      {
        this.node = (Node)this.stack.Pop();
      }
    }

    Node Top()
    {
      int top = this.stack.Count - 1;
      if (top > 0)
      {
        return (Node)this.stack[top];
      }
      return null;
    }

    public override XmlNodeType NodeType
    {
      get
      {
        if (this.state == State.Attr)
        {
          return XmlNodeType.Attribute;
        }
        else if (this.state == State.AttrValue)
        {
          return XmlNodeType.Text;
        }
        else if (this.state == State.EndTag || this.state == State.AutoClose)
        {
          return XmlNodeType.EndElement;
        }
        return this.node.NodeType;
      }
    }

    public override string Name
    {
      get
      {
        return this.LocalName;
      }
    }

    public override string LocalName
    {
      get
      {
        string result = null;
        if (this.state == State.Attr)
        {
          result = this.a.Name;
        }
        else if (this.state == State.AttrValue)
        {
          result = null;
        }
        else
        {
          result = this.node.Name;
        }

        return result;
      }
    }

    public override string NamespaceURI
    {
      get
      {
        // SGML has no namespaces, unless this turned out to be an xmlns attribute.
        if (this.state == State.Attr && StringUtilities.EqualsIgnoreCase(this.a.Name, "xmlns"))
        {
          return "http://www.w3.org/2000/xmlns/";
        }
        return String.Empty;
      }
    }

    public override string Prefix
    {
      get
      {
        // SGML has no namespaces.
        return String.Empty;
      }
    }

    public override bool HasValue
    {
      get
      {
        if (this.state == State.Attr || this.state == State.AttrValue)
        {
          return true;
        }
        return (this.node.Value != null);
      }
    }

    public override string Value
    {
      get
      {
        if (this.state == State.Attr || this.state == State.AttrValue)
        {
          return this.a.Value;
        }
        return this.node.Value;
      }
    }

    public override int Depth
    {
      get
      {
        if (this.state == State.Attr)
        {
          return this.stack.Count;
        }
        else if (this.state == State.AttrValue)
        {
          return this.stack.Count + 1;
        }
        return this.stack.Count - 1;
      }
    }

    public override string BaseURI
    {
      get
      {
        return this.baseUri == null ? "" : this.baseUri.AbsoluteUri;
      }
    }

    public override bool IsEmptyElement
    {
      get
      {
        if (this.state == State.Markup || this.state == State.Attr || this.state == State.AttrValue)
        {
          return this.node.IsEmpty;
        }
        return false;
      }
    }
    public override bool IsDefault
    {
      get
      {
        if (this.state == State.Attr || this.state == State.AttrValue)
          return this.a.IsDefault;
        return false;
      }
    }
    public override char QuoteChar
    {
      get
      {
        if (this.a != null) return this.a.QuoteChar;
        return '\0';
      }
    }

    public override XmlSpace XmlSpace
    {
      get
      {
        for (int i = this.stack.Count - 1; i > 1; i--)
        {
          Node n = (Node)this.stack[i];
          XmlSpace xs = n.Space;
          if (xs != XmlSpace.None) return xs;
        }
        return XmlSpace.None;
      }
    }

    public override string XmlLang
    {
      get
      {
        for (int i = this.stack.Count - 1; i > 1; i--)
        {
          Node n = (Node)this.stack[i];
          string xmllang = n.XmlLang;
          if (xmllang != null) return xmllang;
        }
        return String.Empty;
      }
    }

    public WhitespaceHandling WhitespaceHandling
    {
      get
      {
        return this.whitespaceHandling;
      }
      set
      {
        this.whitespaceHandling = value;
      }
    }

    public override int AttributeCount
    {
      get
      {
        if (this.state == State.Attr || this.state == State.AttrValue)
          return 0;
        if (this.node.NodeType == XmlNodeType.Element ||
            this.node.NodeType == XmlNodeType.DocumentType)
          return this.node.AttributeCount;
        return 0;
      }
    }

    public override string GetAttribute(string name)
    {
      if (this.state != State.Attr && this.state != State.AttrValue)
      {
        int i = this.node.GetAttribute(name);
        if (i >= 0) return GetAttribute(i);
      }
      return null;
    }

    public override string GetAttribute(string name, string namespaceURI)
    {
      return GetAttribute(name); // SGML has no namespaces.
    }

    public override string GetAttribute(int i)
    {
      if (this.state != State.Attr && this.state != State.AttrValue)
      {
        Attribute a = this.node.GetAttribute(i);
        if (a != null)
          return a.Value;
      }
      throw new IndexOutOfRangeException();
    }

    public override string this[int i]
    {
      get
      {
        return GetAttribute(i);
      }
    }

    public override string this[string name]
    {
      get
      {
        return GetAttribute(name);
      }
    }

    public override string this[string name, string namespaceURI]
    {
      get
      {
        return GetAttribute(name, namespaceURI);
      }
    }

    public override bool MoveToAttribute(string name)
    {
      int i = this.node.GetAttribute(name);
      if (i >= 0)
      {
        MoveToAttribute(i);
        return true;
      }
      return false;
    }

    public override bool MoveToAttribute(string name, string ns)
    {
      return MoveToAttribute(name);
    }

    public override void MoveToAttribute(int i)
    {
      Attribute a = this.node.GetAttribute(i);
      if (a != null)
      {
        this.apos = i;
        this.a = a;
        if (this.state != State.Attr)
        {
          this.node.CurrentState = this.state;//save current state.
        }
        this.state = State.Attr;
        return;
      }
      throw new IndexOutOfRangeException();
    }

    public override bool MoveToFirstAttribute()
    {
      if (this.node.AttributeCount > 0)
      {
        MoveToAttribute(0);
        return true;
      }
      return false;
    }

    public override bool MoveToNextAttribute()
    {
      if (this.state != State.Attr && this.state != State.AttrValue)
      {
        return MoveToFirstAttribute();
      }
      if (this.apos < this.node.AttributeCount - 1)
      {
        MoveToAttribute(this.apos + 1);
        return true;
      }
      return false;
    }

    public override bool MoveToElement()
    {
      if (this.state == State.Attr || this.state == State.AttrValue)
      {
        this.state = this.node.CurrentState;
        this.a = null;
        return true;
      }
      return (this.node.NodeType == XmlNodeType.Element);
    }

    bool IsHtml
    {
      get
      {
        return this.isHtml;
      }
    }

    public Encoding GetEncoding()
    {
      if (this.current == null)
      {
        OpenInput();
      }
      return this.current.GetEncoding();
    }

    void OpenInput()
    {
      LazyLoadDtd(this.baseUri);

      if (this.Href != null)
      {
        this.current = new Entity("#document", null, this.href, this.proxy);
      }
      else if (this.inputStream != null)
      {
        this.current = new Entity("#document", null, this.inputStream, this.proxy);
      }
      else
      {
        throw new InvalidOperationException("You must specify input either via Href or InputStream properties");
      }
      this.current.Html = this.IsHtml;
      this.current.Open(null, this.baseUri);
      if (this.current.ResolvedUri != null)
        this.baseUri = this.current.ResolvedUri;

      if (this.current.Html && this.dtd == null)
      {
        this.docType = "HTML";
        LazyLoadDtd(this.baseUri);
      }
    }

    public override bool Read()
    {
      if (current == null)
      {
        OpenInput();
      }
      State start = this.state;
      if (node.Simulated)
      {
        // return the next node
        node.Simulated = false;
        this.node = Top();
        this.state = this.node.CurrentState;
        return true;
      }

      bool foundnode = false;
      while (!foundnode)
      {
        switch (this.state)
        {
          case State.Initial:
            this.state = State.Markup;
            this.current.ReadChar();
            goto case State.Markup;
          case State.Eof:
            if (this.current.Parent != null)
            {
              this.current.Close();
              this.current = this.current.Parent;
            }
            else
            {
              return false;
            }
            break;
          case State.EndTag:
            if (this.endTag == (object)this.node.Name)
            {
              Pop(); // we're done!
              this.state = State.Markup;
              goto case State.Markup;
            }
            Pop(); // close one element
            foundnode = true;// return another end element.
            break;
          case State.Markup:
            if (this.node.IsEmpty)
            {
              Pop();
            }
            Node n = this.node;
            foundnode = ParseMarkup();
            break;
          case State.PartialTag:
            Pop(); // remove text node.
            this.state = State.Markup;
            foundnode = ParseTag(this.partial);
            break;
          case State.PseudoStartTag:
            foundnode = ParseStartTag('<');
            break;
          case State.AutoClose:
            Pop(); // close next node.
            if (this.stack.Count <= this.poptodepth)
            {
              this.state = State.Markup;
              if (this.newnode != null)
              {
                Push(this.newnode); // now we're ready to start the new node.
                this.newnode = null;
                this.state = State.Markup;
              }
              else if (this.node.NodeType == XmlNodeType.Document)
              {
                this.state = State.Eof;
                goto case State.Eof;
              }
            }
            foundnode = true;
            break;
          case State.CData:
            foundnode = ParseCData();
            break;
          case State.Attr:
            goto case State.AttrValue;
          case State.AttrValue:
            this.state = State.Markup;
            goto case State.Markup;
          case State.Text:
            Pop();
            goto case State.Markup;
          case State.PartialText:
            if (ParseText(this.current.Lastchar, false))
            {
              this.node.NodeType = XmlNodeType.Whitespace;
            }
            foundnode = true;
            break;
        }
        if (foundnode && this.node.NodeType == XmlNodeType.Whitespace && this.whitespaceHandling == WhitespaceHandling.None)
        {
          // strip out whitespace (caller is probably pretty printing the XML).
          foundnode = false;
        }
        if (!foundnode && this.state == State.Eof && this.stack.Count > 1)
        {
          this.poptodepth = 1;
          state = State.AutoClose;
          this.node = Top();
          return true;
        }
      }
      if (!foundRoot && (this.NodeType == XmlNodeType.Element ||
              this.NodeType == XmlNodeType.Text ||
              this.NodeType == XmlNodeType.CDATA))
      {
        foundRoot = true;
        if (this.IsHtml && (this.NodeType != XmlNodeType.Element ||
            string.Compare(this.LocalName, "html", true, System.Globalization.CultureInfo.InvariantCulture) != 0))
        {
          // Simulate an HTML root element!
          this.node.CurrentState = this.state;
          Node root = Push("html", XmlNodeType.Element, null);
          SwapTopNodes(); // make html the outer element.
          this.node = root;
          root.Simulated = true;
          root.IsEmpty = false;
          this.state = State.Markup;
          //this.state = State.PseudoStartTag;
          //this.startTag = name;
        }
        return true;
      }
      return true;
    }

    bool ParseMarkup()
    {
      char ch = this.current.Lastchar;
      if (ch == '<')
      {
        ch = this.current.ReadChar();
        return ParseTag(ch);
      }
      else if (ch != Entity.EOF)
      {
        if (this.node.DtdType != null && this.node.DtdType.ContentModel.DeclaredContent == DeclaredContent.CDATA)
        {
          // e.g. SCRIPT or STYLE tags which contain unparsed character data.
          this.partial = '\0';
          this.state = State.CData;
          return false;
        }
        else if (ParseText(ch, true))
        {
          this.node.NodeType = XmlNodeType.Whitespace;
        }
        return true;
      }
      this.state = State.Eof;
      return false;
    }

    static string declterm = " \t\r\n><";
    bool ParseTag(char ch)
    {
      if (ch == '%')
      {
        return ParseAspNet();
      }
      if (ch == '!')
      {
        ch = this.current.ReadChar();
        if (ch == '-')
        {
          return ParseComment();
        }
        else if (ch == '[')
        {
          return ParseConditionalBlock();
        }
        else if (ch != '_' && !Char.IsLetter(ch))
        {
          // perhaps it's one of those nasty office document hacks like '<![if ! ie ]>'
          string value = this.current.ScanToEnd(this.sb, "Recovering", ">"); // skip it
          Log("Ignoring invalid markup '<!" + value + ">");
          return false;
        }
        else
        {
          string name = this.current.ScanToken(this.sb, SgmlReader.declterm, false);
          if (name == "DOCTYPE")
          {
            ParseDocType();
            // In SGML DOCTYPE SYSTEM attribute is optional, but in XML it is required,
            // therefore if there is no SYSTEM literal then add an empty one.
            if (this.GetAttribute("SYSTEM") == null && this.GetAttribute("PUBLIC") != null)
            {
              this.node.AddAttribute("SYSTEM", "", '"', this.folding == CaseFolding.None);
            }
            if (stripDocType)
            {
              return false;
            }
            else
            {
              this.node.NodeType = XmlNodeType.DocumentType;
              return true;
            }
          }
          else
          {
            Log("Invalid declaration '<!{0}...'.  Expecting '<!DOCTYPE' only.", name);
            this.current.ScanToEnd(null, "Recovering", ">"); // skip it
            return false;
          }
        }
      }
      else if (ch == '?')
      {
        this.current.ReadChar();// consume the '?' character.
        return ParsePI();
      }
      else if (ch == '/')
      {
        return ParseEndTag();
      }
      else
      {
        return ParseStartTag(ch);
      }
      return true;
    }

    string ScanName(string terminators)
    {
      string name = this.current.ScanToken(this.sb, terminators, false);
      switch (this.folding)
      {
        case CaseFolding.ToUpper:
          name = name.ToUpper();
          break;
        case CaseFolding.ToLower:
          name = name.ToLower();
          break;
      }
      return this.nametable.Add(name);
    }

    static string tagterm = " \t\r\n=/><";
    static string aterm = " \t\r\n='\"/>";
    static string avterm = " \t\r\n>";
    bool ParseStartTag(char ch)
    {
      string name = null;
      if (state != State.PseudoStartTag)
      {
        if (SgmlReader.tagterm.IndexOf(ch) >= 0)
        {
          this.sb.Length = 0;
          this.sb.Append('<');
          this.state = State.PartialText;
          return false;
        }
        name = ScanName(SgmlReader.tagterm);
      }
      else
      {
        name = this.startTag;
        state = State.Markup;
      }
      Node n = Push(name, XmlNodeType.Element, null);
      n.IsEmpty = false;
      Validate(n);
      ch = this.current.SkipWhitespace();
      while (ch != Entity.EOF && ch != '>')
      {
        if (ch == '/')
        {
          n.IsEmpty = true;
          ch = this.current.ReadChar();
          if (ch != '>')
          {
            Log("Expected empty start tag '/>' sequence instead of '{0}'", ch);
            this.current.ScanToEnd(null, "Recovering", ">");
            return false;
          }
          break;
        }
        else if (ch == '<')
        {
          Log("Start tag '{0}' is missing '>'", name);
          break;
        }
        string aname = ScanName(SgmlReader.aterm);
        ch = this.current.SkipWhitespace();
        if (aname == "," || aname == "=" || aname == ":" || aname == ";")
        {
          continue;
        }
        string value = null;
        char quote = '\0';
        if (ch == '=' || ch == '"' || ch == '\'')
        {
          if (ch == '=')
          {
            this.current.ReadChar();
            ch = this.current.SkipWhitespace();
          }
          if (ch == '\'' || ch == '\"')
          {
            quote = ch;
            value = ScanLiteral(this.sb, ch);
          }
          else if (ch != '>')
          {
            string term = SgmlReader.avterm;
            value = this.current.ScanToken(this.sb, term, false);
          }
        }
        if (aname.Length > 0)
        {
          Attribute a = n.AddAttribute(aname, value, quote, this.folding == CaseFolding.None);
          if (a == null)
          {
            Log("Duplicate attribute '{0}' ignored", aname);
          }
          else
          {
            ValidateAttribute(n, a);
          }
        }
        ch = this.current.SkipWhitespace();
      }
      if (ch == Entity.EOF)
      {
        this.current.Error("Unexpected EOF parsing start tag '{0}'", name);
      }
      else if (ch == '>')
      {
        this.current.ReadChar(); // consume '>'
      }
      if (this.Depth == 1)
      {
        if (this.rootCount == 1)
        {
          // Hmmm, we found another root level tag, soooo, the only
          // thing we can do to keep this a valid XML document is stop
          this.state = State.Eof;
          return false;
        }
        this.rootCount++;
      }
      ValidateContent(n);
      return true;
    }

    bool ParseEndTag()
    {
      this.state = State.EndTag;
      this.current.ReadChar(); // consume '/' char.
      string name = this.ScanName(SgmlReader.tagterm);
      char ch = this.current.SkipWhitespace();
      if (ch != '>')
      {
        Log("Expected empty start tag '/>' sequence instead of '{0}'", ch);
        this.current.ScanToEnd(null, "Recovering", ">");
      }
      this.current.ReadChar(); // consume '>'

      this.endTag = name;
      // Make sure there's a matching start tag for it.
      bool caseInsensitive = (this.folding == CaseFolding.None);
      this.node = (Node)this.stack[this.stack.Count - 1];
      for (int i = this.stack.Count - 1; i > 0; i--)
      {
        Node n = (Node)this.stack[i];
        if (caseInsensitive && string.Compare(n.Name, name, true) == 0)
        {
          this.endTag = n.Name;
          return true;
        }
        else if ((object)n.Name == (object)name)
        {
          return true;
        }
      }
      Log("No matching start tag for '</{0}>'", name);
      this.state = State.Markup;
      return false;
    }

    bool ParseAspNet()
    {
      string value = "<%" + this.current.ScanToEnd(this.sb, "AspNet", "%>") + "%>";
      Push(null, XmlNodeType.CDATA, value);
      return true;
    }

    bool ParseComment()
    {
      char ch = this.current.ReadChar();
      if (ch != '-')
      {
        Log("Expecting comment '<!--' but found {0}", ch);
        this.current.ScanToEnd(null, "Comment", ">");
        return false;
      }
      string value = this.current.ScanToEnd(this.sb, "Comment", "-->");

      // Make sure it's a valid comment!
      int i = value.IndexOf("--");
      while (i >= 0)
      {
        int j = i + 2;
        while (j < value.Length && value[j] == '-')
          j++;
        if (i > 0)
        {
          value = value.Substring(0, i - 1) + "-" + value.Substring(j);
        }
        else
        {
          value = "-" + value.Substring(j);
        }
        i = value.IndexOf("--");
      }
      if (value.Length > 0 && value[value.Length - 1] == '-')
      {
        value += " "; // '-' cannot be last character
      }
      Push(null, XmlNodeType.Comment, value);
      return true;
    }

    static string cdataterm = "\t\r\n[<>";
    bool ParseConditionalBlock()
    {
      char ch = current.ReadChar(); // skip '['
      ch = current.SkipWhitespace();
      string name = current.ScanToken(sb, cdataterm, false);
      if (name != "CDATA")
      {
        Log("Expecting CDATA but found '{0}'", name);
        current.ScanToEnd(null, "CDATA", ">");
        return false;
      }
      ch = current.SkipWhitespace();
      if (ch != '[')
      {
        Log("Expecting '[' but found '{0}'", ch);
        current.ScanToEnd(null, "CDATA", ">");
        return false;
      }
      string value = current.ScanToEnd(sb, "CDATA", "]]>");

      Push(null, XmlNodeType.CDATA, value);
      return true;
    }

    static string dtterm = " \t\r\n>";
    void ParseDocType()
    {
      char ch = this.current.SkipWhitespace();
      string name = this.ScanName(SgmlReader.dtterm);
      Push(name, XmlNodeType.DocumentType, null);
      ch = this.current.SkipWhitespace();
      if (ch != '>')
      {
        string subset = "";
        string pubid = "";
        string syslit = "";

        if (ch != '[')
        {
          string token = this.current.ScanToken(this.sb, SgmlReader.dtterm, false);
          if (token == "PUBLIC")
          {
            ch = this.current.SkipWhitespace();
            if (ch == '\"' || ch == '\'')
            {
              pubid = this.current.ScanLiteral(this.sb, ch);
              this.node.AddAttribute(token, pubid, ch, this.folding == CaseFolding.None);
            }
          }
          else if (token != "SYSTEM")
          {
            Log("Unexpected token in DOCTYPE '{0}'", token);
            this.current.ScanToEnd(null, "DOCTYPE", ">");
          }
          ch = this.current.SkipWhitespace();
          if (ch == '\"' || ch == '\'')
          {
            token = this.nametable.Add("SYSTEM");
            syslit = this.current.ScanLiteral(this.sb, ch);
            this.node.AddAttribute(token, syslit, ch, this.folding == CaseFolding.None);
          }
          ch = this.current.SkipWhitespace();
        }
        if (ch == '[')
        {
          subset = this.current.ScanToEnd(this.sb, "Internal Subset", "]");
          this.node.Value = subset;
        }
        ch = this.current.SkipWhitespace();
        if (ch != '>')
        {
          Log("Expecting end of DOCTYPE tag, but found '{0}'", ch);
          this.current.ScanToEnd(null, "DOCTYPE", ">");
        }

        if (this.dtd == null)
        {
          this.docType = name;
          this.pubid = pubid;
          this.syslit = syslit;
          this.subset = subset;
          LazyLoadDtd(this.current.ResolvedUri);
        }
      }
      this.current.ReadChar();
    }

    static string piterm = " \t\r\n?";
    bool ParsePI()
    {
      string name = this.current.ScanToken(this.sb, SgmlReader.piterm, false);
      string value = null;
      if (this.current.Lastchar != '?')
      {
        // Notice this is not "?>".  This is because Office generates bogus PI's that end with "/>".
        value = this.current.ScanToEnd(this.sb, "Processing Instruction", ">");
      }
      else
      {
        // error recovery.
        value = this.current.ScanToEnd(this.sb, "Processing Instruction", ">");
      }
      // skip xml declarations, since these are generated in the output instead.
      if (name != "xml")
      {
        Push(nametable.Add(name), XmlNodeType.ProcessingInstruction, value);
        return true;
      }
      return false;
    }

    bool ParseText(char ch, bool newtext)
    {
      bool ws = !newtext || this.current.IsWhitespace;
      if (newtext) this.sb.Length = 0;
      //this.sb.Append(ch);
      //ch = this.current.ReadChar();
      this.state = State.Text;
      while (ch != Entity.EOF)
      {
        if (ch == '<')
        {
          ch = this.current.ReadChar();
          if (ch == '/' || ch == '!' || ch == '?' || Char.IsLetter(ch))
          {
            // Hit a tag, so return XmlNodeType.Text token
            // and remember we partially started a new tag.
            this.state = State.PartialTag;
            this.partial = ch;
            break;
          }
          else
          {
            // not a tag, so just proceed.
            this.sb.Append('<');
            this.sb.Append(ch);
            ws = false;
            ch = this.current.ReadChar();
          }
        }
        else if (ch == '&')
        {
          ExpandEntity(this.sb, '<');
          ws = false;
          ch = this.current.Lastchar;
        }
        else
        {
          if (!this.current.IsWhitespace) ws = false;
          this.sb.Append(ch);
          ch = this.current.ReadChar();
        }
      }
      string value = this.sb.ToString();
      Push(null, XmlNodeType.Text, value);
      return ws;
    }

    // This version is slightly different from Entity.ScanLiteral in that
    // it also expands entities.
    public string ScanLiteral(StringBuilder sb, char quote)
    {
      sb.Length = 0;
      char ch = this.current.ReadChar();
      while (ch != Entity.EOF && ch != quote)
      {
        if (ch == '&')
        {
          ExpandEntity(this.sb, quote);
          ch = this.current.Lastchar;
        }
        else
        {
          sb.Append(ch);
          ch = this.current.ReadChar();
        }
      }
      this.current.ReadChar(); // consume end quote.
      return sb.ToString();
    }

    bool ParseCData()
    {
      // Like ParseText(), only it doesn't allow elements in the content.
      // It allows comments and processing instructions and text only and
      // text is not returned as text but CDATA (since it may contain angle brackets).
      // And initial whitespace is ignored.  It terminates when we hit the
      // end tag for the current CDATA node (e.g. </style>).
      bool ws = this.current.IsWhitespace;
      this.sb.Length = 0;
      char ch = this.current.Lastchar;
      if (this.partial != '\0')
      {
        Pop(); // pop the CDATA
        switch (this.partial)
        {
          case '!':
            this.partial = ' '; // and pop the comment next time around
            return ParseComment();
          case '?':
            this.partial = ' '; // and pop the PI next time around
            return ParsePI();
          case '/':
            this.state = State.EndTag;
            return true;    // we are done!
          case ' ':
            break; // means we just needed to pop the Comment, PI or CDATA.
        }
      }
      else
      {
        ch = this.current.ReadChar();
      }

      // if this.partial == '!' then parse the comment and return
      // if this.partial == '?' then parse the processing instruction and return.
      while (ch != Entity.EOF)
      {
        if (ch == '<')
        {
          ch = this.current.ReadChar();
          if (ch == '!')
          {
            ch = this.current.ReadChar();
            if (ch == '-')
            {
              // return what CDATA we have accumulated so far
              // then parse the comment and return to here.
              if (ws)
              {
                this.partial = ' '; // pop comment next time through
                return ParseComment();
              }
              else
              {
                // return what we've accumulated so far then come
                // back in and parse the comment.
                this.partial = '!';
                break;
              }
#if FIX
                        } else if (ch == '['){
                            // We are about to wrap this node as a CDATA block because of it's
                            // type in the DTD, but since we found a CDATA block in the input
                            // we have to parse it as a CDATA block, otherwise we will attempt
                            // to output nested CDATA blocks which of course is illegal.
                            if (this.ParseConditionalBlock()){
                                this.partial = ' ';
                                return true;
                            }
#endif
            }
            else
            {
              // not a comment, so ignore it and continue on.
              this.sb.Append('<');
              this.sb.Append('!');
              this.sb.Append(ch);
              ws = false;
            }
          }
          else if (ch == '?')
          {
            // processing instruction.
            this.current.ReadChar();// consume the '?' character.
            if (ws)
            {
              this.partial = ' '; // pop PI next time through
              return ParsePI();
            }
            else
            {
              this.partial = '?';
              break;
            }
          }
          else if (ch == '/')
          {
            // see if this is the end tag for this CDATA node.
            string temp = this.sb.ToString();
            if (ParseEndTag() && this.endTag == (object)this.node.Name)
            {
              if (ws || temp == "")
              {
                // we are done!
                return true;
              }
              else
              {
                // return CDATA text then the end tag
                this.partial = '/';
                this.sb.Length = 0; // restore buffer!
                this.sb.Append(temp);
                this.state = State.CData;
                break;
              }
            }
            else
            {
              // wrong end tag, so continue on.
              this.sb.Length = 0; // restore buffer!
              this.sb.Append(temp);
              this.sb.Append("</" + this.endTag + ">");
              ws = false;
            }
          }
          else
          {
            // must be just part of the CDATA block, so proceed.
            this.sb.Append('<');
            this.sb.Append(ch);
            ws = false;
          }
        }
        else
        {
          if (!this.current.IsWhitespace && ws) ws = false;
          this.sb.Append(ch);
        }
        ch = this.current.ReadChar();
      }
      string value = this.sb.ToString();
      Push(null, XmlNodeType.CDATA, value);
      if (this.partial == '\0')
        this.partial = ' ';// force it to pop this CDATA next time in.
      return true;
    }

    void ExpandEntity(StringBuilder sb, char terminator)
    {
      char ch = this.current.ReadChar();
      if (ch == '#')
      {
        string charent = this.current.ExpandCharEntity();
        sb.Append(charent);
        ch = this.current.Lastchar;
      }
      else
      {
        this.name.Length = 0;
        while (ch != Entity.EOF &&
            (Char.IsLetter(ch) || ch == '_' || ch == '-'))
        {
          this.name.Append(ch);
          ch = this.current.ReadChar();
        }
        string name = this.name.ToString();
        if (this.dtd != null && name != "")
        {
          Entity e = (Entity)this.dtd.FindEntity(name);
          if (e != null)
          {
            if (e.Internal)
            {
              sb.Append(e.Literal);
              if (ch != terminator)
                ch = this.current.ReadChar();
              return;
            }
            else
            {
              Entity ex = new Entity(name, e.PublicId, e.Uri, this.current.Proxy);
              e.Open(this.current, new Uri(e.Uri));
              this.current = ex;
              this.current.ReadChar();
              return;
            }
          }
          else
          {
            Log("Undefined entity '{0}'", name);
          }
        }
        // Entity is not defined, so just keep it in with the rest of the
        // text.
        sb.Append("&");
        sb.Append(name);
        if (ch != terminator)
        {
          sb.Append(ch);
          ch = this.current.ReadChar();
        }
      }
    }

    public override bool EOF
    {
      get
      {
        return this.state == State.Eof;
      }
    }

    public override void Close()
    {
      if (this.current != null)
      {
        this.current.Close();
        this.current = null;
      }
      if (this.log != null)
      {
        this.log.Close();
        this.log = null;
      }
    }

    public override ReadState ReadState
    {
      get
      {
        if (this.state == State.Initial) return ReadState.Initial;
        else if (this.state == State.Eof) return ReadState.EndOfFile;
        return ReadState.Interactive;
      }
    }

    public override string ReadString()
    {
      if (this.node.NodeType == XmlNodeType.Element)
      {
        this.sb.Length = 0;
        while (Read())
        {
          switch (this.NodeType)
          {
            case XmlNodeType.CDATA:
            case XmlNodeType.SignificantWhitespace:
            case XmlNodeType.Whitespace:
            case XmlNodeType.Text:
              this.sb.Append(this.node.Value);
              break;
            default:
              return this.sb.ToString();
          }
        }
        return this.sb.ToString();
      }
      return this.node.Value;
    }


    public override string ReadInnerXml()
    {
      StringWriter sw = new StringWriter();
      XmlTextWriter xw = new XmlTextWriter(sw);
      xw.Formatting = Formatting.Indented;
      switch (this.NodeType)
      {
        case XmlNodeType.Element:
          Read();
          while (!this.EOF && this.NodeType != XmlNodeType.EndElement)
          {
            xw.WriteNode(this, true);
          }
          Read(); // consume the end tag
          break;
        case XmlNodeType.Attribute:
          sw.Write(this.Value);
          break;
        default:
          // return empty string according to XmlReader spec.
          break;
      }
      xw.Close();
      return sw.ToString();
    }

    public override string ReadOuterXml()
    {
      StringWriter sw = new StringWriter();
      XmlTextWriter xw = new XmlTextWriter(sw);
      xw.Formatting = Formatting.Indented;
      xw.WriteNode(this, true);
      xw.Close();
      return sw.ToString();
    }

    public override XmlNameTable NameTable
    {
      get
      {
        return this.nametable;
      }
    }

    public override string LookupNamespace(string prefix)
    {
      return null;// there are no namespaces in SGML.
    }

    public override void ResolveEntity()
    {
      // We never return any entity reference nodes, so this should never be called.
      throw new InvalidOperationException("Not on an entity reference.");
    }

    public override bool ReadAttributeValue()
    {
      if (this.state == State.Attr)
      {
        this.state = State.AttrValue;
        return true;
      }
      else if (this.state == State.AttrValue)
      {
        return false;
      }
      throw new InvalidOperationException("Not on an attribute.");
    }

    void Validate(Node node)
    {
      if (this.dtd != null)
      {
        ElementDecl e = this.dtd.FindElement(node.Name);
        if (e != null)
        {
          node.DtdType = e;
          if (e.ContentModel.DeclaredContent == DeclaredContent.EMPTY)
            node.IsEmpty = true;
        }
      }
    }

    void ValidateAttribute(Node node, Attribute a)
    {
      ElementDecl e = node.DtdType;
      if (e != null)
      {
        AttDef ad = e.FindAttribute(a.Name);
        if (ad != null)
        {
          a.DtdType = ad;
        }
      }
    }

    void ValidateContent(Node node)
    {
      if (this.dtd != null)
      {
        // See if this element is allowed inside the current element.
        // If it isn't, then auto-close elements until we find one
        // that it is allowed to be in.
        string name = this.nametable.Add(node.Name.ToUpper()); // DTD is in upper case
        int i = 0;
        int top = this.stack.Count - 2;
        if (node.DtdType != null)
        {
          // it is a known element, let's see if it's allowed in the
          // current context.
          for (i = top; i > 0; i--)
          {
            Node n = (Node)this.stack[i];
            if (n.IsEmpty)
              continue; // we'll have to pop this one
            ElementDecl f = n.DtdType;
            if (f != null)
            {
              if (f.Name == this.dtd.Name)
                break; // can't pop the root element.
              if (f.CanContain(name, this.dtd))
              {
                break;
              }
              else if (!f.EndTagOptional)
              {
                // If the end tag is not optional then we can't
                // auto-close it.  We'll just have to live with the
                // junk we've found and move on.
                break;
              }
            }
            else
            {
              // Since we don't understand this tag anyway,
              // we might as well allow this content!
              break;
            }
          }
        }
        if (i == 0)
        {
          // Tag was not found or is not allowed anywhere, ignore it and
          // continue on.
        }
        else if (i < top)
        {
          Node n = (Node)this.stack[top];
          if (i == top - 1 && name == n.Name)
          {
            // e.g. p not allowed inside p, not an interesting error.
          }
          else
          {
            string closing = "";
            for (int k = top; k >= i + 1; k--)
            {
              if (closing != "") closing += ",";
              Node n2 = (Node)this.stack[k];
              closing += "<" + n2.Name + ">";
            }
            Log("Element '{0}' not allowed inside '{1}', closing {2}.",
                name, n.Name, closing);
          }
          this.state = State.AutoClose;
          this.newnode = node;
          Pop(); // save this new node until we pop the others
          this.poptodepth = i + 1;
        }
      }
    }
  }

  enum LiteralType
  {
    CDATA, SDATA, PI
  };

  class Entity
  {
    public const Char EOF = (char)65535;
    public string Proxy;
    public string Name;
    public bool Internal;
    public string PublicId;
    public string Uri;
    public string Literal;
    public LiteralType LiteralType;
    public Entity Parent;
    public bool Html;
    public int Line;
    public char Lastchar;
    public bool IsWhitespace;

    Encoding encoding;
    Uri resolvedUri;
    TextReader stm;
    bool weOwnTheStream;
    int lineStart;
    int absolutePos;

    public Entity(string name, string pubid, string uri, string proxy)
    {
      Name = name;
      PublicId = pubid;
      Uri = uri;
      Proxy = proxy;
      Html = (name != null && StringUtilities.EqualsIgnoreCase(name, "html"));
    }

    public Entity(string name, string literal)
    {
      Name = name;
      Literal = literal;
      Internal = true;
    }

    public Entity(string name, Uri baseUri, TextReader stm, string proxy)
    {
      Name = name;
      Internal = true;
      this.stm = stm;
      this.resolvedUri = baseUri;
      Proxy = proxy;
      Html = (string.Compare(name, "html", true, CultureInfo.InvariantCulture) == 0);
    }

    public Uri ResolvedUri
    {
      get
      {
        if (this.resolvedUri != null) return this.resolvedUri;
        else if (Parent != null) return Parent.ResolvedUri;
        return null;
      }
    }

    public int LinePosition
    {
      get { return this.absolutePos - this.lineStart + 1; }
    }

    public char ReadChar()
    {
      char ch = (char)this.stm.Read();
      if (ch == 0)
      {
        // convert nulls to whitespace, since they are not valid in XML anyway.
        ch = ' ';
      }
      this.absolutePos++;
      if (ch == 0xa)
      {
        IsWhitespace = true;
        this.lineStart = this.absolutePos + 1;
        Line++;
      }
      else if (ch == ' ' || ch == '\t')
      {
        IsWhitespace = true;
        if (Lastchar == 0xd)
        {
          this.lineStart = this.absolutePos;
          Line++;
        }
      }
      else if (ch == 0xd)
      {
        IsWhitespace = true;
      }
      else
      {
        IsWhitespace = false;
        if (Lastchar == 0xd)
        {
          Line++;
          this.lineStart = this.absolutePos;
        }
      }
      Lastchar = ch;
      return ch;
    }

    public void Open(Entity parent, Uri baseUri)
    {
      Parent = parent;
      if (parent != null) this.Html = parent.Html;
      this.Line = 1;
      if (Internal)
      {
        if (this.Literal != null)
          this.stm = new StringReader(this.Literal);
      }
      else if (this.Uri == null)
      {
        this.Error("Unresolvable entity '{0}'", this.Name);
      }
      else
      {
        if (baseUri != null)
        {
          this.resolvedUri = new Uri(baseUri, this.Uri);
        }
        else
        {
          this.resolvedUri = new Uri(this.Uri);
        }

        Stream stream = null;
        Encoding e = Encoding.Default;
        switch (this.resolvedUri.Scheme)
        {
          case "file":
            {
              string path = this.resolvedUri.LocalPath;
              stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            break;
          default:
            //Console.WriteLine("Fetching:" + ResolvedUri.AbsoluteUri);
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(ResolvedUri);
            wr.UserAgent = "Mozilla/4.0 (compatible;);";
            wr.Timeout = 10000; // in case this is running in an ASPX page.
            if (Proxy != null) wr.Proxy = new WebProxy(Proxy);
            wr.PreAuthenticate = false;
            // Pass the credentials of the process.
            wr.Credentials = CredentialCache.DefaultCredentials;

            WebResponse resp = wr.GetResponse();
            Uri actual = resp.ResponseUri;
            if (actual.AbsoluteUri != this.resolvedUri.AbsoluteUri)
            {
              this.resolvedUri = actual;
            }
            string contentType = resp.ContentType.ToLower();
            string mimeType = contentType;
            int i = contentType.IndexOf(';');
            if (i >= 0)
            {
              mimeType = contentType.Substring(0, i);
            }
            if (StringUtilities.EqualsIgnoreCase(mimeType, "text/html"))
            {
              this.Html = true;
            }

            i = contentType.IndexOf("charset");
            e = Encoding.Default;
            if (i >= 0)
            {
              int j = contentType.IndexOf("=", i);
              int k = contentType.IndexOf(";", j);
              if (k < 0) k = contentType.Length;
              if (j > 0)
              {
                j++;
                string charset = contentType.Substring(j, k - j).Trim();
                try
                {
                  e = Encoding.GetEncoding(charset);
                }
                catch (Exception)
                {
                }
              }
            }
            stream = resp.GetResponseStream();
            break;

        }
        this.weOwnTheStream = true;
        HtmlStream html = new HtmlStream(stream, e);
        this.encoding = html.Encoding;
        this.stm = html;
      }
    }

    public Encoding GetEncoding()
    {
      return this.encoding;
    }

    public void Close()
    {
      if (this.weOwnTheStream)
        this.stm.Close();
    }

    public char SkipWhitespace()
    {
      char ch = Lastchar;
      while (ch != Entity.EOF && (ch == ' ' || ch == '\r' || ch == '\n' || ch == '\t'))
      {
        ch = ReadChar();
      }
      return ch;
    }

    public string ScanToken(StringBuilder sb, string term, bool nmtoken)
    {
      sb.Length = 0;
      char ch = Lastchar;
      if (nmtoken && ch != '_' && !Char.IsLetter(ch))
      {
        throw new Exception(
            String.Format("Invalid name start character '{0}'", ch));
      }
      while (ch != Entity.EOF && term.IndexOf(ch) < 0)
      {
        if (!nmtoken || ch == '_' || ch == '.' || ch == '-' || ch == ':' || Char.IsLetterOrDigit(ch))
        {
          sb.Append(ch);
        }
        else
        {
          throw new Exception(
              String.Format("Invalid name character '{0}'", ch));
        }
        ch = ReadChar();
      }
      return sb.ToString();
    }

    public string ScanLiteral(StringBuilder sb, char quote)
    {
      sb.Length = 0;
      char ch = ReadChar();
      while (ch != Entity.EOF && ch != quote)
      {
        if (ch == '&')
        {
          ch = ReadChar();
          if (ch == '#')
          {
            string charent = ExpandCharEntity();
            sb.Append(charent);
            ch = this.Lastchar;
          }
          else
          {
            sb.Append('&');
            sb.Append(ch);
            ch = ReadChar();
          }
        }
        else
        {
          sb.Append(ch);
          ch = ReadChar();
        }
      }
      ReadChar(); // consume end quote.
      return sb.ToString();
    }

    public string ScanToEnd(StringBuilder sb, string type, string terminators)
    {
      if (sb != null) sb.Length = 0;
      int start = Line;
      // This method scans over a chunk of text looking for the
      // termination sequence specified by the 'terminators' parameter.
      char ch = ReadChar();
      int state = 0;
      char next = terminators[state];
      while (ch != Entity.EOF)
      {
        if (ch == next)
        {
          state++;
          if (state >= terminators.Length)
          {
            // found it!
            break;
          }
          next = terminators[state];
        }
        else if (state > 0)
        {
          // char didn't match, so go back and see how much does still match.
          int i = state - 1;
          int newstate = 0;
          while (i >= 0 && newstate == 0)
          {
            if (terminators[i] == ch)
            {
              // character is part of the terminators pattern, ok, so see if we can
              // match all the way back to the beginning of the pattern.
              int j = 1;
              while (i - j >= 0)
              {
                if (terminators[i - j] != terminators[state - j])
                  break;
                j++;
              }
              if (j > i)
              {
                newstate = i + 1;
              }
            }
            else
            {
              i--;
            }
          }
          if (sb != null)
          {
            i = (i < 0) ? 1 : 0;
            for (int k = 0; k <= state - newstate - i; k++)
            {
              sb.Append(terminators[k]);
            }
            if (i > 0) // see if we've matched this char or not
              sb.Append(ch); // if not then append it to buffer.
          }
          state = newstate;
          next = terminators[newstate];
        }
        else
        {
          if (sb != null) sb.Append(ch);
        }
        ch = ReadChar();
      }
      if (ch == 0) Error(type + " starting on line {0} was never closed", start);
      ReadChar(); // consume last char in termination sequence.
      if (sb != null) return sb.ToString();
      return "";
    }

    public string ExpandCharEntity()
    {
      char ch = ReadChar();
      int v = 0;
      if (ch == 'x')
      {
        ch = ReadChar();
        for (; ch != Entity.EOF && ch != ';'; ch = ReadChar())
        {
          int p = 0;
          if (ch >= '0' && ch <= '9')
          {
            p = (int)(ch - '0');
          }
          else if (ch >= 'a' && ch <= 'f')
          {
            p = (int)(ch - 'a') + 10;
          }
          else if (ch >= 'A' && ch <= 'F')
          {
            p = (int)(ch - 'A') + 10;
          }
          else
          {
            break;//we must be done!
            //Error("Hex digit out of range '{0}'", (int)ch);
          }
          v = (v * 16) + p;
        }
      }
      else
      {
        for (; ch != Entity.EOF && ch != ';'; ch = ReadChar())
        {
          if (ch >= '0' && ch <= '9')
          {
            v = (v * 10) + (int)(ch - '0');
          }
          else
          {
            break; // we must be done!
            //Error("Decimal digit out of range '{0}'", (int)ch);
          }
        }
      }
      if (ch == 0)
      {
        Error("Premature {0} parsing entity reference", ch);
      }
      else if (ch == ';')
      {
        ReadChar();
      }
      // HACK ALERT: IE and Netscape map the unicode characters
      if (this.Html && v >= 0x80 & v <= 0x9F)
      {
        // This range of control characters is mapped to Windows-1252!
        int size = CtrlMap.Length;
        int i = v - 0x80;
        int unicode = CtrlMap[i];
        return Convert.ToChar(unicode).ToString();
      }
      return Convert.ToChar(v).ToString();
    }

    static int[] CtrlMap = new int[] {
                                             // This is the windows-1252 mapping of the code points 0x80 through 0x9f.
                                             8364, 129, 8218, 402, 8222, 8230, 8224, 8225, 710, 8240, 352, 8249, 338, 141,
                                             381, 143, 144, 8216, 8217, 8220, 8221, 8226, 8211, 8212, 732, 8482, 353, 8250,
                                             339, 157, 382, 376
                                         };

    public void Error(string msg)
    {
      throw new Exception(msg);
    }

    public void Error(string msg, char ch)
    {
      string str = (ch == Entity.EOF) ? "EOF" : Char.ToString(ch);
      throw new Exception(String.Format(msg, str));
    }

    public void Error(string msg, int x)
    {
      throw new Exception(String.Format(msg, x));
    }

    public void Error(string msg, string arg)
    {
      throw new Exception(String.Format(msg, arg));
    }

    public string Context()
    {
      Entity p = this;
      StringBuilder sb = new StringBuilder();
      while (p != null)
      {
        string msg;
        if (p.Internal)
        {
          msg = String.Format("\nReferenced on line {0}, position {1} of internal entity '{2}'", p.Line, p.LinePosition, p.Name);
        }
        else
        {
          msg = String.Format("\nReferenced on line {0}, position {1} of '{2}' entity at [{3}]", p.Line, p.LinePosition, p.Name, p.ResolvedUri.AbsolutePath);
        }
        sb.Append(msg);
        p = p.Parent;
      }
      return sb.ToString();
    }

    public static bool IsLiteralType(string token)
    {
      return (token == "CDATA" || token == "SDATA" || token == "PI");
    }

    public void SetLiteralType(string token)
    {
      switch (token)
      {
        case "CDATA":
          LiteralType = LiteralType.CDATA;
          break;
        case "SDATA":
          LiteralType = LiteralType.SDATA;
          break;
        case "PI":
          LiteralType = LiteralType.PI;
          break;
      }
    }
  }

  // This class decodes an HTML/XML stream correctly.
  internal class HtmlStream : TextReader
  {
    Stream stm;
    byte[] rawBuffer;
    int rawPos;
    int rawUsed;
    Encoding encoding;
    Decoder decoder;
    char[] buffer;
    int used;
    int pos;
    private const int BUFSIZE = 16384;
    private const int EOF = -1;

    public HtmlStream(Stream stm, Encoding defaultEncoding)
    {
      if (defaultEncoding == null) defaultEncoding = Encoding.UTF8; // default is UTF8
      if (!stm.CanSeek)
      {
        // Need to be able to seek to sniff correctly.
        stm = CopyToMemoryStream(stm);
      }
      this.stm = stm;
      rawBuffer = new Byte[BUFSIZE];
      rawUsed = stm.Read(rawBuffer, 0, 4); // maximum byte order mark
      this.buffer = new char[BUFSIZE];

      // Check byte order marks
      this.decoder = AutoDetectEncoding(rawBuffer, ref rawPos, rawUsed);
      int bom = rawPos;
      if (this.decoder == null)
      {
        this.decoder = defaultEncoding.GetDecoder();
        rawUsed += stm.Read(rawBuffer, 4, BUFSIZE - 4);
        DecodeBlock();
        // Now sniff to see if there is an XML declaration or HTML <META> tag.
        Decoder sd = SniffEncoding();
        if (sd != null)
        {
          this.decoder = sd;
        }
      }

      // Reset to get ready for Read()
      this.stm.Seek(0, SeekOrigin.Begin);
      this.pos = this.used = 0;
      // skip bom
      if (bom > 0)
      {
        stm.Read(this.rawBuffer, 0, bom);
      }
      this.rawPos = this.rawUsed = 0;

    }

    public Encoding Encoding
    {
      get
      {
        return this.encoding;
      }
    }

    Stream CopyToMemoryStream(Stream s)
    {
      int size = 100000; // large heap is more efficient
      byte[] buffer = new byte[size];
      int len;
      MemoryStream r = new MemoryStream();
      while ((len = s.Read(buffer, 0, size)) > 0)
      {
        r.Write(buffer, 0, len);
      }
      r.Seek(0, SeekOrigin.Begin);
      s.Close();
      return r;
    }

    internal void DecodeBlock()
    {
      // shift current chars to beginning.
      if (pos > 0)
      {
        if (pos < used)
        {
          System.Array.Copy(buffer, pos, buffer, 0, used - pos);
        }
        used -= pos;
        pos = 0;
      }
      int len = decoder.GetCharCount(rawBuffer, rawPos, rawUsed - rawPos);
      int available = buffer.Length - used;
      if (available < len)
      {
        char[] newbuf = new char[buffer.Length + len];
        System.Array.Copy(buffer, pos, newbuf, 0, used - pos);
        buffer = newbuf;
      }
      used = pos + decoder.GetChars(rawBuffer, rawPos, rawUsed - rawPos, buffer, pos);
      rawPos = rawUsed; // consumed the whole buffer!
    }
    internal static Decoder AutoDetectEncoding(byte[] buffer, ref int index, int length)
    {
      if (4 <= (length - index))
      {
        uint w = (uint)buffer[index + 0] << 24 | (uint)buffer[index + 1] << 16 | (uint)buffer[index + 2] << 8 | (uint)buffer[index + 3];
        // see if it's a 4-byte encoding
        switch (w)
        {
          case 0xfefffeff:
            index += 4;
            return new Ucs4DecoderBigEngian();

          case 0xfffefffe:
            index += 4;
            return new Ucs4DecoderLittleEndian();

          case 0x3c000000:
            goto case 0xfefffeff;

          case 0x0000003c:
            goto case 0xfffefffe;
        }
        w >>= 8;
        if (w == 0xefbbbf)
        {
          index += 3;
          return Encoding.UTF8.GetDecoder();
        }
        w >>= 8;
        switch (w)
        {
          case 0xfeff:
            index += 2;
            return UnicodeEncoding.BigEndianUnicode.GetDecoder();

          case 0xfffe:
            index += 2;
            return new UnicodeEncoding(false, false).GetDecoder();

          case 0x3c00:
            goto case 0xfeff;

          case 0x003c:
            goto case 0xfffe;
        }
      }
      return null;
    }
    private int ReadChar()
    {
      // Read only up to end of current buffer then stop.
      if (pos < used) return buffer[pos++];
      return EOF;
    }
    private int PeekChar()
    {
      int ch = ReadChar();
      if (ch != EOF)
      {
        pos--;
      }
      return ch;
    }
    private bool SniffPattern(string pattern)
    {
      int ch = PeekChar();
      if (ch != pattern[0]) return false;
      for (int i = 0, n = pattern.Length; ch != EOF && i < n; i++)
      {
        ch = ReadChar();
        char m = pattern[i];
        if (ch != m)
        {
          return false;
        }
      }
      return true;
    }
    private void SniffWhitespace()
    {
      char ch = (char)PeekChar();
      while (ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n')
      {
        int i = pos;
        ch = (char)ReadChar();
        if (ch != ' ' && ch != '\t' && ch != '\r' && ch != '\n')
          pos = i;
      }
    }

    private string SniffLiteral()
    {
      int quoteChar = PeekChar();
      if (quoteChar == '\'' || quoteChar == '"')
      {
        ReadChar();// consume quote char
        int i = this.pos;
        int ch = ReadChar();
        while (ch != EOF && ch != quoteChar)
        {
          ch = ReadChar();
        }
        return (pos > i) ? new string(buffer, i, pos - i - 1) : "";
      }
      return null;
    }
    private string SniffAttribute(string name)
    {
      SniffWhitespace();
      string id = SniffName();
      if (name == id)
      {
        SniffWhitespace();
        if (SniffPattern("="))
        {
          SniffWhitespace();
          return SniffLiteral();
        }
      }
      return null;
    }
    private string SniffAttribute(out string name)
    {
      SniffWhitespace();
      name = SniffName();
      if (name != null)
      {
        SniffWhitespace();
        if (SniffPattern("="))
        {
          SniffWhitespace();
          return SniffLiteral();
        }
      }
      return null;
    }
    private void SniffTerminator(string term)
    {
      int ch = ReadChar();
      int i = 0;
      int n = term.Length;
      while (i < n && ch != EOF)
      {
        if (term[i] == ch)
        {
          i++;
          if (i == n) break;
        }
        else
        {
          i = 0; // reset.
        }
        ch = ReadChar();
      }
    }
    internal Decoder SniffEncoding()
    {
      Decoder decoder = null;
      if (SniffPattern("<?xml"))
      {
        string version = SniffAttribute("version");
        if (version != null)
        {
          string encoding = SniffAttribute("encoding");
          if (encoding != null)
          {
            try
            {
              Encoding enc = Encoding.GetEncoding(encoding);
              if (enc != null)
              {
                this.encoding = enc;
                return enc.GetDecoder();
              }
            }
            catch (Exception)
            {
              // oh well then.
            }
          }
          SniffTerminator(">");
        }
      }
      if (decoder == null)
      {
        return SniffMeta();
      }
      return null;
    }

    internal Decoder SniffMeta()
    {
      int i = ReadChar();
      while (i != EOF)
      {
        char ch = (char)i;
        if (ch == '<')
        {
          string name = SniffName();
          if (name != null && StringUtilities.EqualsIgnoreCase(name, "meta"))
          {
            string httpequiv = null;
            string content = null;
            while (true)
            {
              string value = SniffAttribute(out name);
              if (name == null)
              {
                break;
              }
              if (StringUtilities.EqualsIgnoreCase(name, "http-equiv"))
              {
                httpequiv = value;
              }
              else if (StringUtilities.EqualsIgnoreCase(name, "content"))
              {
                content = value;
              }
            }
            if (httpequiv != null && StringUtilities.EqualsIgnoreCase(httpequiv, "content-type") && content != null)
            {
              int j = content.IndexOf("charset");
              if (j >= 0)
              {
                //charset=utf-8
                j = content.IndexOf("=", j);
                if (j >= 0)
                {
                  j++;
                  int k = content.IndexOf(";", j);
                  if (k < 0) k = content.Length;
                  string charset = content.Substring(j, k - j).Trim();
                  try
                  {
                    Encoding e = Encoding.GetEncoding(charset);
                    this.encoding = e;
                    return e.GetDecoder();
                  }
                  catch
                  {
                  }
                }
              }
            }
          }
        }
        i = ReadChar();

      }
      return null;
    }

    internal string SniffName()
    {
      int c = PeekChar();
      if (c == EOF)
        return null;
      char ch = (char)c;
      int start = pos;
      while (pos < used - 1 && (Char.IsLetterOrDigit(ch) || ch == '-' || ch == '_' || ch == ':'))
      {
        ch = buffer[++pos];
      }
      if (start == pos) return null;
      return new string(buffer, start, pos - start);
    }

    internal void SkipWhitespace()
    {
      char ch = (char)PeekChar();
      while (pos < used - 1 && (ch == ' ' || ch == '\r' || ch == '\n'))
      {
        ch = buffer[++pos];
      }
    }
    internal void SkipTo(char what)
    {
      char ch = (char)PeekChar();
      while (pos < used - 1 && (ch != what))
      {
        ch = buffer[++pos];
      }
    }
    internal string ParseAttribute()
    {
      SkipTo('=');
      if (pos < used)
      {
        pos++;
        SkipWhitespace();
        if (pos < used)
        {
          char quote = buffer[pos];
          pos++;
          int start = pos;
          SkipTo(quote);
          if (pos < used)
          {
            string result = new string(buffer, start, pos - start);
            pos++;
            return result;
          }
        }
      }
      return null;
    }
    public override int Peek()
    {
      int result = Read();
      if (result != EOF)
      {
        pos--;
      }
      return result;
    }
    public override int Read()
    {
      if (pos == used)
      {
        rawUsed = stm.Read(rawBuffer, 0, rawBuffer.Length);
        rawPos = 0;
        if (rawUsed == 0) return EOF;
        DecodeBlock();
      }
      if (pos < used) return buffer[pos++];
      return -1;
    }
    public override int Read(char[] buffer, int start, int length)
    {
      if (pos == used)
      {
        rawUsed = stm.Read(rawBuffer, 0, rawBuffer.Length);
        rawPos = 0;
        if (rawUsed == 0) return -1;
        DecodeBlock();
      }
      if (pos < used)
      {
        length = Math.Min(used - pos, length);
        Array.Copy(this.buffer, pos, buffer, start, length);
        pos += length;
        return length;
      }
      return 0;
    }

    public override int ReadBlock(char[] buffer, int index, int count)
    {
      return Read(buffer, index, count);
    }
    // Read up to end of line, or full buffer, whichever comes first.
    public int ReadLine(char[] buffer, int start, int length)
    {
      int i = 0;
      int ch = ReadChar();
      while (ch != EOF)
      {
        buffer[i + start] = (char)ch;
        i++;
        if (i + start == length)
          break; // buffer is full

        if (ch == '\r')
        {
          if (PeekChar() == '\n')
          {
            ch = ReadChar();
            buffer[i + start] = (char)ch;
            i++;
          }
          break;
        }
        else if (ch == '\n')
        {
          break;
        }
        ch = ReadChar();
      }
      return i;
    }

    public override string ReadToEnd()
    {
      char[] buffer = new char[100000]; // large block heap is more efficient
      int len = 0;
      StringBuilder sb = new StringBuilder();
      while ((len = Read(buffer, 0, buffer.Length)) > 0)
      {
        sb.Append(buffer, 0, len);
      }
      return sb.ToString();
    }
    public override void Close()
    {
      stm.Close();
    }
  }
  internal abstract class Ucs4Decoder : Decoder
  {
    internal byte[] temp = new byte[4];
    internal int tempBytes = 0;
    public override int GetCharCount(byte[] bytes, int index, int count)
    {
      return (count + tempBytes) / 4;
    }
    internal abstract int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
      int i = tempBytes;

      if (tempBytes > 0)
      {
        for (; i < 4; i++)
        {
          temp[i] = bytes[byteIndex];
          byteIndex++;
          byteCount--;
        }
        i = 1;
        GetFullChars(temp, 0, 4, chars, charIndex);
        charIndex++;
      }
      else
        i = 0;
      i = GetFullChars(bytes, byteIndex, byteCount, chars, charIndex) + i;

      int j = (tempBytes + byteCount) % 4;
      byteCount += byteIndex;
      byteIndex = byteCount - j;
      tempBytes = 0;

      if (byteIndex >= 0)
        for (; byteIndex < byteCount; byteIndex++)
        {
          temp[tempBytes] = bytes[byteIndex];
          tempBytes++;
        }
      return i;
    }
    internal char UnicodeToUTF16(UInt32 code)
    {
      byte lowerByte, higherByte;
      lowerByte = (byte)(0xD7C0 + (code >> 10));
      higherByte = (byte)(0xDC00 | code & 0x3ff);
      return ((char)((higherByte << 8) | lowerByte));
    }
  }
  internal class Ucs4DecoderBigEngian : Ucs4Decoder
  {
    internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
      UInt32 code;
      int i, j;
      byteCount += byteIndex;
      for (i = byteIndex, j = charIndex; i + 3 < byteCount; )
      {
        code = (UInt32)(((bytes[i + 3]) << 24) | (bytes[i + 2] << 16) | (bytes[i + 1] << 8) | (bytes[i]));
        if (code > 0x10FFFF)
        {
          throw new Exception("Invalid character 0x" + code.ToString("x") + " in encoding");
        }
        else if (code > 0xFFFF)
        {
          chars[j] = UnicodeToUTF16(code);
          j++;
        }
        else
        {
          if (code >= 0xD800 && code <= 0xDFFF)
          {
            throw new Exception("Invalid character 0x" + code.ToString("x") + " in encoding");
          }
          else
          {
            chars[j] = (char)code;
          }
        }
        j++;
        i += 4;
      }
      return j - charIndex;
    }
  };
  internal class Ucs4DecoderLittleEndian : Ucs4Decoder
  {
    internal override int GetFullChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
      UInt32 code;
      int i, j;
      byteCount += byteIndex;
      for (i = byteIndex, j = charIndex; i + 3 < byteCount; )
      {
        code = (UInt32)(((bytes[i]) << 24) | (bytes[i + 1] << 16) | (bytes[i + 2] << 8) | (bytes[i + 3]));
        if (code > 0x10FFFF)
        {
          throw new Exception("Invalid character 0x" + code.ToString("x") + " in encoding");
        }
        else if (code > 0xFFFF)
        {
          chars[j] = UnicodeToUTF16(code);
          j++;
        }
        else
        {
          if (code >= 0xD800 && code <= 0xDFFF)
          {
            throw new Exception("Invalid character 0x" + code.ToString("x") + " in encoding");
          }
          else
          {
            chars[j] = (char)code;
          }
        }
        j++;
        i += 4;
      }
      return j - charIndex;
    }
  }

  class ElementDecl
  {
    public ElementDecl(string name, bool sto, bool eto, ContentModel cm, string[] inclusions, string[] exclusions)
    {
      Name = name;
      StartTagOptional = sto;
      EndTagOptional = eto;
      ContentModel = cm;
      Inclusions = inclusions;
      Exclusions = exclusions;
    }
    public string Name;
    public bool StartTagOptional;
    public bool EndTagOptional;
    public ContentModel ContentModel;
    public string[] Inclusions;
    public string[] Exclusions;
    public AttList AttList;

    public AttDef FindAttribute(string name)
    {
      return AttList[name.ToUpper()];
    }

    public void AddAttDefs(AttList list)
    {
      if (AttList == null)
      {
        AttList = list;
      }
      else
      {
        foreach (AttDef a in list)
        {
          if (AttList[a.Name] == null)
          {
            AttList.Add(a);
          }
        }
      }
    }

    public bool CanContain(string name, SgmlDtd dtd)
    {
      // return true if this element is allowed to contain the given element.
      if (Exclusions != null)
      {
        foreach (string s in Exclusions)
        {
          if ((object)s == (object)name) // XmlNameTable optimization
            return false;
        }
      }
      if (Inclusions != null)
      {
        foreach (string s in Inclusions)
        {
          if ((object)s == (object)name) // XmlNameTable optimization
            return true;
        }
      }
      return ContentModel.CanContain(name, dtd);
    }
  }

  enum DeclaredContent
  {
    Default, CDATA, RCDATA, EMPTY
  }

  class ContentModel
  {
    public DeclaredContent DeclaredContent;
    public int CurrentDepth;
    public Group Model;

    public ContentModel()
    {
      Model = new Group(null);
    }

    public void PushGroup()
    {
      Model = new Group(Model);
      CurrentDepth++;
    }

    public int PopGroup()
    {
      if (CurrentDepth == 0) return -1;
      CurrentDepth--;
      Model.Parent.AddGroup(Model);
      Model = Model.Parent;
      return CurrentDepth;
    }

    public void AddSymbol(string sym)
    {
      Model.AddSymbol(sym);
    }

    public void AddConnector(char c)
    {
      Model.AddConnector(c);
    }

    public void AddOccurrence(char c)
    {
      Model.AddOccurrence(c);
    }

    public void SetDeclaredContent(string dc)
    {
      switch (dc)
      {
        case "EMPTY":
          this.DeclaredContent = DeclaredContent.EMPTY;
          break;
        case "RCDATA":
          this.DeclaredContent = DeclaredContent.RCDATA;
          break;
        case "CDATA":
          this.DeclaredContent = DeclaredContent.CDATA;
          break;
        default:
          throw new Exception(
              String.Format("Declared content type '{0}' is not supported", dc));
      }
    }

    public bool CanContain(string name, SgmlDtd dtd)
    {
      if (DeclaredContent != DeclaredContent.Default)
        return false; // empty or text only node.
      return Model.CanContain(name, dtd);
    }
  }

  enum GroupType
  {
    None, And, Or, Sequence
  };

  enum Occurrence
  {
    Required, Optional, ZeroOrMore, OneOrMore
  }

  class Group
  {
    public Group Parent;
    public ArrayList Members;
    public GroupType GroupType;
    public Occurrence Occurrence;
    public bool Mixed;

    public bool TextOnly
    {
      get { return this.Mixed && Members.Count == 0; }
    }

    public Group(Group parent)
    {
      Parent = parent;
      Members = new ArrayList();
      this.GroupType = GroupType.None;
      Occurrence = Occurrence.Required;
    }
    public void AddGroup(Group g)
    {
      Members.Add(g);
    }
    public void AddSymbol(string sym)
    {
      if (sym == "#PCDATA")
      {
        Mixed = true;
      }
      else
      {
        Members.Add(sym);
      }
    }
    public void AddConnector(char c)
    {
      if (!Mixed && Members.Count == 0)
      {
        throw new Exception(
            String.Format("Missing token before connector '{0}'.", c)
            );
      }
      GroupType gt = GroupType.None;
      switch (c)
      {
        case ',':
          gt = GroupType.Sequence;
          break;
        case '|':
          gt = GroupType.Or;
          break;
        case '&':
          gt = GroupType.And;
          break;
      }
      if (GroupType != GroupType.None && GroupType != gt)
      {
        throw new Exception(
            String.Format("Connector '{0}' is inconsistent with {1} group.", c, GroupType.ToString())
            );
      }
      GroupType = gt;
    }

    public void AddOccurrence(char c)
    {
      Occurrence o = Occurrence.Required;
      switch (c)
      {
        case '?':
          o = Occurrence.Optional;
          break;
        case '+':
          o = Occurrence.OneOrMore;
          break;
        case '*':
          o = Occurrence.ZeroOrMore;
          break;
      }
      Occurrence = o;
    }

    // Rough approximation - this is really assuming an "Or" group
    public bool CanContain(string name, SgmlDtd dtd)
    {
      // Do a simple search of members.
      foreach (object obj in Members)
      {
        if (obj is String)
        {
          if (obj == (object)name) // XmlNameTable optimization
            return true;
        }
      }
      // didn't find it, so do a more expensive search over child elements
      // that have optional start tags and over child groups.
      foreach (object obj in Members)
      {
        if (obj is String)
        {
          string s = (string)obj;
          ElementDecl e = dtd.FindElement(s);
          if (e != null)
          {
            if (e.StartTagOptional)
            {
              // tricky case, the start tag is optional so element may be
              // allowed inside this guy!
              if (e.CanContain(name, dtd))
                return true;
            }
          }
        }
        else
        {
          Group m = (Group)obj;
          if (m.CanContain(name, dtd))
            return true;
        }
      }
      return false;
    }
  }

  enum AttributeType
  {
    DEFAULT, CDATA, ENTITY, ENTITIES, ID, IDREF, IDREFS, NAME, NAMES, NMTOKEN, NMTOKENS,
    NUMBER, NUMBERS, NUTOKEN, NUTOKENS, NOTATION, ENUMERATION
  }

  enum AttributePresence
  {
    DEFAULT, FIXED, REQUIRED, IMPLIED
  }

  class AttDef
  {
    public string Name;
    public AttributeType Type;
    public string[] EnumValues;
    public string Default;
    public AttributePresence Presence;

    public AttDef(string name)
    {
      Name = name;
    }


    public void SetType(string type)
    {
      switch (type)
      {
        case "CDATA":
          Type = AttributeType.CDATA;
          break;
        case "ENTITY":
          Type = AttributeType.ENTITY;
          break;
        case "ENTITIES":
          Type = AttributeType.ENTITIES;
          break;
        case "ID":
          Type = AttributeType.ID;
          break;
        case "IDREF":
          Type = AttributeType.IDREF;
          break;
        case "IDREFS":
          Type = AttributeType.IDREFS;
          break;
        case "NAME":
          Type = AttributeType.NAME;
          break;
        case "NAMES":
          Type = AttributeType.NAMES;
          break;
        case "NMTOKEN":
          Type = AttributeType.NMTOKEN;
          break;
        case "NMTOKENS":
          Type = AttributeType.NMTOKENS;
          break;
        case "NUMBER":
          Type = AttributeType.NUMBER;
          break;
        case "NUMBERS":
          Type = AttributeType.NUMBERS;
          break;
        case "NUTOKEN":
          Type = AttributeType.NUTOKEN;
          break;
        case "NUTOKENS":
          Type = AttributeType.NUTOKENS;
          break;
        default:
          throw new Exception("Attribute type '" + type + "' is not supported");
      }
    }

    public bool SetPresence(string token)
    {
      bool hasDefault = true;
      if (token == "FIXED")
      {
        Presence = AttributePresence.FIXED;
      }
      else if (token == "REQUIRED")
      {
        Presence = AttributePresence.REQUIRED;
        hasDefault = false;
      }
      else if (token == "IMPLIED")
      {
        Presence = AttributePresence.IMPLIED;
        hasDefault = false;
      }
      else
      {
        throw new Exception(String.Format("Attribute value '{0}' not supported", token));
      }
      return hasDefault;
    }
  }

  class AttList : IEnumerable
  {
    Hashtable AttDefs;

    public AttList()
    {
      AttDefs = new Hashtable();
    }

    public void Add(AttDef a)
    {
      AttDefs.Add(a.Name, a);
    }

    public AttDef this[string name]
    {
      get
      {
        return (AttDef)AttDefs[name];
      }
    }

    public IEnumerator GetEnumerator()
    {
      return AttDefs.Values.GetEnumerator();
    }
  }

  class SgmlDtd
  {
    public string Name;

    Hashtable elements;
    Hashtable pentities;
    Hashtable entities;
    StringBuilder sb;
    Entity current;
    XmlNameTable nameTable;

    public SgmlDtd(string name, XmlNameTable nt)
    {
      this.nameTable = nt;
      this.Name = name;
      this.elements = new Hashtable();
      this.pentities = new Hashtable();
      this.entities = new Hashtable();
      this.sb = new StringBuilder();
    }

    public XmlNameTable NameTable { get { return this.nameTable; } }

    public static SgmlDtd Parse(Uri baseUri, string name, string pubid, string url, string subset, string proxy, XmlNameTable nt)
    {
      SgmlDtd dtd = new SgmlDtd(name, nt);
      if (url != null && url != "")
      {
        dtd.PushEntity(baseUri, new Entity(dtd.Name, pubid, url, proxy));
      }
      if (subset != null && subset != "")
      {
        dtd.PushEntity(baseUri, new Entity(name, subset));
      }
      try
      {
        dtd.Parse();
      }
      catch (Exception e)
      {
        throw new Exception(e.Message + dtd.current.Context());
      }
      return dtd;
    }
    public static SgmlDtd Parse(Uri baseUri, string name, string pubid, TextReader input, string subset, string proxy, XmlNameTable nt)
    {
      SgmlDtd dtd = new SgmlDtd(name, nt);
      dtd.PushEntity(baseUri, new Entity(dtd.Name, baseUri, input, proxy));
      if (subset != null && subset != "")
      {
        dtd.PushEntity(baseUri, new Entity(name, subset));
      }
      try
      {
        dtd.Parse();
      }
      catch (Exception e)
      {
        throw new Exception(e.Message + dtd.current.Context());
      }
      return dtd;
    }

    public Entity FindEntity(string name)
    {
      return (Entity)this.entities[name];
    }

    public ElementDecl FindElement(string name)
    {
      return (ElementDecl)this.elements[name.ToUpper()];
    }

    //-------------------------------- Parser -------------------------
    void PushEntity(Uri baseUri, Entity e)
    {
      e.Open(this.current, baseUri);
      this.current = e;
      this.current.ReadChar();
    }

    void PopEntity()
    {
      if (this.current != null) this.current.Close();
      if (this.current.Parent != null)
      {
        this.current = this.current.Parent;
      }
      else
      {
        this.current = null;
      }
    }

    void Parse()
    {
      char ch = this.current.Lastchar;
      while (true)
      {
        switch (ch)
        {
          case Entity.EOF:
            PopEntity();
            if (this.current == null)
              return;
            ch = this.current.Lastchar;
            break;
          case ' ':
          case '\n':
          case '\r':
          case '\t':
            ch = this.current.ReadChar();
            break;
          case '<':
            ParseMarkup();
            ch = this.current.ReadChar();
            break;
          case '%':
            Entity e = ParseParameterEntity(SgmlDtd.WhiteSpace);
            try
            {
              PushEntity(this.current.ResolvedUri, e);
            }
            catch (Exception ex)
            {
              // bugbug - need an error log.
              Console.WriteLine(ex.Message + this.current.Context());
            }
            ch = this.current.Lastchar;
            break;
          default:
            this.current.Error("Unexpected character '{0}'", ch);
            break;
        }
      }
    }

    void ParseMarkup()
    {
      char ch = this.current.ReadChar();
      if (ch != '!')
      {
        this.current.Error("Found '{0}', but expecing declaration starting with '<!'");
        return;
      }
      ch = this.current.ReadChar();
      if (ch == '-')
      {
        ch = this.current.ReadChar();
        if (ch != '-') this.current.Error("Expecting comment '<!--' but found {0}", ch);
        this.current.ScanToEnd(this.sb, "Comment", "-->");
      }
      else if (ch == '[')
      {
        ParseMarkedSection();
      }
      else
      {
        string token = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
        switch (token)
        {
          case "ENTITY":
            ParseEntity();
            break;
          case "ELEMENT":
            ParseElementDecl();
            break;
          case "ATTLIST":
            ParseAttList();
            break;
          default:
            this.current.Error("Invalid declaration '<!{0}'.  Expecting 'ENTITY', 'ELEMENT' or 'ATTLIST'.", token);
            break;
        }
      }
    }

    char ParseDeclComments()
    {
      char ch = this.current.Lastchar;
      while (ch == '-')
      {
        ch = ParseDeclComment(true);
      }
      return ch;
    }

    char ParseDeclComment(bool full)
    {
      int start = this.current.Line;
      // -^-...--
      // This method scans over a comment inside a markup declaration.
      char ch = this.current.ReadChar();
      if (full && ch != '-') this.current.Error("Expecting comment delimiter '--' but found {0}", ch);
      this.current.ScanToEnd(this.sb, "Markup Comment", "--");
      return this.current.SkipWhitespace();
    }

    void ParseMarkedSection()
    {
      // <![^ name [ ... ]]>
      this.current.ReadChar(); // move to next char.
      string name = ScanName("[");
      if (name == "INCLUDE")
      {
        ParseIncludeSection();
      }
      else if (name == "IGNORE")
      {
        ParseIgnoreSection();
      }
      else
      {
        this.current.Error("Unsupported marked section type '{0}'", name);
      }
    }

    void ParseIncludeSection()
    {
      throw new NotImplementedException("Include Section");
    }

    void ParseIgnoreSection()
    {
      int start = this.current.Line;
      // <!-^-...-->
      char ch = this.current.SkipWhitespace();
      if (ch != '[') this.current.Error("Expecting '[' but found {0}", ch);
      this.current.ScanToEnd(this.sb, "Conditional Section", "]]>");
    }

    string ScanName(string term)
    {
      // skip whitespace, scan name (which may be parameter entity reference
      // which is then expanded to a name)
      char ch = this.current.SkipWhitespace();
      if (ch == '%')
      {
        Entity e = ParseParameterEntity(term);
        ch = this.current.Lastchar;
        // bugbug - need to support external and nested parameter entities
        if (!e.Internal) throw new NotSupportedException("External parameter entity resolution");
        return e.Literal.Trim();
      }
      else
      {
        return this.current.ScanToken(this.sb, term, true);
      }
    }

    Entity ParseParameterEntity(string term)
    {
      // almost the same as this.current.ScanToken, except we also terminate on ';'
      char ch = this.current.ReadChar();
      string name = this.current.ScanToken(this.sb, ";" + term, false);
      name = this.nameTable.Add(name);
      if (this.current.Lastchar == ';')
        this.current.ReadChar();
      Entity e = GetParameterEntity(name);
      return e;
    }

    Entity GetParameterEntity(string name)
    {
      Entity e = (Entity)this.pentities[name];
      if (e == null) this.current.Error("Reference to undefined parameter entity '{0}'", name);
      return e;
    }

    static string WhiteSpace = " \r\n\t";

    void ParseEntity()
    {
      char ch = this.current.SkipWhitespace();
      bool pe = (ch == '%');
      if (pe)
      {
        // parameter entity.
        this.current.ReadChar(); // move to next char
        ch = this.current.SkipWhitespace();
      }
      string name = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
      name = this.nameTable.Add(name);
      ch = this.current.SkipWhitespace();
      Entity e = null;
      if (ch == '"' || ch == '\'')
      {
        string literal = this.current.ScanLiteral(this.sb, ch);
        e = new Entity(name, literal);
      }
      else
      {
        string pubid = null;
        string extid = null;
        string tok = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
        if (Entity.IsLiteralType(tok))
        {
          ch = this.current.SkipWhitespace();
          string literal = this.current.ScanLiteral(this.sb, ch);
          e = new Entity(name, literal);
          e.SetLiteralType(tok);
        }
        else
        {
          extid = tok;
          if (extid == "PUBLIC")
          {
            ch = this.current.SkipWhitespace();
            if (ch == '"' || ch == '\'')
            {
              pubid = this.current.ScanLiteral(this.sb, ch);
            }
            else
            {
              this.current.Error("Expecting public identifier literal but found '{0}'", ch);
            }
          }
          else if (extid != "SYSTEM")
          {
            this.current.Error("Invalid external identifier '{0}'.  Expecing 'PUBLIC' or 'SYSTEM'.", extid);
          }
          string uri = null;
          ch = this.current.SkipWhitespace();
          if (ch == '"' || ch == '\'')
          {
            uri = this.current.ScanLiteral(this.sb, ch);
          }
          else if (ch != '>')
          {
            this.current.Error("Expecting system identifier literal but found '{0}'", ch);
          }
          e = new Entity(name, pubid, uri, this.current.Proxy);
        }
      }
      ch = this.current.SkipWhitespace();
      if (ch == '-')
        ch = ParseDeclComments();
      if (ch != '>')
      {
        this.current.Error("Expecting end of entity declaration '>' but found '{0}'", ch);
      }
      if (pe) this.pentities.Add(e.Name, e);
      else this.entities.Add(e.Name, e);
    }

    void ParseElementDecl()
    {
      char ch = this.current.SkipWhitespace();
      string[] names = ParseNameGroup(ch, true);
      ch = Char.ToUpper(this.current.SkipWhitespace());
      bool sto = false;
      bool eto = false;
      if (ch == 'O' || ch == '-')
      {
        sto = (ch == 'O'); // start tag optional?
        this.current.ReadChar();
        ch = Char.ToUpper(this.current.SkipWhitespace());
        if (ch == 'O' || ch == '-')
        {
          eto = (ch == 'O'); // end tag optional?
          ch = this.current.ReadChar();
        }
      }
      ch = this.current.SkipWhitespace();
      ContentModel cm = ParseContentModel(ch);
      ch = this.current.SkipWhitespace();

      string[] exclusions = null;
      string[] inclusions = null;

      if (ch == '-')
      {
        ch = this.current.ReadChar();
        if (ch == '(')
        {
          exclusions = ParseNameGroup(ch, true);
          ch = this.current.SkipWhitespace();
        }
        else if (ch == '-')
        {
          ch = ParseDeclComment(false);
        }
        else
        {
          this.current.Error("Invalid syntax at '{0}'", ch);
        }
      }

      if (ch == '-')
        ch = ParseDeclComments();

      if (ch == '+')
      {
        ch = this.current.ReadChar();
        if (ch != '(')
        {
          this.current.Error("Expecting inclusions name group", ch);
        }
        inclusions = ParseNameGroup(ch, true);
        ch = this.current.SkipWhitespace();
      }

      if (ch == '-')
        ch = ParseDeclComments();


      if (ch != '>')
      {
        this.current.Error("Expecting end of ELEMENT declaration '>' but found '{0}'", ch);
      }

      foreach (string name in names)
      {
        string atom = name.ToUpper();
        atom = this.nameTable.Add(name);
        this.elements.Add(atom, new ElementDecl(atom, sto, eto, cm, inclusions, exclusions));
      }
    }

    static string ngterm = " \r\n\t|,)";
    string[] ParseNameGroup(char ch, bool nmtokens)
    {
      ArrayList names = new ArrayList();
      if (ch == '(')
      {
        ch = this.current.ReadChar();
        ch = this.current.SkipWhitespace();
        while (ch != ')')
        {
          // skip whitespace, scan name (which may be parameter entity reference
          // which is then expanded to a name)
          ch = this.current.SkipWhitespace();
          if (ch == '%')
          {
            Entity e = ParseParameterEntity(SgmlDtd.ngterm);
            PushEntity(this.current.ResolvedUri, e);
            ParseNameList(names, nmtokens);
            PopEntity();
            ch = this.current.Lastchar;
          }
          else
          {
            string token = this.current.ScanToken(this.sb, SgmlDtd.ngterm, nmtokens);
            token = token.ToUpper();
            string atom = this.nameTable.Add(token);
            names.Add(atom);
          }
          ch = this.current.SkipWhitespace();
          if (ch == '|' || ch == ',') ch = this.current.ReadChar();
        }
        this.current.ReadChar(); // consume ')'
      }
      else
      {
        string name = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, nmtokens);
        name = name.ToUpper();
        name = this.nameTable.Add(name);
        names.Add(name);
      }
      return (string[])names.ToArray(typeof(String));
    }

    void ParseNameList(ArrayList names, bool nmtokens)
    {
      char ch = this.current.Lastchar;
      ch = this.current.SkipWhitespace();
      while (ch != Entity.EOF)
      {
        string name;
        if (ch == '%')
        {
          Entity e = ParseParameterEntity(SgmlDtd.ngterm);
          PushEntity(this.current.ResolvedUri, e);
          ParseNameList(names, nmtokens);
          PopEntity();
          ch = this.current.Lastchar;
        }
        else
        {
          name = this.current.ScanToken(this.sb, SgmlDtd.ngterm, true);
          name = name.ToUpper();
          name = this.nameTable.Add(name);
          names.Add(name);
        }
        ch = this.current.SkipWhitespace();
        if (ch == '|')
        {
          ch = this.current.ReadChar();
          ch = this.current.SkipWhitespace();
        }
      }
    }

    static string dcterm = " \r\n\t>";
    ContentModel ParseContentModel(char ch)
    {
      ContentModel cm = new ContentModel();
      if (ch == '(')
      {
        this.current.ReadChar();
        ParseModel(')', cm);
        ch = this.current.ReadChar();
        if (ch == '?' || ch == '+' || ch == '*')
        {
          cm.AddOccurrence(ch);
          this.current.ReadChar();
        }
      }
      else if (ch == '%')
      {
        Entity e = ParseParameterEntity(SgmlDtd.dcterm);
        PushEntity(this.current.ResolvedUri, e);
        cm = ParseContentModel(this.current.Lastchar);
        PopEntity(); // bugbug should be at EOF.
      }
      else
      {
        string dc = ScanName(SgmlDtd.dcterm);
        cm.SetDeclaredContent(dc);
      }
      return cm;
    }

    static string cmterm = " \r\n\t,&|()?+*";
    void ParseModel(char cmt, ContentModel cm)
    {
      // Called when part of the model is made up of the contents of a parameter entity
      int depth = cm.CurrentDepth;
      char ch = this.current.Lastchar;
      ch = this.current.SkipWhitespace();
      while (ch != cmt || cm.CurrentDepth > depth) // the entity must terminate while inside the content model.
      {
        if (ch == Entity.EOF)
        {
          this.current.Error("Content Model was not closed");
        }
        if (ch == '%')
        {
          Entity e = ParseParameterEntity(SgmlDtd.cmterm);
          PushEntity(this.current.ResolvedUri, e);
          ParseModel(Entity.EOF, cm);
          PopEntity();
          ch = this.current.SkipWhitespace();
        }
        else if (ch == '(')
        {
          cm.PushGroup();
          this.current.ReadChar();// consume '('
          ch = this.current.SkipWhitespace();
        }
        else if (ch == ')')
        {
          ch = this.current.ReadChar();// consume ')'
          if (ch == '*' || ch == '+' || ch == '?')
          {
            cm.AddOccurrence(ch);
            ch = this.current.ReadChar();
          }
          if (cm.PopGroup() < depth)
          {
            this.current.Error("Parameter entity cannot close a paren outside it's own scope");
          }
          ch = this.current.SkipWhitespace();
        }
        else if (ch == ',' || ch == '|' || ch == '&')
        {
          cm.AddConnector(ch);
          this.current.ReadChar(); // skip connector
          ch = this.current.SkipWhitespace();
        }
        else
        {
          string token;
          if (ch == '#')
          {
            ch = this.current.ReadChar();
            token = "#" + this.current.ScanToken(this.sb, SgmlDtd.cmterm, true); // since '#' is not a valid name character.
          }
          else
          {
            token = this.current.ScanToken(this.sb, SgmlDtd.cmterm, true);
          }
          token = token.ToUpper();
          token = this.nameTable.Add(token);// atomize it.
          ch = this.current.Lastchar;
          if (ch == '?' || ch == '+' || ch == '*')
          {
            cm.PushGroup();
            cm.AddSymbol(token);
            cm.AddOccurrence(ch);
            cm.PopGroup();
            this.current.ReadChar(); // skip connector
            ch = this.current.SkipWhitespace();
          }
          else
          {
            cm.AddSymbol(token);
            ch = this.current.SkipWhitespace();
          }
        }
      }
    }

    void ParseAttList()
    {
      char ch = this.current.SkipWhitespace();
      string[] names = ParseNameGroup(ch, true);
      AttList attlist = new AttList();
      ParseAttList(attlist, '>');
      foreach (string name in names)
      {
        ElementDecl e = (ElementDecl)this.elements[name];
        if (e == null)
        {
          this.current.Error("ATTLIST references undefined ELEMENT {0}", name);
        }
        e.AddAttDefs(attlist);
      }
    }

    static string peterm = " \t\r\n>";
    void ParseAttList(AttList list, char term)
    {
      char ch = this.current.SkipWhitespace();
      while (ch != term)
      {
        if (ch == '%')
        {
          Entity e = ParseParameterEntity(SgmlDtd.peterm);
          PushEntity(this.current.ResolvedUri, e);
          ParseAttList(list, Entity.EOF);
          PopEntity();
          ch = this.current.SkipWhitespace();
        }
        else if (ch == '-')
        {
          ch = ParseDeclComments();
        }
        else
        {
          AttDef a = ParseAttDef(ch);
          list.Add(a);
        }
        ch = this.current.SkipWhitespace();
      }
    }

    AttDef ParseAttDef(char ch)
    {
      ch = this.current.SkipWhitespace();
      string name = ScanName(SgmlDtd.WhiteSpace);
      name = name.ToUpper();
      name = this.nameTable.Add(name);
      AttDef attdef = new AttDef(name);

      ch = this.current.SkipWhitespace();
      if (ch == '-')
        ch = ParseDeclComments();

      ParseAttType(ch, attdef);

      ch = this.current.SkipWhitespace();
      if (ch == '-')
        ch = ParseDeclComments();

      ParseAttDefault(ch, attdef);

      ch = this.current.SkipWhitespace();
      if (ch == '-')
        ch = ParseDeclComments();

      return attdef;

    }

    void ParseAttType(char ch, AttDef attdef)
    {
      if (ch == '%')
      {
        Entity e = ParseParameterEntity(SgmlDtd.WhiteSpace);
        PushEntity(this.current.ResolvedUri, e);
        ParseAttType(this.current.Lastchar, attdef);
        PopEntity(); // bugbug - are we at the end of the entity?
        ch = this.current.Lastchar;
        return;
      }

      if (ch == '(')
      {
        attdef.EnumValues = ParseNameGroup(ch, false);
        attdef.Type = AttributeType.ENUMERATION;
      }
      else
      {
        string token = ScanName(SgmlDtd.WhiteSpace);
        if (token == "NOTATION")
        {
          ch = this.current.SkipWhitespace();
          if (ch != '(')
          {
            this.current.Error("Expecting name group '(', but found '{0}'", ch);
          }
          attdef.Type = AttributeType.NOTATION;
          attdef.EnumValues = ParseNameGroup(ch, true);
        }
        else
        {
          attdef.SetType(token);
        }
      }
    }

    void ParseAttDefault(char ch, AttDef attdef)
    {
      if (ch == '%')
      {
        Entity e = ParseParameterEntity(SgmlDtd.WhiteSpace);
        PushEntity(this.current.ResolvedUri, e);
        ParseAttDefault(this.current.Lastchar, attdef);
        PopEntity(); // bugbug - are we at the end of the entity?
        ch = this.current.Lastchar;
        return;
      }

      bool hasdef = true;
      if (ch == '#')
      {
        this.current.ReadChar();
        string token = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, true);
        hasdef = attdef.SetPresence(token);
        ch = this.current.SkipWhitespace();
      }
      if (hasdef)
      {
        if (ch == '\'' || ch == '"')
        {
          string lit = this.current.ScanLiteral(this.sb, ch);
          attdef.Default = lit;
          ch = this.current.SkipWhitespace();
        }
        else
        {
          string name = this.current.ScanToken(this.sb, SgmlDtd.WhiteSpace, false);
          name = name.ToUpper();
          name = this.nameTable.Add(name);
          attdef.Default = name; // bugbug - must be one of the enumerated names.
          ch = this.current.SkipWhitespace();
        }
      }
    }
  }

  class StringUtilities
  {
    public static bool EqualsIgnoreCase(string a, string b)
    {
      return string.Compare(a, b, true, CultureInfo.InvariantCulture) == 0;
    }
  }
}

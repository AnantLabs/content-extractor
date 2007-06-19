//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ComponentModel;
//using System.Security.Cryptography;
//using MetaTech.Library;
//using System.IO;
//using System.Xml.Serialization;
//using System.Reflection;
//using System.Xml;
//
//namespace ContentExtractor.Core
//{
//  class CoreLicenseProvider : LicenseProvider
//  {
//    private XmlSerializer _serializer = null;
//    public XmlSerializer Serializer
//    {
//      get
//      {
//        if (_serializer == null)
//          _serializer = new XmlSerializer(typeof(CoreLicense.SignedData));
//        return _serializer;
//      }
//    }
//
//    public override System.ComponentModel.License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
//    {
//      if (context.UsageMode == LicenseUsageMode.Runtime)
//      {
//        try
//        {
//          if (File.Exists(CoreLicense.FileName))
//          {
//            using (StreamReader reader = new StreamReader(CoreLicense.FileName))
//            using (XmlReader xmlReader = XmlReader.Create(reader))
//            {
//              if (Serializer.CanDeserialize(xmlReader))
//              {
//                CoreLicense.SignedData signed = (CoreLicense.SignedData)Serializer.Deserialize(xmlReader);
//
//                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
//                rsa.FromXmlString(Properties.Resources.PublicKey);
//
//                if (CoreLicense.VerifySign<CoreLicense.Persist>(rsa, signed.Data, signed.Sign))
//                  return new CoreLicense(Convert.ToBase64String(signed.Sign), signed.Data); ;
//              }
//            }
//          }
//        }
//        catch
//        { }
//        if (!allowExceptions)
//          return null;
//        else
//          throw new LicenseException(type);
//      }
//      else
//        return null;
//    }
//
//    private byte[] GetLicenseBytes()
//    {
//      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CoreLicense.LicenseFileName);
//      if (!File.Exists(path))
//      {
//        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CoreLicense.LicenseFileName);
//        if (!File.Exists(path))
//          path = string.Empty;
//      }
//      if (File.Exists(path))
//        return File.ReadAllBytes(path);
//      else
//        return new byte[0];
//    }
//  }
//
//  public class CoreLicense : System.ComponentModel.License
//  {
//    [XmlRoot("License")]
//    public class SignedData
//    {
//      [XmlElement("Data")]
//      public Persist Data;
//      public byte[] Sign;
//    }
//
//    public struct Persist
//    {
//      public string Name;
//      public DateTime RegisterDate;
//    }
//
//    public const string LicenseFileName = "license";
//
//    internal CoreLicense(string key, Persist persist)
//    {
//      this.key = key;
//      this.persist = persist;
//    }
//    private string key;
//    public readonly Persist persist;
//
//    public override string LicenseKey
//    {
//      get
//      {
//        return key;
//      }
//    }
//
//    public override void Dispose()
//    {
//    }
//
//    private static MD5CryptoServiceProvider _Md5;
//    public static MD5CryptoServiceProvider Md5
//    {
//      get
//      {
//        if (_Md5 == null)
//        {
//          _Md5 = new MD5CryptoServiceProvider();
//        }
//        return _Md5;
//      }
//    }
//
//    internal static string FileName
//    {
//      get
//      {
//
//        bool forAllUsers = true;
//        string dir = forAllUsers ? System.Windows.Forms.Application.CommonAppDataPath : System.Windows.Forms.Application.LocalUserAppDataPath;
//        string path = Path.Combine(dir, CoreLicense.LicenseFileName);
//        return path;
//      }
//    }
//
//    public static byte[] CalculateSign<T>(RSACryptoServiceProvider privateKey, T value)
//    {
//      string text = XmlSerialization2.SaveToText<T>(value);
//      byte[] hash = Md5.ComputeHash(Encoding.UTF8.GetBytes(text));
//      RSAPKCS1SignatureFormatter sf = new RSAPKCS1SignatureFormatter(privateKey);
//      sf.SetHashAlgorithm("MD5");
//      return sf.CreateSignature(hash);
//    }
//
//    internal static bool VerifySign<T1>(RSACryptoServiceProvider publicKey, T1 data, byte[] sign)
//    {
//      string text = XmlSerialization2.SaveToText<T1>(data);
//      byte[] hash = Md5.ComputeHash(Encoding.UTF8.GetBytes(text));
//      RSAPKCS1SignatureDeformatter sf = new RSAPKCS1SignatureDeformatter(publicKey);
//      sf.SetHashAlgorithm("MD5");
//      return sf.VerifySignature(hash, sign);
//    }
//  }
//
//  class CoreLicenseContext : LicenseContext
//  {
//    static CoreLicenseContext()
//    {
//      InitContext();
//    }
//
//    public static void InitContext()
//    {
//      LicenseManager.CurrentContext = CoreLicenseContext.Default;
//    }
//
//    private static CoreLicenseContext _defaultContext = null;
//    public static CoreLicenseContext Default
//    {
//      get
//      {
//        if (_defaultContext == null)
//          _defaultContext = new CoreLicenseContext();
//        return _defaultContext;
//      }
//    }
//
//    public CoreLicenseContext()
//      : base()
//    {
//
//    }
//
//
//    public override void SetSavedLicenseKey(Type type, string key)
//    {
//      if (type == typeof(Model))
//      {
//        File.WriteAllBytes(CoreLicense.FileName, Convert.FromBase64String(key));
//      }
//      else
//        base.SetSavedLicenseKey(type, key);
//    }
//    public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
//    {
//      if (type == typeof(Model))
//      {
//        if (File.Exists(CoreLicense.FileName))
//          return Convert.ToBase64String(File.ReadAllBytes(CoreLicense.FileName));
//        else
//          return string.Empty;
//      }
//      else
//        return base.GetSavedLicenseKey(type, resourceAssembly);
//    }
//  }
//
//}

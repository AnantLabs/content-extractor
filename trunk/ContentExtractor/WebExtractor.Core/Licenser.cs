//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using System.Security.Cryptography;
//using MetaTech.Library;
//
//namespace ContentExtractor.Core
//{
//  public class LicenseRegistrator
//  {
//    public static void LicenseFromFile(string fileName)
//    {
//      if (File.Exists(fileName))
//        LicenseFromBytes(File.ReadAllBytes(fileName));
//    }
//
//    public static void LicenseFromBytes(byte[] license)
//    {
//      LicenseFromBytes(license, false);
//    }
//
//    private static void LicenseFromBytes(byte[] license, bool forAllUsers)
//    {
//      Environment.SpecialFolder folder = forAllUsers ? Environment.SpecialFolder.CommonApplicationData : Environment.SpecialFolder.ApplicationData;
//      string path = Path.Combine(Environment.GetFolderPath(folder), CoreLicense.LicenseFileName);
//    }
//  }
//}

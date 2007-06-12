using System;

namespace NineRays.Decompiler
{
	/// <summary>
	/// This attribute you can use in your assemblies to prevent from Spices.Net Decompiler decompilation 
	/// You can attach this attribute to type(class) or to entire assembly
	/// Simply attach this attribute to assembly attributes (usually located in AssemblyInfo.cs) as following:
	/// [assembly: NineRays.Decompiler.NotDecompile]
	/// Warning: If you use antiILDASM do not include and attach this attribute to your assembly
	///          In this case Spices.Obfuscator embeds this attribute and attaches to assembly 
	///          attributes automatically
	/// </summary>
	//Do not use this attribute - Spices.Obfuscator automatically attach this attribute to obfuscated assemblies
	[AttributeUsage(AttributeTargets.Assembly)]
	public class NotDecompile : Attribute
	{}
}

namespace NineRays.Obfuscator
{
	/// <summary>
	/// Attribute to disable string encryption on method or constructor
	/// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
	public class DontEncryptStringsAttribute : Attribute
	{
    public DontEncryptStringsAttribute(){}
    
	}

  /// <summary>
  /// Attribute to disable replacing method by stub on method or constructor
  /// </summary>
  //You can change visibility of this class to internal to limit visiblity of this class by single assembly
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
  public class DontStubAttribute : Attribute
  {
    public DontStubAttribute(){}
    
  }

  /// <summary>
  /// Attribute to disable anonymizing on method or constructor
  /// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
  public class DontAnonymizeAttribute : Attribute
  {
    public DontAnonymizeAttribute(){}    
  }

  /// <summary>
	/// Attribute to define namespace of the obfuscated type
	/// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Struct | AttributeTargets.Enum)]
	public class SpecialNamespaceAttribute : Attribute
	{
		private string name = null;
		public SpecialNamespaceAttribute(string Name)
		{
			this.name = Name;
		}
		public string Name
		{
			get 
			{
				return name;
			}
		}
	}

	/// <summary>
	/// Attribute to define name of the obfuscated type
	/// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
	public class SpecialNameAttribute : Attribute
	{
		private string name = null;
		public SpecialNameAttribute(string Name)
		{
			this.name = Name;
		}
		public string Name
		{
			get 
			{
				return name;
			}
		}
	}

	/// <summary>
	/// Attribute to include member into obfuscation
	/// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
	public class ObfuscateAttribute : Attribute
	{
		public ObfuscateAttribute(){}
	}

	/// <summary>
	/// Attribute to exclude member from obfuscation
	/// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
	public class NotObfuscateAttribute : Attribute
	{
		public NotObfuscateAttribute(){}
	}

	/// <summary>
	/// Allows to exclude from obfuscation classes that matches attribute pattern string
	/// Note: pattern string will be used for analysis only on the assembly, that attribute attached
	/// Analysis based on member fully qualified name 
	/// Example: [assembly:NineRays.Obfuscator.NotObfuscateMembersAttribute("MyCompany.Fo*")]
	///  - will be excluded all types with names beginning MyCompany.Fo (MyCompany.Foo, MyCompany.ForEachEnumerator) and his members (methods, fields, properties) 
	/// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class NotObfuscateMembersAttribute : Attribute
	{
		private string pattern = string.Empty;
		public NotObfuscateMembersAttribute(string pattern)
		{
			this.pattern = pattern;
		}
		public string Pattern
		{
			get 
			{
				return pattern;
			}
		}
	}  

  /// <summary>
  /// Allows to include into obfuscation classes and their members that matches attribute pattern string
  /// Note: pattern string will be used for analysis only on the assembly, that attribute attached
  /// Analysis based on member fully qualified name 
  /// Example: [assembly:NineRays.Obfuscator.ObfuscateMembersAttribute("MyCompany.Fo*")]
  ///  - will be included types and their members with names beginning MyCompany.Fo (MyCompany.Foo, MyCompany.ForEachEnumerator) and his members (methods, fields, properties) 
  /// </summary>
	//You can change visibility of this class to internal to limit visiblity of this class by single assembly
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
  public class ObfuscateMembersAttribute : Attribute
  {
    private string pattern = string.Empty;
    public ObfuscateMembersAttribute(string pattern)
    {
      this.pattern = pattern;
    }
    public string Pattern
    {
      get 
      {
        return pattern;
      }
    }
  }  

}

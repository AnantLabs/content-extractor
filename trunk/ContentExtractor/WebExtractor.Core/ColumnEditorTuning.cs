using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ContentExtractor.Core;

namespace ContentExtractor.Core
{
  internal class FunctionTypeConverter : StringConverter
  {
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }
    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }
    public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return new StandardValuesCollection(Functions.AllFunctions);
    }
  }
}

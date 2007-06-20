package MetaTech.Library
{
  public class JScriptExecutor
  {
    function Execute(template, s:String)
    {
      return eval(s, "unsafe");
    }
    function CalculateValue(template, result, s:String) 
    {
      return eval(s,"unsafe");
    }
  }
}


namespace BrawlLib.Internal.Reflection
{
    public static class MethodInfoExtension
    {
        //public static object InvokeBlind(this MethodInfo info, object target, string[] argStrings)
        //{
        //    ParameterInfo[] pArr = info.GetParameters();
        //    object[] args = new object[pArr.Length];
        //    for (int i = 0; i < pArr.Length; i++)
        //    {
        //        ParameterInfo p = pArr[i];
        //        //find name in strings
        //        string name = String.Format("/{0}=", p.Name);
        //        foreach (string s in argStrings)
        //        {
        //            if (s.StartsWith(name, StringComparison.OrdinalIgnoreCase))
        //            {
        //                args[i] = 
        //            }
        //        }
        //    }
        //}
        //public static object InvokeBlind(this MethodInfo info, Dictionary<string, object> namedParams, params object[] orderedParams)
        //{
        //}
    }
}
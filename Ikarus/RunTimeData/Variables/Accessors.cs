using System;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using Ikarus.MovesetFile;
using Ikarus.ModelViewer;

namespace Ikarus
{
    public static unsafe partial class IC
    {
        static IC()
        {
            var m = typeof(IC).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
            foreach (MethodInfo method in m)
            {
                Type t = method.ReturnType;
                string s = method.Name.Substring(1);

                int i;
                if (!int.TryParse(s, System.Globalization.NumberStyles.Number, CultureInfo.InvariantCulture, out i))
                    continue;

                if (t == typeof(bool))
                    Bit.Add(i, (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), method));
                else if (t == typeof(int))
                    Basic.Add(i, (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), method));
                else if (t == typeof(float))
                    Float.Add(i, (Func<float>)Delegate.CreateDelegate(typeof(Func<float>), method));
            }
        }

        public static VarFuncHolder<float> Float = new VarFuncHolder<float>("IC");
        public static VarFuncHolder<int> Basic = new VarFuncHolder<int>("IC");
        public static VarFuncHolder<bool> Bit = new VarFuncHolder<bool>("IC");

        public static void ClearAll()
        {
            Float.Clear();
            Basic.Clear();
            Bit.Clear();
        }

        public static float Get(VariableType var, int num)
        {
            switch (var)
            {
                case VariableType.Basic: return Basic[num];
                case VariableType.Float: return Float[num];
                case VariableType.Bit: return Bit[num] ? 1 : 0;
            }
            return 0;
        }

    }
    public static unsafe class RA
    {
        public static VarHolder<float> Float = new VarHolder<float>("RA");
        public static VarHolder<int> Basic = new VarHolder<int>("RA");
        public static VarHolder<bool> Bit = new VarHolder<bool>("RA");

        public static void ClearAll()
        {
            Float.Clear();
            Basic.Clear();
            Bit.Clear();
        }

        public static float Get(VariableType var, int num)
        {
            switch (var)
            {
                case VariableType.Basic: return Basic[num];
                case VariableType.Float: return Float[num];
                case VariableType.Bit: return Bit[num] ? 1 : 0;
            }
            return 0;
        }
        public static void Set(VariableType var, int num, double value)
        {
            switch (var)
            {
                case VariableType.Basic:
                    Basic[num] = (int)(value + 0.5f);
                    break;
                case VariableType.Float:
                    Float[num] = (float)value;
                    break;
                case VariableType.Bit:
                    Bit[num] = value == 0 ? false : true;
                    break;
            }
        }
    }
    public static unsafe class LA
    {
        public static VarHolder<float> Float = new VarHolder<float>("LA");
        public static VarHolder<int> Basic = new VarHolder<int>("LA");
        public static VarHolder<bool> Bit = new VarHolder<bool>("LA");

        public static void ClearAll()
        {
            Float.Clear();
            Basic.Clear();
            Bit.Clear();
        }

        public static float Get(VariableType var, int num)
        {
            switch (var)
            {
                case VariableType.Basic: return Basic[num];
                case VariableType.Float: return Float[num];
                case VariableType.Bit: return Bit[num] ? 1 : 0;
            }
            return 0;
        }
        public static void Set(VariableType var, int num, double value)
        {
            switch (var)
            {
                case VariableType.Basic:
                    Basic[num] = (int)(value + 0.5f);
                    break;
                case VariableType.Float:
                    Float[num] = (float)value;
                    break;
                case VariableType.Bit:
                    Bit[num] = value == 0.0f ? false : true;
                    break;
            }
        }
    }

    public class VarHolder<T> : Dictionary<int, T> where T : IConvertible
    {
        public VarHolder(string name) : base() { _name = name; }
        private string _name;

        private bool _logCalls = true;
        public bool LogCalls { get { return _logCalls; } set { _logCalls = value; } }

        public new T this[int id]
        {
            get
            {
                bool contains = ContainsKey(id);
                T value = contains ? base[id] : default(T);

                if (LogCalls)
                    LogCall(value, id, false);

                return value;
            }
            set
            {
                base[id] = value;

                if (LogCalls)
                    LogCall(value, id, true);
            }
        }
        private unsafe void LogCall(T value, int id, bool set)
        {
            int type = value is float ? 0 : value is int ? 1 : value is bool ? 2 : 3;
            string var = "";
            string val = "";
            switch (type)
            {
                case 0:
                    var = "Float";
                    val = value.ToSingle(CultureInfo.InvariantCulture).ToString();
                    break;
                case 1:
                    var = "Basic";
                    val = value.ToInt32(CultureInfo.InvariantCulture).ToString();
                    break;
                case 2:
                    var = "Bit";
                    val = value.ToBoolean(CultureInfo.InvariantCulture).ToString();
                    break;
                default:
                    return;
            }
            RunTime.Log(String.Format("{0}.{1}[{2}] {3} {4}", _name, var, id, set ? "set to" : "returned", val.ToString().ToLower()));
        }
    }
    public class VarFuncHolder<T> : Dictionary<int, Func<T>> where T : IConvertible
    {
        public VarFuncHolder(string name) : base() { _name = name; }
        private string _name;

        private bool _logCalls = true;
        public bool LogCalls { get { return _logCalls; } set { _logCalls = value; } }

        public new T this[int id]
        {
            get
            {
                bool contains = ContainsKey(id);
                T value = contains ? base[id]() : default(T);

                if (LogCalls)
                    LogCall(value, id, contains);

                return value;
            }
        }
        private unsafe void LogCall(T value, int id, bool contains)
        {
            int type = value is float ? 0 : value is int ? 1 : value is bool ? 2 : 3;
            string var = "";
            string val = "";
            switch (type)
            {
                case 0:
                    var = "Float";
                    val = value.ToSingle(CultureInfo.InvariantCulture).ToString();
                    break;
                case 1:
                    var = "Basic";
                    val = value.ToInt32(CultureInfo.InvariantCulture).ToString();
                    break;
                case 2:
                    var = "Bit";
                    val = value.ToBoolean(CultureInfo.InvariantCulture).ToString();
                    break;
                default:
                    return;
            }
            RunTime.Log(String.Format("{0}.{1}[{2}] returned {3} {4}", _name, var, id, val, contains ? "" : "(This variable is not supported)"));
        }
    }
}
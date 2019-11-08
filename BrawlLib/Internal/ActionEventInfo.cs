using System;
using System.Collections.Generic;

namespace BrawlLib.Internal
{
    public class ActionEventInfo
    {
        public ActionEventInfo()
        {
            defaultParams = new long[0];
            _syntax = "";
        }

        public ActionEventInfo(long id, string name, string description, string[] paramNames, string[] paramDesc)
        {
            idNumber = id;
            _name = name;
            _description = description;
            _syntax = "";
            Params = paramNames;
            pDescs = paramDesc;
            defaultParams = new long[0];
            Enums = new Dictionary<int, List<string>>();
        }

        public ActionEventInfo(long id, string name, string description, string[] paramNames, string[] paramDesc,
                               string syntax, long[] dfltParams)
        {
            idNumber = id;
            _name = name;
            _description = description;
            _syntax = syntax;
            Params = paramNames;
            pDescs = paramDesc;
            defaultParams = dfltParams;
            Enums = new Dictionary<int, List<string>>();
        }

        public void SetDfltParameters(string s)
        {
            if (s == null)
            {
                return;
            }

            Array.Resize(ref defaultParams, s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                try
                {
                    defaultParams[i] = long.Parse(s.Substring(i, 1));
                }
                catch
                {
                    defaultParams[i] = 0;
                }
            }
        }

        public long GetDfltParameter(int i)
        {
            if (i >= defaultParams.Length)
            {
                return 0;
            }

            if (defaultParams[i] > 6)
            {
                return 0;
            }

            return defaultParams[i];
        }

        public override string ToString()
        {
            return _name;
        }

        public long idNumber;
        public string _name;
        public string _description;
        public string _syntax;

        public string[] Params;
        public string[] pDescs;

        public long[] defaultParams;

        public Dictionary<int, List<string>> Enums;
    }
}
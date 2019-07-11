using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlManagerLib
{
    public class AlternateStageEntry
    {
        public class Alternate
        {
            public ushort ButtonMask { get; set; }
            public char Letter { get; set; }
        }

        public IEnumerable<Alternate> Random { get; set; }
        public IEnumerable<Alternate> ButtonActivated { get; set; }
    }
}
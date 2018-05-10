using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLib
{
    public class FactionSystemInf
    {
        public string systemName { get; set; }
        public double influence { get; set; }
        public string factionAboveName { get; set; } = "";
        public double factionAboveInf { get; set; } = 0;
        public string factionBelowName { get; set; } = "";
        public double factionBelowInf { get; set; } = 0;
        public DateTime LastUpdate { get; set; }
        public bool DataIsOld { get; set; }
        public int TooClose { get; set; } = 0;
    }
}

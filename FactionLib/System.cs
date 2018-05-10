using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLibSystem
{
    public class Faction
    {
        public string name_lower { get; set; }
        public string name { get; set; }
    }

    public class Doc
    {
        public string _id { get; set; }
        public string name_lower { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public int __v { get; set; }
        public string name { get; set; }
        public string government { get; set; }
        public string allegiance { get; set; }
        public string state { get; set; }
        public string security { get; set; }
        public int population { get; set; }
        public string primary_economy { get; set; }
        public string controlling_minor_faction { get; set; }
        public DateTime updated_at { get; set; }
        public int eddb_id { get; set; }
        public IList<Faction> factions { get; set; }
    }

    public class System
    {
        public IList<Doc> docs { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactionLibFaction
{
    public class RecoveringState
    {
        public string state { get; set; }
        public int trend { get; set; }
    }

    public class PendingState
    {
        public string state { get; set; }
        public int trend { get; set; }
    }

    public class FactionPresence
    {
        public string system_name { get; set; }
        public string system_name_lower { get; set; }
        public string state { get; set; }
        public double influence { get; set; }
        public IList<RecoveringState> recovering_states { get; set; }
        public IList<PendingState> pending_states { get; set; }
    }

    public class Doc
    {
        public string _id { get; set; }
        public string name { get; set; }
        public int __v { get; set; }
        public string name_lower { get; set; }
        public DateTime updated_at { get; set; }
        public string government { get; set; }
        public string allegiance { get; set; }
        public int eddb_id { get; set; }
        public IList<FactionPresence> faction_presence { get; set; }
    }

    public class Faction
    {
        public IList<Doc> docs { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }
}

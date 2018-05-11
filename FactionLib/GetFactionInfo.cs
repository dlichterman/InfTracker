using FactionLibFaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FactionLib
{
    public class GetFactionInfo
    {
        const string baseURL = "https://elitebgs.kodeblox.com/api/ebgs/v4";
        const string factionURL = "/factions?name=";
        const string systemURL = "/systems?name="; 

        public async Task<List<FactionSystemInf>> GetFactionList(string factionName, IProgress<int> progress)
        { 
            List<FactionSystemInf> lst = new List<FactionSystemInf>();
            var jsonBase = await new WebClient().DownloadStringTaskAsync(baseURL + factionURL + Uri.EscapeDataString(factionName));
            FactionLibFaction.Faction mainFaction = JsonConvert.DeserializeObject<FactionLibFaction.Faction>(jsonBase);
            string facName = mainFaction.docs[0].name;
            foreach(FactionPresence systems in mainFaction.docs[0].faction_presence)
            {
                FactionSystemInf fsi = new FactionSystemInf();
                fsi.systemName = systems.system_name;
                fsi.influence = systems.influence;
                lst.Add(fsi);
            }

            int tickCutoff = 14;
            DateTime dtCutoff;

            if (DateTime.UtcNow > new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, tickCutoff, 0, 0, DateTimeKind.Utc))
            {
                dtCutoff = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, tickCutoff, 0, 0, DateTimeKind.Utc);
            }
            else
            {
                dtCutoff = new DateTime(DateTime.UtcNow.AddDays(-1).Year, DateTime.UtcNow.AddDays(-1).Month, DateTime.UtcNow.AddDays(-1).Day, tickCutoff, 0, 0, DateTimeKind.Utc);
            }

            List<FactionLibFaction.Faction> factionCache = new List<Faction>();
            int i = 0;
            foreach(FactionSystemInf systemToSearch in lst)
            {
                //Grab all factions in said system
                var jsonSys = await new WebClient().DownloadStringTaskAsync(baseURL + systemURL + Uri.EscapeDataString(systemToSearch.systemName));
                FactionLibSystem.System sys = JsonConvert.DeserializeObject<FactionLibSystem.System>(jsonSys);

                systemToSearch.LastUpdate = sys.docs[0].updated_at;

                //Grab each faction and find influence and compare
                foreach(FactionLibSystem.Faction fac in sys.docs[0].factions)
                {
                    if (fac.name != facName)
                    {
                        //first look for cached data
                        Faction searchedFaction = factionCache.Find(p => p.docs[0].name == fac.name);
                        if (searchedFaction != null)
                        {
                            foreach (FactionPresence factionSearched in searchedFaction.docs[0].faction_presence)
                            {
                                if (systemToSearch.systemName == factionSearched.system_name)
                                {
                                    if (factionSearched.influence >= systemToSearch.influence && ((systemToSearch.factionAboveInf == 0) || (factionSearched.influence < systemToSearch.factionAboveInf)))
                                    {
                                        systemToSearch.factionAboveInf = factionSearched.influence;
                                        systemToSearch.factionAboveName = fac.name;
                                    }
                                    else if (factionSearched.influence < systemToSearch.influence && ((systemToSearch.factionBelowInf == 0) || (factionSearched.influence > systemToSearch.factionBelowInf)))
                                    {
                                        systemToSearch.factionBelowInf = factionSearched.influence;
                                        systemToSearch.factionBelowName = fac.name;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var jsonFac = await new WebClient().DownloadStringTaskAsync(baseURL + factionURL + Uri.EscapeDataString(fac.name));
                            FactionLibFaction.Faction finf = JsonConvert.DeserializeObject<FactionLibFaction.Faction>(jsonFac);
                            foreach (FactionPresence factionSearched in finf.docs[0].faction_presence)
                            {
                                if (systemToSearch.systemName == factionSearched.system_name)
                                {
                                    if (factionSearched.influence >= systemToSearch.influence && ((systemToSearch.factionAboveInf == 0) || (factionSearched.influence < systemToSearch.factionAboveInf)))
                                    {
                                        systemToSearch.factionAboveInf = factionSearched.influence;
                                        systemToSearch.factionAboveName = fac.name;
                                    }
                                    else if (factionSearched.influence < systemToSearch.influence && ((systemToSearch.factionBelowInf == 0) || (factionSearched.influence > systemToSearch.factionBelowInf)))
                                    {
                                        systemToSearch.factionBelowInf = factionSearched.influence;
                                        systemToSearch.factionBelowName = fac.name;
                                    }
                                }
                            }
                            factionCache.Add(finf);
                        }
                    }

                }
                i++;

                //update progress
                progress.Report(i*100 / lst.Count());
            }

            foreach (FactionSystemInf facSysInf in lst)
            {
                facSysInf.influence = facSysInf.influence * 100.0000;
                facSysInf.factionBelowInf = facSysInf.factionBelowInf * 100.0000;
                facSysInf.factionAboveInf = facSysInf.factionAboveInf * 100.0000;

                if(
                  ((facSysInf.factionAboveInf > 0) && (facSysInf.factionAboveInf- facSysInf.influence <= 3))
                  ||
                  ((facSysInf.factionBelowInf > 0) && (facSysInf.influence - facSysInf.factionBelowInf <= 3))
                   )
                {
                    facSysInf.TooClose = 1;
                }
                else if (
                    ((facSysInf.factionAboveInf > 0) && (facSysInf.factionAboveInf - facSysInf.influence <= 5))
                    ||
                    ((facSysInf.factionBelowInf > 0) && (facSysInf.influence - facSysInf.factionBelowInf <= 5))
                   )
                {
                    facSysInf.TooClose = 2;
                }

                if (facSysInf.LastUpdate < dtCutoff)
                {
                    facSysInf.DataIsOld = true;
                }

            }
            lst = lst.OrderBy(p => p.systemName).ToList();
            return lst;
        }
        
    }
}

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
            var jsonBase = new WebClient().DownloadString(baseURL + factionURL + Uri.EscapeDataString(factionName));
            FactionLibFaction.Faction f = JsonConvert.DeserializeObject<FactionLibFaction.Faction>(jsonBase);
            string factionLowercase = f.docs[0].name;
            List<string> lstOfSys = new List<string>();
            foreach(FactionPresence ff in f.docs[0].faction_presence)
            {
                lstOfSys.Add(ff.system_name);
                FactionSystemInf fsi = new FactionSystemInf();
                fsi.systemName = ff.system_name;
                fsi.influence = ff.influence;
                
                lst.Add(fsi);
            }

            List<FactionLibFaction.Faction> facCache = new List<Faction>();
            int i = 0;
            foreach(FactionSystemInf ff in lst)
            {
                //Grab all factions in said system
                var jsonSys = await new WebClient().DownloadStringTaskAsync(baseURL + systemURL + Uri.EscapeDataString(ff.systemName));
                FactionLibSystem.System s = JsonConvert.DeserializeObject<FactionLibSystem.System>(jsonSys);

                ff.LastUpdate = s.docs[0].updated_at;

                //Grab each faction and find influence and compare
                foreach(FactionLibSystem.Faction fac in s.docs[0].factions)
                {
                    if (fac.name != factionLowercase)
                    {
                        //first look for cached data
                        Faction ffffffffff = facCache.Find(p => p.docs[0].name == fac.name);
                        if (ffffffffff != null)
                        {
                            foreach (FactionPresence IveRunOutOfVars in ffffffffff.docs[0].faction_presence)
                            {
                                if (ff.systemName == IveRunOutOfVars.system_name)
                                {
                                    if (IveRunOutOfVars.influence >= ff.influence && ((ff.factionAboveInf == 0) || (IveRunOutOfVars.influence < ff.factionAboveInf)))
                                    {
                                        ff.factionAboveInf = IveRunOutOfVars.influence;
                                        ff.factionAboveName = fac.name;
                                    }
                                    else if (IveRunOutOfVars.influence < ff.influence && ((ff.factionBelowInf == 0) || (IveRunOutOfVars.influence > ff.factionBelowInf)))
                                    {
                                        ff.factionBelowInf = IveRunOutOfVars.influence;
                                        ff.factionBelowName = fac.name;
                                    }

                                }
                            }
                        }
                        else
                        {
                            var jsonFac = await new WebClient().DownloadStringTaskAsync(baseURL + factionURL + Uri.EscapeDataString(fac.name));
                            FactionLibFaction.Faction finf = JsonConvert.DeserializeObject<FactionLibFaction.Faction>(jsonFac);
                            foreach (FactionPresence IveRunOutOfVars in finf.docs[0].faction_presence)
                            {
                                if (ff.systemName == IveRunOutOfVars.system_name)
                                {
                                    if (IveRunOutOfVars.influence >= ff.influence && ((ff.factionAboveInf == 0) || (IveRunOutOfVars.influence < ff.factionAboveInf)))
                                    {
                                        ff.factionAboveInf = IveRunOutOfVars.influence;
                                        ff.factionAboveName = fac.name;
                                    }
                                    else if (IveRunOutOfVars.influence < ff.influence && ((ff.factionBelowInf == 0) || (IveRunOutOfVars.influence > ff.factionBelowInf)))
                                    {
                                        ff.factionBelowInf = IveRunOutOfVars.influence;
                                        ff.factionBelowName = fac.name;
                                    }

                                }
                            }
                            facCache.Add(finf);
                        }
                    }

                }
                i++;

                //update progress
                progress.Report(i*100 / lst.Count());
            }

            foreach (FactionSystemInf ff in lst)
            {
                ff.influence = ff.influence * 100.0000;
                ff.factionBelowInf = ff.factionBelowInf * 100.0000;
                ff.factionAboveInf = ff.factionAboveInf * 100.0000;

                if(
                  ((ff.factionAboveInf > 0) && (ff.factionAboveInf- ff.influence <= 3))
                  ||
                  ((ff.factionBelowInf > 0) && (ff.influence - ff.factionBelowInf <= 3))
                   )
                {
                    ff.TooClose = 1;
                }
                else if (
                    ((ff.factionAboveInf > 0) && (ff.factionAboveInf - ff.influence <= 5))
                    ||
                    ((ff.factionBelowInf > 0) && (ff.influence - ff.factionBelowInf <= 5))
                   )
                {
                    ff.TooClose = 2;
                }

                if(ff.LastUpdate < DateTime.UtcNow.AddHours(-12))
                {
                    ff.DataIsOld = true;
                }


            }
            lst = lst.OrderBy(p => p.systemName).ToList();
            return lst;
        }
        
    }
}

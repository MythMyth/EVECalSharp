using EveCal.BPs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal
{
    public enum FacilityType
    {
        SOURCE,
        ADV_COMPONENT,
        ADV_LARGE_SHIP,
        ADV_MED_SHIP,
        ADV_SMALL_SHIP,
        FUEL_COMP,
        LARGE_SHIP,
        MEDIUM_SHIP,
        SMALL_SHIP,
        REACTION, 
        MODULE
    };

    public enum ActivityType
    {
        Manufacturing = 1,
        Researching_Technology = 2,
        Researching_Time_Productivity = 3,
        Researching_Material_Productivity = 4,
        Copying = 5,
        Duplicating = 6,
        Reverse_Engineering = 7,
        Invention = 8,
        Reaction = 9
    }

    
    internal class Storage
    {
        
        static Storage instance;
        static Mutex mutex = new Mutex();
        const string storagePath = "storage";
        string struct_path = "https://esi.evetech.net/latest/universe/structures/{structure_id}";
        Dictionary<FacilityType, Dictionary<string, int>> SortedAssets;
        Dictionary<string, Dictionary<string, int>> AllAsset;
        Dictionary<string, string> LocationName;
        Dictionary<string, int> BPC;
        Dictionary<FacilityType, string> FacilityMatch;
        Dictionary<FacilityType, string> FacilityName;
        Dictionary<string, int> Running;
        Dictionary<string, int> Copying;
        Mutex file_mutex;
        public static Storage GetInstance()
        {
            mutex.WaitOne();
            if(instance == null)
            {
                instance = new Storage();
            }
            mutex.ReleaseMutex();
            return instance;
        }
        public static void UpdateAsset(Dictionary<string, Dictionary<string, int>> assets)
        {
            GetInstance()._UpdateAsset(assets);
        }
        public static void UpdateAsset(string content, FacilityType type, string name)
        {
            GetInstance()._UpdateAsset(content, type, name);
        }

        public static void UpdateBPC(Dictionary<string, int> bpc)
        {
            GetInstance()._UpdateBPC(bpc);
        }
        public static int Get(FacilityType type, string iname)
        {
            return GetInstance()._Get(type, iname);
        }

        public static int GetBPCCount(string name)
        {
            return GetInstance()._GetBPCCount(name);
        }

        public static Dictionary<string, int> GetHaulable()
        {
            return GetInstance()._GetHaulable();
        }

        public static Dictionary<string, int> GetFacilityAsset(FacilityType type)
        {
            return GetInstance()._GetFacilityAsset(type);
        }

        public static void SetRunningJob(string s)
        {
            GetInstance()._SetRunningJob(s);
        }

        public static void SetRunningJob(List<Dictionary<string, string>> jobs)
        {
            GetInstance()._SetRunningJob(jobs);
        }

        public static Dictionary<string, int> GetRunningJob()
        {
            return GetInstance()._GetRunningJob();
        }

        public static string GetName(FacilityType type)
        {
            return GetInstance()._GetName(type);
        }

        public static Dictionary<string, string> GetFacilityNames()
        {
            return GetInstance()._GetFacilityNames();
        }

        public static Dictionary<FacilityType, string> GetFacilityMatch()
        {
            return GetInstance()._GetFacilityMatch();
        }

        public static void SaveFacilityMatch()
        {
            GetInstance()._SaveFacilityMatch();
        }

        public Storage()
        {
            SortedAssets = new Dictionary<FacilityType, Dictionary<string, int>>();
            Running = new Dictionary<string, int>();
            FacilityName = new Dictionary<FacilityType, string>();
            AllAsset = new Dictionary<string, Dictionary<string, int>>();
            LocationName = new Dictionary<string, string>();
            FacilityMatch = new Dictionary<FacilityType, string>();
            BPC = new Dictionary<string, int>();
            Copying = new Dictionary<string, int>();
            file_mutex = new Mutex();
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            foreach(FacilityType ftype in (Enum.GetValues(typeof(FacilityType)))) {
                SortedAssets.Add(ftype, new Dictionary<string, int>());
                string filename = storagePath + "\\" + ftype;
                if (File.Exists(filename))
                {
                    string[] lines = LoadFile(filename);
                    int len = lines.Length;
                    int start = 0;
                    if(len > 0)
                    {
                        if (lines[0].Split("\t").Length == 1)
                        {
                            start = 1;
                            FacilityName.Add(ftype, lines[0]);
                        }
                        else
                        {
                            FacilityName.Add(ftype, ftype.ToString());
                        } 
                            
                    }
                    for (int i = start; i < len; i++)
                    {
                        string[] parts = lines[i].Split('\t');
                        if(parts.Length > 1)
                        {
                            string name = parts[0].Trim();
                            int number = int.Parse(parts[1].Trim());
                            if (parts[1].Trim() == "") number = 1;
                            if (!SortedAssets[ftype].ContainsKey(name))
                            {
                                SortedAssets[ftype].Add(name, 0);
                            }
                            SortedAssets[ftype][name] += number;
                        }
                    }
                }
            }

            if(File.Exists(storagePath + "\\Running"))
            {
                string[] lines = LoadFile(storagePath + "\\Running");
                foreach(string line in lines)
                {
                    string[] parts = line.Split("\t");
                    if(parts.Length > 1)
                    {
                        string name = parts[0].Trim();
                        int number = int.Parse(parts[1]);
                        if (!Running.ContainsKey(name)) Running.Add(name, 0);
                        Running[name] += number;
                    }
                }
            }
            LoadFacilityMatch();
            LoadLocationName();
            LoadAllAsset();
        }

        void LoadFacilityMatch()
        {
            file_mutex.WaitOne();
            if (File.Exists(storagePath + "\\FacilityMatch"))
            {
                string[] all_match = File.ReadAllLines(storagePath + "\\FacilityMatch");
                foreach (string line in all_match)
                {
                    string[] p = line.Split("\t");
                    if (p.Length < 2) continue;
                    FacilityType type = (FacilityType)int.Parse(p[0]);
                    string id = p[1].Trim();
                    if (FacilityMatch.ContainsKey(type))
                    {
                        FacilityMatch[type] = id;
                    }
                    else
                    {
                        FacilityMatch.Add(type, id);
                    }
                }
            }
            else
            {
                File.Create(storagePath + "\\FacilityMatch");
            }
            file_mutex.ReleaseMutex();
        }

        void _SaveFacilityMatch()
        {
            file_mutex.WaitOne();
            FileStream fs;
            if(File.Exists(storagePath + "\\FacilityMatch"))
            {
                fs = File.Open(storagePath + "\\FacilityMatch", FileMode.Truncate);
            } else
            {
                fs = File.Open(storagePath + "\\FacilityMatch", FileMode.OpenOrCreate);
            }
            StreamWriter writer = new StreamWriter(fs);
            foreach(FacilityType type in FacilityMatch.Keys)
            {
                writer.WriteLine("" + (int)type + "\t" + FacilityMatch[type]);
            }
            writer.Close();
            fs.Close();
            file_mutex.ReleaseMutex();
        }

        void LoadLocationName()
        {
            if (File.Exists(storagePath + "\\FacilityName"))
            {
                string[] lines = File.ReadAllLines(storagePath + "\\FacilityName");
                foreach(string line in lines)
                {
                    string[] p = line.Split("\t");
                    if (p.Length < 2) continue;
                    string id = p[0].Trim();
                    string name = p[1];
                    if(!LocationName.ContainsKey(id))
                    {
                        LocationName.Add(id, name);
                    } else
                    {
                        LocationName[id] = name;
                    }
                }
            }
            else
            {
                File.Create(storagePath + "\\FacilityName");
            }
        }

        void SaveLocationName()
        {
            FileStream fs;
            if (File.Exists(storagePath + "\\FacilityName"))
            {
                fs = File.Open(storagePath + "\\FacilityName", FileMode.Truncate);
            } else
            {
                fs = File.Open(storagePath + "\\FacilityName", FileMode.OpenOrCreate);
            }
            StreamWriter writer = new StreamWriter(fs);
            foreach(string id in LocationName.Keys)
            {
                writer.WriteLine(id + "\t" + LocationName[id]);
            }
            writer.Close();
            fs.Close();
        }

        void LoadAllAsset()
        {
            if (!Directory.Exists(storagePath + "\\AllAsset"))
            {
                Directory.CreateDirectory(storagePath + "\\AllAsset");
            }
            AllAsset.Clear();
            string[] allAssetFiles = Directory.GetFiles(storagePath + "\\AllAsset");
            foreach (string file in allAssetFiles)
            {
                string[] lines = File.ReadAllLines(file);
                string facId = file.Split('\\').Last();
                if(!AllAsset.ContainsKey(facId)) {
                    AllAsset.Add(facId, new Dictionary<string, int>());
                }
                foreach (string line in lines)
                {
                    string[] p = line.Split("\t");
                    if (p.Length < 2) continue;
                    string name = p[0].Trim();
                    int qty = int.Parse(p[1].Trim());
                    if (!AllAsset[facId].ContainsKey(name))
                    {
                        AllAsset[facId].Add(name, 0);
                    }
                    AllAsset[facId][name] += qty;
                }
            }

            BPC.Clear();
            if(File.Exists(storagePath + "\\AllAsset\\BPC"))
            {
                string[] lines = File.ReadAllLines(storagePath + "\\AllAsset\\BPC");
                foreach(string line in lines)
                {
                    string[] p = line.Split("\t");
                    if (p.Length < 2) continue;
                    if (!BPC.ContainsKey(p[0].Trim())) BPC.Add(p[0].Trim(), 0);
                    BPC[p[0].Trim()] += int.Parse(p[1]);
                }
            }

            Running.Clear();
            if (File.Exists(storagePath + "\\Running"))
            {
                string[] lines = File.ReadAllLines(storagePath + "\\Running");
                foreach(string line in lines)
                {
                    string[] p = line.Split("\t");
                    if (p.Length < 2) continue;
                    if(!Running.ContainsKey(p[0].Trim())) Running.Add(p[0].Trim(), 0);
                    Running[p[0].Trim()] += int.Parse(p[1]);
                }
            }

            Copying.Clear();
            if (File.Exists(storagePath + "\\Copying"))
            {
                string[] lines = File.ReadAllLines(storagePath + "\\Copying");
                foreach (string line in lines)
                {
                    string[] p = line.Split("\t");
                    if (p.Length < 2) continue;
                    if (!Copying.ContainsKey(p[0].Trim())) Copying.Add(p[0].Trim(), 0);
                    Copying[p[0].Trim()] += int.Parse(p[1]);
                }
            }
        }

        void SaveAllAsset()
        {
            foreach(string facId in AllAsset.Keys)
            {
                FileStream fs;
                if(File.Exists(storagePath + "\\AllAsset\\" + facId))
                {
                    fs = File.Open(storagePath + "\\AllAsset\\" + facId, FileMode.Truncate);
                } else
                {
                    fs = File.Open(storagePath + "\\AllAsset\\" + facId, FileMode.OpenOrCreate);
                }
                StreamWriter writer = new StreamWriter(fs);
                foreach(string id in AllAsset[facId].Keys)
                {
                    writer.WriteLine(id + "\t" + AllAsset[facId][id]);
                }
                writer.Close();
                fs.Close();
            }
        }

        void SaveBPC()
        {
            FileStream fs;
            if (File.Exists(storagePath + "\\AllAsset\\BPC"))
            {
                fs = File.Open(storagePath + "\\AllAsset\\BPC", FileMode.Truncate);
            }
            else
            {
                fs = File.Open(storagePath + "\\AllAsset\\BPC", FileMode.OpenOrCreate);
            }
            StreamWriter writer = new StreamWriter(fs);
            foreach(string id in BPC.Keys)
            {
                writer.WriteLine(id + "\t" + BPC[id]);
            }
            writer.Close();
            fs.Close();
        }
        string[] LoadFile(string fname)
        {
            string text = File.ReadAllText(fname);
            return text.Split("\n");
        }
        public void _UpdateAsset(Dictionary<string, Dictionary<string, int>> assets)
        {
            AllAsset.Clear();
            foreach(string locid in assets.Keys)
            {
                AllAsset.Add(locid, new Dictionary<string, int>());
                foreach(string id in assets[locid].Keys)
                {
                    if (!AllAsset[locid].ContainsKey(id))
                    {
                        AllAsset[locid].Add(Cache.GetName(id), 0);
                    }
                    AllAsset[locid][Cache.GetName(id)] += assets[locid][id];
                }
                HttpClient client = new HttpClient();
                var response = client.GetAsync(struct_path.Replace("{structure_id}", locid) + "?token=" + CharacterManager.GetRandomToken()).GetAwaiter().GetResult();
                string res_str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Dictionary<string, object> res = JsonConvert.DeserializeObject<Dictionary<string, object>>(res_str);
                    if(LocationName.ContainsKey(locid))
                    {
                        LocationName[locid] = res["name"].ToString();
                    } else
                    {
                        LocationName.Add(locid, res["name"].ToString());
                    }
                }
            }
            SaveAllAsset();
            SaveLocationName();
        }

        public void _UpdateBPC(Dictionary<string, int> bpc)
        {
            BPC.Clear();
            foreach(string bpc_id in bpc.Keys)
            {
                string name = Cache.GetName(bpc_id);
                if(!BPC.ContainsKey(name))
                {
                    BPC.Add(name, 0);
                }
                BPC[name] += bpc[bpc_id];
            }
            SaveBPC();
        }
        public void _UpdateAsset(string content, FacilityType type, string name)
        {
            if (!SortedAssets.ContainsKey(type)) return; 
            Dictionary<string, int> map = SortedAssets[type];
            if(!File.Exists(storagePath + "\\" + type))
            {
                File.Create(storagePath + "\\" + type).Close();
            }
            FileStream f = File.Open(storagePath + "\\" + type, FileMode.Truncate);
            StreamWriter writer = new StreamWriter(f);
            map.Clear();
            string[] assets = content.Split("\n");
            writer.WriteLine(name);
            FacilityName[type] = name;
            foreach(string asset in assets)
            {
                string[] info = asset.Split("\t");
                if(info.Length >= 2)
                {
                    string assetName = info[0].Trim();
                    int number = 0;
                    if (info[1].Trim() != "")
                    {
                        number = int.Parse(info[1].Replace(",", "").Trim());
                    } else
                    {
                        number = 1;
                    }
                    if(!map.ContainsKey(assetName))
                    {
                        map.Add(assetName, 0);
                    }
                    map[assetName] += number;
                    writer.WriteLine(assetName + "\t" + number);
                }
            }
            writer.Close();
            f.Close();
        }

        public int _Get(FacilityType type, string iname)
        {
            if (!FacilityMatch.ContainsKey(type)) return 0;
            Dictionary<string, int> map = AllAsset[FacilityMatch[type]];
            if(map.ContainsKey(iname))
            {
                return map[iname];
            }
            return 0;
        }

        public Dictionary<string, int> _GetFacilityAsset(FacilityType type)
        {
            if(FacilityMatch.ContainsKey(type) && AllAsset.ContainsKey(FacilityMatch[type]))
            {
                return AllAsset[FacilityMatch[type]];
            }
            return new Dictionary<string, int>();
        }

        public Dictionary<string, int> _GetHaulable()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            foreach(FacilityType facility in FacilityMatch.Keys)
            {
                if (!AllAsset.ContainsKey(FacilityMatch[facility])) continue;
                foreach(string mat in AllAsset[FacilityMatch[facility]].Keys)
                {
                    if(facility == FacilityType.SOURCE)
                    {
                        if (!map.ContainsKey(mat)) map.Add(mat, 0);
                        map[mat] += AllAsset[FacilityMatch[facility]][mat];
                        continue;
                    }
                    BP bp = Loader.Get(mat);
                    if(bp != null && bp.MakeAt() == facility) 
                    {
                        if(!map.ContainsKey(mat))map.Add(mat, 0);
                        map[mat] += AllAsset[FacilityMatch[facility]][mat];
                    }
                }
            }

            foreach(string key in Running.Keys)
            {
                BP bp = Loader.Get(key.Trim());
                if (bp == null) continue;
                if (!map.ContainsKey(key.Trim())) map.Add(key.Trim(), 0);
                map[key.Trim()] += (bp.GetOutput() * Running[key]);
            }

            return map;
        }

        public void _SetRunningJob(string s)
        {
            if (!File.Exists(storagePath + "\\Running"))
            {
                File.Create(storagePath + "\\Running").Close();
            }
            FileStream f = File.Open(storagePath + "\\Running", FileMode.Truncate);
            StreamWriter writer = new StreamWriter(f);
            string[] jobs = s.Split("\n");
            Running.Clear();
            foreach(string job in jobs)
            {
                string[] col = job.Split("\t");
                if(col.Length < 4) continue;
                int jobRun =  int.Parse(col[1]);
                if (col[2].Trim() == "Reaction")
                {
                    string bpName = col[3].Trim().Replace(" Reaction Formula", "");
                    if(!Running.ContainsKey(bpName)) Running.Add(bpName, 0);
                    Running[bpName] += jobRun;
                } else if(col[2].Trim() == "Manufacturing")
                {
                    string bpName = col[3].Trim().Replace(" Blueprint", "");
                    if (!Running.ContainsKey(bpName)) Running.Add(bpName, 0);
                    Running[bpName] += jobRun;
                } else if(col[2].Trim() == "")
                {
                    string bpName = col[3].Trim();
                    if (!Running.ContainsKey(bpName)) Running.Add(bpName, 0);
                    Running[bpName] += jobRun;
                }
            }

            foreach(string key in Running.Keys)
            {
                writer.WriteLine(key + "\t" + Running[key]);
            }
            writer.Close();
            f.Close();
        }

        public void _SetRunningJob(List<Dictionary<string, string>> jobs)
        {
            if (!File.Exists(storagePath + "\\Running"))
            {
                File.Create(storagePath + "\\Running").Close();
            }
            FileStream f = File.Open(storagePath + "\\Running", FileMode.Truncate);
            StreamWriter writer = new StreamWriter(f);
            Running.Clear();
            Copying.Clear();
            foreach (Dictionary<string, string> job in jobs)
            {
                if (int.Parse(job["activity_id"]) == (int)ActivityType.Reaction)
                {
                    string bpName = Cache.GetName(job["blueprint_type_id"]).Trim().Replace(" Reaction Formula", "");
                    int jobRun = int.Parse(job["runs"]);
                    if (!Running.ContainsKey(bpName)) Running.Add(bpName, 0);
                    Running[bpName] += jobRun;
                } else if (int.Parse(job["activity_id"]) == (int)ActivityType.Manufacturing)
                {
                    string bpName = Cache.GetName(job["blueprint_type_id"]).Trim().Replace(" Blueprint", "");
                    int jobRun = int.Parse(job["runs"]);
                    if (!Running.ContainsKey(bpName)) Running.Add(bpName, 0);
                    Running[bpName] += jobRun;
                } else if (int.Parse(job["activity_id"]) == (int)ActivityType.Copying)
                {
                    string bpName = Cache.GetName(job["blueprint_type_id"]).Trim().Replace(" Blueprint", "");
                    int jobRun = int.Parse(job["runs"]);
                    if (!Copying.ContainsKey(bpName)) Copying.Add(bpName, 0);
                    Copying[bpName] += jobRun;
                }
            }
            foreach (string key in Running.Keys)
            {
                writer.WriteLine(key + "\t" + Running[key]);
            }
            writer.Close();
            f.Close();


            if (!File.Exists(storagePath + "\\Copying"))
            {
                f = File.Open(storagePath + "\\Copying", FileMode.Create);
            }
            else
            {
                f = File.Open(storagePath + "\\Copying", FileMode.Truncate);
            }
            writer = new StreamWriter(f);
            foreach (string key in Copying.Keys)
            {
                writer.WriteLine(key + "\t" + Copying[key]);
            }
            writer.Close();
            f.Close();
        }

        public Dictionary<string, int> _GetRunningJob()
        {
            return Running;
        }

        public int _GetBPCCount(string name)
        {
            int ret = 0;
            if (Copying.ContainsKey(name)) ret += Copying[name];
            if (BPC.ContainsKey(name)) ret += Copying[name];
            return ret;
        }

        public string _GetName(FacilityType type)
        {
            if (FacilityMatch.ContainsKey(type)) return LocationName[FacilityMatch[type]];
            return type.ToString();
        }

        public Dictionary<string, string> _GetFacilityNames()
        {
            return LocationName;
        }

        public Dictionary<FacilityType, string> _GetFacilityMatch()
        {
            return FacilityMatch;
        }

    }
}

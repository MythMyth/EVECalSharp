using EveCal.BPs;
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

        Dictionary<FacilityType, Dictionary<string, int>> AllAsset;
        Dictionary<FacilityType, string> FacilityName;
        Dictionary<string, int> Running;
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

        public static void UpdateAsset(string content, FacilityType type, string name)
        {
            GetInstance()._UpdateAsset(content, type, name);
        }

        public static int Get(FacilityType type, string iname)
        {
            return GetInstance()._Get(type, iname);
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

        public Storage()
        {
            AllAsset = new Dictionary<FacilityType, Dictionary<string, int>>();
            Running = new Dictionary<string, int>();
            FacilityName = new Dictionary<FacilityType, string>();
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            foreach(FacilityType ftype in (Enum.GetValues(typeof(FacilityType)))) {
                AllAsset.Add(ftype, new Dictionary<string, int>());
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
                            if (!AllAsset[ftype].ContainsKey(name))
                            {
                                AllAsset[ftype].Add(name, 0);
                            }
                            AllAsset[ftype][name] += number;
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
        }

        string[] LoadFile(string fname)
        {
            string text = File.ReadAllText(fname);
            return text.Split("\n");
        }

        public void _UpdateAsset(string content, FacilityType type, string name)
        {
            if (!AllAsset.ContainsKey(type)) return; 
            Dictionary<string, int> map = AllAsset[type];
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
            if (!AllAsset.ContainsKey(type)) return 0;
            Dictionary<string, int> map = AllAsset[type];
            if(map.ContainsKey(iname))
            {
                return map[iname];
            }
            return 0;
        }

        public Dictionary<string, int> _GetFacilityAsset(FacilityType type)
        {
            return AllAsset[type];
        }

        public Dictionary<string, int> _GetHaulable()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            foreach(FacilityType facility in AllAsset.Keys)
            {
                foreach(string mat in AllAsset[facility].Keys)
                {
                    if(facility == FacilityType.SOURCE)
                    {
                        if (!map.ContainsKey(mat)) map.Add(mat, 0);
                        map[mat] += AllAsset[facility][mat];
                        continue;
                    }
                    BP bp = Loader.Get(mat);
                    if(bp != null && bp.MakeAt() == facility) 
                    {
                        if(!map.ContainsKey(mat))map.Add(mat, 0);
                        map[mat] += AllAsset[facility][mat];
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
            Running.Clear();
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
                }
            }
        }

        public Dictionary<string, int> _GetRunningJob()
        {
            return Running;
        }

        public string _GetName(FacilityType type)
        {
            if (FacilityName.ContainsKey(type)) return FacilityName[type];
            return "";
        }

    }
}

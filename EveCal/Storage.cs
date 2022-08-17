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
        MODULE, 
        COPY_RESEARCH,
        INVENTION
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
        Dictionary<string, int> BPC;
        Dictionary<FacilityType, string> FacilityName;
        Mutex file_mutex;

        //Refactor to SQLITE

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

        public static void UpdateBPC(Dictionary<string, int> bpc)
        {
            GetInstance()._UpdateBPC(bpc);
        }
        public static int GetAssetCountAt(FacilityType type, string iname)
        {
            return GetInstance()._GetAssetCountAt(type, iname);
        }

        public static int GetAvailableBPC(string name)
        {
            return GetInstance()._GetAvailableBPC(name);
        }

        public static Dictionary<string, int> GetHaulable()
        {
            return GetInstance()._GetHaulable();
        }

        public static Dictionary<string, int> GetFacilityAsset(FacilityType type)
        {
            return GetInstance()._GetFacilityAsset(type);
        }

        public static void UpdateRunningJob(List<Dictionary<string, string>> jobs)
        {
            GetInstance()._UpdateRunningJob(jobs);
        }

        public static Dictionary<string, int> GetRunningJob(ActivityType type)
        {
            return GetInstance()._GetRunningJob(type);
        }

        public static string GetFacilityName(FacilityType type)
        {
            return GetInstance()._GetFacilityName(type);
        }

        public static Dictionary<string, string> GetFacilityList()
        {
            return GetInstance()._GetFacilityList();
        }

        public static string GetFacilityIdByType(FacilityType type)
        {
            return GetInstance()._GetFacilityIdByType(type);
        }

        public static void UpdateFacilityMapping(FacilityType type, string Id)
        {
            GetInstance()._UpdateFacilityMapping(type, Id);
        }

        public Storage()
        {
            SortedAssets = new Dictionary<FacilityType, Dictionary<string, int>>();
            FacilityName = new Dictionary<FacilityType, string>();
            AllAsset = new Dictionary<string, Dictionary<string, int>>();
            BPC = new Dictionary<string, int>();
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
            LoadAllAsset();
        }

        Dictionary<string, string> LoadLocationName()
        {
            return SQLiteDB.GetInstance().QueryLocationName("SELECT * FROM Facility;");
        }

        void SaveLocationName(Dictionary<string, string> LocationName)
        {
            SQLiteDB.GetInstance().Exe("DELETE FROM Facility WHERE 1 = 1;");
            foreach (string id in LocationName.Keys)
            {
                SQLiteDB.GetInstance().Exe($"INSERT INTO Facility (Id, Name) VALUES ('{id}', '{LocationName[id]}');");
            }
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
            Dictionary<string, string> LocationName = new Dictionary<string, string>();
            foreach (string locid in assets.Keys)
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
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
            SaveLocationName(LocationName);
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
        
        public int _GetAssetCountAt(FacilityType type, string iname)
        {
            string FacilityId = SQLiteDB.GetInstance().GetFacilityIdForType(type);
            if (FacilityId == "") return 0;
            Dictionary<string, int> map = AllAsset[FacilityId];
            if(map.ContainsKey(iname))
            {
                return map[iname];
            }
            return 0;
        }

        public Dictionary<string, int> _GetFacilityAsset(FacilityType type)
        {
            string FacilityId = SQLiteDB.GetInstance().GetFacilityIdForType(type);
            if (FacilityId != "" && AllAsset.ContainsKey(FacilityId))
            {
                return AllAsset[FacilityId];
            }
            return new Dictionary<string, int>();
        }

        public Dictionary<string, int> _GetHaulable()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            Dictionary<FacilityType, string> FacilityMatch = SQLiteDB.GetInstance().GetFacilityMatch();
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
            //Get all manufacturing
            Dictionary<string, int> running = SQLiteDB.GetInstance().GetRunningJobType(ActivityType.Manufacturing);
            foreach(string key in running.Keys)
            {
                BP bp = Loader.Get(key.Trim());
                if (bp == null) continue;
                if (!map.ContainsKey(key.Trim())) map.Add(key.Trim(), 0);
                map[key.Trim()] += (bp.GetOutput() * running[key]);
            }
            //Get all reaction
            running = SQLiteDB.GetInstance().GetRunningJobType(ActivityType.Reaction);
            foreach (string key in running.Keys)
            {
                BP bp = Loader.Get(key.Trim());
                if (bp == null) continue;
                if (!map.ContainsKey(key.Trim())) map.Add(key.Trim(), 0);
                map[key.Trim()] += (bp.GetOutput() * running[key]);
            }

            return map;
        }

        public void _UpdateRunningJob(List<Dictionary<string, string>> jobs)
        {
            SQLiteDB.GetInstance().ClearRunningJob();
            foreach (Dictionary<string, string> job in jobs)
            {
                SQLiteDB.GetInstance().AddRunningJob((ActivityType)int.Parse(job["activity_id"]), job["blueprint_type_id"], int.Parse(job["runs"]));
            }
        }

        public Dictionary<string, int> _GetRunningJob(ActivityType type)
        {
            return SQLiteDB.GetInstance().GetRunningJobType(type);
        }

        public int _GetAvailableBPC(string name)
        {
            int ret = 0;
            Dictionary<string, int> Copying = SQLiteDB.GetInstance().GetRunningJobType(ActivityType.Copying);
            if (Copying.ContainsKey(name)) ret += Copying[name];
            if (BPC.ContainsKey(name)) ret += BPC[name];
            return ret;
        }

        public string _GetFacilityName(FacilityType type)
        {
            string FacilityId = SQLiteDB.GetInstance().GetFacilityIdForType(type);
            if (FacilityId != "")
            {
                Dictionary<string, string> LocationName = SQLiteDB.GetInstance().QueryLocationName($"SELECT * FROM Facility WHERE Id = '{FacilityId}';");
                if (LocationName.Count > 0)
                    return LocationName[FacilityId];
                else
                    return type.ToString();
            }
            return type.ToString();
        }

        public Dictionary<string, string> _GetFacilityList()
        {
            return LoadLocationName();
        }

        public string _GetFacilityIdByType(FacilityType type)
        {
            return SQLiteDB.GetInstance().GetFacilityIdForType(type);
        }

        public void _UpdateFacilityMapping(FacilityType type, string Id)
        {
            SQLiteDB.GetInstance().SaveFacilityMatch(type, Id);
        }

    }
}

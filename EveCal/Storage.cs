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
        Dictionary<string, int> BPC;

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

        public static void UpadateAsset(List<Dictionary<string, string>> AllLoadedAsset)
        {
            GetInstance()._UpadateAsset(AllLoadedAsset);
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

        public static Dictionary<FacilityType, Dictionary<string, int>> GetAllAsset()
        {
            return GetInstance()._GetAllAsset();
        }

        public static Dictionary<string, int> GetAssetAt(string facid)
        {
            return GetInstance()._GetAssetAt(facid);
        }

        public static Dictionary<string, int> GetAssetAt(FacilityType type)
        {
            string facid = GetFacilityIdByType(type);
            if (facid == "") return new Dictionary<string, int>();
            return GetAssetAt(facid);
        }

        public static Dictionary<FacilityType, Dictionary<string, int>> GetAllBPC()
        {
            return GetInstance()._GetAllBPC();
        }

        public static Dictionary<FacilityType, Dictionary<string, int>> GetOuptutRunningJobInFacility()
        {
            return GetInstance()._GetOuptutRunningJobInFacility();
        }

        public Storage()
        {
            BPC = new Dictionary<string, int>();
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }
            LoadAllAsset();
        }

        Dictionary<string, string> LoadLocationName()
        {
            return SQLiteDB.GetInstance().QueryLocationName("SELECT * FROM Facility;");
        }

        void LoadAllAsset()
        {
            if (!Directory.Exists(storagePath + "\\AllAsset"))
            {
                Directory.CreateDirectory(storagePath + "\\AllAsset");
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

        public void _UpadateAsset(List<Dictionary<string, string>> AllLoadedAsset)
        {
            SQLiteDB.GetInstance().UpdateAsset(AllLoadedAsset);
        }

        public void _UpdateRunningJob(List<Dictionary<string, string>> jobs)
        {
            SQLiteDB.GetInstance().ClearRunningJob();
            foreach (Dictionary<string, string> job in jobs)
            {
                SQLiteDB.GetInstance().AddRunningJob((ActivityType)int.Parse(job["activity_id"]), job["blueprint_type_id"], int.Parse(job["runs"]), job["station_id"]);
            }
        }

        public Dictionary<string, int> _GetRunningJob(ActivityType type)
        {
            return SQLiteDB.GetInstance().GetRunningJobType(type);
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

        public Dictionary<FacilityType, Dictionary<string, int>> _GetAllAsset()
        {
            return SQLiteDB.GetInstance().GetAllAsset();
        }

        public Dictionary<FacilityType, Dictionary<string, int>> _GetOuptutRunningJobInFacility()
        {
            return SQLiteDB.GetInstance().GetOuptutRunningJobInFacility();
        }

        public Dictionary<string, int> _GetAssetAt(string facid)
        {
            return SQLiteDB.GetInstance().GetAssetAt(facid);
        }

        public Dictionary<FacilityType, Dictionary<string, int>> _GetAllBPC()
        {
            return SQLiteDB.GetInstance().GetAllBPC();
        }
    }
}

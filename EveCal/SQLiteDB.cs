using EveCal.BPs;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EveCal
{
    internal class SQLiteDB
    {
        static SQLiteDB instance;
        static Mutex mutex = new Mutex();
        SqliteConnection db;

        public static SQLiteDB GetInstance()
        {
            mutex.WaitOne();
            if (instance == null)
            {
                instance = new SQLiteDB();
            }
            mutex.ReleaseMutex();
            return instance;
        }

        private SQLiteDB()
        {
            db = new SqliteConnection("Data Source=data.db");
            string createCharacterTable = "CREATE TABLE IF NOT EXISTS Character ( Id INTEGER PRIMARY KEY,Name TEXT, code TEXT, token TEXT, refresh TEXT); ";
            string createFacilityListTable = @"CREATE TABLE IF NOT EXISTS Facility (Id TEXT PRIMARY KEY, Name TEXT);";
            string createFacilityMatchTable = @"CREATE TABLE IF NOT EXISTS FacilityMatch (Type INTEGER PRIMARY KEY, FacilityId);";
            string createJobRunningTable = @"CREATE TABLE IF NOT EXISTS JobRunning (Id INTEGER PRIMARY KEY AUTOINCREMENT, ActivityType INTEGER, BPTypeId TEXT, Run INTEGER, LocId TEXT);";
            string createCacheTable = @"CREATE TABLE IF NOT EXISTS Cache (Id INTEGER PRIMARY KEY, Name TEXT);";
            string createAssetTable = @"CREATE TABLE IF NOT EXISTS Asset (Id INTEGER PRIMARY KEY AUTOINCREMENT, TypeId TEXT, LocId TEXT, Quantity INTEGER, IsBPC INTEGER);";
            try
            {
                db.Open();
                SqliteCommand comm = db.CreateCommand();
                comm.CommandText = createCharacterTable;
                comm.ExecuteNonQuery();
                comm.CommandText = createFacilityListTable;
                comm.ExecuteNonQuery();
                comm.CommandText = createFacilityMatchTable;
                comm.ExecuteNonQuery();
                comm.CommandText = createJobRunningTable;
                comm.ExecuteNonQuery();
                comm.CommandText = createCacheTable;
                comm.ExecuteNonQuery();
                comm.CommandText = createAssetTable;
                comm.ExecuteNonQuery();
            } 
            catch(Exception e)
            {

            }

        }

        public int Exe(string str_command) { 
            try
            {
                SqliteCommand comm = db.CreateCommand();
                comm.CommandText = str_command;
                return comm.ExecuteNonQuery();
            } 
            catch(Exception e)
            {
                return -1;
            }
        }

        public Dictionary<string, CharInfo> QueryCharacter(string query)
        {
            Dictionary<string, CharInfo> list = new Dictionary<string, CharInfo>();

            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = query;
            SqliteDataReader reader = comm.ExecuteReader();
            while(reader.Read())
            {
                string Id = reader.GetInt32(0).ToString();
                string Name = reader.GetString(1).ToString();
                string code = reader.GetString(2).ToString();
                string token = reader.GetString(3).ToString();
                string refresh = reader.GetString(4).ToString();
                list.Add(Id, new CharInfo(Name, Id, token, refresh, code));
            }
            return list;
        }

        public Dictionary<string, string> QueryLocationName(string query)
        {
            Dictionary<string , string> list = new Dictionary<string , string>();

            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = query;
            SqliteDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                string Id = reader.GetString(0).ToString();
                string Name = reader.GetString(1).ToString();
                list.Add(Id, Name);
            }

            return list;
        }

        public void ClearFacilityNames()
        {
            Exe("DELETE FROM Facility WHERE 1 = 1;");
        }

        public void AddFacilityName(string id, string name)
        {
            Exe($"INSERT INTO Facility (Id, Name) VALUES('{id}', '{name}');");
        }

        public string GetFacilityIdForType(FacilityType type)
        {
            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT * FROM FacilityMatch WHERE Type = '{(int)type}';";
            SqliteDataReader reader = comm.ExecuteReader();
            string Id = "";
            while (reader.Read())
            {
                Id = reader.GetString(1).ToString();
            }
            return Id;
        }

        public void SaveFacilityMatch(FacilityType type, string Id)
        {
            Exe($"DELETE FROM FacilityMatch WHERE Type = '{(int)type}';INSERT INTO FacilityMatch (Type, FacilityId) VALUES ('{(int)type}', '{Id}');");
        }

        public Dictionary<FacilityType, string> GetFacilityMatch()
        {
            Dictionary<FacilityType, string> list = new Dictionary<FacilityType, string>();

            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT * FROM FacilityMatch;";
            SqliteDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                FacilityType Type = (FacilityType)reader.GetInt64(0);
                string Id = reader.GetString(1).ToString();
                list.Add(Type, Id);
            }

            return list;
        }

        public void AddRunningJob(ActivityType type, string BPTypeId, int run, string LocId)
        {
            Exe($"INSERT INTO JobRunning (ActivityType, BPTypeId, Run, Locid) VALUES ('{(int)type}', '{BPTypeId}', '{run}', '{LocId}');");
        }

        public void ClearRunningJob()
        {
            Exe("DELETE FROM JobRunning WHERE 1 = 1;");
        }

        public Dictionary<string, int> GetRunningJobType(ActivityType type)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();

            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT * FROM JobRunning WHERE ActivityType = '{(int)type}';";
            SqliteDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                string BPType = reader.GetString(2);
                BPType = Cache.GetName(BPType);
                if (type == ActivityType.Reaction)
                {
                    BPType = BPType.Trim().Replace(" Reaction Formula", "");
                    if (BPType == "Fulleride") BPType = "Fullerides";
                }
                if(type == ActivityType.Manufacturing || type == ActivityType.Copying)
                {
                    BPType = BPType.Trim().Replace(" Blueprint", "");
                }
                int run = reader.GetInt32(3);
                if(!list.ContainsKey(BPType))
                {
                    list.Add(BPType, run);
                } else
                {
                    list[BPType] += run;
                }
            }

            return list;
        }

        public Dictionary<FacilityType, Dictionary<string, int>> GetOuptutRunningJobInFacility()
        {
            Dictionary<FacilityType, Dictionary<string, int>> running = new Dictionary<FacilityType, Dictionary<string, int>>();

            foreach (FacilityType facilityType in Enum.GetValues(typeof(FacilityType)))
            {
                string facid = GetFacilityIdForType(facilityType);
                running.Add(facilityType, new Dictionary<string, int>());
                if (facid == "") continue;
                SqliteCommand comm = db.CreateCommand();
                comm.CommandText = $"SELECT Name, Run FROM JobRunning LEFT JOIN Cache ON BPTypeId = Cache.Id WHERE LocId = '{facid}';";
                SqliteDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) || reader.IsDBNull(1)) continue;
                    string BPType = reader.GetString(0).Trim().Replace(" Reaction Formula", "").Replace(" Blueprint", "");
                    if (BPType == "Fulleride") BPType = "Fullerides";
                    int run = reader.GetInt32(1);
                    BP bp = Loader.Get(BPType);
                    if (bp == null) continue;
                    if (!running[facilityType].ContainsKey(BPType)) running[facilityType].Add(BPType, 0);
                    running[facilityType][BPType] += run * bp.GetOutput();
                }
            }
            return running;
        }

        public bool IsCacheIdAvailable(string id)
        {
            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT * FROM Cache WHERE Id = '{id}';";
            SqliteDataReader reader = comm.ExecuteReader();
            return reader.Read();
        }

        public void AddCacheId(string id, string name)
        {
            Exe($"INSERT INTO Cache (Id, Name) VALUES ('{id}', '{name}');");
        }

        public string GetCacheIdName(string id)
        {
            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT * FROM Cache WHERE Id = '{id}';";
            SqliteDataReader reader = comm.ExecuteReader();
            string ret = ""; 
            if(reader.Read())
            {
                ret = reader.GetString(1);
            }
            return ret;
        }

        public void UpdateAsset(List<Dictionary<string, string>> AllLoadedAsset)
        {
            Exe("DELETE FROM Asset WHERE 1 = 1;");
            foreach(Dictionary<string, string> asset in AllLoadedAsset)
            {
                if (asset["location_flag"] != "Hangar") continue;
                Exe($"INSERT INTO Asset (TypeId , LocId, Quantity, IsBPC) VALUES ('{asset["type_id"]}','{asset["location_id"]}','{asset["quantity"]}','{((!asset.ContainsKey("is_blueprint_copy") || asset["is_blueprint_copy"] == "false") ? "false":"true")}');");
            }
        }

        public List<string> GetAllFacilityId()
        {
            List<string> ids = new List<string>();

            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT DISTINCT LocId FROM Asset;";
            SqliteDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                ids.Add(reader.GetString(0));
            }

            return ids;
        }

        public Dictionary<string, int> GetAssetAt(string facid)
        {
            Dictionary<string, int> asset = new Dictionary<string, int>();

            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT Name, Quantity FROM Asset LEFT JOIN Cache ON TypeId = Cache.Id WHERE LocId = '{facid}' AND IsBPC = 'false';";
            SqliteDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if(reader.IsDBNull(0) || reader.IsDBNull(1)) continue;
                string name = reader.GetString(0);
                int quantity = reader.GetInt32(1);

                if (!asset.ContainsKey(name)) asset.Add(name, 0);
                asset[name] += quantity;

            }

            return asset;
        }

        public Dictionary<FacilityType, Dictionary<string, int>> GetAllAsset()
        {
            Dictionary<FacilityType, Dictionary<string, int>> assets = new Dictionary<FacilityType, Dictionary<string, int>>();

            foreach (FacilityType facilityType in Enum.GetValues(typeof(FacilityType)))
            {
                string facid = GetFacilityIdForType(facilityType);
                if (facid == "") assets.Add(facilityType, new Dictionary<string, int>());
                else assets.Add(facilityType, GetAssetAt(facid));
            }

            return assets;
        }

        public Dictionary<FacilityType, Dictionary<string, int>> GetAllBPC()
        {
            Dictionary<FacilityType, Dictionary<string, int>> bpcs = new Dictionary<FacilityType, Dictionary<string, int>>();

            foreach (FacilityType facilityType in Enum.GetValues(typeof(FacilityType)))
            {
                bpcs.Add(facilityType, new Dictionary<string, int>());
                string facid = GetFacilityIdForType(facilityType);
                if (facid == "") continue;
                SqliteCommand comm = db.CreateCommand();
                comm.CommandText = $"SELECT Name, Quantity FROM Asset LEFT JOIN Cache ON TypeId = Cache.Id WHERE LocId = '{facid}' AND IsBPC = 'true';";
                SqliteDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.IsDBNull(0) || reader.IsDBNull(1)) continue;
                    string name = reader.GetString(0);
                    int quantity = reader.GetInt32(1);
                    if (!bpcs[facilityType].ContainsKey(name)) bpcs[facilityType].Add(name, quantity);
                    else bpcs[facilityType][name] += quantity;
                }
            }
            return bpcs;
        }

        public Dictionary<string, int> GetNumberOfItems(List<string> items)
        {
            string names = "";
            foreach (string item in items)
            {
                names += "'" + item + "', ";
            }
            if (names.Length > 0) names = names.Substring(0, names.Length - 2);
            Dictionary<string, int> ret = new Dictionary<string, int>();
            if (names.Length == 0) return ret;
            SqliteCommand comm = db.CreateCommand();
            comm.CommandText = $"SELECT Name, Quantity FROM Asset LEFT JOIN Cache ON TypeId = Cache.Id WHERE Name in ({names});";
            SqliteDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                if (reader.IsDBNull(0) || reader.IsDBNull(1)) continue;
                string name = reader.GetString(0);
                int quantity = reader.GetInt32(1);

                if (!ret.ContainsKey(name)) ret.Add(name, 0);
                ret[name] += quantity;

            }
            return ret;
        }
    }
}

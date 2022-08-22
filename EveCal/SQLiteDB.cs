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
            string createJobRunningTable = @"CREATE TABLE IF NOT EXISTS JobRunning (Id INTEGER PRIMARY KEY AUTOINCREMENT, ActivityType INTEGER, BPTypeId TEXT, Run INTEGER);";
            string createCacheTable = @"CREATE TABLE IF NOT EXISTS Cache (Id INTEGER PRIMARY KEY, Name TEXT);";
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

        public void AddRunningJob(ActivityType type, string BPTypeId, int run)
        {
            Exe($"INSERT INTO JobRunning (ActivityType, BPTypeId, Run) VALUES ('{(int)type}', '{BPTypeId}', '{run}');");
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
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal
{

    class Descriptor
    {
        public int ME, TE;
        public int output;
        public int prob;
        public Descriptor(int mE, int tE, int output, int prob)
        {
            ME = mE;
            TE = tE;
            this.output = output;
            this.prob = prob;
        }
    }
    internal class Cache
    {
        static Cache instance;
        static Mutex mutex = new Mutex();
        public static Dictionary<string, Descriptor> DescriptorList = new Dictionary<string, Descriptor>()
        {
            {"Accelerant Decryptor", new Descriptor(2, 10, 1, 20) },
            {"Attainment Decryptor", new Descriptor(-1, 4, 4, 80) },
            {"Augmentation Decryptor",new Descriptor(-2, 2, 9, -40) },
            {"Optimized Attainment Decryptor", new Descriptor(1, -2, 2, 90) },
            {"Optimized Augmentation Decryptor", new Descriptor(2, 0, 7, -10) },
            {"Parity Decryptor", new Descriptor(1, -2, 3, 50) },
            {"Process Decryptor", new Descriptor(3, 6, 0, 10) },
            {"Symmetry Decryptor", new Descriptor(1, 8, 2 ,0) }
        };

        public static Cache GetInstance()
        {
            if(instance == null)
            {
                instance = new Cache();
            }
            return instance;
        }

        public static void AddIds(List<string> ids)
        {
            mutex.WaitOne();
            GetInstance()._AddIds(ids);
            mutex.ReleaseMutex();
        }

        public static string GetName(string id) {
            mutex.WaitOne();
            string ret = GetInstance()._GetName(id);
            mutex.ReleaseMutex();
            return ret;
        }

        public void _AddIds(List<string> ids)
        {
            string request_ids = "";
            foreach(string id in ids)
            {
                if(!SQLiteDB.GetInstance().IsCacheIdAvailable(id))
                {
                    request_ids += "," + id;
                }
            }
            if (request_ids.Length > 0)
                request_ids = request_ids.Substring(1);
            request_ids = "[" + request_ids + "]";
            var body = new Dictionary<string, string>()
            {
                { request_ids , "ids"}
            };
            HttpClient client = new HttpClient();
            var content = new StringContent(request_ids);
            var response = client.PostAsync("https://esi.evetech.net/latest/universe/names/?datasource=tranquility", content).GetAwaiter().GetResult();
            string res_str = (response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).ToString();
            if (response.IsSuccessStatusCode)
            {
                List < Dictionary<string, string>> res = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(res_str);
                foreach(var item in res)
                {
                    if(item.ContainsKey("category") && item["category"] == "inventory_type")
                    {
                        SQLiteDB.GetInstance().AddCacheId(item["id"], item["name"]);
                    }
                }
            }
        }

        public string _GetName(string id)
        {
            return SQLiteDB.GetInstance().GetCacheIdName(id);
        }
    }
}

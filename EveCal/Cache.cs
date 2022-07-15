using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal
{
    internal class Cache
    {
        static Cache instance;
        static Mutex mutex = new Mutex();

        Dictionary<string, string> names = new Dictionary<string, string>();

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
                if(!names.ContainsKey(id))
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
                        names.Add(item["id"], item["name"]);
                    }
                }
            }
        }

        public string _GetName(string id)
        {
            if (names.ContainsKey(id)) return names[id];
            return "";
        }
    }
}

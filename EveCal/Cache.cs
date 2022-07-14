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

        public static async Task AddIds(List<string> ids)
        {
            mutex.WaitOne();
            await GetInstance()._AddIds(ids);
            mutex.ReleaseMutex();
        }

        public static string GetName(string id) {
            mutex.WaitOne();
            string ret = GetInstance()._GetName(id);
            mutex.ReleaseMutex();
            return ret;
        }

        public async Task _AddIds(List<string> ids)
        {
            string request_ids = "[";
            foreach(string id in ids)
            {
                if(!names.ContainsKey(id))
                {
                    request_ids += id + ",";
                }
            }
            request_ids += "]";

            var body = new Dictionary<string, string>()
            {
                { "datasource", "tranquility" },
                { "ids ", request_ids }
            };
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(body);
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("https://esi.evetech.net/latest/universe/names/", content);
            if(response.IsSuccessStatusCode)
            {
                List < Dictionary<string, string>> res = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>((await response.Content.ReadAsStringAsync()).ToString());
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

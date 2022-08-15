using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EveCal
{
    public class CharInfo {
        public string Name;
        public string Id;
        public string token;
        public string refresh;
        public string code;
        public string IndyEtag;
        public List<string> AssetEtag;
        public CharInfo(string name, string id, string token, string refresh, string code)
        {
            Name = name;
            Id = id;
            this.token = token;
            this.refresh = refresh;
            this.code = code;
            IndyEtag = "";
            AssetEtag = new List<string>();
        }
    }
    internal class CharacterManager
    {
        static CharacterManager instance;
        static Mutex mutex = new Mutex();
        Dictionary<string, CharInfo> characters;
        string autho_code;
        string token_path = "https://login.eveonline.com/oauth/token";
        static CharacterManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CharacterManager();
            }
            return instance;
        }

        public static string GetRandomToken()
        {
            mutex.WaitOne();
            string token = GetInstance()._GetRandomToken();
            mutex.ReleaseMutex();
            return token;
        }

        public static void AddCharacter(CharInfo info)
        {
            mutex.WaitOne();
            GetInstance()._AddCharacter(info);
            mutex.ReleaseMutex();
        }

        public static void RemoveCharacter(string cid) {
            mutex.WaitOne();
            GetInstance()._RemoveCharacter(cid);
            mutex.ReleaseMutex();
        }

        public static void GetRefreshToken(string code, string refreshToken)
        {
            mutex.WaitOne();
            GetInstance()._GetRefreshToken(code, refreshToken);
            mutex.ReleaseMutex();
        }

        public static void GetToken(string code)
        {
            mutex.WaitOne();
            GetInstance()._GetToken(code);
            mutex.ReleaseMutex();
        }

        public void _GetToken(string code)
        {
            var body = new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", code }
            };
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(body);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", autho_code);
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = client.PostAsync(token_path, content).GetAwaiter().GetResult();
            string res_data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(res_data);
            if (res.ContainsKey("access_token"))
            {
                _GetCharInfo(code, res["access_token"], res["refresh_token"]);
            }
            else
            {

            }
        }

        string verify_url = "https://login.eveonline.com/oauth/verify";
        void _GetCharInfo(string code, string token, string refresh)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.GetAsync(verify_url).GetAwaiter().GetResult();
            string res_data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(res_data);
            if (res.ContainsKey("CharacterID"))
            {
                _AddCharacter(new CharInfo(res["CharacterName"], res["CharacterID"], token, refresh, code));
            }
            else
            {
            }
        }

        public void _GetRefreshToken(string code, string refreshToken)
        {
            Dictionary<string, string> body = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
            };
            HttpClient client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(body);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", autho_code);
            var response = client.PostAsync(token_path, content).GetAwaiter().GetResult();
            string res_data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>(res_data);
            if (res.ContainsKey("access_token"))
            {
                _GetCharInfo(code, res["access_token"], res["refresh_token"]);
            } 
            else
            {

            }
        }

        public static Dictionary<string, CharInfo> GetCharList()
        {
            mutex.WaitOne();
            Dictionary<string, CharInfo> ret = GetInstance()._GetCharList();
            mutex.ReleaseMutex();
            return ret;
        }

        public CharacterManager()
        {
            characters = SQLiteDB.GetInstance().QueryCharacter("SELECT * FROM Character;");
            try
            {
                string[] keys = File.ReadAllLines("key.cfg");
                keys[0] = keys[0].Trim();
                keys[1] = keys[1].Trim();
                autho_code = Convert.ToBase64String(Encoding.UTF8.GetBytes(keys[0] + ":" + keys[1]));
            }
            catch (Exception ex)
            {
                autho_code = "";
            }
        }


        public void _AddCharacter(CharInfo info)
        {
            if(characters.ContainsKey(info.Id))
            {
                characters[info.Id].token = info.token;
                characters[info.Id].refresh = info.refresh;
                characters[info.Id].code = info.code;
                int res = SQLiteDB.GetInstance().Exe($"UPDATE Character SET code = '{info.code}', token = '{info.token}', refresh = '{info.refresh}'WHERE Id = '{info.Id}';");
            } else
            {
                characters.Add(info.Id, info);
                int res = SQLiteDB.GetInstance().Exe($"INSERT INTO Character (Id, Name, code, token, refresh) VALUES ('{info.Id}', '{info.Name}', '{info.code}', '{info.token}', '{info.refresh}'); ");
            }
        }

        public string _GetRandomToken()
        {
            if (characters.Count == 0) return "";
            return characters[characters.Keys.ToArray()[0]].token;
        }
        public void _RemoveCharacter(string id)
        {
            characters.Remove(id);
            SQLiteDB.GetInstance().Exe($"DELETE FROM Character WHERE Id = '{id}';");
        }

        public Dictionary<string, CharInfo> _GetCharList()
        {
            return characters;
        }
    }
}

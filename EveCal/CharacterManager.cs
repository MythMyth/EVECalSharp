﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EveCal
{
    public class CharInfo {
        public string Name;
        public string Id;
        public string token;
        public string refresh;
        public string code;
        public CharInfo(string name, string id, string token, string refresh, string code)
        {
            Name = name;
            Id = id;
            this.token = token;
            this.refresh = refresh;
            this.code = code;
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

        public static async Task GetRefreshToken(string code, string refreshToken)
        {
            mutex.WaitOne();
            await GetInstance()._GetRefreshToken(code, refreshToken);
            mutex.ReleaseMutex();
        }

        string verify_url = "https://login.eveonline.com/oauth/verify";
        public async Task _GetRefreshToken(string code, string refreshToken)
        {
            Dictionary<string, string> body = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken }
            };
            HttpClient client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(body);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", autho_code);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(token_path, content);
            Dictionary<string, string> res = JsonConvert.DeserializeObject<Dictionary<string, string>>((await response.Content.ReadAsStringAsync()).ToString());
            if (res.ContainsKey("access_token"))
            {
                HttpClient cclient = new HttpClient();
                cclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res["access_token"]);
                var cresponse = await cclient.GetAsync(verify_url);
                Dictionary<string, string> cres = JsonConvert.DeserializeObject<Dictionary<string, string>>((await cresponse.Content.ReadAsStringAsync()).ToString());
                if (res.ContainsKey("CharacterID"))
                {
                    _AddCharacter(new CharInfo(cres["CharacterName"], cres["CharacterID"], res["access_token"], res["refresh_token"], code));
                }
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
            characters = new Dictionary<string, CharInfo>();
            if (!Directory.Exists("Character"))
            {
                Directory.CreateDirectory("Character");
            }
            LoadCharacters();

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
        void LoadCharacters()
        {
            DirectoryInfo d = new DirectoryInfo("Character");
            foreach (FileInfo inf in d.GetFiles())
            {
                string name = inf.Name;
                int idx = name.LastIndexOf('\\');
                name = name.Substring(idx + 1);
                string[] lines = File.ReadAllLines("Character\\" + name);
                if (lines.Length >= 5)
                {
                    CharInfo c = new CharInfo(lines[0].Trim(), lines[1].Trim(), lines[2].Trim(), lines[3].Trim(), lines[4].Trim());
                    characters.Add(c.Id, c);
                }
            }
        }


        public void _AddCharacter(CharInfo info)
        {
            if(characters.ContainsKey(info.Id))
            {
                characters[info.Id] = info;
                FileStream fs = new FileStream("Character\\" + info.Id, FileMode.Truncate);
                StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine(info.Name);
                writer.WriteLine(info.Id);
                writer.WriteLine(info.token);
                writer.WriteLine(info.refresh);
                writer.WriteLine(info.code);
                writer.Flush();
                writer.Close();
                fs.Close();
            } else
            {
                characters.Add(info.Id, info);
                FileStream fs = new FileStream("Character\\" + info.Id, FileMode.Create);
                StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine(info.Name);
                writer.WriteLine(info.Id);
                writer.WriteLine(info.token);
                writer.WriteLine(info.refresh);
                writer.WriteLine(info.code);
                writer.Flush();
                writer.Close();
                fs.Close();
            }
        }

        public void _RemoveCharacter(string id)
        {
            characters.Remove(id);
            File.Delete("Character\\" + id);
        }

        public Dictionary<string, CharInfo> _GetCharList()
        {
            return characters;
        }
    }
}

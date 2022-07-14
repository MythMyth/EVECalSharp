using System;
using System.Collections.Generic;
using System.Linq;
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

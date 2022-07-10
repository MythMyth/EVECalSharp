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
        }

        public void _AddCharacter(CharInfo info)
        {
            if(characters.ContainsKey(info.Id))
            {
                characters[info.Id] = info;
            } else
            {
                characters.Add(info.Id, info);
            }
        }

        public Dictionary<string, CharInfo> _GetCharList()
        {
            return characters;
        }
    }
}

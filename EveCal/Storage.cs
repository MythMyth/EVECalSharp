using EveCal.BPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal
{
    public enum FacilityType
    {
        ADV_COMPONENT,
        ADV_LARGE_SHIP,
        ADV_MED_SHIP,
        ADV_SMALL_SHIP,
        FUEL_COMP,
        LARGE_SHIP,
        MEDIUM_SHIP,
        SMALL_SHIP,
        REACTION
    };
    internal class Storage
    {
        static Storage instance;
        static Mutex mutex = new Mutex();

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

        public static void UpdateAsset(string content, FacilityType type)
        {
            GetInstance()._UpdateAsset(content, type);
        }

        public static int Get(FacilityType type, string iname)
        {
            return GetInstance()._Get(type, iname);
        }

        public static Dictionary<string, int> GetHaulable()
        {
            return GetInstance()._GetHaulable();
        }

        Dictionary<FacilityType, Dictionary<string, int>> AllAsset;

        public Storage()
        {
            AllAsset = new Dictionary<FacilityType, Dictionary<string, int>>();
            foreach(FacilityType ftype in (Enum.GetValues(typeof(FacilityType)))) {
                AllAsset.Add(ftype, new Dictionary<string, int>());
            }
        }

        public void _UpdateAsset(string content, FacilityType type)
        {
            if (!AllAsset.ContainsKey(type)) return; 
            Dictionary<string, int> map = AllAsset[type];
            
            map.Clear();
            string[] assets = content.Split("\n");
            foreach(string asset in assets)
            {
                string[] info = asset.Split("\t");
                if(info.Length >= 2)
                {
                    string assetName = info[0].Trim();
                    int number = 0;
                    if (info[1].Trim() != "")
                    {
                        number = int.Parse(info[1].Trim());
                    }
                    if(map.ContainsKey(assetName))
                    {
                        map[assetName] += number;
                    } else
                    {
                        map.Add(assetName, number);
                    }
                }
            }

        }

        public int _Get(FacilityType type, string iname)
        {
            if (!AllAsset.ContainsKey(type)) return 0;
            Dictionary<string, int> map = AllAsset[type];
            if(map.ContainsKey(iname))
            {
                return map[iname];
            }
            return 0;
        }

        public Dictionary<string, int> _GetHaulable()
        {
            Dictionary<string, int> map = new Dictionary<string, int>();

            foreach(FacilityType facility in AllAsset.Keys)
            {
                foreach(string mat in AllAsset[facility].Keys)
                {
                    BP bp = Loader.Get(mat);
                    if(bp != null && bp.MakeAt() == facility) 
                    {
                        if(!map.ContainsKey(mat))map.Add(mat, 0);
                        map[mat] += AllAsset[facility][mat];
                    }
                }
            }

            return map;
        }
    }
}

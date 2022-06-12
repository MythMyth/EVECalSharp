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

        public void UpdateAsset(string content, FacilityType type)
        {
            GetInstance()._UpdateAsset(content, type);
        }

        public int Get(FacilityType type, string iname)
        {
            return GetInstance()._Get(type, iname);
        }

        Dictionary<string, int> AdvCompFacility;
        Dictionary<string, int> AdvLargeShipFacility;
        Dictionary<string, int> AdvMedShipFacility;
        Dictionary<string, int> AdvSmallShipFacility;
        Dictionary<string, int> FuelAndComponentFacility;
        Dictionary<string, int> ReactionFacility;
        Dictionary<string, int> LargeShipFacility;
        Dictionary<string, int> MedShipFacility;
        Dictionary<string, int> SmallShipFacility;

        public Storage()
        {
            AdvCompFacility = new Dictionary<string, int>();
            AdvLargeShipFacility = new Dictionary<string, int>();
            AdvMedShipFacility = new Dictionary<string, int>();
            AdvSmallShipFacility = new Dictionary<string, int>();
            FuelAndComponentFacility = new Dictionary<string, int>();
            ReactionFacility= new Dictionary<string, int>();
            LargeShipFacility = new Dictionary<string, int>();
            MedShipFacility = new Dictionary<string, int>();
            SmallShipFacility = new Dictionary<string, int>();
        }

        public void _UpdateAsset(string content, FacilityType type)
        {
            Dictionary<string, int> map = new Dictionary<string, int>();
            switch(type)
            {
                case FacilityType.ADV_COMPONENT:
                    map = AdvCompFacility;
                    break;
                case FacilityType.ADV_LARGE_SHIP:
                    map = AdvLargeShipFacility;
                    break;
                case FacilityType.ADV_MED_SHIP:
                    map = AdvMedShipFacility;
                    break;
                case FacilityType.ADV_SMALL_SHIP:
                    map = AdvSmallShipFacility;
                    break;
                case FacilityType.FUEL_COMP:
                    map = FuelAndComponentFacility;
                    break;
                case FacilityType.LARGE_SHIP:
                    map = LargeShipFacility;
                    break;
                case FacilityType.MEDIUM_SHIP:
                    map = MedShipFacility;
                    break;
                case FacilityType.SMALL_SHIP:
                    map = SmallShipFacility;
                    break;
                case FacilityType.REACTION:
                    map = ReactionFacility;
                    break;
                default:
                    break;

            }
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
            Dictionary<string, int> map;
            switch(type)
            {
                case FacilityType.ADV_COMPONENT:
                    map = AdvCompFacility;
                    break;
                case FacilityType.ADV_LARGE_SHIP: 
                    map = AdvLargeShipFacility;
                    break;
                case FacilityType.ADV_SMALL_SHIP: 
                    map = AdvSmallShipFacility;
                    break;
                case FacilityType.ADV_MED_SHIP:
                    map = AdvMedShipFacility;
                    break;
                case FacilityType.FUEL_COMP:
                    map = FuelAndComponentFacility;
                    break;
                case FacilityType.LARGE_SHIP:
                    map = LargeShipFacility;
                    break;
                case FacilityType.MEDIUM_SHIP:
                    map = MedShipFacility;
                    break;
                case FacilityType.SMALL_SHIP:
                    map = SmallShipFacility;
                    break;
                case FacilityType.REACTION:
                    map = ReactionFacility;
                    break;
                default:
                    map = null;
                    break;
            }
            if(map == null) return 0;
            if(map.ContainsKey(iname))
            {
                return map[iname];
            }
            return 0;
        }
    }
}

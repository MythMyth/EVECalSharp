using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EveCal.BPs;

namespace EveCal
{
    internal class Loader
    {
        static Loader instance;
        static Mutex mutex = new Mutex();
        Dictionary<string, BP> allBP;
        Dictionary<string, double> FacilityAndRigReduce;

        const string ReduceConfigFile = "Reduction.cfg";
        static Loader GetInstance()
        {
            mutex.WaitOne();
            if(instance == null)
            {
                instance = new Loader();
            }
            mutex.ReleaseMutex();
            return instance;
        }

        public static bool Have(string bpName)
        {
            return GetInstance()._Have(bpName);
        }

        public static BP? Get(string bpName)
        {
            return GetInstance()._Get(bpName);
        }

        public static double GetReduction(string val)
        {
            return GetInstance()._GetReduction(val);
        }

        public Loader() {
            allBP = new Dictionary<string, BP>();
            FacilityAndRigReduce = new Dictionary<string, double>();
            LoadReductionConfig();

            LoadReactionFuelAndAdvComponent();
            LoadShipBPs();
        }

        void LoadReactionFuelAndAdvComponent()
        {
            string[] freactions = Directory.GetFiles("Blueprint\\Reaction");
            foreach (string filePath in freactions)
            {
                Reaction r = new Reaction(filePath);
                allBP.Add(r.GetName(), r);
            }

            string[] fadvcomp = Directory.GetFiles("Blueprint\\AdvancedComponent");
            foreach (string filePath in fadvcomp)
            {
                AdvComponent advc = new AdvComponent(filePath);
                allBP.Add(advc.GetName(), advc);
            }

            FuelBlock hellium = new FuelBlock("Hellium");
            FuelBlock hydro = new FuelBlock("Hydrogen");
            FuelBlock nitro = new FuelBlock("Nitrogen");
            FuelBlock oxy = new FuelBlock("Oxygen");

            allBP.Add("Hellium Fuel Block", hellium);
            allBP.Add("Hydrogen Fuel Block", hydro);
            allBP.Add("Nitrogen Fuel Block", nitro);
            allBP.Add("Oxygen Fuel Block", oxy);
        }

        void LoadShipBPs()
        {
            string[] shipbps = Directory.GetFiles("Blueprint\\Ship");
            foreach (string filePath in shipbps)
            {
                string indicator = filePath.Split("\\").Last().Substring(0, 2);
                switch (indicator)
                {
                    case "SB":
                        SmallShip sb = new SmallShip(filePath);
                        allBP.Add(sb.GetName(), sb);
                        break;
                    case "MB":
                        MediumShip mb = new MediumShip(filePath);
                        allBP.Add(mb.GetName(), mb);
                        break;
                    case "LB":
                        LargeShip lb = new LargeShip(filePath);
                        allBP.Add(lb.GetName(), lb);
                        break;
                    case "CB":

                        break;
                    case "SA":
                        AdvSmallShip sa = new AdvSmallShip(filePath);
                        allBP.Add(sa.GetName(), sa);
                        break;
                    case "MA":
                        AdvMediumShip ma = new AdvMediumShip(filePath);
                        allBP.Add(ma.GetName(), ma);
                        break;
                    case "LA":
                        AdvLargeShip la = new AdvLargeShip(filePath);
                        allBP.Add(la.GetName(), la);
                        break;
                    case "CA":
                        break;
                    default:
                        break;
                }
            }
        }

        void LoadReductionConfig()
        {
            if (!File.Exists(ReduceConfigFile)) return;
            string[] lines = File.ReadAllLines(ReduceConfigFile);
            FacilityAndRigReduce.Clear();
            foreach(string line in lines)
            {
                string[] parts = line.Split("=");
                if (parts.Length < 2) continue;
                string name = parts[0].Trim();
                string val = parts[1].Trim();
                if (val.Length == 0) continue;
                if (val[0] < '0' || val[0] > '9')
                {
                    if(FacilityAndRigReduce.ContainsKey(val))
                    {
                        FacilityAndRigReduce.Add(name, FacilityAndRigReduce[val]);
                    } else
                    {
                        continue;
                    }
                } else
                {
                    FacilityAndRigReduce.Add(name, double.Parse(val));
                }
            }
        }
        public bool _Have(string bpName)
        {
            return allBP.ContainsKey(bpName);
        }

        public BP? _Get(string bpName)
        {
            if(allBP.ContainsKey(bpName))
            {
                return allBP[bpName];
            }
            return null;
        }

        public double _GetReduction(string val)
        {
            if (FacilityAndRigReduce.ContainsKey(val)) return FacilityAndRigReduce[val];
            return 0;
        }
    }
}

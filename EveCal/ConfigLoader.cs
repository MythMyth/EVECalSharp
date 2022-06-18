using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal
{
    internal class ConfigLoader
    {
        static ConfigLoader instance;
        static Mutex mutex = new Mutex();
        Dictionary<string, double> FacilityAndRigReduce;
        const string ReduceConfigFile = "Reduction.cfg";

        static ConfigLoader GetInstance()
        {
            mutex.WaitOne();
            if (instance == null)
            {
                instance = new ConfigLoader();
            }
            mutex.ReleaseMutex();
            return instance;
        }
        public ConfigLoader()
        {
            FacilityAndRigReduce = new Dictionary<string, double>();
            LoadReductionConfig();
        }

        void LoadReductionConfig()
        {
            if (!File.Exists(ReduceConfigFile)) return;
            string[] lines = File.ReadAllLines(ReduceConfigFile);
            FacilityAndRigReduce.Clear();
            foreach (string line in lines)
            {
                string[] parts = line.Split("=");
                if (parts.Length < 2) continue;
                string name = parts[0].Trim();
                string val = parts[1].Trim();
                if (val.Length == 0) continue;
                if (val[0] < '0' || val[0] > '9')
                {
                    if (FacilityAndRigReduce.ContainsKey(val))
                    {
                        FacilityAndRigReduce.Add(name, FacilityAndRigReduce[val]);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    FacilityAndRigReduce.Add(name, double.Parse(val));
                }
            }
        }


        public double _GetReduction(string val)
        {
            if (FacilityAndRigReduce.ContainsKey(val)) return FacilityAndRigReduce[val];
            return 0;
        }


        public static double GetReduction(string val)
        {
            return GetInstance()._GetReduction(val);
        }
    }
}

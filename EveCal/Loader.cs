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
        Dictionary<string, BP> allBP = new Dictionary<string, BP>();
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

        public Loader() {
            string[] freactions = Directory.GetFiles("Blueprint\\Reaction");
            foreach(string filePath in freactions)
            {
                Reaction r = new Reaction(filePath);
                allBP.Add(r.GetName(), r);
            }

            string[] fadvcomp = Directory.GetFiles("Blueprint\\AdvancedComponent");
            foreach(string filePath in fadvcomp)
            {
                AdvComponent advc = new AdvComponent(filePath);
                allBP.Add(advc.GetName(), advc);
            }
        }

        public bool Have(string bpName)
        {
            return allBP.ContainsKey(bpName);
        }

        public BP? Get(string bpName)
        {
            if(allBP.ContainsKey(bpName))
            {
                return allBP[bpName];
            }
            return null;
        }
    }
}

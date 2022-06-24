using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class BP : Item 
    {
        protected double FacilityReduce, RigReduce;
        protected int output, maxRun, ME, TE;
        public Dictionary<string, int> material;
        protected FacilityType _MakeAt;
        public BP()
        {
            output = 1;
            maxRun = 1;
            FacilityReduce = 0;
            RigReduce = 0;
            ME = 0;
            TE = 0;
            material = new Dictionary<string, int>();
        }
        public Dictionary<string, int> Cal(int amount, ref int trueRun, bool round = false)
        {
            trueRun = 0;
            int run = amount / output;
            if (amount % output != 0) run++;

            Dictionary<string, int> ret = new Dictionary<string, int>();
            while(run > 0)
            {
                int partRun = (maxRun < run) ? maxRun : (round ? maxRun : run);
                trueRun += partRun;
                run -= maxRun;
                foreach(KeyValuePair<string, int> pair in material)
                {
                    int count = Math.Max( partRun, (int)Math.Ceiling( Math.Round( ( pair.Value * ((100.0 - ME) / 100.0) * ((100.0 - RigReduce) / 100.0) * ((100.0 - FacilityReduce) / 100.0)) * partRun , 2) ) );
                    if(!ret.ContainsKey(pair.Key.Trim()))
                    {
                        ret.Add(pair.Key.Trim(), 0);
                    }
                    ret[pair.Key.Trim()] += count;
                }
            }

            return ret;
        }

        public int GetOutput() { return output; }

        public FacilityType MakeAt()
        {
            return _MakeAt;
        }

    }


}

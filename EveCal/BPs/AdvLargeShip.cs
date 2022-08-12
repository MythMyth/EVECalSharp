using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class AdvLargeShip : BP
    {
        public AdvLargeShip(string fname) {
            string text = System.IO.File.ReadAllText(fname);
            fname = fname.Split("\\").Last();
            string[] lines = text.Split("\n");
            maxRun = int.Parse(lines[0]);
            ME = int.Parse(lines[1]);
            TE = int.Parse(lines[2]);
            int len = lines.Length;
            for (int i = 3; i < len; i++)
            {
                string[] pair = lines[i].Split(" x ");
                if (pair.Length < 2) continue;
                string matName = pair[0].Trim();
                int amount = int.Parse(pair[1]);
                material[matName] = amount;
            }
            name = fname.Replace("_", " ").Trim().Substring(3);
            _MakeAt = FacilityType.ADV_LARGE_SHIP;
            RigReduce = ConfigLoader.GetReduction("ADV_LARGE_RIG");
            FacilityReduce = ConfigLoader.GetReduction("ADV_LARGE_FAC");
        }

        public override int BPCNeed(int run)
        {
            return run / maxRun;
        }
    }
}

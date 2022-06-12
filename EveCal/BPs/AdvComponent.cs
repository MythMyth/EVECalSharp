using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class AdvComponent : BP
    {
        public AdvComponent(string fname)
        {
            string text = System.IO.File.ReadAllText(fname);
            fname = fname.Split("\\").Last();
            string[] lines = text.Split("\n");
            output = int.Parse(lines[0]);
            maxRun = int.Parse(lines[1]);
            int len = lines.Length;
            for(int i = 2; i < len; i++)
            {
                string[] pair = lines[i].Split(" x ");
                if(pair.Length < 2) continue;
                string matName = pair[0];
                string matNum = pair[1];
                material[matName] = int.Parse(matNum);
            }
            name = fname.Replace("_", " ");
            ME = 10;
            RigReduce = 2.6;
            _MakeAt = FacilityType.ADV_COMPONENT;
        }
    }
}

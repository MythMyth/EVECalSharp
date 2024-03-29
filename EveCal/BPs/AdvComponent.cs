﻿using System;
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
                string matName = pair[0].Trim();
                string matNum = pair[1];
                material[matName] = int.Parse(matNum);
            }
            name = fname.Replace("_", " ").Trim();
            ME = 10;
            RigReduce = ConfigLoader.GetReduction("ADV_COMP_RIG");
            FacilityReduce = ConfigLoader.GetReduction("ADV_COMP_FAC");
            _MakeAt = FacilityType.ADV_COMPONENT;
        }

        public override int BPCNeed(int run)
        {
            return run / maxRun;
        }
    }
}

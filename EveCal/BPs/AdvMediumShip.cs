﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class AdvMediumShip : BP
    {
        public AdvMediumShip(string fname)
        {
            string text = System.IO.File.ReadAllText(fname);
            fname = fname.Split("\\").Last();
            string[] lines = text.Split("\n");
            maxRun = int.Parse(lines[0]);
            ME = int.Parse(lines[1]);
            TE = int.Parse(lines[2]);
            int len = lines.Length;
            for(int i = 3; i < len; i++)
            {
                string[] pair = lines[i].Split(" x ");
                if(pair.Length < 2) continue;
                string matName = pair[0];
                int amount = int.Parse(pair[1]);
                material[matName] = amount;
            }
            name = fname.Replace("_", " ");
            RigReduce = Loader.GetReduction("ADV_MED_RIG");
            FacilityReduce = Loader.GetReduction("ADV_MED_FAC");
            _MakeAt = FacilityType.ADV_MED_SHIP;
        }

    }
}

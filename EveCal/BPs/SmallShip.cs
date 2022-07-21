﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class SmallShip : BP
    {
        public SmallShip(string fname) {
            _MakeAt = FacilityType.SMALL_SHIP;
            ME = 10;
            TE = 20;
            string[] lines = File.ReadAllLines(fname);
            name = fname.Split("\\").Last().Substring(3).Replace("_", " ").Trim();
            int len = lines.Length;
            maxRun = int.Parse(lines[0]);
            for (int i = 1; i < len; i++)
            {
                string[] part = lines[i].Split("\t");
                if (part.Length < 2) continue;
                material.Add(part[0].Trim(), int.Parse(part[1]));
            }
            RigReduce = ConfigLoader.GetReduction("SMALL_RIG");
            FacilityReduce = ConfigLoader.GetReduction("SMALL_FAC");
        }
    }
}

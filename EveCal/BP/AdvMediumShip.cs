﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BP
{
    internal class AdvMediumShip : BP
    {
        public AdvMediumShip(string fname)
        {
            string text = System.IO.File.ReadAllText("Blueprint/Ship/" + fname);
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
            RigReduce = 5.0;
            FacilityReduce = 1.0;
        }

    }
}

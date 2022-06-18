using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class Reaction : BP
    {
        public Reaction(string fname)
        {
            string text = System.IO.File.ReadAllText(fname);
            fname = fname.Split("\\").Last();
            string[] lines = text.Split("\n");
            maxRun = 100;
            RigReduce = 2.6;
            string indicator = lines[0].Trim();
            if(indicator == "CM")
            {
                output = int.Parse(lines[1]);
                material[lines[2].Trim()] = 5; //Fuel block;
                int len = lines.Length;
                for(int i = 3; i < len; i++)
                {
                    material[lines[i].Trim()] = 100;
                }
            } else 
            {
                output = 200;
                material[lines[1]] = 5;
                material[lines[2]] = 100;
                material[lines[3]] = 100;
            }
            name = fname.Replace("_", " "); 
            RigReduce = ConfigLoader.GetReduction("REACTION_RIG");
            FacilityReduce = ConfigLoader.GetReduction("REACTION_FAC");
            _MakeAt = FacilityType.REACTION;
        }
    }
}

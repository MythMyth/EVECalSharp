using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class Module :BP
    {
        public Module(string fname)
        {
            string text = System.IO.File.ReadAllText(fname);
            fname = fname.Split("\\").Last();
            string[] lines = text.Split("\n");
            maxRun = int.Parse(lines[0].Trim());
            if (lines[1].Split('\t').Length > 1)
            {
                int len = lines.Length;
                for (int i = 1; i < len; i++)
                {
                    string[] part = lines[i].Split("\t");
                    material.Add(part[0].Trim(), int.Parse(part[1]));
                }
            } else
            {
                ME = int.Parse(lines[1].Trim());
                TE = int.Parse(lines[2].Trim());
                int len = lines.Length;
                for(int i = 3; i < len; i++)
                {
                    string[] part = lines[i].Split("\t");
                    material.Add(part[0].Trim(), int.Parse(part[1]));
                }
            }
            name = fname.Replace("_", " ").Trim();
            FacilityReduce = ConfigLoader.GetReduction("MODULE_FAC");
            RigReduce = ConfigLoader.GetReduction("MODULE_RIG");
            _MakeAt = FacilityType.MODULE;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class RAM : BP
    {
        public static string[] rams = {"R.A.M.- Ammunition Tech", "R.A.M.- Armor/Hull Tech", "R.A.M.- Electronics", "R.A.M.- Energy Tech", "R.A.M.- Robotics", "R.A.M.- Shield Tech", "R.A.M.- Starship Tech", "R.A.M.- Weapon Tech" };
        public RAM(string fname)
        {
            name = fname;
            maxRun = 200;
            ME = 10;
            TE = 20;
            RigReduce = ConfigLoader.GetReduction("ADV_COMP_RIG");
            FacilityReduce = ConfigLoader.GetReduction("ADV_COMP_FAC");
            _MakeAt = FacilityType.ADV_COMPONENT;
            material.Add("Tritanium", 556);
            material.Add("Pyerite", 444);
            material.Add("Mexallon", 222);
            material.Add("Isogen", 82);
            material.Add("Nocxium", 36);
        }
    }
}

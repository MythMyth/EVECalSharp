using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class FuelBlock :BP
    {
        public FuelBlock(string fname)
        {
            output = 40;
            maxRun = 200;
            material["Robotics"] = 1;
            material["Enriched Uranium"] = 4;
            material["Mechanical Parts"] = 4;
            material["Coolant"] = 9;
            material["Strontium Clathrates"] = 20;
            material["Oxygen"] = 22;
            material["Heavy Water"] = 170;
            material["Liquid Ozone"] = 350;
            if (fname == "Hellium")
            {
                material["Helium Isotopes"] = 450;
            }
            else if (fname == "Hydrogen")
            {
                material["Hydrogen Isotopes"] = 450;
            }
            else if (fname == "Oxygen")
            {
                material["Oxygen Isotopes"] = 450;
            }
            else
            {
                material["Nitrogen Isotopes"] = 450;
            }
            ME = 10;
            TE = 20;
            _MakeAt = FacilityType.FUEL_COMP;
        }
    }
}

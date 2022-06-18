using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class AdvSmallShip:BP
    {
        public AdvSmallShip(string fname) {
            _MakeAt = FacilityType.ADV_SMALL_SHIP;

            RigReduce = ConfigLoader.GetReduction("ADV_SMALL_RIG");
            FacilityReduce = ConfigLoader.GetReduction("ADV_SMALL_FAC");
        }
    }
}

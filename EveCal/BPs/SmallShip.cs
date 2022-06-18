using System;
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
            RigReduce = Loader.GetReduction("SMALL_RIG");
            FacilityReduce = Loader.GetReduction("SMALL_FAC");
        }
    }
}

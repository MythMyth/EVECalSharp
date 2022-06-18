using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class MediumShip : BP
    {
        public MediumShip(string fname) {
            _MakeAt = FacilityType.MEDIUM_SHIP;
            RigReduce = Loader.GetReduction("MED_RIG");
            FacilityReduce = Loader.GetReduction("MED_FAC");
        }
    }
}

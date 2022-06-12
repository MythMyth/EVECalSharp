using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class LargeShip : BP
    {
        public LargeShip(string fname) {
            _MakeAt = FacilityType.LARGE_SHIP;
        }
    }
}

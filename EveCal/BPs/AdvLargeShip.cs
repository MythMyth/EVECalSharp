using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class AdvLargeShip : BP
    {
        public AdvLargeShip(string fname) {
            _MakeAt = Storage.FacilityType.ADV_LARGE_SHIP;
        }
    }
}

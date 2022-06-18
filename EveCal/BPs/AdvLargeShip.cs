﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveCal.BPs
{
    internal class AdvLargeShip : BP
    {
        public AdvLargeShip(string fname) {
            _MakeAt = FacilityType.ADV_LARGE_SHIP;
            RigReduce = ConfigLoader.GetReduction("ADV_LARGE_RIG");
            FacilityReduce = ConfigLoader.GetReduction("ADV_LARGE_FAC");
        }
    }
}

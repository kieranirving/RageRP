using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Data.Events
{
    public class Types
    {
        public enum EventTypes
        {
            ConvinienceStore,
            ClothingStore,
            MaskStore,
            VehicleStore,
            GunStore,
            Garage,
            House,
            None
        }
        public enum VehicleStore
        {
            SportsVehicle,
            PoliceVehicle,
            EMSVehicle,
            PoliceAir,
            EMSAir,
            None
        }
    }
}
using GTANetworkAPI;
using System;

namespace RageRP.DTO
{
    public class DTOVehicle : DefaultData
    {
        public long VehicleID { get; set; }
        public long CharacterID { get; set; }

        public int Handle { get; set; }

        public string carModel { get; set; }
        public int colour1 { get; set; }
        public int colour2 { get; set; }
        public string plate { get; set; }
        public string mods { get; set; }
        public bool inGarage { get; set; }
        public string trunk { get; set; }

        public string spawnName { get; set; }
    }

    public class DTOClosestVehicle
    {
        public Vehicle vehicle { get; set; }
        public float distance { get; set; }
    }
}
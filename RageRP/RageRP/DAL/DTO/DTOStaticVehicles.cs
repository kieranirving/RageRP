using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.DAL.DTO
{
    public class DTOStaticVehicles
    {
        public VehicleHash vHash { get; set; }
        public uint Hash { get; set; }
        public Vector3 location { get; set; }
        public float heading { get; set; }
        public int color1 { get; set; }
        public int color2 { get; set; }
        public string plate { get; set; }
    }
}

using GTANetworkAPI;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.Data
{
    public class StaticVehicles
    {
        public static void RenderShowVehicles()
        {
            c.WriteLine("Spawning ShowCars");
            foreach (var s in ShowCars())
            {
                if(s.Hash != uint.MinValue)
                {
                    NAPI.Vehicle.CreateVehicle(s.Hash, s.location, s.heading, s.color1, s.color2, s.plate, 255, true, false, 0);
                } 
                else
                {
                    NAPI.Vehicle.CreateVehicle(s.vHash, s.location, s.heading, s.color1, s.color2, s.plate, 255, true, false, 0);
                }
            }
            c.WriteLine("ShowCars Spawned");
        }

        private static List<DTOStaticVehicles> ShowCars()
        {
            var _vehicles = new List<DTOStaticVehicles>();
            _vehicles.Add(new DTOStaticVehicles() { vHash = VehicleHash.Adder, location = new Vector3(-49.6910f, -1092.337f, 25.78409f), heading = 116.5624f, color1 = 120, color2 = 0, plate = "PDM" });
            _vehicles.Add(new DTOStaticVehicles() { Hash = 0xC52C6B93, location = new Vector3(-45.9289f, -1094.532f, 25.78409f), heading = 116.5624f, color1 = 0, color2 = 0, plate = "PDM" }); //Dominator3
            _vehicles.Add(new DTOStaticVehicles() { vHash = VehicleHash.Massacro, location = new Vector3(-41.4727f, -1095.665f, 25.78409f), heading = 116.5624f, color1 = 88, color2 = 0, plate = "PDM" });
            _vehicles.Add(new DTOStaticVehicles() { Hash = 0xE1C03AB0, location = new Vector3(-46.8595f, -1102.112f, 25.78449f), heading = 116.5624f, color1 = 50, color2 = 0, plate = "PDM" }); //Schlagen
            return _vehicles;
        }
    }
}

using GTANetworkAPI;
using RageRP.DTO;
using System.Collections.Generic;

namespace RageRP.Server.Helpers
{
    public class VehicleHelper : Script
    {
        public static Vehicle GetClosestVehicle(Client player, float distance = 5f)
        {
            Vehicle vehicle = null;
            foreach (Vehicle veh in NAPI.Pools.GetAllVehicles())
            {
                Vector3 vehPos = veh.Position;
                float distanceVehicleToPlayer = player.Position.DistanceTo(vehPos);

                if (distanceVehicleToPlayer < distance && player.Dimension == veh.Dimension)
                {
                    distance = distanceVehicleToPlayer;
                    vehicle = veh;
                }
            }
            return vehicle;
        }

        public static List<DTOClosestVehicle> GetClosestVehicles(Client player, float distance = 20f)
        {
            List<DTOClosestVehicle> vehicles = new List<DTOClosestVehicle>();
            foreach (Vehicle veh in NAPI.Pools.GetAllVehicles())
            {
                Vector3 vehPos = veh.Position;
                float distanceVehicleToPlayer = player.Position.DistanceTo(vehPos);

                if (distanceVehicleToPlayer < distance && player.Dimension == veh.Dimension)
                {
                    vehicles.Add(new DTOClosestVehicle()
                    {
                        vehicle = veh,
                        distance = distanceVehicleToPlayer
                    });
                }
            }
            return vehicles;
        }
    }
}

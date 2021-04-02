using GTANetworkAPI;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Helpers;
using System.Collections.Generic;
using static RageRP.Data.Events.Types;

namespace RageRP.Server.Data
{
    public class Stores
    {
        public static void RenderStores()
        {
            c.WriteLine("Spawning Markers");

            foreach (var s in ConvinienceStores()) Markers.Render(s.Id, s.markerType, s.location, s.eventType, s.parameter2, "");
            foreach (var s in VehicleStores()) Markers.Render(s.Id, s.markerType, s.location, s.eventType, s.parameter2, "Press ~g~E ~w~To View Vehicle Stock");
            c.WriteLine("Markers Spawned");
        }
        public static List<DTOStores> ConvinienceStores()
        {
            var _stores = new List<DTOStores>();

            return _stores;
        }

        public static List<DTOStores> VehicleStores()
        {
            var _stores = new List<DTOStores>();

            int id = 1;
            _stores.Add(new DTOStores()
            {
                Id = id++,
                Name = "Premium Deluxe Motorsport",
                StoreName = "PDM",
                markerType = MarkerType.VerticalCylinder,
                eventType = EventTypes.VehicleStore,
                location = new Vector3(-34.2071f, -1103.478f, 25.4223),
                parameter2 = VehicleStore.SportsVehicle,
                vehicleSpawn = new Vector3(-32.4721f, -1102.8917f, 28.4878),
                vehicleSpawnHeading = 116.5624f,
                vehicleSpawnCamera = new Vector3(-37.9997f, -1104.3062f, 28.8098),
                vehicleSpawnCameraLookat = new Vector3(-37.9921f, -1104.2641f, 28.7930f)
            });

            return _stores;
        }
    }
}
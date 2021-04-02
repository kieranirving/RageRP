using GTANetworkAPI;
using static RageRP.Data.Events.Types;

namespace RageRP.Server.DAL.DTO
{
    public class DTOStores
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
        public EventTypes eventType { get; set; }
        public dynamic parameter2 { get; set; }
        public MarkerType markerType { get; set; }
        public Vector3 location { get; set; }

        public Vector3 vehicleSpawn { get; set; }
        public float vehicleSpawnHeading { get; set; }
        public Vector3 vehicleSpawnCamera { get; set; }
        public Vector3 vehicleSpawnCameraLookat { get; set; }
    }
}
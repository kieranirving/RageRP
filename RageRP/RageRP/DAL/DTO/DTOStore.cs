using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.DAL.DTO
{
    public class DTOStore
    {
        public string Name { get; set; }
        public string StoreName { get; set; }
        public List<DTOVehicleList> vehicles { get; set; }

        public Vector3 vehicleSpawn { get; set; }
        public float vehicleSpawnHeading { get; set; }
        public Vector3 vehicleSpawnCamera { get; set; }
        public Vector3 vehicleSpawnCameraLookat { get; set; }
    }
}

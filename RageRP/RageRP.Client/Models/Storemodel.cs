using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Models
{
    public class StoreModel
    {
        public string Name { get; set; }
        public string StoreName { get; set; }
        public List<VehicleListModel> vehicles { get; set; }

        public Vector3 vehicleSpawn { get; set; }
        public float vehicleSpawnHeading { get; set; }
        public Vector3 vehicleSpawnCamera { get; set; }
        public Vector3 vehicleSpawnCameraLookat { get; set; }
    }
}

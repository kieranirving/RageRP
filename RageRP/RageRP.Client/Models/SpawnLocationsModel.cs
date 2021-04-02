using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Models
{
    public class SpawnLocationsModel
    {
        public List<SpawnLocationModel> locations { get; set; }
    }

    public class SpawnLocationViewModel
    {
        public long id { get; set; }
        public string Name { get; set; }
    }

    public class SpawnLocationModel
    {
        public long id { get; set; }
        public string Name { get; set; }

        public float camPosX { get; set; }
        public float camPosY { get; set; }
        public float camPosZ { get; set; }

        public float camLookX { get; set; }
        public float camLookY { get; set; }
        public float camLookZ { get; set; }

        public float spawnX { get; set; }
        public float spawnY { get; set; }
        public float spawnZ { get; set; }
        public float heading { get; set; }
    }
}

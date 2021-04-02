using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Models
{
    public class BackgroundsModel
    {
        public List<BackgroundsModel> locations { get; set; }
    }

    public class BackgroundModel
    {
        public long id { get; set; }
        public string Name { get; set; }

        public float camPosX { get; set; }
        public float camPosY { get; set; }
        public float camPosZ { get; set; }

        public float camLookX { get; set; }
        public float camLookY { get; set; }
        public float camLookZ { get; set; }
    }
}

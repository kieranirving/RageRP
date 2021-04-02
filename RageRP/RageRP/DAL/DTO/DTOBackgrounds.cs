using RageRP.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.DAL.DTO
{
    public class DTOBackgrounds : DefaultData
    {
        public List<DTOBackground> cameras { get; set; }
    }

    public class DTOBackground
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

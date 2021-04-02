using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.DAL.DTO
{
    public class DTOReport
    {
        public int reportID { get; set; }
        public long ReportingPlayerID { get; set; }
        public long ReportedPlayerID { get; set; }
        public int ReportedHandle { get; set; }
        public int ReportingHandle { get; set; }
        public string Reason { get; set; }
        public string ReportedPlayer { get; set; }
        public string ReportingPlayer { get; set; }
        public DateTime ReportedAt { get; set; }
        public bool isAssigned { get; set; }
        public int AssignedTo { get; set; }
    }
}
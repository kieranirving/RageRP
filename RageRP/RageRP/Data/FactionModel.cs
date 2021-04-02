using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.Data
{
    public class FactionModel
    {
        public Client currentPlayer { get; set; }
        public List<Client> clients { get; set; }
        public string rank { get; set; }
        public bool isCommand { get; set; }
        public bool isPolice { get; set; }
        public bool isEMS { get; set; }
        public int PoliceLevel { get; set; }
        public int EMSLevel { get; set; }
        public bool hasError { get; set; }
    }
}

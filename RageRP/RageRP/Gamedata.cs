
using RageRP.DTO;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Helpers;
using System.Collections.Generic;

namespace RageRP.Server
{
    public static class Gamedata
    {
        private static bool _isDebug;
        public static bool isDebug
        {
            get
            {
                return _isDebug;
            }
            set
            {
                _isDebug = value;
                c.WriteLine($"isDebug set to {value}");
            }
        }
        private static string _version;
        public static string version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                c.WriteLine($"version set to {value}");
            }
        }
        private static int _StartingCash;
        public static int StartingCash
        {
            get
            {
                return _StartingCash;
            }
            set
            {
                _StartingCash = value;
                c.WriteLine($"StartingCash set to {value}");
            }
        }
        private static int _StartingBank;
        public static int StartingBank
        {
            get
            {
                return _StartingBank;
            }
            set
            {
                _StartingBank = value;
                c.WriteLine($"StartingBank set to {value}");
            }
        }

        private static string _WebsiteURL;
        public static string WebsiteURL
        {
            get
            {
                return _WebsiteURL;
            }
            set
            {
                _WebsiteURL = value;
                c.WriteLine($"WebsiteURL set to {value}");
            }
        }

        private static int _creationDimension;
        public static int CreationDimension
        {
            get
            {
                return _creationDimension;
            }
            set
            {
                _creationDimension = value;
            }
        }

        public static HashSet<DTOReport> reports;
        public static int reportCount;

        public static bool isOOCActive { get; set; }

        public static HashSet<DTOPlayer> playerList;
        public static HashSet<DTOVehicle> vehicleList;

        private static List<string> _playerCommands;
        public static List<string> PlayerCommands
        {
            get
            {
                return _playerCommands;
            }
            set
            {
                if (_playerCommands == null)
                    _playerCommands = value;
            }
        }

        private static List<string> _adminCommands;
        public static List<string> AdminCommands
        {
            get
            {
                return _adminCommands;
            }
            set
            {
                if (_adminCommands == null)
                    _adminCommands = value;
            }
        }

        private static List<string> _factionCommands;
        public static List<string> FactionCommands
        {
            get
            {
                return _factionCommands;
            }
            set
            {
                if (_factionCommands == null)
                    _factionCommands = value;
            }
        }

        private static List<string> _groupCommands;
        public static List<string> GroupCommands
        {
            get
            {
                return _groupCommands;
            }
            set
            {
                if(_groupCommands == null)
                    _groupCommands = value;
            }
        }
    }
}
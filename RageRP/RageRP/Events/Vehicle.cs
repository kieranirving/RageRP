using GTANetworkAPI;
using RageRP.DTO;
using RageRP.Server.Helpers;
using RageRP.Server.Services;
using System;
using System.Linq;

namespace RageRP.Server.Events
{
    public class Vehicle : Script
    {
        private PlayerService _playerService;
        private SpawnService _spawnService;

        public Vehicle()
        {
            _playerService = new PlayerService();
            _spawnService = new SpawnService();
        }

        #region Commands
        [Command("lock")]
        [RemoteEvent("lockvehicle")]
        public void LockVehicle(Client player)
        {
            //Get the closest vehicles to us
            var vehicles = VehicleHelper.GetClosestVehicles(player);
            var currentPlayer = Gamedata.playerList.Where(x => x.Handle == player.Handle.Value).FirstOrDefault();
            if (vehicles.Count != 0 && currentPlayer != null)
            {
                var ownedVehicles = vehicles.Where(x => x.vehicle.Handle.Value == Gamedata.vehicleList.Where(y => y.Handle == x.vehicle.Handle.Value && y.CharacterID == currentPlayer.currentCharacter.CharacterID).FirstOrDefault().Handle);
                if (ownedVehicles == null)
                    return;

                var _ownedVehicles = ownedVehicles.ToList();
                if (_ownedVehicles.Count() != 0)
                {
                    DTOClosestVehicle closestVehicle = _ownedVehicles[0];
                    foreach (var veh in _ownedVehicles)
                    {
                        if (veh.distance < closestVehicle.distance)
                        {
                            closestVehicle = veh;
                        }
                    }
                    bool locked = false; string lockStatus = "";
                    if (NAPI.Vehicle.GetVehicleLocked(closestVehicle.vehicle))
                    {
                        locked = false;
                        lockStatus = "unlocked";
                    }
                    else
                    {
                        locked = true;
                        lockStatus = "locked";
                    }
                    NAPI.Vehicle.SetVehicleLocked(closestVehicle.vehicle, locked);
                    player.SendChatMessage($"You have {lockStatus} your {closestVehicle.vehicle.DisplayName}");
                }
            }
        }

        [Command("engine", Alias = "e")]
        public void ToggleEngine(Client player)
        {
            if(player.Vehicle != null)
            {
                if(player.VehicleSeat == -1)
                {
                    string CharacterName = player.GetSharedData("DisplayName");
                    if(!player.Vehicle.EngineStatus)
                    {
                        player.Vehicle.EngineStatus = true;
                        player.Vehicle.SetSharedData("isEngineOn", true);
                    }
                    else
                    {
                        player.Vehicle.EngineStatus = false;
                        player.Vehicle.SetSharedData("isEngineOn", false);
                    }
                }
            }
        }
        #endregion

        #region Events
        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Client player, GTANetworkAPI.Vehicle vehicle, sbyte seatID)
        {
            if(!vehicle.EngineStatus)
            {
                player.SendChatMessage($"!{{#fff83d}} [INFO] Start the engine with /e or /engine");
            }
        }
        #endregion

        #region SyncEvents
        [RemoteEvent("SirenSync")]
        public void SirenSync(Client player)
        {
            c.WriteLine("SirenSync");
            if (player.Vehicle != null)
            {
                var siren = player.Vehicle.GetSharedData("isSirenPlaying");
                if (siren == null)
                    siren = false;
                else
                    siren = !siren;

                string sirenType = player.Vehicle.GetSharedData("sirenType");
                //We only want to handle this now if its null
                if (string.IsNullOrEmpty(sirenType))
                {
                    sirenType = "VEHICLES_HORNS_SIREN_1";
                    player.Vehicle.SetSharedData("sirenType", sirenType);
                    player.Vehicle.SetSharedData("sirenTypeID", 1);
                }

                player.Vehicle.SetSharedData("isSirenPlaying", siren);
            }
        }

        [RemoteEvent("SirenTypeSync")]
        public void SirenTypeSync(Client player)
        {
            c.WriteLine("SirenTypeSync");
            if (player.Vehicle != null)
            {
                string siren = player.Vehicle.GetSharedData("sirenType");
                var sirenTypeID = player.Vehicle.GetSharedData("sirenTypeID");
                if (string.IsNullOrEmpty(siren))
                {
                    siren = "VEHICLES_HORNS_SIREN_1";
                    sirenTypeID = 1;
                }

                bool isPolice = Convert.ToBoolean(player.Vehicle.GetSharedData("isPolice"));
                bool isFire = Convert.ToBoolean(player.Vehicle.GetSharedData("isFire"));
                bool isEMS = Convert.ToBoolean(player.Vehicle.GetSharedData("isEMS"));

                switch (sirenTypeID)
                {
                    case 1:
                        siren = "VEHICLES_HORNS_SIREN_2";
                        sirenTypeID = 2;
                        break;
                    case 2:
                        if (isPolice)
                        {
                            siren = "VEHICLES_HORNS_POLICE_WARNING";
                        }
                        if (isFire)
                        {
                            siren = "VEHICLES_HORNS_FIRETRUCK_WARNING";
                        }
                        if (isEMS)
                        {
                            siren = "VEHICLES_HORNS_AMBULANCE_WARNING";
                        }
                        sirenTypeID = 3;
                        break;
                    case 3:
                        siren = "VEHICLES_HORNS_SIREN_1";
                        sirenTypeID = 1;
                        break;
                    default:
                        //Do nothing because we handled it above
                        break;
                }
                player.Vehicle.SetSharedData("sirenType", siren);
                player.Vehicle.SetSharedData("sirenTypeID", sirenTypeID);
            }
        }

        [RemoteEvent("LightbarSync")]
        public void LightbarSync(Client player)
        {
            c.WriteLine("LightbarSync");
            if (player.Vehicle != null)
            {
                var lightbar = player.Vehicle.GetSharedData("isLightbarOn");
                if (lightbar == null)
                    lightbar = false;
                else
                    lightbar = !lightbar;

                player.Vehicle.SetSharedData("isLightbarOn", lightbar);
            }
        }

        [RemoteEvent("HornSync")]
        public void HornSync(Client player)
        {
            c.WriteLine("HornSync");
            if (player.Vehicle != null)
            {
                var horn = player.Vehicle.GetSharedData("isHornPlaying");
                if (horn == null)
                    horn = true;
                else
                    horn = !horn;

                player.Vehicle.SetSharedData("isHornPlaying", horn);
            }
        }

        [RemoteEvent("TakedownSync")]
        public void TakedownPlay(Client player)
        {
            c.WriteLine("TakedownSync");
            if (player.Vehicle != null)
            {
                var takedown = player.Vehicle.GetSharedData("isTakeDownPlaying");
                if (takedown == null)
                    takedown = true; //Needs to be true otherwise the siren starts playing immediately and fucks everything up.
                else
                    takedown = !takedown;

                player.Vehicle.SetSharedData("isTakeDownPlaying", takedown);
            }
        }

        [RemoteEvent("SoundIDSync")]
        public void SoundIDSync(Client player, int Handle, string dataName, int soundid)
        {
            c.WriteLine($"SoundIDSync {Handle} {dataName} {soundid}");
            var vehicles = NAPI.Pools.GetAllVehicles();
            vehicles.Where(x => x.Handle.Value == Handle).FirstOrDefault().SetSharedData(dataName, soundid);
            //player.Vehicle.SetSharedData(dataName, soundid);
        }
        #endregion
    }
}
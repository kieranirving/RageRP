using GTANetworkAPI;
using Newtonsoft.Json;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Data;
using RageRP.Server.Helpers;
using System;
using System.Collections.Generic;
using static RageRP.Data.Events.Types;

namespace RageRP.Server.Events
{
    public class ColShapeEvent : Script
    {
        private int Id { get; set; }
        private EventTypes type { get; set; }
        private dynamic p2 { get; set; }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void EnterColShape(ColShape col, Client player)
        {
            Id = Convert.ToInt32(col.GetSharedData("Id"));
            type = (EventTypes)col.GetSharedData("Type");
            p2 = col.GetSharedData("p2");
            player.SetSharedData("EnteredColShape", true);
            player.SetSharedData("EnteredColShapeId", Id);
        }

        [RemoteEvent("OpenStore")]
        private void OpenStore(Client player)
        {
            if(Id != 0 && type != EventTypes.None && p2 != null)
            {
                DTOStore _store = new DTOStore();
                List<DTOVehicleList> _vehicles = null;
                switch (type)
                {
                    case EventTypes.ConvinienceStore:
                    case EventTypes.ClothingStore:
                    case EventTypes.MaskStore:
                    case EventTypes.GunStore:
                    case EventTypes.Garage:
                    case EventTypes.House:
                        break;
                    case EventTypes.VehicleStore:
                        VehicleStore choice = (VehicleStore)p2;
                        var store = Stores.VehicleStores().Find(x => x.Id == Id);
                        if (choice == VehicleStore.PoliceVehicle)
                            _vehicles = VehicleStores.PoliceVehicles();
                        else if (choice == VehicleStore.PoliceAir)
                            _vehicles = VehicleStores.PoliceHelicopters();
                        else if (choice == VehicleStore.EMSVehicle)
                            _vehicles = VehicleStores.EMSVehicles();
                        else if (choice == VehicleStore.EMSAir)
                            _vehicles = VehicleStores.EMSHelicopters();
                        else if (choice == VehicleStore.SportsVehicle)
                            _vehicles = VehicleStores.SportsVehicles();
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [ERROR] Failed to get Vehicle Data for store {Id}. ErrRef: fruitpositionkingdomgoneverb");
                            return;
                        }
                        _store.Name = store.Name;
                        _store.StoreName = store.StoreName;
                        _store.vehicles = _vehicles;
                        _store.vehicleSpawn = store.vehicleSpawn;
                        _store.vehicleSpawnCamera = store.vehicleSpawnCamera;
                        _store.vehicleSpawnCameraLookat = store.vehicleSpawnCameraLookat;
                        _store.vehicleSpawnHeading = store.vehicleSpawnHeading;
                        break;
                    default:
                        player.SendChatMessage($"!{{#ff847c}} [ERROR] Failed to get marker type for Id {Id}. ErrRef: statewillsavestreetsense");
                        return;
                }

                string jsonString = JsonConvert.SerializeObject(_store);
                player.TriggerEvent("OpenStore", jsonString);
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        public void ExitColShape(ColShape col, Client player)
        {
            Id = 0;
            type = EventTypes.None;
            p2 = null;
            player.SetSharedData("EnteredColShape", false);
            player.SetSharedData("EnteredColShapeId", 0);
            //var type = (EventTypes)col.GetSharedData("Type");
            //switch (type)
            //{
            //    case EventTypes.ConvinienceStore:
            //    case EventTypes.ClothingStore:
            //    case EventTypes.MaskStore:
            //    case EventTypes.GunStore:
            //    case EventTypes.Garage:
            //    case EventTypes.House:
            //        break;
            //    case EventTypes.VehicleStore:
            //        break;
            //    default:
            //        break;
            //}
        }

        [Command("marker", GreedyArg = true)]
        public void CreateMaker(Client player, string message)
        {
            //NAPI.TextLabel.CreateTextLabel(message, player.Position, 3, 1f, 4, new Color(255, 0, 0, 255), dimension:0);

            foreach (var s in Stores.VehicleStores()) Markers.Render(s.Id, s.markerType, player.Position, s.eventType, s.parameter2, "Press ~g~E ~w~To View Vehicle Stock");
        }
    }
}

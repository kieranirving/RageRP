using RAGE;
using RAGE.Elements;
using RageRP.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RAGE.Events;

namespace RageRP.Client.Vehicle
{
    class Vehicle : Events.Script
    {
        private bool isLockPressed = false;
        private bool isLightsPressed = false;
        private bool isSirenPressed = false;
        private bool isHornPressed = false;
        private bool isTakedownPressed = false;
        private bool isSirenTypeChangePressed = false;

        public Vehicle()
        {
            AddDataHandler("isLightbarOn", lightbarHandler);
            AddDataHandler("isSirenPlaying", sirenHandler);
            AddDataHandler("isTakeDownPlaying", takedownHandler);
            AddDataHandler("sirenType", sirenTypeHandler);
            AddDataHandler("isHornPlaying", hornHandler);

            AddDataHandler("vehicleHandleID", vehicleIDHandler);

            Events.Tick += Tick;
            Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;
            Events.OnEntityStreamIn += OnEntityStreamIn;
        }

        private async void lightbarHandler(Entity entity, object arg)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;

            if (vehicle.GetClass() != 18)
                return;

            bool lightbar = Convert.ToBoolean(vehicle.GetSharedData("isLightbarOn"));
            bool siren = Convert.ToBoolean(vehicle.GetSharedData("isSirenPlaying"));

            vehicle.SetSiren(lightbar);

            if (!lightbar && siren)
            {
                Events.CallRemote("SirenSync");
            }

            //Disable the native GTAV Sirens
            vehicle.SetSirenSound(true);
        }

        private async void sirenHandler(Entity entity, object arg)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;

            if (vehicle.GetClass() != 18)
                return;

            try
            {
                bool state = Convert.ToBoolean(vehicle.GetSharedData("isSirenPlaying"));
                string type = vehicle.GetSharedData("sirenType").ToString();
                bool lightbar = Convert.ToBoolean(vehicle.GetSharedData("isLightbarOn"));
                
                if (!lightbar && state)
                {
                    Events.CallRemote("LightbarSync");
                }

                int sharedSirenId = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-sirenSoundID"));
                
                playSirenSound(vehicle, state, type, "sirenHandler", sharedSirenId);
            }
            catch(Exception ex)
            {
                Chat.Output(ex.Message);
            }
            

            //Disable the native GTAV Sirens
            vehicle.SetSirenSound(true);
        }

        private async void sirenTypeHandler(Entity entity, object arg)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;

            if (vehicle.GetClass() != 18)
                return;

            string siren = vehicle.GetSharedData("sirenType").ToString();
            bool sirenState = Convert.ToBoolean(vehicle.GetSharedData("isSirenPlaying"));
            int sirenSoundID = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-sirenSoundID"));
            if (sirenState && !isHornPressed)
            {
                RAGE.Game.Audio.StopSound(sirenSoundID);
                RAGE.Game.Audio.PlaySoundFromEntity(sirenSoundID, siren, vehicle.Handle, "", true, 0);
            }
        }

        private async void playSirenSound(RAGE.Elements.Vehicle vehicle, bool state, string type, string stack, int sharedSoundId)
        {
            try
            {
                //var sharedSoundId = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-sirenSoundID"));
                if (state)
                {
                    RAGE.Game.Audio.PlaySoundFromEntity(sharedSoundId, type, vehicle.Handle, "", true, 0);
                }
                else
                {
                    RAGE.Game.Audio.StopSound(sharedSoundId);
                }
            }
            catch (Exception ex)
            {
                //TODO Replace this with an error number
                Chat.Output(ex.Message);
            }
        }

        private async void takedownHandler(Entity entity, object arg)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;
            
            if (vehicle.GetClass() != 18)
                return;
            
            bool state = Convert.ToBoolean(vehicle.GetSharedData("isTakeDownPlaying"));
            try
            {
                int soundid = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-takedownSoundID"));
                if (state)
                {
                    RAGE.Game.Audio.PlaySoundFromEntity(soundid, "VEHICLES_HORNS_SIREN_1", vehicle.Handle, "", true, 0);
                }
                else
                {
                    RAGE.Game.Audio.StopSound(soundid);
                }
            }
            catch(Exception ex)
            {
                //TODO Replace this with an error number
                Chat.Output(ex.Message);
            }
        }

        private async void hornHandler(Entity entity, object arg)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;

            if (vehicle.GetClass() != 18)
                return;

            bool state = Convert.ToBoolean(vehicle.GetSharedData("isHornPlaying"));
            try
            {
                int sirenSoundId = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-sirenSoundID"));
                bool isSirenPlaying = Convert.ToBoolean(vehicle.GetSharedData("isSirenPlaying"));
                int hornID = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-hornID"));
                //Chat.Output($"Horn siren {isSirenPlaying} {state} {hornID}");
                if (state)
                {
                    if (isSirenPlaying)
                    {
                        RAGE.Game.Audio.StopSound(sirenSoundId);
                        RAGE.Game.Audio.PlaySoundFromEntity(hornID, "SIRENS_AIRHORN", vehicle.Handle, "", true, 0);
                    }
                    else
                    {
                        RAGE.Game.Audio.PlaySoundFromEntity(hornID, "SIRENS_AIRHORN", vehicle.Handle, "", true, 0);
                    }
                }
                else
                {
                    if (hornID != -1)
                    {
                        RAGE.Game.Audio.StopSound(hornID);
                        if(isSirenPlaying)
                        {
                            string type = vehicle.GetSharedData("sirenType").ToString();
                            playSirenSound(vehicle, isSirenPlaying, type, "hornHandler", sirenSoundId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO Replace this with an error number
                Chat.Output(ex.Message);
            }
        }

        private async void vehicleIDHandler(Entity entity, object arg)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;

            if (vehicle.GetClass() != 18)
                return;

            int vehicleHandleID = Convert.ToInt32(vehicle.GetSharedData("vehicleHandleID"));
            bool isEmergency = Convert.ToBoolean(vehicle.GetSharedData("isEmergency"));
            if (vehicleHandleID != 0 && isEmergency)
            {
                int hornid = await GetSoundIDAsync();
                int sirenSoundID = await GetSoundIDAsync();
                int takedownSoundID = await GetSoundIDAsync();
                Events.CallRemote("SoundIDSync", vehicleHandleID, $"{Globals.Name}-hornID", hornid);
                Events.CallRemote("SoundIDSync", vehicleHandleID, $"{Globals.Name}-sirenSoundID", sirenSoundID);
                Events.CallRemote("SoundIDSync", vehicleHandleID, $"{Globals.Name}-takedownSoundID", takedownSoundID);
            }
        }

        private async void OnPlayerEnterVehicle(RAGE.Elements.Vehicle vehicle, int seatId)
        {
            if(seatId == -1)
            {
                bool isEngineOn = Convert.ToBoolean(vehicle.GetSharedData("isEngineOn"));
                if(!isEngineOn)
                {
                    vehicle.SetEngineOn(false, true, false);
                }
            }
        }

        private async void OnEntityStreamIn(Entity entity)
        {
            if (entity.Type != RAGE.Elements.Type.Vehicle)
                return;

            RAGE.Elements.Vehicle vehicle = (RAGE.Elements.Vehicle)entity;

            if (vehicle.GetClass() != 18)
                return;

            try
            {
                var lightbar = vehicle.GetSharedData("isLightbarOn");
                var siren = vehicle.GetSharedData("isSirenPlaying");
                var sirenType = vehicle.GetSharedData("sirenType");

                int sharedSirenId = Convert.ToInt32(vehicle.GetSharedData($"{Globals.Name}-sirenSoundID"));

                if (siren != null && sirenType != null)
                    playSirenSound(vehicle, (bool)siren, sirenType.ToString(), "entity", sharedSirenId);

                vehicle.SetSiren(lightbar == null ? false : Convert.ToBoolean(lightbar));
                vehicle.SetSirenSound(true);

                int vehicleHandleID = Convert.ToInt32(vehicle.GetSharedData("vehicleHandleID"));
                bool isEmergency = Convert.ToBoolean(vehicle.GetSharedData("isEmergency"));
                if (vehicleHandleID != 0 && isEmergency)
                {
                    int hornid = await GetSoundIDAsync();
                    int sirenSoundID = await GetSoundIDAsync();
                    int takedownSoundID = await GetSoundIDAsync();

                    Events.CallRemote("SoundIDSync", vehicleHandleID, $"{Globals.Name}-hornID", hornid);
                    Events.CallRemote("SoundIDSync", vehicleHandleID, $"{Globals.Name}-sirenSoundID", sirenSoundID);
                    Events.CallRemote("SoundIDSync", vehicleHandleID, $"{Globals.Name}-takedownSoundID", takedownSoundID);
                }
            }
            catch (Exception ex)
            {
                //TODO Replace this with an error number
                Chat.Output(ex.Message);
            }
        }

        public async void Tick(List<Events.TickNametagData> nametags)
        {
            //Check if the player is spawned
            if (!Globals.isSpawned)
                return;
            //Check if the chat is open
            if (Globals.isChatOpen)
                return;

            //Lock & Unlock
            if (RAGE.Input.IsDown((int)ConsoleKey.U) && !isLockPressed)
            {
                isLockPressed = true;
                CallRemote("lockvehicle");
            }
            if (!RAGE.Input.IsDown((int)ConsoleKey.U) && isLockPressed)
                isLockPressed = false;

            //Check if the player is in a vehicle
            var _vehicle = RAGE.Elements.Player.LocalPlayer.Vehicle;
            if (_vehicle != null)
            {
                //Check that the vehicle is in the "Emergency" Class
                if (_vehicle.GetClass() == 18)
                {
                    //Disable radio control
                    RAGE.Game.Audio.SetUserRadioControlEnabled(false);
                    RAGE.Game.Audio.SetRadioToStationName("OFF");

                    //Disable inputs
                    RAGE.Game.Pad.DisableControlAction(0, 86, true);
                    RAGE.Game.Pad.DisableControlAction(0, 85, true);
                    RAGE.Game.Pad.DisableControlAction(0, 80, true);

                    //Siren Controls
                    if (RAGE.Input.IsDown((int)ConsoleKey.Q) && !isLightsPressed)
                    {
                        isLightsPressed = true;
                        //Check that the player is in the driver seat
                        if (_vehicle.GetPedInSeat(-1, 0) == RAGE.Elements.Player.LocalPlayer.Handle)
                        {
                            RAGE.Game.Audio.PlaySoundFrontend(-1, "Retune_High", "MP_RADIO_SFX", true);
                            Events.CallRemote("LightbarSync");
                        }
                    }
                    if (!RAGE.Input.IsDown((int)ConsoleKey.Q) && isLightsPressed)
                        isLightsPressed = false;

                    //Siren Type Change Controls
                    if (RAGE.Input.IsDown((int)ConsoleKey.OemPlus) && !isSirenTypeChangePressed)
                    {
                        isSirenTypeChangePressed = true;
                        //Check that the player is in the driver seat
                        if (_vehicle.GetPedInSeat(-1, 0) == RAGE.Elements.Player.LocalPlayer.Handle)
                        {
                            RAGE.Game.Audio.PlaySoundFrontend(-1, "Retune_High", "MP_RADIO_SFX", true);
                            Events.CallRemote("SirenTypeSync");
                        }
                    }
                    if (!RAGE.Input.IsDown((int)ConsoleKey.OemPlus) && isSirenTypeChangePressed)
                        isSirenTypeChangePressed = false;

                    //Lightbar Controls
                    if (RAGE.Input.IsDown((int)ConsoleKey.N) && !isSirenPressed && !Globals.isChatOpen)
                    {
                        isSirenPressed = true;
                        //Check that the player is in the driver seat
                        if (_vehicle.GetPedInSeat(-1, 0) == RAGE.Elements.Player.LocalPlayer.Handle)
                        {
                            RAGE.Game.Audio.PlaySoundFrontend(-1, "Retune_High", "MP_RADIO_SFX", true);
                            Events.CallRemote("SirenSync");
                        }
                    }
                    if (!RAGE.Input.IsDown((int)ConsoleKey.N) && isSirenPressed && !Globals.isChatOpen)
                    {
                        //await Task.Delay(100);//Might need this
                        isSirenPressed = false;
                    }

                    //Takedown controls
                    if (RAGE.Input.IsDown((int)ConsoleKey.R) && !isTakedownPressed)
                    {
                        if(!Convert.ToBoolean(_vehicle.GetSharedData("isSirenPlaying")))
                        {
                            isTakedownPressed = true;
                            //Check that the player is in the driver seat
                            if (_vehicle.GetPedInSeat(-1, 0) == RAGE.Elements.Player.LocalPlayer.Handle)
                            {
                                RAGE.Game.Audio.PlaySoundFrontend(-1, "Retune_High", "MP_RADIO_SFX", true);
                                Events.CallRemote("TakedownSync");
                            }
                        }
                    }
                    if (!RAGE.Input.IsDown((int)ConsoleKey.R) && isTakedownPressed)
                    {
                        //await Task.Delay(100);//Might need this
                        isTakedownPressed = false;
                        Events.CallRemote("TakedownSync");
                    }

                    //Horn Controls
                    if (RAGE.Input.IsDown((int)ConsoleKey.E) && !isHornPressed)
                    {
                        isHornPressed = true;
                        //Check that the player is in the driver seat
                        if (_vehicle.GetPedInSeat(-1, 0) == RAGE.Elements.Player.LocalPlayer.Handle)
                        {
                            Events.CallRemote("HornSync");
                        }
                    }
                    if (!RAGE.Input.IsDown((int)ConsoleKey.E) && isHornPressed)
                    {
                        //await Task.Delay(100);//Might need this
                        isHornPressed = false;
                        Events.CallRemote("HornSync");
                    }
                }
            }
        }

        public async Task<int> GetSoundIDAsync()
        {
            return RAGE.Game.Audio.GetSoundId();
        }
    }
}
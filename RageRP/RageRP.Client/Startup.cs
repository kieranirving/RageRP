using System;
using Newtonsoft.Json;
using RAGE;
using RAGE.Game;
using RageRP.Client.Models;

namespace RageRP.Client
{
    class Startup : Events.Script
    {
        private int SetupCamera;
        public Startup()
        {
            Console.WriteLine("RageRP Client Started");

            Discord.Update("Playing RageRP - Dev Server", "");


            RAGE.Game.Vehicle.DisplayDistantVehicles(true);
            RAGE.Game.Graphics.DisableVehicleDistantlights(false);
            RAGE.Game.Misc.SetFadeOutAfterDeath(false);


            Events.Add("startupCamera", SetStartupCamera);
            Events.Add("closeStartupCamera", CloseStartupCamera);
        }

        private async void SetStartupCamera(object[] args)
        {
            var pID = RAGE.Game.Player.PlayerId();

            var background = JsonConvert.DeserializeObject<BackgroundModel>(args[0].ToString());

            SetupCamera = Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), background.camPosX, background.camPosY, background.camPosZ, 180f, 0f, 0f, 90.0f, true, 0);

            Cam.PointCamAtCoord(SetupCamera, background.camLookX, background.camLookY, background.camLookZ);
            Cam.SetCamActive(SetupCamera, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);

            Ui.DisplayHud(false);
            Ui.DisplayRadar(false);
            Chat.Activate(false);
            Chat.Show(false);

            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(background.camPosX, background.camPosY, background.camPosZ);

            RAGE.Elements.Player.LocalPlayer.FreezePosition(true);
            RAGE.Elements.Player.LocalPlayer.SetInvincible(true);
        }

        private void CloseStartupCamera(object[] args)
        {
            Cam.DestroyCam(SetupCamera, true);
            Cam.SetCamActive(SetupCamera, false);
            Cam.RenderScriptCams(false, false, 0, true, false, 0);

            Ui.DisplayHud(true);
            Chat.Activate(true);
            Chat.Show(true);

            //RAGE.Elements.Player.LocalPlayer.FreezePosition(false);
            //RAGE.Elements.Player.LocalPlayer.SetInvincible(false);
        }
    }
}
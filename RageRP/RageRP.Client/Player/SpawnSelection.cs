using Newtonsoft.Json;
using RAGE;
using RAGE.Game;
using RageRP.Client.Helpers;
using RageRP.Client.Models;
using System;
using System.Collections.Generic;
using static RageRP.Client.Helpers.BrowserHelper;

namespace RageRP.Client.Player
{
    public class SpawnSelection : Events.Script
    {
        private static int _browserId;
        private static int _camera;
        private static SpawnLocationsModel _spawnLocations;

        public SpawnSelection()
        {
            _browserId = 0;

            //CEF Events
            Events.Add("ChangeLocation", ChangeLocation);
            Events.Add("SelectSpawn", SelectSpawn);

            //Server Events
            Events.Add("ShowSpawnSelections", ShowSpawnSelections);
        }

        public static void ShowSpawnSelections(object[] args)
        {
            var spawnLocations = JsonConvert.DeserializeObject<SpawnLocationsModel>(args[0].ToString());
            _spawnLocations = spawnLocations;

            var firstLocation = spawnLocations.locations[0];
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(firstLocation.camPosX, firstLocation.camPosY, firstLocation.camPosZ);
            _camera = Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), firstLocation.camPosX, firstLocation.camPosY, firstLocation.camPosZ, -185f, 0f, 0f, 90.0f, true, 0);
            Cam.PointCamAtCoord(_camera, firstLocation.camLookX, firstLocation.camLookY, firstLocation.camLookZ);
            Cam.SetCamActive(_camera, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);

            Ui.DisplayHud(false);
            Ui.DisplayRadar(false);
            Chat.Activate(false);
            Chat.Show(false);

            RAGE.Elements.Player.LocalPlayer.SetAlpha(0, false);

            RAGE.Elements.Player.LocalPlayer.SetDefaultComponentVariation();

            RAGE.Elements.Player.LocalPlayer.FreezePosition(true);
            RAGE.Elements.Player.LocalPlayer.SetInvincible(true);

            var _spawnViewModel = new List<SpawnLocationViewModel>();
            foreach(var s in spawnLocations.locations)
            {
                _spawnViewModel.Add(new SpawnLocationViewModel()
                {
                    id = s.id,
                    Name = s.Name
                });
            }

            string jsonString = JsonConvert.SerializeObject(_spawnViewModel);

            _browserId = BrowserHelper.CreateBrowser(new windowObject()
            {
                url = "package://cs_packages/RageRP/html/spawnSelection.html",
                function = BrowserFunctions.SPAWNSELECT_INIT,
                parameters = new object[]
                {
                    Json.Escape(jsonString)
                }
            });
        }

        public static void ChangeLocation(object[] args)
        {
            int id = Convert.ToInt32(args[0].ToString());
            var location = _spawnLocations.locations.Find(x => x.id == id);
            Cam.SetCamCoord(_camera, location.camPosX, location.camPosY, location.camPosZ);
            Cam.PointCamAtCoord(_camera, location.camLookX, location.camLookY, location.camLookZ);
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(location.camPosX, location.camPosY, location.camPosZ);
        }

        public static void SelectSpawn(object[] args)
        {
            int id = Convert.ToInt32(args[0].ToString());
            var location = _spawnLocations.locations.Find(x => x.id == id);
            RAGE.Elements.Player.LocalPlayer.SetAlpha(255, false);

            BrowserHelper.DestroyBrowser(_browserId);

            Cam.DestroyCam(_camera, true);
            Cam.SetCamActive(_camera, false);
            Cam.RenderScriptCams(false, false, 0, true, false, 0);

            Ui.DisplayHud(true);
            Chat.Activate(true);
            Chat.Show(true);

            RAGE.Elements.Player.LocalPlayer.FreezePosition(false);
            RAGE.Elements.Player.LocalPlayer.SetInvincible(false);
            
            RAGE.Elements.Player.LocalPlayer.Position = new Vector3(location.spawnX, location.spawnY, location.spawnZ);
            RAGE.Elements.Player.LocalPlayer.SetHeading(location.heading);

            Globals.isSpawned = true;

            _spawnLocations = null;

            //Start the HUD Elements
            Globals.Cash = Convert.ToInt32(RAGE.Elements.Player.LocalPlayer.GetSharedData("Cash"));
            Events.CallLocal("StartVoiceOverlay");
            Events.CallLocal("StartMoneyOverlay");
        }
    }
}
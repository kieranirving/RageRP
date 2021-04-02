using Newtonsoft.Json;
using RAGE;
using RAGE.Game;
using RAGE.Ui;
using RageRP.Client.Helpers;
using RageRP.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static RAGE.Events;
using static RageRP.Client.Helpers.BrowserHelper;

namespace RageRP.Client.Player
{
    class Store : Events.Script
    {
        private int _demoVehicle;
        private int _browserId;
        private bool isInteractPressed = false;
        private static int _camera;
        private Vector3 _currentSpawn;
        private float _spawnHeading;
        public Store()
        {
            _browserId = 0;

            Events.Add("OpenStore", OpenStore);
            Events.Add("HideStore", HideStore);
            Events.Add("ViewVehicle", ViewVehicle);
            Events.Tick += Tick;
        }

        private void OpenStore(object[] args)
        {
            if (_browserId == 0)
            {
                var model = JsonConvert.DeserializeObject<StoreModel>(args[0].ToString());

                Chat.Output($"Name: {model.Name}");
                Chat.Output($"StoreName: {model.StoreName}");

                if(model.vehicles.Count != 0)
                {
                    _camera = Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), model.vehicleSpawnCamera.X, model.vehicleSpawnCamera.Y, model.vehicleSpawnCamera.Z, 180f, 0f, 0f, 50.0f, true, 0);
                    Cam.PointCamAtCoord(_camera, model.vehicleSpawnCameraLookat.X, model.vehicleSpawnCameraLookat.Y, model.vehicleSpawnCameraLookat.Z);
                    Cam.SetCamActive(_camera, true);
                    Cam.RenderScriptCams(true, false, 0, true, false, 0);

                    _currentSpawn = model.vehicleSpawn;
                    _spawnHeading = model.vehicleSpawnHeading;

                    Ui.DisplayHud(false);
                    Ui.DisplayRadar(false);
                    Chat.Activate(false);
                    Chat.Show(false);

                    RAGE.Elements.Player.LocalPlayer.FreezePosition(true);

                    _browserId = BrowserHelper.CreateBrowser(new windowObject()
                    {
                        url = "package://cs_packages/RageRP/html/store.html",
                        function = BrowserFunctions.STORE_SHOW,
                        parameters = new object[] {
                            model.StoreName,
                            JsonConvert.SerializeObject(model.vehicles)
                        }
                    });
                }
            }
        }

        private void HideStore(object[] args)
        {
            BrowserHelper.DestroyBrowser(_browserId);
            _demoVehicle = 0;
            _browserId = 0;
            Cursor.Visible = false;
            Cam.DestroyCam(_camera, true);
            Cam.SetCamActive(_camera, false);
            Cam.RenderScriptCams(false, false, 0, true, false, 0);

            Ui.DisplayHud(true);
            Chat.Activate(true);
            Chat.Show(true);

            RAGE.Elements.Player.LocalPlayer.FreezePosition(false);
        }

        private void PurchaseVehicle(object[] args)
        {

        }

        private void ViewVehicle(object[] args)
        {
            string id = args[0].ToString();
            if(!string.IsNullOrEmpty(id))
            {
                if(_demoVehicle != 0)
                {
                    RAGE.Game.Vehicle.DeleteVehicle(ref _demoVehicle);
                    _demoVehicle = 0;
                }
                uint requestedVehicle = RAGE.Game.Misc.GetHashKey(id);
                _demoVehicle = RAGE.Game.Vehicle.CreateVehicle(requestedVehicle, _currentSpawn.X, _currentSpawn.Y, _currentSpawn.Z, _spawnHeading, false, false, 0);
            }
            else
            {
                Chat.Output("it went wrong :(");
            }
        }

        public async void Tick(List<TickNametagData> nametags)
        {
            if (Globals.isChatOpen)
                return;
            if (!Globals.isSpawned)
                return;

            if(_camera == 0 && _demoVehicle != 0)
            {
                RAGE.Game.Vehicle.DeleteVehicle(ref _demoVehicle);
                _demoVehicle = 0;
            }

            bool isEnteredColShape = Convert.ToBoolean(RAGE.Elements.Player.LocalPlayer.GetSharedData("EnteredColShape"));
            if (isEnteredColShape)
            {
                if (RAGE.Input.IsDown((int)ConsoleKey.E) && !isInteractPressed)
                {
                    isInteractPressed = true;
                    CallRemote("OpenStore");
                }
                if (!RAGE.Input.IsDown((int)ConsoleKey.E) && isInteractPressed)
                    isInteractPressed = false;
            }
        }
    }
}

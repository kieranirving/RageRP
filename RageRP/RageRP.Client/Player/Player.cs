using Newtonsoft.Json;
using RAGE;
using RAGE.Game;
using RAGE.Ui;
using RageRP.Client.Helpers;
using RageRP.Client.Models;
using System;
using static RageRP.Client.Helpers.BrowserHelper;

namespace RageRP.Client.Player
{
    class Player : Events.Script
    {
        private static int _browserId;
        private static int _camera;
        private static PedModel _playerPed;
        private static int _gender;
        private static bool _showSpawnSelection;
        public Player()
        {
            _browserId = 0;
            _gender = 0;
            _showSpawnSelection = false;

            //Player Commands
            Events.OnPlayerCommand += OnPlayerCommand;

            //CEF Events
            Events.Add("getPlayerData", GetPlayerData);
            Events.Add("loadCharacter", LoadCharacter);
            Events.Add("createCharacter", CreateCharacter);
            Events.Add("deleteCharacter", DeleteCharacter);
            Events.Add("login", Login);
            Events.Add("setCharacterGender", SetCharacterGender);
            Events.Add("updateCharacter", UpdateCharacter);
            Events.Add("editFace", EditFace);
            Events.Add("editBody", EditBody);
            Events.Add("SaveCharacter", SaveCharacter);
            Events.Add("Disconnect", Disconnect);

            //Server Events
            Events.Add("disconnected", Disconnected);
            Events.Add("GotPlayerCharacters", GotPlayerCharacters);
            Events.Add("RefreshCharacterData", GetPlayerData);
            Events.Add("showSelectionScreen", CharacterSelectionScreen);
            Events.Add("showLogin", ShowLogin);
            Events.Add("LoadedCharacter", LoadedCharacter);
            Events.Add("CreatedCharacter", CreatedCharacter);
            Events.Add("UpdatedCharacter", UpdatedCharacter);
            Events.Add("syncTime", SyncTime);
        }

        public static async void OnPlayerCommand(string cmd, RAGE.Events.CancelEventArgs cancel)
        {
            switch(cmd)
            {
                case "getpos":
                    string x = RAGE.Elements.Player.LocalPlayer.Position.X.ToString();
                    string y = RAGE.Elements.Player.LocalPlayer.Position.Y.ToString();
                    string z = RAGE.Elements.Player.LocalPlayer.Position.Z.ToString();
                    string heading = RAGE.Elements.Player.LocalPlayer.GetHeading().ToString();
                    Chat.Output($"X {x} | Y {y} | Z {z} | H {heading}");
                    break;
                default:
                    break;
            }
        }

        public static async void GetPlayerData(object[] args)
        {
            Events.CallRemote("GetPlayerData", Globals.PlayerID);
        }

        public static async void GotPlayerCharacters(object[] args)
        {
            var characterList = args[0].ToString();
            //List<CharacterModel> characters = JsonConvert.DeserializeObject<List<CharacterModel>>(characterList);
            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.STARTUP_SHOWCHARSEL,
                index = _browserId,
                parameters = new object[]
                {
                    Json.Escape(characterList)
                }
            });
        }

        public static async void SyncTime(object[] args)
        {
            DateTime serverTime = Convert.ToDateTime(RAGE.Elements.Player.LocalPlayer.GetSharedData("ServerTime").ToString());
            Clock.SetClockTime(serverTime.Hour, serverTime.Minute, serverTime.Second);
        }

        public static void Disconnect(object[] args)
        {
            Events.CallRemote("Disconnect");
        }

        public static void Disconnected(object[] args)
        {
            string reason = args[0].ToString();
            string expiry = null;
            if(string.IsNullOrEmpty(expiry))
            {
                _browserId = BrowserHelper.CreateBrowser(new windowObject()
                {
                    url = "package://cs_packages/RageRP/html/login.html",
                    function = BrowserFunctions.LOGIN_DISCONNECT,
                    parameters = new object[]
                    {
                        reason
                    }
                });
            }
            else
            {
                _browserId = BrowserHelper.CreateBrowser(new windowObject()
                {
                    url = "package://cs_packages/RageRP/html/login.html",
                    function = BrowserFunctions.LOGIN_DISCONNECT,
                    parameters = new object[]
                    {
                        reason, expiry
                    }
                });
            }
        }

        public static void ShowLogin(object[] args)
        {
            DateTime serverTime = Convert.ToDateTime(RAGE.Elements.Player.LocalPlayer.GetSharedData("ServerTime").ToString());
            Globals.Name = RAGE.Elements.Player.LocalPlayer.GetSharedData("PlayerName").ToString();
            
            // Set the hour from the server
            Clock.SetClockTime(serverTime.Hour, serverTime.Minute, serverTime.Second);
            Globals.PlayerID = args[0].ToString();
            Globals.version = args[1].ToString();

            _browserId = BrowserHelper.CreateBrowser(new windowObject()
            {
                url = "package://cs_packages/RageRP/html/login.html",
                function = BrowserFunctions.LOGIN_INIT,
                parameters = new object[]
                {
                    Globals.Name
                }
            });
        }

        public static void Login(object[] args)
        {
            string password = args[0].ToString();
            Events.CallRemote("login", Globals.PlayerID, password);
            BrowserHelper.DestroyBrowser(_browserId);
        }

        public static void CharacterSelectionScreen(object[] args)
        {
            _browserId = BrowserHelper.CreateBrowser(new windowObject()
            {
                url = "package://cs_packages/RageRP/html/startup.html",
                function = BrowserFunctions.STARTUP_INIT,
                parameters = new object[]
                {
                    Globals.Name
                }
            });
        }

        public static void CreateCharacter(object[] args)
        {
            string CharacterName = args[0].ToString();
            int Year = Convert.ToInt32(args[1]);
            int Month = Convert.ToInt32(args[2]);
            int Day = Convert.ToInt32(args[3]);
            DateTime DateOfBirth = new DateTime(Year, Month, Day);
            _showSpawnSelection = true;
            Events.CallRemote("CreateCharacter", Globals.PlayerID, CharacterName, DateOfBirth.ToString());
        }

        public static void DeleteCharacter(object[] args)
        {
            string CharacterID = args[0].ToString();
            Events.CallRemote("DeleteCharacter", Globals.PlayerID, CharacterID);
        }

        public static void LoadCharacter(object[] args)
        {
            string CharacterID = args[0].ToString();
            Events.CallRemote("LoadCharacter", Globals.PlayerID, CharacterID);
        }

        public static void LoadedCharacter(object[] args)
        {
            _playerPed = new PedModel();
            CharacterModel character = JsonConvert.DeserializeObject<CharacterModel>(args[0].ToString());
            Globals.CharacterID = character.Character_ID;
            Globals.CharacterName = character.CharacterName;
            _playerPed = JsonConvert.DeserializeObject<PedModel>(character.PedString);
            UpdatePlayerPed();
            Events.CallLocal("closeStartupCamera");
            BrowserHelper.DestroyBrowser(_browserId);

            if(character.isNewCharacter)
            {
                RAGE.Elements.Player.LocalPlayer.Position = new Vector3(402.8664f, -996.4108f, -99.00027f);
                RAGE.Elements.Player.LocalPlayer.SetHeading(-185.0f);
                RAGE.Elements.Player.LocalPlayer.Dimension = (uint)character.CharacterDimension;

                _camera = Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), 402.8664f, -997.5515f, -98.4f, -185f, 0f, 0f, 20.0f, true, 0);
                Cam.PointCamAtCoord(_camera, 402.8664f, -996.4108f, -98.3f);
                Cam.SetCamActive(_camera, true);
                Cam.RenderScriptCams(true, false, 0, true, false, 0);

                Ui.DisplayHud(false);
                Ui.DisplayRadar(false);
                Chat.Activate(false);
                Chat.Show(false);

                RAGE.Elements.Player.LocalPlayer.SetDefaultComponentVariation();

                RAGE.Elements.Player.LocalPlayer.FreezePosition(true);
                RAGE.Elements.Player.LocalPlayer.SetInvincible(true);

                OverlayValues.Blemish = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(0);
                OverlayValues.FacialHair = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(1);
                OverlayValues.Eyebrows = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(2);
                OverlayValues.Ageing = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(3);
                OverlayValues.Makeup = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(4);
                OverlayValues.Blush = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(5);
                OverlayValues.Complexion = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(6);
                OverlayValues.SunDamage = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(7);
                OverlayValues.Lipstick = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(8);
                OverlayValues.MolesFreckles = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(9);
                OverlayValues.ChestHair = RAGE.Elements.Player.LocalPlayer.GetHeadOverlayValue(10);

                _browserId = BrowserHelper.CreateBrowser(new windowObject()
                {
                    url = "package://cs_packages/RageRP/html/characterCreation.html",
                    function = BrowserFunctions.CHARACTERCREATE_INIT,
                    parameters = new object[]
                    {
                        OverlayValues.Blemish,
                        OverlayValues.FacialHair,
                        OverlayValues.Eyebrows,
                        OverlayValues.Ageing,
                        OverlayValues.Makeup,
                        OverlayValues.Blush,
                        OverlayValues.Complexion,
                        OverlayValues.SunDamage,
                        OverlayValues.Lipstick,
                        OverlayValues.MolesFreckles,
                        OverlayValues.ChestHair
                    }
                });
            }
            else
            {
                Events.CallRemote("GetSpawnLocations", Globals.CharacterID);
            }
        }

        public static void EditFace(object[] args)
        {
            Cam.SetCamCoord(_camera, 402.8664f, -997.5515f, -98.4f);
            Cam.PointCamAtCoord(_camera, 402.8664f, -996.4108f, -98.3f);
            Cam.SetCamFov(_camera, 20.0f);
            RAGE.Elements.Player.LocalPlayer.SetDefaultComponentVariation();
        }

        public static void EditBody(object[] args)
        {
            Cam.SetCamCoord(_camera, 402.8664f, -997.5515f, -98.5f);
            Cam.PointCamAtCoord(_camera, 402.8664f, -996.4108f, -98.5f);
            Cam.SetCamFov(_camera, 50.0f);
            Chat.Output(_gender.ToString());
            if (_gender == Gender.Male)
            {
                RAGE.Elements.Player.LocalPlayer.SetDefaultComponentVariation();
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(3, 15, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(4, 21, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(6, 34, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(8, 15, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(11, 15, 0, 2);
            }
            else if(_gender == Gender.Female)
            {
                RAGE.Elements.Player.LocalPlayer.SetDefaultComponentVariation();
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(3, 15, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(4, 10, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(6, 35, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(8, 15, 0, 2);
                RAGE.Elements.Player.LocalPlayer.SetComponentVariation(11, 15, 0, 2);
            }
        }

        public static void SetCharacterGender(object[] args)
        {
            int gender = Convert.ToInt32(args[0].ToString());
            uint genderModel = Misc.GetHashKey("mp_m_freemode_01");
            switch(gender)
            {
                case Gender.Male:
                    genderModel = Misc.GetHashKey("mp_m_freemode_01");
                    _playerPed.firstHeadShape = 0;
                    _playerPed.secondHeadShape = 0;
                    _gender = Gender.Male;
                    break;
                case Gender.Female:
                    genderModel = Misc.GetHashKey("mp_f_freemode_01");
                    _playerPed.firstHeadShape = 0;
                    _playerPed.secondHeadShape = 24;
                    _gender = Gender.Female;
                    break;
                default:
                    genderModel = Misc.GetHashKey("mp_m_freemode_01");
                    _playerPed.firstHeadShape = 0;
                    _playerPed.secondHeadShape = 0;
                    _gender = Gender.Male;
                    break;
            }
            Chat.Output(_gender.ToString());
            //RAGE.Game.Player.SetPlayerModel(genderModel);
            RAGE.Elements.Player.LocalPlayer.Model = genderModel;
            UpdatePlayerPed();
        }

        public static void UpdateCharacter(object[] args)
        {
            _playerPed.firstHeadShape = Convert.ToInt32(args[0].ToString());
            _playerPed.secondHeadShape = Convert.ToInt32(args[1].ToString());
            _playerPed.firstSkinTone = string.IsNullOrEmpty(args[2].ToString()) ? Convert.ToInt32(args[0].ToString()) : Convert.ToInt32(args[2].ToString());
            _playerPed.secondSkinTone = string.IsNullOrEmpty(args[3].ToString()) ? Convert.ToInt32(args[0].ToString()) : Convert.ToInt32(args[3].ToString());
            _playerPed.hairModel = Convert.ToInt32(args[4].ToString());
            _playerPed.firstHairColor = Convert.ToInt32(args[5].ToString());
            _playerPed.secondHairColor = Convert.ToInt32(args[6].ToString());
            _playerPed.beardModel = Convert.ToInt32(args[7].ToString());
            _playerPed.beardColor = Convert.ToInt32(args[8].ToString());
            _playerPed.chestModel = Convert.ToInt32(args[9].ToString());
            _playerPed.chestColor = Convert.ToInt32(args[10].ToString());
            _playerPed.blemishesModel = Convert.ToInt32(args[11].ToString());
            _playerPed.ageingModel = Convert.ToInt32(args[12].ToString());
            _playerPed.complexionModel = Convert.ToInt32(args[13].ToString());
            _playerPed.sundamageModel = Convert.ToInt32(args[14].ToString());
            _playerPed.frecklesModel = Convert.ToInt32(args[15].ToString());
            _playerPed.eyesColor = Convert.ToInt32(args[16].ToString());
            _playerPed.eyebrowsModel = Convert.ToInt32(args[17].ToString());
            _playerPed.eyebrowsColor = Convert.ToInt32(args[18].ToString());
            _playerPed.makeupModel = Convert.ToInt32(args[19].ToString());
            _playerPed.blushModel = Convert.ToInt32(args[20].ToString());
            _playerPed.blushColor = Convert.ToInt32(args[21].ToString());
            _playerPed.lipstickModel = Convert.ToInt32(args[22].ToString());
            _playerPed.lipstickColor = Convert.ToInt32(args[23].ToString());
            _playerPed.headMix = float.Parse(args[24].ToString());
            _playerPed.skinMix = float.Parse(args[25].ToString());
            _playerPed.noseWidth = float.Parse(args[26].ToString());
            _playerPed.noseHeight = float.Parse(args[27].ToString());
            _playerPed.noseLength = float.Parse(args[28].ToString());
            _playerPed.noseBridge = float.Parse(args[29].ToString());
            _playerPed.noseTip = float.Parse(args[30].ToString());
            _playerPed.noseShift = float.Parse(args[31].ToString());
            _playerPed.browHeight = float.Parse(args[32].ToString());
            _playerPed.browWidth = float.Parse(args[33].ToString());
            _playerPed.cheekboneHeight = float.Parse(args[34].ToString());
            _playerPed.cheekboneWidth = float.Parse(args[35].ToString());
            _playerPed.cheeksWidth = float.Parse(args[36].ToString());
            _playerPed.eyes = float.Parse(args[37].ToString());
            _playerPed.lips = float.Parse(args[38].ToString());
            _playerPed.jawWidth = float.Parse(args[39].ToString());
            _playerPed.jawHeight = float.Parse(args[40].ToString());
            _playerPed.chinLength = float.Parse(args[41].ToString());
            _playerPed.chinPosition = float.Parse(args[42].ToString());
            _playerPed.chinWidth = float.Parse(args[43].ToString());
            _playerPed.chinShape = float.Parse(args[44].ToString());
            _playerPed.neckWidth = float.Parse(args[45].ToString());
            UpdatePlayerPed();
        }

        public static void CreatedCharacter(object[] args)
        {
            string CharacterID = args[0].ToString();
            Events.CallRemote("LoadCharacter", Globals.PlayerID, CharacterID);
        }

        public static void SaveCharacter(object[] args)
        {
            string jsonString = JsonConvert.SerializeObject(_playerPed);
            Events.CallRemote("UpdateCharacter", Globals.CharacterID, jsonString);
        }

        public static void UpdatedCharacter(object[] args)
        {
            var response = JsonConvert.DeserializeObject<ResponseMessage>(args[0].ToString());
            if(response.success)
            {
                BrowserHelper.DestroyBrowser(_browserId);
                Cursor.Visible = false;

                Cam.DestroyCam(_camera, true);
                Cam.SetCamActive(_camera, false);
                Cam.RenderScriptCams(false, false, 0, true, false, 0);

                if (_showSpawnSelection)
                {
                    RAGE.Elements.Player.LocalPlayer.FreezePosition(true);
                    RAGE.Elements.Player.LocalPlayer.SetInvincible(true);
                    Events.CallRemote("GetSpawnLocations", Globals.CharacterID);
                }
                else
                {
                    Ui.DisplayHud(true);
                    Chat.Activate(true);
                    Chat.Show(true);

                    RAGE.Elements.Player.LocalPlayer.FreezePosition(false);
                    RAGE.Elements.Player.LocalPlayer.SetInvincible(false);
                }
            }
            else
            {
                if(response.hasError)
                {
                    //TODO Handle Errors
                }
                else
                {
                    //We should never get here
                    //TODO Handle this inevitible outcome...
                }
            }
        }

        private static void UpdatePlayerPed()
        {
            RAGE.Elements.Player.LocalPlayer.SetHeadBlendData(_playerPed.firstHeadShape, _playerPed.secondHeadShape, 0, _playerPed.firstSkinTone, _playerPed.secondSkinTone, 0, _playerPed.headMix, _playerPed.skinMix, 0, false);
            RAGE.Elements.Player.LocalPlayer.SetComponentVariation(2, _playerPed.hairModel, 0, 0);
            RAGE.Elements.Player.LocalPlayer.SetHairColor(_playerPed.firstHairColor, _playerPed.secondHairColor);
            RAGE.Elements.Player.LocalPlayer.SetEyeColor(_playerPed.eyesColor);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(1, _playerPed.beardModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlayColor(1, 1, _playerPed.beardColor, 0);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(10, _playerPed.chestModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlayColor(10, 1, _playerPed.chestColor, 0);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(2, _playerPed.eyebrowsModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlayColor(2, 1, _playerPed.eyebrowsColor, 0);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(5, _playerPed.blushModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlayColor(5, 2, _playerPed.blushColor, 0);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(8, _playerPed.lipstickModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlayColor(8, 2, _playerPed.lipstickColor, 0);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(0, _playerPed.blemishesModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(3, _playerPed.ageingModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(6, _playerPed.complexionModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(7, _playerPed.sundamageModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(9, _playerPed.frecklesModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetHeadOverlay(4, _playerPed.makeupModel, 1.0f);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(0, _playerPed.noseWidth);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(1, _playerPed.noseHeight);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(2, _playerPed.noseLength);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(3, _playerPed.noseBridge);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(4, _playerPed.noseTip);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(5, _playerPed.noseShift);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(6, _playerPed.browHeight);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(7, _playerPed.browWidth);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(8, _playerPed.cheekboneHeight);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(9, _playerPed.cheekboneWidth);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(10, _playerPed.cheeksWidth);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(11, _playerPed.eyes);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(12, _playerPed.lips);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(13, _playerPed.jawWidth);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(14, _playerPed.jawHeight);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(15, _playerPed.chinLength);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(16, _playerPed.chinPosition);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(17, _playerPed.chinWidth);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(18, _playerPed.chinShape);
            RAGE.Elements.Player.LocalPlayer.SetFaceFeature(19, _playerPed.neckWidth);
        }
    }
}
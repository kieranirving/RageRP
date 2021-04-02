using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Newtonsoft.Json;
using RageRP.DTO;
using RageRP.Server.Commands;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Data;
using RageRP.Server.Encryption;
using RageRP.Server.Helpers;
using RageRP.Server.Services;

namespace RageRP.Server
{
    public class Main : Script
    {
        private List<DTOBackground> _backgrounds;
        
        public Main()
        {
            Gamedata.CreationDimension = 1;
            _backgrounds = new List<DTOBackground>();

            bool shouldStart = SystemCheck();

            if (!shouldStart)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                splash();
                c.WriteLine("Failed To start");
                c.WriteLine("Please Restart Server");
                while(true)
                {
                    var key = Console.ReadKey();
                }
            }
            Console.ResetColor();
        }

        [ServerEvent(Event.ResourceStart)]
        public async void OnStart()
        {
            Gamedata.isDebug = Convert.ToBoolean(NAPI.Resource.GetSetting<string>(this, "isDebug"));
            Gamedata.version = NAPI.Resource.GetSetting<string>(this, "version");
            Gamedata.StartingCash = Convert.ToInt32(NAPI.Resource.GetSetting<string>(this, "startCash"));
            Gamedata.StartingBank = Convert.ToInt32(NAPI.Resource.GetSetting<string>(this, "startBank"));
            Gamedata.WebsiteURL = NAPI.Resource.GetSetting<string>(this, "WebsiteURL");

            Gamedata.playerList = new HashSet<DTOPlayer>();
            Gamedata.vehicleList = new HashSet<DTOVehicle>();
            Gamedata.reports = new HashSet<DTOReport>();
            Gamedata.reportCount = 0;

            //Set the Error Message
            NAPI.Server.SetCommandErrorMessage("~r~ERROR:~w~ Command not found, please try again");

            //Global Settings
            NAPI.Server.SetAutoSpawnOnConnect(false);
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoRespawnAfterDeath(false);

            //Map IPL's
            NAPI.World.RequestIpl("canyonrvrdeep");
            NAPI.World.RequestIpl("canyonrvrshallow");

            var _spawnService = new SpawnService();
            c.WriteLine("Getting Backgrounds");

            var backgrounds = await _spawnService.GetBackgrounds();
            foreach (var b in backgrounds.cameras)
            {
                _backgrounds.Add(new DTOBackground()
                {
                    id = b.id,
                    Name = b.Name,
                    camLookX = b.camLookX,
                    camLookY = b.camLookY,
                    camLookZ = b.camLookZ,
                    camPosX = b.camPosX,
                    camPosY = b.camPosY,
                    camPosZ = b.camPosZ
                });
            }

            c.WriteLine("Got Backgrounds");

            //Get the background locations and shit
            await Task.Factory.StartNew(() =>
            {
                NAPI.Task.Run(() =>
                {
                    Stores.RenderStores();
                    StaticVehicles.RenderShowVehicles();
                    Blips.RenderBlips();

                    Gamedata.AdminCommands = Help.GetAdminCommands();
                    Gamedata.FactionCommands = Help.GetFactionCommands();
                    Gamedata.GroupCommands = Help.GetGroupCommands();
                    Gamedata.PlayerCommands = Help.GetPlayerCommands();

                    splash();
                });
            });
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            string name = ""; string customReason = "";
            var CharacterName = player.GetSharedData("CharacterName");
            if (!string.IsNullOrEmpty(CharacterName))
                name = CharacterName;
            else
                name = player.Name;
            if(type == DisconnectionType.Left || type == DisconnectionType.Timeout)
            {
                switch (type)
                {
                    case DisconnectionType.Left:
                        customReason = "(Left)";
                        break;
                    case DisconnectionType.Timeout:
                        customReason = "(Timed Out)";
                        break;
                }

                string kickMessage = $"{name} has Disconnected {customReason}";
                NAPI.Chat.SendChatMessageToAll($"!{{#6d6d6d}} {kickMessage}");
                c.WriteLine(kickMessage);
            }

            string Player_ID = player.GetSharedData("playerID");
            if (!string.IsNullOrEmpty(Player_ID))
            {
                long playerID = EncryptionService.DecryptID(Player_ID);
                Gamedata.playerList.RemoveWhere(x => x.PlayerID == playerID);
            }
        }

        [ServerEvent(Event.PlayerConnected)]
        public async void OnPlayerConnected(Client player)
        {
            c.WriteLine($"Connecting {player.SocialClubName} {player.Address}");
            // Set the default skin and transparency
            NAPI.Player.SetPlayerSkin(player, PedHash.FreemodeMale01);
            NAPI.Entity.SetEntityTransparency(player, 0);
            
            player.SetSharedData("ServerTime", DateTime.Now.ToString("HH:mm:ss"));
            player.SetSharedData("PlayerName", player.SocialClubName);

            var rand = new Random();
            var backgroundJson = JsonConvert.SerializeObject(_backgrounds[rand.Next(_backgrounds.Count())]);
            player.TriggerEvent("startupCamera", backgroundJson);

            var _playerService = new PlayerService();

            var dto = await _playerService.GetPlayerBySocialClubName(player.SocialClubName);
            if (dto.success)
            {
                if(dto.isPermBanned || dto.TempBanDate >= DateTime.Now)
                {
                    var admin = new Admin();
                    string disconnectMessage;
                    if (dto.isPermBanned)
                    {
                        c.WriteLine($"Kicked {player.SocialClubName}(License: {dto.License}) Permbanned");
                        disconnectMessage = $"{player.SocialClubName}, You are banned.<br>Reason: {dto.BanReason}.<br>Appeal at {Gamedata.WebsiteURL}.<br>(License {dto.License})";
                    }
                    else if (dto.TempBanDate > DateTime.Now)
                    {
                        disconnectMessage = $"Sorry {player.SocialClubName},<br>You are banned until {dto.TempBanDate.ToString("dd/MM/yyyy hh:mm tt")}.<br>Reason: {dto.BanReason}!<br>Appeal at {Gamedata.WebsiteURL}.<br>(License {dto.License})";
                    }
                    else
                    {
                        disconnectMessage = $"Well {player.SocialClubName}, You should not have ended up here.<br>Submit a bug report at {Gamedata.WebsiteURL} & reference 'baconwellbreadbrotherhere'.<br>(License {dto.License})";
                    }

                    player.SendChatMessage(disconnectMessage.Replace("<br>", " "));
                    player.TriggerEvent("disconnected", disconnectMessage);
                    await Task.Delay(1000);
                    player.Kick(disconnectMessage.Replace("<br>", " "));

                    string kickMessage = $"{player.SocialClubName} has Disconnected (Banned)";
                    NAPI.Chat.SendChatMessageToAll($"!{{#6d6d6d}} {kickMessage}");

                    return;
                }
                var isWhitelisted = await _playerService.CheckIfWhitelisted(dto.License);
                if (isWhitelisted)
                {
                    player.SendChatMessage($"Welcome back {player.SocialClubName}!");
                    c.WriteLine($"{player.SocialClubName}(License: {dto.License}) Whitelisted");
                    player.TriggerEvent("showLogin", dto.Player_ID, Gamedata.version);
                    player.TriggerEvent("setVersion", Gamedata.version);
                }
                else
                {
                    c.WriteLine($"{player.SocialClubName}(License: {dto.License}) Not whitelisted");
                    string disconnectMessage = $"Sorry {player.SocialClubName}, You are not whitelisted!<br>Apply at {Gamedata.WebsiteURL}.<br>(License {dto.License})";
                    player.SendChatMessage(disconnectMessage.Replace("<br>", " "));
                    player.TriggerEvent("disconnected", disconnectMessage);
                    await Task.Delay(1000);
                    player.Kick(disconnectMessage.Replace("<br>", " "));
                }
            }
            else
            {
                await _playerService.InsertPlayer(player.SocialClubName);
                var insertedPlayer = await _playerService.GetPlayerBySocialClubName(player.SocialClubName);
                c.WriteLine($"{player.SocialClubName}(License: {insertedPlayer.License}) Not whitelisted");
                string disconnectMessage = $"Sorry {player.SocialClubName}, You are not whitelisted!<br>Apply at {Gamedata.WebsiteURL}.<br>(License {dto.License})";
                player.SendChatMessage(disconnectMessage.Replace("<br>", " "));
                player.TriggerEvent("disconnected", disconnectMessage);
                await Task.Delay(1000);
                player.Kick(disconnectMessage.Replace("<br>", " "));
            }
        }

        private void splash()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" ██▀███   ▄▄▄        ▄████ ▓█████  ██▀███   ██▓███  ");
            Console.WriteLine("▓██ ▒ ██▒▒████▄     ██▒ ▀█▒▓█   ▀ ▓██ ▒ ██▒▓██░  ██▒");
            Console.WriteLine("▓██ ░▄█ ▒▒██  ▀█▄  ▒██░▄▄▄░▒███   ▓██ ░▄█ ▒▓██░ ██▓▒");
            Console.WriteLine("▒██▀▀█▄  ░██▄▄▄▄██ ░▓█  ██▓▒▓█  ▄ ▒██▀▀█▄  ▒██▄█▓▒ ▒");
            Console.WriteLine("░██▓ ▒██▒ ▓█   ▓██▒░▒▓███▀▒░▒████▒░██▓ ▒██▒▒██▒ ░  ░");
            Console.WriteLine("░ ▒▓ ░▒▓░ ▒▒   ▓▒█░ ░▒   ▒ ░░ ▒░ ░░ ▒▓ ░▒▓░▒▓▒░ ░  ░");
            Console.WriteLine("  ░▒ ░ ▒░  ▒   ▒▒ ░  ░   ░  ░ ░  ░  ░▒ ░ ▒░░▒ ░     ");
            Console.WriteLine("  ░░   ░   ░   ▒   ░ ░   ░    ░     ░░   ░ ░░       ");
            Console.WriteLine("   ░           ░  ░      ░    ░  ░   ░              ");
            c.WriteLine($"{Gamedata.version} Started");
            c.WriteLine("Author: TheDustiestBin");
            c.WriteLines(1, true);
            Console.ResetColor();
        }

        private bool SystemCheck()
        {
            var _sysCheck = new SystemChecks();
            Console.ForegroundColor = ConsoleColor.Cyan;
            c.WriteLine("SystemCheck: Testing Database Connection");
            Console.ResetColor();

            return _sysCheck.testConnection();
        }        
    }
}
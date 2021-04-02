using GTANetworkAPI;
using Newtonsoft.Json;
using RageRP.DTO;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Encryption;
using RageRP.Server.Helpers;
using RageRP.Server.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RageRP.Server.Events
{
    public class Player : Script
    {
        private PlayerService _playerService;
        private SpawnService _spawnService;
        public Player()
        {
            _playerService = new PlayerService();
            _spawnService = new SpawnService();
        }

        [RemoteEvent("login")]
        public async void Login(Client player, string PlayerID, string password)
        {
            var playerData = await _playerService.GetPlayerBySocialClubName(player.SocialClubName);

            if (string.IsNullOrEmpty(password))
            {
                c.WriteLine($"{player.SocialClubName}(License: {playerData.License}) Failed Login (No PW) - {player.Address}");
                string disconnectMessage = "No Password provided. Please try again";
                player.TriggerEvent("disconnected", disconnectMessage);
                await Task.Delay(1000);
                player.Kick(disconnectMessage);
            }

            long playerID = EncryptionService.DecryptID(PlayerID);
            string EncryptedPassword = EncryptionService.EncryptPassword(password);

            if (EncryptionService.DecryptID(playerData.Player_ID) == playerID)
            {
                string dataPassword = EncryptionService.DecryptPassword(playerData.Password);
                if (dataPassword == password)
                {
                    c.WriteLine($"{player.SocialClubName}(License: {playerData.License}) Successful Login");
                    playerData.SocialClubName = player.SocialClubName;
                    playerData.currentCharacter = new DTOCharacter();
                    playerData.PlayerID = playerID;
                    playerData.Handle = player.Handle.Value;
                    Gamedata.playerList.Add(playerData);
                    player.TriggerEvent("showSelectionScreen");

                    //Shared player objects
                    player.SetSharedData("isAdminOnDuty", false);
                    player.SetSharedData("isAdmin", playerData.isAdmin);
                    player.SetSharedData("AdminLevel", playerData.AdminLevel);
                    player.SetSharedData("isTalking", false);
                    player.SetSharedData("handleID", player.Handle.Value);
                    player.SetSharedData("isDead", false);
                    player.SetSharedData("playerID", playerData.Player_ID);
                    player.SetSharedData("isFrozen", false);
                    player.SetSharedData("isMuted", false);
                }
                else
                {
                    c.WriteLine($"{player.SocialClubName}(License: {playerData.License}) Failed Login - {player.Address}");
                    string disconnectMessage = "Login Failed. Please reconnect and try again";
                    player.TriggerEvent("disconnected", disconnectMessage);
                    await Task.Delay(1000);
                    player.Kick(disconnectMessage);
                }
            }
            else
            {
                c.WriteLine($"{player.SocialClubName}(License: {playerData.License}) Failed Login - {player.Address}");
                string disconnectMessage = "Login Failed. Please reconnect and try again.";
                player.TriggerEvent("disconnected", disconnectMessage);
                await Task.Delay(1000);
                player.Kick(disconnectMessage);
            }
        }

        [RemoteEvent("GetPlayerData")]
        public async void GetPlayerData(Client player, string PlayerID)
        {
            long playerID = EncryptionService.DecryptID(PlayerID);
            var characters = await _playerService.GetPlayerCharacters(playerID);
            var jsonString = JsonConvert.SerializeObject(characters);
            player.TriggerEvent("GotPlayerCharacters", jsonString);
        }

        [RemoteEvent("CreateCharacter")]
        public async void CreateCharacter(Client player, string PlayerID, string CharacterName, string DateOfBirth)
        {
            long playerID = EncryptionService.DecryptID(PlayerID);
            long characterID = await _playerService.InsertCharacter(new DTOCharacter()
            {
                PlayerID = playerID,
                CharacterName = CharacterName,
                Cash = Gamedata.StartingCash,
                Bank = Gamedata.StartingBank,
                CurrentPed = "nocurrentped",
                Gender = 0,
                PedString = JsonConvert.SerializeObject(new DTOPed())
            });
            NAPI.Entity.SetEntityTransparency(player, 255);
            string CharacterID = EncryptionService.EncryptID(characterID);
            player.TriggerEvent("CreatedCharacter", CharacterID);
        }

        [RemoteEvent("DeleteCharacter")]
        public async void DeleteCharacter(Client player, string PlayerID, string CharacterID)
        {
            long playerID = EncryptionService.DecryptID(PlayerID);
            long characterID = EncryptionService.DecryptID(CharacterID);
            bool result = await _playerService.DeleteCharacter(playerID, characterID);
            player.TriggerEvent("RefreshCharacterData");
        }

        [RemoteEvent("LoadCharacter")]
        public async void LoadCharacter(Client player, string PlayerID, string CharacterID)
        {
            long playerID = EncryptionService.DecryptID(PlayerID);
            long characterID = EncryptionService.DecryptID(CharacterID);
            var currentPlayer = Gamedata.playerList.Where(x => x.PlayerID == playerID && x.SocialClubName == player.SocialClubName).FirstOrDefault();
            if (currentPlayer != null)
            {
                var character = await _playerService.GetCharacter(playerID, characterID);
                currentPlayer.currentCharacter = character;
                currentPlayer.currentCharacter.CharacterID = characterID;
                if (character.isNewCharacter)
                {
                    character.CharacterDimension = Gamedata.CreationDimension; Gamedata.CreationDimension = Gamedata.CreationDimension++;
                }
                string jsonString = JsonConvert.SerializeObject(character);
                player.TriggerEvent("LoadedCharacter", jsonString);
                player.SetSharedData("DisplayName", character.CharacterName);
                player.SetSharedData("CharacterName", character.CharacterName);
                player.SetSharedData("CharacterID", characterID);
                player.SetSharedData("VoiceDistance", "Talking");
                player.SetSharedData("Cash", character.Cash);
                player.SetSharedData("Bank", character.Bank);
                player.SetSharedData("isEMS", character.isEMS);
                player.SetSharedData("EMSLevel", character.EMSLevel);
                player.SetSharedData("isPolice", character.isPolice);
                player.SetSharedData("PoliceLevel", character.PoliceLevel);
                player.SetSharedData("Department", character.Department);
                player.SetSharedData("BadgeNumber", character.BadgeNumber);
                NAPI.Player.SetPlayerName(player, character.CharacterName);

                player.TriggerEvent("RenderPhone");
            }
        }

        [RemoteEvent("UpdateCharacter")]
        public async void UpdateCharacter(Client player, string CharacterID, string PedData)
        {
            //var data = JsonConvert.DeserializeObject<DTOPed>(PedData);
            long characterID = EncryptionService.DecryptID(CharacterID);
            var current = Gamedata.playerList.Where(x => x.currentCharacter.CharacterID == characterID && x.SocialClubName == player.SocialClubName).FirstOrDefault();
            if (current != null)
            {
                current.currentCharacter.PedString = PedData;
                var updated = await _playerService.UpdateCharacter(current.currentCharacter);
                if (updated)
                {
                    string jsonString = "";
                    if (current.currentCharacter.isNewCharacter)
                    {
                        current.currentCharacter.isNewCharacter = false;

                        var response = new ResponseMessage()
                        {
                            success = true,
                            showSpawnSelection = true
                        };
                        jsonString = JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        var response = new ResponseMessage()
                        {
                            success = true
                        };
                        jsonString = JsonConvert.SerializeObject(response);
                    }
                    player.TriggerEvent("UpdatedCharacter", jsonString);
                }
                else
                {
                    //TODO Add Error Message?
                }
            }
            else
            {
                //TODO Add Error Message?
            }
        }

        [RemoteEvent("GetSpawnLocations")]
        public async void GetSpawnLocations(Client player, string CharacterID)
        {
            NAPI.Entity.SetEntityTransparency(player, 255);
            long characterID = EncryptionService.DecryptID(CharacterID);
            var current = Gamedata.playerList.Where(x => x.currentCharacter.CharacterID == characterID && x.SocialClubName == player.SocialClubName).FirstOrDefault();
            if (current != null)
            {
                var locations = await _spawnService.GetSpawnLocationsByCharacterID(characterID);
                if (locations.success)
                {
                    var jsonString = JsonConvert.SerializeObject(locations);
                    player.TriggerEvent("ShowSpawnSelections", jsonString);
                }
                else
                {
                    //TODO Add Error Message?
                    //Default spawn to airport?
                }
            }
            else
            {
                //TODO Add Error Message?
            }
        }

        [RemoteEvent("ConsoleData")]
        public async void ConsoleData(Client player, object[] _params)
        {
            foreach (var p in _params)
            {
                Console.WriteLine(p.ToString());
            }
        }

        [RemoteEvent("Disconnect")]
        public async void Disconnect(Client player)
        {
            string disconnectMessage = "Client Disconnect";
            player.TriggerEvent("disconnected", disconnectMessage);
            await Task.Delay(1000);
            player.Kick(disconnectMessage);
        }
    }
}

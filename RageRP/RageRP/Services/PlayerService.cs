using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using RageRP.DTO;

namespace RageRP.Server.Services
{
    public class PlayerService : Script
    {
        private DAL.Data.Player _player;

        public PlayerService()
        {
            _player = new DAL.Data.Player(Gamedata.isDebug);
        }
        
        public async Task<bool> CheckIfWhitelisted(string License)
        {
            return await _player.CheckIfWhitelisted(License);
        }

        public async Task<DTOPlayer> GetPlayerBySocialClubName(string License)
        {
            return await _player.GetPlayerBySocialClubName(License);
        }

        public async Task<long> InsertPlayer(string playerName)
        {
            return await _player.InsertPlayer(playerName);
        }

        public async Task<long> InsertCharacter(DTOCharacter dto)
        {
            return await _player.InsertCharacter(dto);
        }

        public async Task<bool> UpdateCharacter(DTOCharacter dto)
        {
            return await _player.UpdateCharacter(dto);
        }

        public async Task<DTOCharacter> GetCharacter(long PlayerID, long CharacterID)
        {
            var result = await _player.GetCharacter(PlayerID, CharacterID);
            
            return result;
        }

        public async Task<List<DTOCharacter>> GetPlayerCharacters(long PlayerID)
        {
            return await _player.GetPlayerCharacters(PlayerID);
        }

        public async Task<bool> DeleteCharacter(long PlayerID, long CharacterID)
        {
            return await _player.DeleteCharacter(PlayerID, CharacterID);
        }

        public async Task<bool> UpdateCharacterBank(DTOCharacter dto)
        {
            return await _player.UpdateCharacterBank(dto);
        }

        public async Task<bool> UpdateCharacterCash(DTOCharacter dto)
        {
            return await _player.UpdateCharacterCash(dto);
        }

        public async Task<bool> UpdateCharacterFaction(DTOCharacter dto)
        {
            return await _player.UpdateCharacterFaction(dto);
        }
    }
}
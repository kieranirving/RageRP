using RageRP.Server.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RageRP.Server.Services
{
    public class SpawnService
    {
        private DAL.Data.Spawn _spawn;
        
        public SpawnService()
        {
            _spawn = new DAL.Data.Spawn(Gamedata.isDebug);
        }

        public async Task<DTOSpawnLocations> GetSpawnLocationsByCharacterID(long CharacterID)
        {
            return await _spawn.GetSpawnLocationsByCharacterID(CharacterID);
        }

        public async Task<DTOBackgrounds> GetBackgrounds()
        {
            return await _spawn.GetSpawnCameras();
        }
    }
}

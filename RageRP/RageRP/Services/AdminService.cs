using RageRP.DTO;
using System.Threading.Tasks;

namespace RageRP.Server.Services
{
    public class AdminService
    {
        private DAL.Data.Admin _admin;

        public AdminService()
        {
            _admin = new DAL.Data.Admin(Gamedata.isDebug);
        }

        public async Task<bool> BanPlayer(DTOPlayer player)
        {
            return await _admin.BanPlayer(player);
        }

        public async Task<bool> UpdateAdminLevel(DTOPlayer dto)
        {
            return await _admin.UpdateAdminLevel(dto);
        }
    }
}
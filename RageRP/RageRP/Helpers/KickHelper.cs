using GTANetworkAPI;
using RageRP.Server.Data;

namespace RageRP.Server.Helpers
{
    public class KickHelper
    {
        private Client _target;
        private AdminKickHandler _type;
        private string _reason;

        public KickHelper(Client target, AdminKickHandler type, string reason = null)
        {
            _target = target;
            _type = type;
            _reason = reason;
        }

        public void Kick()
        {
            string name = ""; string customReason = "";
            var CharacterName = _target.GetSharedData("CharacterName");
            if (!string.IsNullOrEmpty(CharacterName))
                name = CharacterName;
            else
                name = _target.SocialClubName;
            switch (_type)
            {
                case AdminKickHandler.Kick:
                    customReason = "(Kicked)";
                    break;
                case AdminKickHandler.Banned:
                    customReason = "(Banned)";
                    break;
                default:
                    break;
            }
            
            string kickMessage = $"{CharacterName} has Disconnected {customReason}";
            NAPI.Chat.SendChatMessageToAll($"!{{#6d6d6d}} {kickMessage}");
            _target.Kick();
            c.WriteLine(kickMessage);
        }
    }
}

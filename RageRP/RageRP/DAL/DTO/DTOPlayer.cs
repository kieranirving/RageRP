using System;

namespace RageRP.DTO
{
    public class DTOPlayer : DefaultData
    {
        public long PlayerID { get; set; }
        public string Player_ID { get; set; }
        public string PlayerName { get; set; }
        public string License { get; set; }
        public DateTime DateCreated { get; set; }
        public bool isAdmin { get; set; }
        public int AdminLevel { get; set; }
        public string Password { get; set; }

        public bool isPermBanned { get; set; }
        public DateTime TempBanDate { get; set; }
        public string BanReason { get; set; }

        public bool aduty { get; set; }

        public int Handle { get; set; }
        public string SocialClubName { get; set; }

        public DTOCharacter currentCharacter { get; set; }
    }
}
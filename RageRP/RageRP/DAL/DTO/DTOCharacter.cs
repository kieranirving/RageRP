using RageRP.Server.DAL.DTO;

namespace RageRP.DTO
{
    public class DTOCharacter : DefaultData
    {
        public long PlayerID { get; set; }
        public long CharacterID { get; set; }
        public string Character_ID { get; set; }
        public string CharacterName { get; set; }
        public int Gender { get; set; }
        public string GenderString { get; set; }
        public string CurrentPed { get; set; }
        public int Cash { get; set; }
        public int Bank { get; set; }
        public DTOPed CustomPed { get; set; }
        public string PedString { get; set; }
        public bool isNewCharacter { get; set; }
        public int CharacterDimension { get; set; }
        public bool isPolice { get; set; }
        public int PoliceLevel { get; set; }
        public bool isEMS { get; set; }
        public int EMSLevel { get; set; }
        public int Department { get; set; }
        public string BadgeNumber { get; set; }
    }
}
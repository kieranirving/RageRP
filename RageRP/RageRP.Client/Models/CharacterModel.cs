namespace RageRP.Client.Models
{
    public class CharacterModel
    {
        public string Character_ID { get; set; }
        public string CharacterName { get; set; }
        public int Gender { get; set; }
        public string GenderString { get; set; }
        public string CurrentPed { get; set; }
        public int Cash { get; set; }
        public int Bank { get; set; }
        //This will change to a clothing object at some point
        public PedModel CustomPed { get; set; }
        public string PedString { get; set; }
        public bool isNewCharacter { get; set; }
        public int CharacterDimension { get; set; }
    }
}

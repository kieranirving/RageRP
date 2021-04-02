using RageRP.Server.DAL.DTO;
using System.Collections.Generic;

namespace RageRP.Server.Data
{
    public class FactionDepartments
    {
        #region PoliceDepartments
        public static List<DTOFaction> Police() //1
        {
            var _ranks = new List<DTOFaction>();
            _ranks.Add(new DTOFaction() { Level = 9, Rank = "Chief Of Police", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 8, Rank = "Deputy Chief", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 7, Rank = "Commander", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 6, Rank = "Captain", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 5, Rank = "Lieutenant", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 4, Rank = "Sergeant" });
            _ranks.Add(new DTOFaction() { Level = 3, Rank = "Corporal" });
            _ranks.Add(new DTOFaction() { Level = 2, Rank = "Officer" });
            _ranks.Add(new DTOFaction() { Level = 1, Rank = "Cadet" });
            return _ranks;
        }
        public static List<DTOFaction> PoliceDetective() //2
        {
            var _ranks = new List<DTOFaction>();
            _ranks.Add(new DTOFaction() { Level = 7, Rank = "Det. Commander", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 6, Rank = "Det. Captain", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 5, Rank = "Det. Lieutenant", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 4, Rank = "Det. Sergeant" });
            _ranks.Add(new DTOFaction() { Level = 3, Rank = "Detective II" });
            _ranks.Add(new DTOFaction() { Level = 2, Rank = "Detective" });
            return _ranks;
        }
        public static List<DTOFaction> PoliceSRT() //3
        {
            var _ranks = new List<DTOFaction>();
            _ranks.Add(new DTOFaction() { Level = 7, Rank = "SRT Commander", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 6, Rank = "SRT Captain", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 5, Rank = "SRT Lieutenant", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 4, Rank = "SRT Sergeant" });
            _ranks.Add(new DTOFaction() { Level = 3, Rank = "SRT Officer II" });
            _ranks.Add(new DTOFaction() { Level = 2, Rank = "SRT Officer" });
            return _ranks;
        }
        #endregion

        #region EMSDepartments
        public static List<DTOFaction> FireDept() //1
        {
            var _ranks = new List<DTOFaction>();
            _ranks.Add(new DTOFaction() { Level = 9, Rank = "Fire & EMS Chief", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 8, Rank = "Deputy Chief", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 7, Rank = "Battalion Chief", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 6, Rank = "Captain", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 5, Rank = "Lieutenant", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 4, Rank = "Engineer" });
            _ranks.Add(new DTOFaction() { Level = 3, Rank = "FireFighter II" });
            _ranks.Add(new DTOFaction() { Level = 2, Rank = "Firefighter" });
            _ranks.Add(new DTOFaction() { Level = 1, Rank = "Fire Candidate" });
            return _ranks;
        }
        public static List<DTOFaction> EMS() //2
        {
            var _ranks = new List<DTOFaction>();
            _ranks.Add(new DTOFaction() { Level = 6, Rank = "Paramedic In Charge", isCommand = true });
            _ranks.Add(new DTOFaction() { Level = 5, Rank = "Paramedic II" });
            _ranks.Add(new DTOFaction() { Level = 4, Rank = "Paramedic" });
            _ranks.Add(new DTOFaction() { Level = 3, Rank = "Advanced EMT" });
            _ranks.Add(new DTOFaction() { Level = 2, Rank = "EMT" });
            _ranks.Add(new DTOFaction() { Level = 1, Rank = "EMT Candidate" });
            return _ranks;
        }
        #endregion
    }
}

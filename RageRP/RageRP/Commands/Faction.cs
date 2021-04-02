using GTANetworkAPI;
using RageRP.Server.DAL.DTO;
using RageRP.Server.Data;
using RageRP.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RageRP.Server.Commands
{
    class Faction : Script
    {
        private PlayerService _playerService;
        public Faction()
        {
            _playerService = new PlayerService();
        }

        [Command("factionhelp", Alias = "fhelp")]
        public async void Help(Client player)
        {
            string commands = $"!{{##efefef}} Available Faction Commands: ";
            foreach (var c in Gamedata.FactionCommands)
            {
                commands += $"!{{#ffffff}}{c}, ";
            }
            commands = commands.Remove(commands.Length - 1);
            player.SendChatMessage($"{commands}");
        }

        [Command("faction", GreedyArg = true)]
        public async void factionManage(Client player, string action, int id, string greedyarg = null)
        {
            FactionModel result = GetRank(new FactionModel() { currentPlayer = player });
            if (result.hasError)
                return;

            if(result.isCommand)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);

                if(target != null)
                {
                    if (target == player)
                    {
                        player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot manage yourself!");
                        return;
                    }

                    string CharacterName = player.GetSharedData("CharacterName");
                    string Name = target.GetSharedData("CharacterName");
                    long CharacterID = long.Parse(target.GetSharedData("CharacterID").ToString());

                    bool updateResult = false;
                    int currentLevel = 0; int targetLevel = 0;

                    bool isTargetEMS = Convert.ToBoolean(target.GetSharedData("isEMS"));
                    bool isTargetPolice = Convert.ToBoolean(target.GetSharedData("isPolice"));

                    int _rank = 0;
                    if (action.ToLower() == "rank")
                    {
                        if (!string.IsNullOrEmpty(greedyarg))
                            _rank = Convert.ToInt32(greedyarg);
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [ERROR] No rank supplied.");
                            return;
                        }
                    }
                    int _newDepartment = 0;
                    if (action.ToLower() == "department")
                    {
                        if (!string.IsNullOrEmpty(greedyarg))
                        {
                            _newDepartment = Convert.ToInt32(greedyarg);
                            if (isTargetEMS)
                                _rank = Convert.ToInt32(target.GetSharedData("EMSLevel"));
                            else if(isTargetPolice)
                                _rank = Convert.ToInt32(target.GetSharedData("PoliceLevel"));
                            else
                            {
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] Player does not have a rank. Submit a /report to get this fixed.");
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] If no staff are online, Submit a report on the forums at {Gamedata.WebsiteURL}");
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] ErrRef: repeatreceivedegreedayfish");
                            }
                        }
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [ERROR] No department supplied.");
                            return;
                        }
                    }

                    List<DTOFaction> ranks = null;
                    List<Client> clients = null;
                    int department = Convert.ToInt32(target.GetSharedData("Department"));
                    string currentRank = "";
                    switch (action.ToLower())
                    {
                        case "add":
                            if (result.isEMS)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isEMS")) == true);
                                if (isTargetEMS)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot add someone that is already in your faction!");
                                    return;
                                }

                                if (department == 1)
                                    ranks = FactionDepartments.FireDept();
                                else if (department == 2)
                                    ranks = FactionDepartments.EMS();
                                else
                                    ranks = FactionDepartments.FireDept();
                            }
                            else if (result.isPolice)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isPolice")) == true);
                                if (isTargetPolice)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot add someone that is already in your faction!");
                                    return;
                                }

                                if (department == 1)
                                    ranks = FactionDepartments.Police();
                                else if (department == 2)
                                    ranks = FactionDepartments.PoliceDetective();
                                else if (department == 3)
                                    ranks = FactionDepartments.PoliceSRT();
                                else
                                    ranks = FactionDepartments.Police();
                            }
                            updateResult = await _playerService.UpdateCharacterFaction(new DTO.DTOCharacter()
                            {
                                CharacterID = CharacterID,
                                isEMS = result.isEMS,
                                isPolice = result.isPolice,
                                EMSLevel = result.isEMS ? _rank : 0,
                                PoliceLevel = result.isPolice ? _rank : 0,
                                Department = 1
                            });
                            target.SetSharedData("isPolice", result.isPolice);
                            target.SetSharedData("isEMS", result.isEMS);
                            target.SetSharedData("EMSLevel", result.isEMS ? _rank : 0);
                            target.SetSharedData("PoliceLevel", result.isPolice ? _rank : 0);
                            target.SetSharedData("Department", 1);

                            string rank1 = ranks.Find(x => x.Level == 1).Rank;
                            foreach (var c in clients)
                            {
                                c.SendChatMessage($"!{{#96baff}} [Faction] {rank1} {Name} has joined the Faction!");
                            }
                            break;
                        case "remove":
                            if (result.isEMS)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isEMS")) == true);
                                if (!isTargetEMS)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot remove someone that is not in your faction!");
                                    return;
                                }
                                currentLevel = result.EMSLevel;
                                targetLevel = Convert.ToInt32(target.GetSharedData("EMSLevel"));

                                if (department == 1)
                                    ranks = FactionDepartments.Police();
                                else if (department == 2)
                                    ranks = FactionDepartments.PoliceDetective();
                                else if (department == 3)
                                    ranks = FactionDepartments.PoliceSRT();
                                else
                                    ranks = FactionDepartments.Police();
                            }
                            else if (result.isPolice)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isPolice")) == true);
                                if (!isTargetPolice)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot remove someone that is not in your faction!");
                                    return;
                                }
                                currentLevel = result.PoliceLevel;
                                targetLevel = Convert.ToInt32(target.GetSharedData("PoliceLevel"));

                                if (department == 1)
                                    ranks = FactionDepartments.Police();
                                else if (department == 2)
                                    ranks = FactionDepartments.PoliceDetective();
                                else if (department == 3)
                                    ranks = FactionDepartments.PoliceSRT();
                                else
                                    ranks = FactionDepartments.Police();
                            }
                            
                            if (currentLevel <= targetLevel)
                            {
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot remove someone of the same or higher rank!");
                                return;
                            }

                            updateResult = await _playerService.UpdateCharacterFaction(new DTO.DTOCharacter()
                            {
                                CharacterID = CharacterID,
                                isEMS = false,
                                isPolice = false,
                                EMSLevel = result.isEMS ? 0 : 0,
                                PoliceLevel = result.isPolice ? 0 : 0,
                                Department = 0
                            });
                            target.SetSharedData("isPolice", false);
                            target.SetSharedData("isEMS", false);
                            target.SetSharedData("EMSLevel", 0);
                            target.SetSharedData("PoliceLevel", 0);
                            target.SetSharedData("Department", 0);

                            currentRank = ranks.Find(x => x.Level == targetLevel).Rank;
                            foreach (var c in clients)
                            {
                                c.SendChatMessage($"!{{#96baff}} [Faction] {currentRank} {Name} was removed from the faction by {CharacterName}");
                            }
                            break;
                        case "rank":
                            if (result.isEMS)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isEMS")) == true);
                                if (!isTargetEMS)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot manage someone that is not in your faction!");
                                    return;
                                }
                                currentLevel = result.EMSLevel;
                                targetLevel = Convert.ToInt32(target.GetSharedData("EMSLevel"));

                                if (department == 1)
                                    ranks = FactionDepartments.FireDept();
                                else if (department == 2)
                                    ranks = FactionDepartments.EMS();
                                else
                                    ranks = FactionDepartments.FireDept();
                            }
                            else if (result.isPolice)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isPolice")) == true);
                                if (!isTargetPolice)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot manage someone that is not in your faction!");
                                    return;
                                }
                                currentLevel = result.PoliceLevel;
                                targetLevel = Convert.ToInt32(target.GetSharedData("PoliceLevel"));

                                if (department == 1)
                                    ranks = FactionDepartments.Police();
                                else if (department == 2)
                                    ranks = FactionDepartments.PoliceDetective();
                                else if (department == 3)
                                    ranks = FactionDepartments.PoliceSRT();
                                else
                                    ranks = FactionDepartments.Police();
                            }
                            
                            if (currentLevel <= targetLevel)
                            {
                                player.SendChatMessage($"!{{#ff847c}} [error] you cannot manage someone of the same or higher rank!");
                                return;
                            }

                            if (ranks.Find(x => x.Level == _rank) != null)
                            {
                                updateResult = await _playerService.UpdateCharacterFaction(new DTO.DTOCharacter()
                                {
                                    CharacterID = CharacterID,
                                    isEMS = Convert.ToBoolean(target.GetSharedData("isEMS")),
                                    isPolice = Convert.ToBoolean(target.GetSharedData("isPolice")),
                                    EMSLevel = result.isEMS ? _rank : 0,
                                    PoliceLevel = result.isPolice ? _rank : 0,
                                    Department = Convert.ToInt32(target.GetSharedData("Department").ToString())
                                });
                                target.SetSharedData("EMSLevel", result.isEMS ? _rank : 0);
                                target.SetSharedData("PoliceLevel", result.isPolice ? _rank : 0);

                                var currentTargetFactionRank = ranks.Find(x => x.Level == targetLevel);
                                int currentRankLevel = currentTargetFactionRank.Level;
                                string currentTargetRank = currentTargetFactionRank.Rank;
                                string rankAction = "";
                                var newTargetFactionRank = ranks.Find(x => x.Level == _rank);
                                string newTargetRank = newTargetFactionRank.Rank;
                                if (currentRankLevel <= _rank)
                                    rankAction = $"Promoted to {newTargetRank}";
                                else
                                    rankAction = $"Demoted to {newTargetRank}";

                                foreach (var c in clients)
                                {
                                    c.SendChatMessage($"!{{#96baff}} [Faction] {currentTargetRank} {Name} was {rankAction} to  by {CharacterName}");
                                }
                            }
                            else
                            {
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] Rank {_rank} does not exist in that players department({department})!");
                                return;
                            }
                            break;
                        case "department":
                            if (result.isEMS)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isEMS")) == true);
                                if (!isTargetEMS)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot manage someone that is not in your faction!");
                                    return;
                                }
                                currentLevel = result.EMSLevel;
                                targetLevel = Convert.ToInt32(target.GetSharedData("EMSLevel"));

                                if (_newDepartment == 1)
                                    ranks = FactionDepartments.FireDept();
                                else if (_newDepartment == 2)
                                    ranks = FactionDepartments.EMS();
                                else
                                    ranks = FactionDepartments.FireDept();
                            }
                            else if (result.isPolice)
                            {
                                clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isEMS")) == true);
                                if (!isTargetPolice)
                                {
                                    player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot manage someone that is not in your faction!");
                                    return;
                                }
                                currentLevel = result.PoliceLevel;
                                targetLevel = Convert.ToInt32(target.GetSharedData("PoliceLevel"));

                                if (_newDepartment == 1)
                                    ranks = FactionDepartments.Police();
                                else if (_newDepartment == 2)
                                    ranks = FactionDepartments.PoliceDetective();
                                else if (_newDepartment == 3)
                                    ranks = FactionDepartments.PoliceSRT();
                                else
                                    ranks = FactionDepartments.Police();
                            }

                            if (currentLevel <= targetLevel)
                            {
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] You cannot manage someone of the same or higher rank!");
                                return;
                            }

                            if (ranks.Find(x => x.Level == _rank) != null)
                            {
                                updateResult = await _playerService.UpdateCharacterFaction(new DTO.DTOCharacter()
                                {
                                    CharacterID = CharacterID,
                                    isEMS = Convert.ToBoolean(target.GetSharedData("isEMS")),
                                    isPolice = Convert.ToBoolean(target.GetSharedData("isPolice")),
                                    EMSLevel = result.isEMS ? _rank : 0,
                                    PoliceLevel = result.isPolice ? _rank : 0,
                                    Department = _newDepartment
                                });
                                target.SetSharedData("Department", _newDepartment);
                                player.SendChatMessage($"!{{#96baff}} [FACTION] Set {Name}'s Department to {_newDepartment}");
                                target.SendChatMessage($"!{{#96baff}} [FACTION] Your Department was set to to {_newDepartment} by {CharacterName}");
                            }
                            else
                            {
                                player.SendChatMessage($"!{{#ff847c}} [ERROR] The Players rank is too high for Department !{{#ffffff}}{department}. !{{#ff847c}}Please set the rank to the appropriate rank before changing departments.");
                                return;
                            }
                            break;
                        default:
                            player.SendChatMessage($"!{{#ff847c}} [ERROR] Action '{action}' does not exist!");
                            return;
                    }
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [ERROR] That player does not exist!");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("f", GreedyArg = true)]
        public async void factionChat(Client player, string message)
        {
            string CharacterName = player.GetSharedData("CharacterName").ToString();

            FactionModel result = GetRank(new FactionModel() { currentPlayer = player });
            if (result.hasError)
                return;

            foreach (var c in result.clients)
            {
                c.SendChatMessage($"!{{#96baff}} [FactionChat] {result.rank} {CharacterName}: !{{#ffffff}}{message}");
            }
            AuditLog($"[FactionChat] {CharacterName}: {message}");
        }

        private FactionModel GetRank(FactionModel model)
        {
            bool isPolice = Convert.ToBoolean(model.currentPlayer.GetSharedData("isPolice"));
            bool isEMS = Convert.ToBoolean(model.currentPlayer.GetSharedData("isEMS"));
            int policeLevel = Convert.ToInt32(model.currentPlayer.GetSharedData("PoliceLevel"));
            int emsLevel = Convert.ToInt32(model.currentPlayer.GetSharedData("EMSLevel"));

            model.isPolice = isPolice;
            model.isEMS = isEMS;
            model.PoliceLevel = policeLevel;
            model.EMSLevel = emsLevel;

            if (!isPolice && !isEMS)
            {
                model.currentPlayer.SendChatMessage($"!{{#ff847c}} [ERROR] You are not in a faction!");
                model.hasError = true;
                return model;
            }
            if (policeLevel == 0 && emsLevel == 0)
            {
                model.currentPlayer.SendChatMessage($"!{{#ff847c}} [ERROR] You do not have a rank. Submit a /report to get this fixed.");
                model.currentPlayer.SendChatMessage($"!{{#ff847c}} [ERROR] If no staff are online, Submit a report on the forums at {Gamedata.WebsiteURL}");
                model.currentPlayer.SendChatMessage($"!{{#ff847c}} [ERROR] ErrRef: towhatkilljoinstop");
                model.hasError = true;
                return model;
            }

            var players = NAPI.Pools.GetAllPlayers();

            int department = Convert.ToInt32(model.currentPlayer.GetSharedData("Department"));
            List<DTOFaction> ranks = null;
            if (isPolice)
            {
                model.clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isPolice")) == true);
                if (department == 1)
                    ranks = FactionDepartments.Police();
                else if(department == 2)
                    ranks = FactionDepartments.PoliceDetective();
                else if(department == 3)
                    ranks = FactionDepartments.PoliceSRT();
                else
                    ranks = FactionDepartments.Police();

                var playerRank = ranks.FirstOrDefault(x => x.Level == policeLevel);
                model.rank = playerRank.Rank;
                model.isCommand = playerRank.isCommand;
            }
            else if (isEMS)
            {
                model.clients = players.FindAll(x => Convert.ToBoolean(x.GetSharedData("isEMS")) == true);
                if(department == 1)
                    ranks = FactionDepartments.FireDept();
                else if (department == 2)
                    ranks = FactionDepartments.EMS();
                else
                    ranks = FactionDepartments.FireDept();

                var playerRank = ranks.FirstOrDefault(x => x.Level == emsLevel);
                model.rank = playerRank.Rank;
                model.isCommand = playerRank.isCommand;
            }
            else
            {
                model.currentPlayer.SendChatMessage($"!{{#ff847c}} [ERROR] An unexpected error occured. ErrRef: regionroadhighnoeat");
                model.hasError = true;
                return model;
            }

            return model;
        }
        
        private void AuditLog(string Audit)
        {

        }
    }
}

 using GTANetworkAPI;
using RageRP.DTO;
using RageRP.Server.Data;
using RageRP.Server.Encryption;
using RageRP.Server.Helpers;
using RageRP.Server.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RageRP.Server.Commands
{
    public class Admin : Script
    {
        private AdminService _adminService;
        private PlayerService _playerService;
        public Admin()
        {
            _adminService = new AdminService();
            _playerService = new PlayerService();
        }

        [Command("ahelp")]
        public async void Help(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            if(isAdmin && AdminLevel >= 1)
            {
                string commands = $"!{{##efefef}} Available Admin Commands: ";
                foreach (var c in Gamedata.AdminCommands)
                {
                    commands += $"!{{#ffffff}} {c}, ";
                }
                commands = commands.Remove(commands.Length - 1);
                player.SendChatMessage($"{commands}");
            }
        }

        [Command("a", SensitiveInfo = true, GreedyArg = true)]
        public void AdminChat(Client player, string Message)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var admins = Gamedata.playerList.Where(x => x.isAdmin == true).ToList();
                var players = NAPI.Pools.GetAllPlayers();
                foreach (var a in admins)
                {
                    var _player = players.Where(x => x.Handle.Value == a.Handle).FirstOrDefault();
                    _player.SendChatMessage($"!{{#66ffff}} [AdminChat] Level {AdminLevel} Admin {CharacterName}: !{{#ffffff}}{Message}");
                }
                AuditLog($"[AdminChat] Admin {CharacterName}: {Message}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("kick", SensitiveInfo = true, GreedyArg = true)]
        public async void Kick(Client player, int id, string Reason = null)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");
                
                if (string.IsNullOrEmpty(Reason))
                    Reason = "No Reason Given";

                string disconnectMessage = $"Kicked by {CharacterName} - {Reason}";
                target.TriggerEvent("disconnected", disconnectMessage);
                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} was kicked by {CharacterName}. Reason: {Reason}");

                await Task.Delay(600);
                target.Kick(disconnectMessage);
                AuditLog($"[ADMIN] {Name} was kicked by {CharacterName}. Reason: {Reason}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("slap", SensitiveInfo = true)]
        public void Slap(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                //Slap the person
                target.Position = new Vector3(target.Position.X, target.Position.Y, target.Position.Z + 8f);

                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {CharacterName} has slapped {Name}");
                AuditLog($"[ADMIN] {CharacterName} has slapped {Name}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }
        
        [Command("aduty", SensitiveInfo = true)]
        public void ADuty(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                bool currentDuty = Convert.ToBoolean(player.GetSharedData("isAdminOnDuty"));
                if(!currentDuty)
                {
                    NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {CharacterName} is now on duty");
                    player.TriggerEvent("setInvincible", true);
                }
                else
                {
                    NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {CharacterName} is no longer on duty");
                    //NAPI.Player.SetPlayerName(player, CharacterName);
                    player.TriggerEvent("setInvincible", false);
                }
                player.SetSharedData("isAdminOnDuty", !currentDuty);
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }
        
        [Command("afine", SensitiveInfo = true, GreedyArg = true)]
        public async void Fine(Client player, int id, int Amount, string Reason = null)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                if (string.IsNullOrEmpty(Reason))
                    Reason = "No Reason Given";

                int cash = Convert.ToInt32(target.GetSharedData("Cash"));
                int bank = Convert.ToInt32(target.GetSharedData("Bank"));

                int ActualAmount = Amount;

                if(cash < Amount)
                {
                    if(cash != 0)
                    {
                        Amount = Amount - cash;
                        cash = 0;
                    }
                }
                else
                {
                    Amount = Amount - cash;
                    cash = Amount;
                }
                if(Amount != 0)
                {
                    bank = bank - Amount;
                }

                await _playerService.UpdateCharacterCash(new DTO.DTOCharacter()
                {
                    CharacterID = long.Parse(target.GetSharedData("CharacterID").ToString()),
                    Cash = cash
                });
                await _playerService.UpdateCharacterBank(new DTO.DTOCharacter()
                {
                    CharacterID = long.Parse(target.GetSharedData("CharacterID").ToString()),
                    Bank = bank
                });

                target.SetSharedData("Cash", cash);
                target.SetSharedData("Bank", bank);

                target.TriggerEvent("RemoveCash", cash);

                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} has been fined {ActualAmount} by {CharacterName}. Reason: {Reason}");
                target.SendChatMessage($"!{{#ff847c}} [ADMIN] Your bank balance is now ${bank}");
                AuditLog($"[ADMIN] {Name} has been fined {ActualAmount} by {CharacterName}. Reason: {Reason}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("freeze", SensitiveInfo = true)]
        public void Freeze(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                bool isFrozen = Convert.ToBoolean(target.GetSharedData("isFrozen"));
                string action;
                if(!isFrozen)
                    action = "frozen";
                else
                    action = "unfrozen";

                isFrozen = !isFrozen;
                target.SetSharedData("isFrozen", isFrozen);
                target.TriggerEvent("freezeHandler", isFrozen);

                AdminAudit(CharacterName, $"has {action} {Name}");
                AuditLog($"[ADMIN] {CharacterName} {action} {Name}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("reports", SensitiveInfo = true)]
        public void Reports(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var reports = Gamedata.reports;
                if(reports.Count == 0)
                {
                    player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}No reports");
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}There are !{{#ff847c}}{reports.Count} !{{#ffffff}}active reports");
                }
                //foreach (var r in reports)
                //{
                //    player.SendChatMessage($"!{{#ff847c}} [REPORT #{r.reportID}] {r.ReportingPlayer} Reported {r.ReportedPlayer} (${r.playerID} || H:{r.Handle}) at {r.ReportedAt}");
                //    player.SendChatMessage($"!{{#ff847c}} [REPORT #{r.reportID}] Reason: {r.reason}");
                //}
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("getreport")]
        public void GetReport(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var reports = Gamedata.reports;
                var assignedReport = reports.FirstOrDefault(x => x.isAssigned == true && x.AssignedTo == player.Handle.Value);
                if(assignedReport != null)
                {
                    player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}} You cannot be assigned to more than one report at once.");
                }
                else
                {
                    if (reports.Count == 0)
                    {
                        player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}} No reports to get");
                    }
                    else
                    {
                        var report = reports.LastOrDefault(x => x.isAssigned == false && x.ReportedHandle != player.Handle.Value);
                        if(report != null)
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT #{report.reportID}] !{{#ffffff}}{report.ReportingPlayer}({report.ReportingPlayerID} || H:{report.ReportingHandle}) Reported {report.ReportedPlayer}({report.ReportedPlayerID} || H:{report.ReportedHandle}) at {report.ReportedAt}");
                            player.SendChatMessage($"!{{#ff847c}} [REPORT #{report.reportID}] !{{#ffffff}}Reason: {report.Reason}");
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Use !{{#31f13e}}/acceptreport !{{#ffffff}}or !{{#f23232}}/rejectreport");
                            report.isAssigned = true;
                            report.AssignedTo = player.Handle.Value;
                        }
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}} There are no reports you can accept right now");
                        }
                    }
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("acceptreport", SensitiveInfo = true)]
        public void AcceptReport(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var reports = Gamedata.reports;
                var report = reports.FirstOrDefault(x => x.isAssigned == true && x.AssignedTo == player.Handle.Value);
                if(report != null)
                {
                    var players = NAPI.Pools.GetAllPlayers();
                    var reportingplayer = players.Find(x => x.Handle.Value == report.ReportingHandle);
                    if(reportingplayer != null)
                    {
                        //Check that the current handle player is the same as the one that reported, so we don't accidentally send the messages to the wrong person.
                        long reportingPlayerID = EncryptionService.DecryptID(reportingplayer.GetSharedData("playerID"));
                        if(reportingPlayerID != report.ReportingPlayerID)
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}The reporting player is no longer connected.");
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}has been automatically closed.");
                            Gamedata.reports.Remove(report);
                        }
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#31f13e}}Accepted !{{#ffffff}}report !{{#ff847c}}{report.reportID}");
                            reportingplayer.SendChatMessage($"!{{#eb5454}} [REPORT] !{{#ffffff}}Your report against {report.ReportedPlayer} has been !{{#63eb53}}accepted !{{#ffffff}}by Admin {CharacterName}.");
                        }
                    }
                    else
                    {
                        player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}The reporting player is no longer connected.");
                        player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}has been automatically closed.");

                        //TODO: Set a flag for the panel so we can log this as an unresolved ticket for the panel.

                        Gamedata.reports.Remove(report);
                    }
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}You do not have an assigned report.");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("rejectreport", SensitiveInfo = true)]
        public void RejectReport(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var reports = Gamedata.reports;
                var report = reports.FirstOrDefault(x => x.isAssigned == true && x.AssignedTo == player.Handle.Value);
                if (report != null)
                {
                    var players = NAPI.Pools.GetAllPlayers();
                    var reportingplayer = players.Find(x => x.Handle.Value == report.ReportingHandle);
                    if (reportingplayer != null)
                    {
                        //Check that the current handle player is the same as the one that reported, so we don't accidentally send the messages to the wrong person.
                        long reportingPlayerID = EncryptionService.DecryptID(reportingplayer.GetSharedData("playerID"));
                        if (reportingPlayerID != report.ReportingPlayerID)
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}The reporting player is no longer connected.");
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}Ihas been automatically !{{#f23232}}closed.");
                            Gamedata.reports.Remove(report);
                        }
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ff847c}}Rejected !{{#ffffff}}report !{{#ff847c}}#{report.reportID}");
                            reportingplayer.SendChatMessage($"!{{#eb5454}} [REPORT] !{{#ffffff}}Your report against {report.ReportedPlayer} has been !{{#eb5454}}rejected !{{#ffffff}} by Admin {CharacterName}.");
                        }
                    }
                    else
                    {
                        player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}The reporting player is no longer connected.");
                        player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}has been automatically !{{#f23232}}closed.");

                        //TODO: Set a flag for the panel so we can log this as an unresolved ticket for the panel.

                        Gamedata.reports.Remove(report);
                    }
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}You do not have an assigned report.");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("closereport", SensitiveInfo = true)]
        public void CloseReport(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 1)
            {
                var reports = Gamedata.reports;
                var report = reports.FirstOrDefault(x => x.isAssigned == true && x.AssignedTo == player.Handle.Value);
                if (report != null)
                {
                    var players = NAPI.Pools.GetAllPlayers();
                    var reportingplayer = players.Find(x => x.Handle.Value == report.ReportingHandle);
                    if (reportingplayer != null)
                    {
                        //Check that the current handle player is the same as the one that reported, so we don't accidentally send the messages to the wrong person.
                        long reportingPlayerID = EncryptionService.DecryptID(reportingplayer.GetSharedData("playerID"));
                        if (reportingPlayerID != report.ReportingPlayerID)
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}Ihas been !{{#ff847c}}closed.");
                        }
                        else
                        {
                            player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}has been !{{#ff847c}}closed.");
                            reportingplayer.SendChatMessage($"!{{#eb5454}} [REPORT] !{{#ffffff}}Your report against !{{#eb5454}}{report.ReportedPlayer} !{{#ffffff}}has been !{{#eb5454}}closed !{{#ffffff}} by Admin {CharacterName}.");
                        }
                    }
                    else
                    {
                        player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Ingame Report !{{#ff847c}}#{report.reportID} !{{#ffffff}}has been !{{#ff847c}}closed.");
                    }
                    //TODO: Set ticket as closed for the panel
                    Gamedata.reports.Remove(report);
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}You do not have an assigned report.");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("togglemute")]
        public void toggleMute(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                bool isMuted = Convert.ToBoolean(target.GetSharedData("isMuted"));
                bool mute = false;
                if (isMuted)
                    mute = false;
                else
                    mute = true;

                target.SetSharedData("isMuted", mute);
                target.TriggerEvent("ToggleMuted", mute, "Muted by an Administrator");

                if(mute)
                    NAPI.ClientEvent.TriggerClientEventForAll("mutedPlayer", target);

                string muteAction = mute ? "muted" : "unmuted";
                target.SendChatMessage($"!{{#ff847c}}You were {muteAction} by {CharacterName}!");
                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} was {muteAction}  by {CharacterName}.");
                AuditLog($"[ADMIN] {CharacterName} {muteAction} {Name}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("tp", Alias = "teleport")]
        public void Teleport(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");
                player.Position = new Vector3(target.Position.X + 1f, target.Position.Y, target.Position.Z + 1f);
                AdminAudit(CharacterName, $"teleported to {Name}");
                AuditLog($"[ADMIN] {CharacterName} teleported to {Name}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("tphere", Alias = "teleporthere")]
        public void TeleportHere(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");
                target.Position = new Vector3(player.Position.X + 1f, player.Position.Y, player.Position.Z + 1f);
                AdminAudit(CharacterName, $"teleported {Name} to them");
                target.SendChatMessage($"!{{#ff847c}} You were teleported to {CharacterName}");
                AuditLog($"[ADMIN] {CharacterName} teleported {Name} to them");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("tempban", SensitiveInfo = true, GreedyArg = true)]
        public async void TempBan(Client player, int id, string Time, string Reason = null)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");
                
                if (string.IsNullOrEmpty(Reason))
                    Reason = "No Reason Given";

                Regex regex = new Regex(@"^[0-9a-zA-Z]+$");
                if (!regex.IsMatch(Time))
                    return;

                var number = Regex.Match(Time, @"[0-9]+");
                int _Time = Convert.ToInt32(number.Value);
                var modifer = Regex.Match(Time, @"[a-zA-Z]+");
                DateTime FinalTime;
                string _modiferType;
                switch(modifer.Value.ToLower())
                {
                    case "m":
                        FinalTime = DateTime.Now.AddMinutes(_Time);
                        _modiferType = "Minutes";
                        break;
                    case "h":
                        FinalTime = DateTime.Now.AddHours(_Time);
                        _modiferType = "Hours";
                        break;
                    case "d":
                        FinalTime = DateTime.Now.AddDays(_Time);
                        _modiferType = "Days";
                        break;
                    default:
                        FinalTime = DateTime.Now.AddDays(_Time);
                        _modiferType = "Days";
                        break;
                }
                if(FinalTime > DateTime.Now.AddDays(7))
                {
                    player.SendChatMessage($"!{{#ff847c}} [ADMIN USAGE] /tempban [id] [Time (E.G. 30m, 10h, 2d)] [?Reason]");
                    return;
                }

                var _target = Gamedata.playerList.Where(x => x.Handle == target.Handle.Value).FirstOrDefault();

                var adminService = new AdminService();
                bool result = await adminService.BanPlayer(new DTOPlayer()
                {
                    PlayerID = _target.PlayerID,
                    isPermBanned = false,
                    TempBanDate = FinalTime,
                    BanReason = Reason
                });

                if(result)
                {
                    NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} was temporarily banned for {_Time} {_modiferType} by {CharacterName}. Reason: {Reason}");
                    AuditLog($"[ADMIN] {CharacterName} temporarily banned {Name} for {_Time} {_modiferType}. Reason: {Reason}");
                    await Task.Delay(100);
                    var kickHelper = new KickHelper(target, AdminKickHandler.Banned);
                    kickHelper.Kick();
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [ERROR] Failed to TempBan {CharacterName}({id}). ErrRef: surprisepaperhotwatercreate");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("spectate")]
        public void Spectate(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                player.SetSharedData("specatingPlayer", Name);

                player.TriggerEvent("spectate", (Entity)target);

                AuditLog($"[ADMIN] {CharacterName} started spectating {Name}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("stopspectating")]
        public void StopSpectate(Client player, int id)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                player.ResetSharedData("specatingPlayer");

                player.TriggerEvent("stopSpectating");

                AuditLog($"[ADMIN] {CharacterName} stopped spectating {Name}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("toggleooc", SensitiveInfo = true)]
        public void ToggleOOC(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 2)
            {
                Gamedata.isOOCActive = !Gamedata.isOOCActive;
                if(Gamedata.isOOCActive)
                    NAPI.Chat.SendChatMessageToAll($"!{{#FFFFFF}} Global OOC has been !{{#7cff7e}} Activated");
                else
                    NAPI.Chat.SendChatMessageToAll($"!{{#FFFFFF}} Global OOC has been !{{#ff847c}} Deactivated");

                AuditLog($"[ADMIN] {CharacterName} Toggled OOC Chat to {Gamedata.isOOCActive}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("respawn", SensitiveInfo = true)]
        public void Respawn(Client player, int id, bool force = false)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");
                target.SetSharedData("ServerTime", DateTime.Now.ToString("HH:mm:ss"));
                if(force)
                    NAPI.Player.SpawnPlayer(target, target.Position);
                target.StopAnimation();
                target.TriggerEvent("syncTime");
                target.TriggerEvent("setInvincible", false);
                NAPI.Player.SetPlayerHealth(target, 100);
                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} was respawned by {CharacterName}.");
                AuditLog($"[ADMIN] {CharacterName} Respawned {Name}. Force: {force}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("kill", SensitiveInfo = true, GreedyArg = true)]
        public void Kill(Client player, int id, string Reason = null)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");
                
                if (string.IsNullOrEmpty(Reason))
                    Reason = "No Reason Given";

                target.Health = 0;
                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} was killed by {CharacterName}. Reason: {Reason}");
                AuditLog($"[ADMIN] {CharacterName} killed {Name}. Reason: {Reason}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("ban", SensitiveInfo = true, GreedyArg = true)]
        public async void Ban(Client player, int id, string Reason)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);
                var Name = target.GetSharedData("CharacterName");

                if (string.IsNullOrEmpty(Reason))
                    Reason = "No Reason Given";

                var _target = Gamedata.playerList.Where(x => x.Handle == target.Handle.Value).FirstOrDefault();

                var adminService = new AdminService();
                bool result = await adminService.BanPlayer(new DTOPlayer()
                {
                    PlayerID = _target.PlayerID,
                    isPermBanned = true,
                    BanReason = Reason
                });

                if (result)
                {
                    NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name} was banned by {CharacterName}. Reason: {Reason}");
                    AuditLog($"[ADMIN] {CharacterName} banned {Name}. Reason: {Reason}");
                    await Task.Delay(100);
                    var kickHelper = new KickHelper(target, AdminKickHandler.Banned);
                    kickHelper.Kick();
                }
                else
                {
                    player.SendChatMessage($"!{{#ff847c}} [ERROR] Failed to Ban {CharacterName}({id}). ErrRef: tenteethmelrosefinishnear");
                }

                AuditLog($"[ADMIN] {CharacterName} banned {Name}. Reason: {Reason}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("modifyvehicle", SensitiveInfo = true)]
        public void ModifyVehicle(Client player, int modid, int index)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var vehicle = player.Vehicle;
                if (vehicle != null)
                {
                    vehicle.SetMod(modid, index);
                    AdminAudit(CharacterName, $"modified their vehicle mod {modid} to {index}");
                    AuditLog($"[ADMIN] {CharacterName} modified their vehicle mod {modid} to {index}");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("modifyvehicleextra", SensitiveInfo = true)]
        public void ModifyVehicleExtra(Client player, int extraid, bool toggle)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var vehicle = player.Vehicle;
                if (vehicle != null)
                {
                    vehicle.SetExtra(extraid, toggle);
                    AdminAudit(CharacterName, $"modified their vehicle extra {extraid} to {toggle}");
                    AuditLog($"[ADMIN] {CharacterName} modified their vehicle extra {extraid} to {toggle}");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("spawnvehicle", SensitiveInfo = true)]
        public async void SpawnVehicle(Client player, string Name, int col1 = 0, int col2 = 0)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                int requestedVehicle = (int)NAPI.Util.GetHashKey(Name);
                if (requestedVehicle != 0)
                {
                    Vector3 carPos = player.Position;
                    Vector3 rotation = player.Rotation;

                    Random rand = new Random();
                    if (col1 == 0)
                    {
                        col1 = rand.Next(0, 255);
                    }
                    if (col2 == 0)
                    {
                        col2 = rand.Next(0, 255);
                    }
                    await Task.Factory.StartNew(() =>
                    {
                        NAPI.Task.Run(() =>
                        {
                            Vehicle veh = NAPI.Vehicle.CreateVehicle(requestedVehicle, carPos, new Vector3(0f, 0f, rotation.Z), col1, col2, numberPlate: "ADMIN");

                            player.SetIntoVehicle(veh, -1);
                            player.SendChatMessage($"Spawned Vehicle {veh.DisplayName} {veh.ClassName}");
                            if (veh.ClassName == 18)
                            {
                                veh.SetSharedData("isEmergency", true);
                            }
                            else
                            {
                                veh.SetSharedData("isEmergency", false);
                            }

                            veh.SetSharedData("vehicleHandleID", veh.Handle.Value);
                            veh.SetSharedData("isEngineOn", false);
                            veh.EngineStatus = false;

                            veh.SetSharedData("isPolice", VehicleStores.PoliceVehicles().Any(x => x.VehicleCode == Name));
                            veh.SetSharedData("isFire", VehicleStores.FireVehicles().Any(x => x.VehicleCode == Name));
                            veh.SetSharedData("isEMS", VehicleStores.EMSVehicles().Any(x => x.VehicleCode == Name));
                            Gamedata.vehicleList.Add(new DTOVehicle()
                            {
                                CharacterID = long.Parse(player.GetSharedData("CharacterID").ToString()),
                                Handle = veh.Handle.Value
                            });

                            AuditLog($"[ADMIN] {CharacterName} spawned a {Name}({veh.Handle.Value})");
                        });
                    });
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("repairvehicle", SensitiveInfo = true)]
        public void RepairVehicle(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var vehicle = player.Vehicle;
                if(vehicle != null)
                {
                    NAPI.Vehicle.RepairVehicle(vehicle);
                    NAPI.Vehicle.SetVehicleHealth(vehicle, 100f);
                    NAPI.Vehicle.SetVehicleBodyHealth(vehicle, 100f);
                    NAPI.Vehicle.SetVehicleEngineHealth(vehicle, 100f);
                    AdminAudit(CharacterName, $"repaired their vehicle");
                    AuditLog($"[ADMIN] {CharacterName} repaired their vehicle");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("deletevehicle", SensitiveInfo = true)]
        public void DeleteVehicle(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 3)
            {
                var vehicle = player.Vehicle;
                if (vehicle != null)
                {
                    Gamedata.vehicleList.RemoveWhere(x => x.Handle == vehicle.Handle.Value);
                    vehicle.Delete();
                    AuditLog($"[ADMIN] {CharacterName} deleted a vehicle");
                }
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("spawnweapon", SensitiveInfo = true)]
        public void SpawnWeapon(Client player, WeaponHash Name, int Ammo)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 4)
            {
                NAPI.Player.GivePlayerWeapon(player, Name, Ammo);
                AdminAudit(CharacterName, $"spawned weapon {Name} with {Ammo} ammo");
                AuditLog($"[ADMIN] {CharacterName} spawned weapon {Name} with {Ammo} ammo");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("modifyweapon", SensitiveInfo = true)]
        public void ModifyWeapon(Client player, WeaponHash Mod, WeaponComponent Selection)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 4)
            {
                NAPI.Player.SetPlayerWeaponComponent(player, Mod, Selection);
                AdminAudit(CharacterName, $"modified their weapon {Mod} {Selection}");
                AuditLog($"[ADMIN] {CharacterName} modified their weapon {Mod} {Selection}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("settime")]
        public void SetTime(Client player, int hour, int minute)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 4)
            {
                NAPI.World.SetTime(hour, minute, 0);
                AuditLog($"[ADMIN] {CharacterName} set the time to {hour}:{minute}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("setweather")]
        public void SetWeather(Client player, int Weather)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 4)
            {
                NAPI.World.SetWeather((Weather)Weather);
                AuditLog($"[ADMIN] {CharacterName} set the weather to {Weather}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        [Command("adminlevel", SensitiveInfo = true)]
        public async void AdminLevel(Client player, int id, int Level)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
            string CharacterName = player.GetSharedData("CharacterName").ToString();
            if (isAdmin && AdminLevel >= 5)
            {
                var players = NAPI.Pools.GetAllPlayers();
                var target = players.Find(x => x.Handle.Value == id);

                if(target == player)
                {
                    player.SendChatMessage("!{{#ff847c}} You cannot change your own admin level");
                    return;
                }

                var Name = target.GetSharedData("CharacterName");
                var targetPlayer = Gamedata.playerList.Where(x => x.Handle == id).FirstOrDefault();

                target.SetSharedData("AdminLevel", Level);
                bool _isAdmin = false;
                if (Level == 0)
                    _isAdmin = false;
                else
                    _isAdmin = true;

                target.SetSharedData("isAdmin", _isAdmin);
                targetPlayer.isAdmin = _isAdmin;
                targetPlayer.AdminLevel = Level;

                long playerID = EncryptionService.DecryptID(target.GetSharedData("playerID"));

                await _adminService.UpdateAdminLevel(new DTOPlayer()
                {
                    PlayerID = playerID,
                    AdminLevel = Level,
                    isAdmin = _isAdmin
                });

                NAPI.Chat.SendChatMessageToAll($"!{{#ff847c}} [ADMIN] {Name}'s Admin Level was set to {Level} By {CharacterName}.");
                AuditLog($"[ADMIN] {CharacterName} set the admin level of {Name} to {Level}");
            }
            else
            {
                player.SendChatMessage("TODO_NOPERMISSION");
            }
        }

        private void AdminAudit(string player, string message)
        {
            var admins = Gamedata.playerList.Where(x => x.isAdmin == true);
            var players = NAPI.Pools.GetAllPlayers();
            foreach (var a in admins)
            {
                var current = players.Where(x => x.Handle.Value == a.Handle).FirstOrDefault();
                current.SendChatMessage($"!{{#ff847c}} [AdminAudit] {player} {message}");
            }
        }

        private void AuditLog(string Audit)
        {

        }
    }
}
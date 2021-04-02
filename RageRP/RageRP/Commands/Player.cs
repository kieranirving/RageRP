using GTANetworkAPI;
using RageRP.Server.Encryption;
using RageRP.Server.Helpers;
using RageRP.Server.Services;
using System;
using System.Linq;

namespace RageRP.Server.Commands
{
    public class Player : Script
    {
        private PlayerService _playerService;
        public Player()
        {
            _playerService = new PlayerService();
        }

        [Command("me", GreedyArg = true)]
        public async void TextEmoteAction(Client player, string message)
        {
            var players = NAPI.Pools.GetAllPlayers();
            string name = player.GetSharedData("DisplayName").ToString();
            bool _chattingIsAdmin = Convert.ToBoolean(player.GetSharedData("isAdminOnDuty"));
            int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));

            foreach (var p in players)
            {
                string colour = "#C2A2DA";
                float distance = p.Position.DistanceTo(player.Position);


                //If the distance is more than the max for a local message
                if (distance > 30.0f)
                    continue;

                colour = DistanceHelper.getDistanceEmoteColour(distance);

                p.SendChatMessage($"!{{{colour}}} * {name}({sharedID}) {message}");
            }
            //string _colour = "#C2A2DA";
            //player.SendChatMessage($"!{{{_colour}}} * {name}({player.Handle}) {message}");
        }

        [Command("do", GreedyArg = true)]
        public async void TextEmoteActionExtra(Client player, string message)
        {
            var players = NAPI.Pools.GetAllPlayers();
            string name = player.GetSharedData("DisplayName").ToString();
            bool _chattingIsAdmin = Convert.ToBoolean(player.GetSharedData("isAdminOnDuty"));
            int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));

            foreach (var p in players)
            {
                string colour = "#C2A2DA";
                float distance = p.Position.DistanceTo(player.Position);


                //If the distance is more than the max for a local message
                if (distance > 30.0f)
                    continue;

                colour = DistanceHelper.getDistanceEmoteColour(distance);

                p.SendChatMessage($"!{{{colour}}} * {message} [{name}({sharedID})]");
            }
        }

        [Command("admins")]
        public void Admins(Client player)
        {
            bool isAdmin = Convert.ToBoolean(player.GetSharedData("isAdmin"));
            var admins = Gamedata.playerList.Where(x => x.isAdmin == true);
            var ondutyAdmins = admins.Where(x => x.aduty == true).OrderByDescending(x => x.AdminLevel);
            NAPI.Chat.SendChatMessageToPlayer(player, $"!{{#66ffff}}========[Admins On Duty]========");
            if (!ondutyAdmins.Any())
            {
                NAPI.Chat.SendChatMessageToPlayer(player, $"!{{#FFFFFF}} None");
            }
            else
            {
                foreach (var a in ondutyAdmins)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"!{{#66ffff}} Admin Level {a.AdminLevel}: !{{#FFFFFF}} {a.currentCharacter.CharacterName}");
                }
            }
            if (isAdmin)
            {
                var offdutyAdmins = admins.Where(x => x.aduty == false).OrderByDescending(x => x.AdminLevel);
                foreach (var a in offdutyAdmins)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, $"!{{#9b9b9b}} Admin Level {a.AdminLevel}: {a.currentCharacter.CharacterName}");
                }
            }
        }

        [Command("report", GreedyArg = true)]
        public async void report(Client player, int id, string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                player.SendChatMessage($"!{{#eb5454}} You must supply a reason!");
                return;
            }
            var admins = Gamedata.playerList.Where(x => x.isAdmin == true);
            var players = NAPI.Pools.GetAllPlayers();
            var target = players.Find(x => x.Handle.Value == id);
            string TargetName = target.GetSharedData("CharacterName").ToString();
            string CharacterName = player.GetSharedData("CharacterName").ToString();

            long ReportingPlayerID = EncryptionService.DecryptID(player.GetSharedData("playerID"));
            long ReportedPlayerID = EncryptionService.DecryptID(target.GetSharedData("playerID"));

            Gamedata.reportCount = Gamedata.reportCount + 1;
            int count = Gamedata.reportCount;
            Gamedata.reports.Add(new DAL.DTO.DTOReport()
            {
                ReportedPlayerID = ReportedPlayerID,
                ReportingPlayerID = ReportingPlayerID,
                ReportedHandle = id,
                ReportingHandle = player.Handle.Value,
                ReportedPlayer = TargetName,
                ReportingPlayer = CharacterName,
                Reason = reason,
                ReportedAt = DateTime.Now,
                reportID = count
            });

            foreach (var a in admins)
            {
                var _player = players.Where(x => x.Handle.Value == a.Handle).FirstOrDefault();
                _player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}{CharacterName} has reported {TargetName}");
                _player.SendChatMessage($"!{{#ff847c}} [REPORT] !{{#ffffff}}Reason: {reason}");
            }
            player.SendChatMessage($"!{{#eb5454}} [REPORT] !{{#ffffff}}Your report(!{{#eb5454}}#{count}!{{#ffffff}}) has been recieved and will be reviewed by an Administrator");
        }
        [Command("pay")]
        public async void pay(Client player, int id, int amount)
        {
            int currentCash = Convert.ToInt32(player.GetSharedData("Cash"));
            int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));
            int currentCharacterID = long.Parse(player.GetSharedData("CharacterID"));
            string currentName = player.GetSharedData("DisplayName").ToString();
            if (amount > currentCash)
            {
                player.SendChatMessage("!{{#eb5454}} You do not have enough money!");
                return;
            }

            var players = NAPI.Pools.GetAllPlayers();
            var target = players.Find(x => x.Handle.Value == id);
            if(target == null)
            {
                player.SendChatMessage("!{{#eb5454}} That player does not exist!");
                return;
            }
            if(target == player)
            {
                player.SendChatMessage("!{{#eb5454}} You cannot pay yourself!");
                return;
            }

            float distance = target.Position.DistanceTo(player.Position);
            if (distance > 5.0f)
            {
                player.SendChatMessage("!{{#eb5454}} You are too far away!");
                return;
            }

            int targetCash = Convert.ToInt32(target.GetSharedData("Cash"));
            int targetSharedID = Convert.ToInt32(target.GetSharedData("handleID"));
            int targetCharacterID = long.Parse(target.GetSharedData("CharacterID"));
            string targetName = target.GetSharedData("DisplayName").ToString();
            targetCash = targetCash + amount;
            currentCash = currentCash - amount;

            await _playerService.UpdateCharacterCash(new DTO.DTOCharacter()
            {
                CharacterID = currentCharacterID,
                Cash = currentCash
            });
            await _playerService.UpdateCharacterCash(new DTO.DTOCharacter()
            {
                CharacterID = targetCharacterID,
                Cash = targetCash
            });

            target.SetSharedData("Cash", targetCash);
            player.SetSharedData("Cash", currentCash);

            target.TriggerEvent("AddCash", targetCash);
            player.TriggerEvent("RemoveCash", currentCash);

            foreach (var p in players)
            {
                string colour = "#C2A2DA";
                float _distance = p.Position.DistanceTo(player.Position);

                //If the distance is more than the max for a local message
                if (distance > 30.0f)
                    continue;

                colour = DistanceHelper.getDistanceEmoteColour(_distance);

                p.SendChatMessage($"!{{{colour}}} * {currentName}({sharedID}) gives {targetName} ${amount}");
            }
        }

        [Command("advertisement", Alias = "ad", GreedyArg = true)]
        public async void me(Client player, string message)
        {

        }

        [Command("help")]
        public async void Help(Client player)
        {
            string commands = $"!{{##efefef}} Available Commands: ";
            foreach(var c in Gamedata.PlayerCommands)
            {
                commands += $"!{{#ffffff}} {c}, ";
            }
            commands = commands.Remove(commands.Length - 1);
            player.SendChatMessage($"{commands}");
        }

        [Command("time")]
        public async void Time(Client player)
        {
            player.SendChatMessage($"The current time is {DateTime.Now.ToString("HH:mm")}");
        }
    }
}

using GTANetworkAPI;
using RageRP.Server.Helpers;
using System;

namespace RageRP.Server.Events
{
    public class Chat : Script
    {
        #region TextChat
        [ServerEvent(Event.ChatMessage)]
        public async void localChat(Client player, string message)
        {
            var players = NAPI.Pools.GetAllPlayers();
            string name = player.GetSharedData("DisplayName").ToString();
            bool _chattingIsAdmin = Convert.ToBoolean(player.GetSharedData("isAdminOnDuty"));
            int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));

            string _colour = "#FFFFFF";
            if (_chattingIsAdmin)
            {
                name = $"On Duty Admin {name}";
                _colour = "#ff847c";
            }

            foreach (var p in players)
            {
                string colour = "#ffffff";
                float distance = p.Position.DistanceTo(player.Position);

                //If the distance is more than the max for a local message
                if (distance > 50.0f)
                    continue;

                //We don't want to go through the colour checks if the user is an admin
                if (!_chattingIsAdmin)
                    colour = DistanceHelper.getDistanceChatColour(distance);

                p.SendChatMessage($"!{{{colour}}} {name}({sharedID}): {message}");
            }

            //player.SendChatMessage($"!{{{_colour}}} {name}({player.Handle}): {message}");
        }

        [Command("b", GreedyArg = true)]
        public async void localOOCChat(Client player, string message)
        {
            var players = NAPI.Pools.GetAllPlayers();
            string name = player.GetSharedData("DisplayName").ToString();
            bool _chattingIsAdmin = Convert.ToBoolean(player.GetSharedData("isAdminOnDuty"));

            int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));

            string _colour = "#c6c6c6";
            if (_chattingIsAdmin)
            {
                name = $"On Duty Admin {name}";
                _colour = "#ff847c";
            }

            //player.SendChatMessage($"!{{{_colour}}} (( {name}({player.Handle}): {message} ))");

            foreach (var p in players)
            {
                string colour = "#c6c6c6";
                float distance = p.Position.DistanceTo(player.Position);

                //If the distance is more than the max for a local ooc message
                if (distance > 30.0f)
                    continue;

                p.SendChatMessage($"!{{{colour}}} (( {name}({p.Handle.Value}): {message} ))");
            }
        }
        
        [Command("ooc", Alias = "o", GreedyArg = true)]
        public async void OOCChat(Client player, string message)
        {
            if (Gamedata.isOOCActive)
            {
                string name = player.GetSharedData("DisplayName").ToString();
                bool _chattingIsAdmin = Convert.ToBoolean(player.GetSharedData("isAdminOnDuty"));
                if (_chattingIsAdmin)
                {
                    int AdminLevel = Convert.ToInt32(player.GetSharedData("AdminLevel"));
                    name = $"Admin Level {AdminLevel} {name}";
                }

                int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));

                NAPI.Chat.SendChatMessageToAll($"!{{#548dff}} [OOC] {name} ({sharedID}): !{{#ffffff}}{message}");
            }
            else
            {
                player.SendChatMessage("OOC Is not currently Enabled");
            }
        }

        //[Command("getallplayers")]
        //public async void GetAllPlayers(Client player)
        //{
        //    var players = NAPI.Pools.GetAllPlayers();
        //    foreach(var p in players)
        //    {
        //        var characterName = p.GetSharedData("CharacterName").ToString();
        //        player.SendChatMessage($"{characterName} {p.Handle.Value}");
        //    }
        //}

        [Command("pm", GreedyArg = true)]
        public async void PrivateMessage(Client player, int id, string message)
        {
            var players = NAPI.Pools.GetAllPlayers();
            string name = player.GetSharedData("DisplayName").ToString();
            var target = players.Find(x => x.Handle.Value == id);
            if(target != null)
            {
                int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));
                if (target.Handle == player.Handle)
                {
                    player.SendChatMessage($"You can't send messages to yourself");
                }
                else
                {
                    int targetSharedID = Convert.ToInt32(target.GetSharedData("handleID"));
                    player.SendChatMessage($"!{{#ffcc00}} * PM Sent to {target.GetSharedData("CharacterName")} ({targetSharedID}): {message}");
                    target.SendChatMessage($"!{{#ffcc00}} * PM From {player.GetSharedData("CharacterName")} ({sharedID}): {message}");
                    target.SetSharedData("LastPMID", player.Handle.Value);
                }
            }
            else
            {
                player.SendChatMessage($"Could not find player {id}");
            }
        }

        [Command("re", GreedyArg = true)]
        public async void replyToPm(Client player, string message)
        {
            var players = NAPI.Pools.GetAllPlayers();
            string name = player.GetSharedData("DisplayName").ToString();
            var lastID = player.GetSharedData("LastPMID");
            if(lastID != null)
            {
                int sharedID = Convert.ToInt32(player.GetSharedData("handleID"));
                var target = players.Find(x => x.Handle.Value == Convert.ToInt32(lastID));
                int targetSharedID = Convert.ToInt32(target.GetSharedData("handleID"));
                player.SendChatMessage($"!{{#ffcc00}} * PM Sent to {target.GetSharedData("CharacterName")} ({targetSharedID}): {message}");
                target.SendChatMessage($"!{{#ffcc00}} * PM From {player.GetSharedData("CharacterName")} ({sharedID}): {message}");
                player.ResetSharedData("LastPMID");
            }
            else
            {
                player.SendChatMessage($"There is no PM To reply to.");
            }
        }
        
        [Command("radio", Alias = "r", GreedyArg = true)]
        public async void radio(Client player, string message)
        {

        }
        #endregion

        #region VoiceChat
        [RemoteEvent("add_voice_listener")]
        public async void addVoiceListener(Client player, Client target)
        {
            if (target.Exists)
            {
                player.EnableVoiceTo(target);
            }
        }

        [RemoteEvent("remove_voice_listener")]
        public async void removeVoiceListener(Client player, Client target)
        {
            if (target.Exists)
            {
                player.DisableVoiceTo(target);
            }
        }

        [RemoteEvent("ToggleTalking")]
        public async void toggleTalking(Client player)
        {
            bool isMuted = player.GetSharedData("isMuted");
            if (!isMuted)
            {
                bool isTalking = (bool)player.GetSharedData("isTalking");
                if (isTalking)
                    player.SetSharedData("isTalking", false);
                else
                    player.SetSharedData("isTalking", true);
            }
        }

        [RemoteEvent("ChangeVoiceDistance")]
        public async void ChangeVoiceDistance(Client player)
        {
            string distance = player.GetSharedData("VoiceDistance");
            if(!string.IsNullOrEmpty(distance))
            {
                string newDistance = "";
                if (distance == "Talking")
                    newDistance = "Shouting";
                else if (distance == "Shouting")
                    newDistance = "Whisper";
                else if (distance == "Whisper")
                    newDistance = "Talking";

                player.SetSharedData("VoiceDistance", newDistance);
                player.TriggerEvent("SetVoiceDistance", newDistance);
            }
            else
            {
                player.SetSharedData("VoiceDistance", "Talking");
            }
        }
        #endregion
    }
}

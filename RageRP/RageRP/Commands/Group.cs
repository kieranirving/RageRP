using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Server.Commands
{
    class Group : Script
    {
        [Command("grouphelp", Alias = "ghelp")]
        public async void Help(Client player)
        {
            string commands = $"!{{##efefef}} Available Group Commands: ";
            foreach (var c in Gamedata.GroupCommands)
            {
                commands += $"!{{#ffffff}}{c}, ";
            }
            commands = commands.Remove(commands.Length - 1);
            player.SendChatMessage($"{commands}");
        }

        [Command("group", GreedyArg = true)]
        public async void groupManage(Client player, string message)
        {

        }

        [Command("g", GreedyArg = true)]
        public async void groupChat(Client player, string message)
        {

        }
    }
}

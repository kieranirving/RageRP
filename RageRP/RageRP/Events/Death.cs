using GTANetworkAPI;
using System;

namespace RageRP.Server.Events
{
    public class Death : Script
    {
        //Not implemented yet :(
        //[ServerEvent(Event.PlayerDamage)]
        //public void OnPlayerDamage(Client player, float healthLoss, float armorLoss)
        //{
        //    // Some code
        //    player.SendChatMessage("wot");
        //}

        [ServerEvent(Event.PlayerDeath)]
        public async void OnPlayerDeath(Client player, Client killer, uint reason)
        {
            player.SetSharedData("ServerTime", DateTime.Now.ToString("HH:mm:ss"));
            player.SetSharedData("isDead", true);
        }
    }
}
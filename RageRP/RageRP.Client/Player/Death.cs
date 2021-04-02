using RAGE;
using System;

namespace RageRP.Client.Player
{
    public class Death : Events.Script
    {
        public Death()
        {
            Events.Add("setInvincible", isInvincible);
        }

        public async void isInvincible(object[] args)
        {
            bool value = Convert.ToBoolean(args[0].ToString());
            RAGE.Elements.Player.LocalPlayer.SetInvincible(value);
            //RAGE.Game.Invoker.Invoke(RAGE.Game.Natives.SetPlayerInvincible)
        }
    }
}

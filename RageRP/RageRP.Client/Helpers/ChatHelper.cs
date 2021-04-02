using RAGE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RageRP.Client.Helpers
{
    public class ChatHelper : Events.Script
    {
        public ChatHelper()
        {
            Events.Add("updateChateState", UpdateChatState);
        }

        private async void UpdateChatState(object[] args)
        {
            bool chatState = Convert.ToBoolean(args[0]);
            await Task.Delay(250);
            Globals.isChatOpen = chatState;
        }
    }
}
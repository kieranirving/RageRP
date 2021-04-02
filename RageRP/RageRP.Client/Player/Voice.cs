using RAGE;
using RageRP.Client.Helpers;
using System;
using System.Collections.Generic;
using static RageRP.Client.Helpers.BrowserHelper;

namespace RageRP.Client.Player
{
    class Voice : Events.Script
    {
        private bool isTalking = false;
        private bool isTogglingDistance = false;
        private int _browserId = 0;
        private bool isChatMuted = true;
        public Voice()
        {
            Events.Add("SetVoiceDistance", VoiceDistanceHandler);
            Events.Add("StartVoiceOverlay", StartVoiceOverlay);
            Events.Add("ToggleMuted", ToggleMuted);
            Events.Tick += Tick;
        }

        private void VoiceDistanceHandler(object[] args)
        {
            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.VOICE_TOGGLEDISTANCE,
                index = _browserId,
                parameters = new object[] { args[0].ToString() }
            });
        }

        private void StartVoiceOverlay(object[] args)
        {
            _browserId = BrowserHelper.CreateBrowser(new windowObject()
            {
                url = "package://cs_packages/RageRP/html/voice.html",
                function = BrowserFunctions.VOICE_INIT,
                parameters = new object[] { }
            });
        }

        private void ToggleMuted(object[] args)
        {
            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.VOICE_TOGGLEMUTED,
                index = _browserId,
                parameters = new object[] {
                    Convert.ToBoolean(args[0].ToString()),
                    args[1].ToString()
                }
            });
        }

        private void Tick(List<Events.TickNametagData> nametags)
        {
            if (Globals.isChatOpen)
                return;
            if (!Globals.isSpawned)
                return;

            bool isCurrentlyTalking = false;
            if (RAGE.Input.IsDown((int)ConsoleKey.F4) && !isTalking)
            {
                isTalking = true;
                isChatMuted = false;
                Events.CallLocal("setIsMuted", isChatMuted);
                isCurrentlyTalking = true;
                Events.CallRemote("ToggleTalking");
            }
            if (!RAGE.Input.IsDown((int)ConsoleKey.F4) && isTalking)
            {
                isTalking = false;
                isChatMuted = true;
                Events.CallLocal("setIsMuted", isChatMuted);
                isCurrentlyTalking = true;
                Events.CallRemote("ToggleTalking");
            }
            if(isCurrentlyTalking)
            {
                BrowserHelper.ExecuteFunction(new windowEventObject()
                {
                    function = BrowserFunctions.VOICE_TOGGLETALKING,
                    index = _browserId,
                    parameters = new object[] { isChatMuted }
                });
            }

            if (RAGE.Input.IsDown((int)ConsoleKey.End) && !isTogglingDistance)
            {
                isTogglingDistance = true;
                Events.CallRemote("ChangeVoiceDistance");
            }
            if (!RAGE.Input.IsDown((int)ConsoleKey.End) && isTogglingDistance)
                isTogglingDistance = false;
        }
    }
}
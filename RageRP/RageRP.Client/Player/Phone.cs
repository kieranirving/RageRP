using RAGE;
using RAGE.Ui;
using RageRP.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using static RAGE.Events;
using static RageRP.Client.Helpers.BrowserHelper;

namespace RageRP.Client.Player
{
    public class Phone : Events.Script
    {
        private static int _browserId;
        private bool isPhoneUpPressed = false;
        private bool isPhoneDisplayed = false;
        public Phone()
        {
            Events.Tick += Tick;
            Events.Add("RenderPhone", RenderPhone);
            Events.Add("HidePhone", HidePhone);
        }

        public async void RenderPhone(object[] args)
        {
            _browserId = BrowserHelper.CreateBrowser(new windowObject()
            {
                url = "package://cs_packages/RageRP/html/phone.html",
                function = BrowserFunctions.PHONE_INIT,
                parameters = new object[]
                {
                    "fuck"//data
                }
            });
        }

        public async void HidePhone(object[] args)
        {
            if (isPhoneDisplayed)
            {
                Cursor.Visible = false; //Enable the cursor
                BrowserHelper.ExecuteFunction(new windowEventObject()
                {
                    function = BrowserFunctions.PHONE_HIDE,
                    index = _browserId,
                    parameters = new object[] { }
                });
                isPhoneDisplayed = false;
            }
        }

        public async void Tick(List<TickNametagData> nametags)
        {
            if (Globals.isChatOpen)
                return;
            if (!Globals.isSpawned)
                return;

            //Phone Control
            if (RAGE.Input.IsDown((int)ConsoleKey.UpArrow) && !isPhoneUpPressed)
            {
                isPhoneUpPressed = true;

                if (!isPhoneDisplayed)
                {
                    Cursor.Visible = true; //Enable the cursor
                    BrowserHelper.ExecuteFunction(new windowEventObject()
                    {
                        function = BrowserFunctions.PHONE_SHOW,
                        index = _browserId,
                        parameters = new object[] { }
                    });
                    isPhoneDisplayed = true;
                    //Animation amb@code_human_wander_mobile@male@base
                }
            }
            if (!RAGE.Input.IsDown((int)ConsoleKey.UpArrow) && isPhoneUpPressed)
                isPhoneUpPressed = false;
        }
    }
}

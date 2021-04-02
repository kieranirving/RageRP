using RAGE;
using RAGE.Elements;
using RageRP.Client.Helpers;
using System;
using System.Collections.Generic;
using static RageRP.Client.Helpers.BrowserHelper;

namespace RageRP.Client.Player
{
    class Money : Events.Script
    {
        private int _browserId = 0;
        public Money()
        {
            Events.Add("StartMoneyOverlay", StartMoneyOverlay);
            Events.Add("AddCash", Add);
            Events.Add("RemoveCash", Remove);
        }

        private void StartMoneyOverlay(object[] args)
        {
            _browserId = BrowserHelper.CreateBrowser(new windowObject()
            {
                url = "package://cs_packages/RageRP/html/money.html",
                function = BrowserFunctions.MONEY_INIT,
                parameters = new object[] { Globals.Cash }
            });
        }

        private void Remove(object[] args)
        {
            int cash = Convert.ToInt32(RAGE.Elements.Player.LocalPlayer.GetSharedData("Cash"));
            int difference = Math.Abs(cash - Globals.Cash);

            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.MONEY_REMOVE,
                index = _browserId,
                parameters = new object[] { difference }
            });

            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.MONEY_SETAMOUNT,
                index = _browserId,
                parameters = new object[] { cash }
            });

            Globals.Cash = cash;
        }

        private void Add(object[] args)
        {
            int cash = Convert.ToInt32(RAGE.Elements.Player.LocalPlayer.GetSharedData("Cash"));
            int difference = Math.Abs(cash - Globals.Cash);

            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.MONEY_ADD,
                index = _browserId,
                parameters = new object[] { difference }
            });

            BrowserHelper.ExecuteFunction(new windowEventObject()
            {
                function = BrowserFunctions.MONEY_SETAMOUNT,
                index = _browserId,
                parameters = new object[] { cash }
            });

            Globals.Cash = cash;
        }
    }
}

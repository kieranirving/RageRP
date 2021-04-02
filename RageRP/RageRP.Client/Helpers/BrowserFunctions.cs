using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Helpers
{
    public static class BrowserFunctions
    {
        public static string LOGIN_DISCONNECT { get { return "login.disconnect"; } }
        public static string LOGIN_INIT { get { return "login.init"; } }

        public static string STARTUP_INIT { get { return "startup.init"; } }
        public static string STARTUP_SHOWCHARSEL { get { return "startup.showCharacterSelection"; } }

        public static string CHARACTERCREATE_INIT { get { return "charCreate.init"; } }

        public static string SPAWNSELECT_INIT { get { return "spawnSelect.init"; } }

        public static string WATERMARK_INIT { get { return "watermark.showWatermark"; } }

        public static string PHONE_INIT { get { return "phone.init"; } }
        public static string PHONE_SHOW { get { return "phone.showPhone"; } }
        public static string PHONE_HIDE { get { return "phone.hidePhone"; } }

        public static string VOICE_INIT { get { return "voice.init"; } }
        public static string VOICE_TOGGLETALKING { get { return "voice.toggleTalking"; } }
        public static string VOICE_TOGGLEDISTANCE { get { return "voice.changeTalkingDistance"; } }
        public static string VOICE_TOGGLEMUTED { get { return "voice.toggleMuted"; } }

        public static string MONEY_INIT { get { return "money.init"; } }
        public static string MONEY_SETAMOUNT { get { return "money.setAmount"; } }
        public static string MONEY_ADD { get { return "money.add"; } }
        public static string MONEY_REMOVE { get { return "money.remove"; } }

        public static string STORE_SHOW { get { return "store.openStore"; } }
    }
}

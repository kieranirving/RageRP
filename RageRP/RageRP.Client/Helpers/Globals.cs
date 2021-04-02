using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Helpers
{
    public class Globals
    {
        private static string name;
        public static string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = value;
                }
            }
        }

        private static string playerID;
        public static string PlayerID
        {
            get { return playerID; }
            set
            {
                if (string.IsNullOrEmpty(playerID))
                {
                    playerID = value;
                }
            }
        }
        public static string CharacterID { get; set; }
        public static string CharacterName { get; set; }
        public static int Cash { get; set; }

        public static int DefaultDimension
        {
            get { return 0; }
        }

        public static bool isSpawned { get; set; }
        public static bool isChatOpen { get; set; }

        private static string _version;
        public static string version
        {
            get { return _version; }
            set
            {
                if (string.IsNullOrEmpty(_version))
                {
                    _version = value;
                }
            }
        }
    }

    class Gender
    {
        public const int Male = 1;
        public const int Female = 2;
    }

    class OverlayValues
    {
        public static int Blemish { get; set; }
        public static int FacialHair { get; set; }
        public static int Eyebrows { get; set; }
        public static int Ageing { get; set; }
        public static int Makeup { get; set; }
        public static int Blush { get; set; }
        public static int Complexion { get; set; }
        public static int SunDamage { get; set; }
        public static int Lipstick { get; set; }
        public static int MolesFreckles { get; set; }
        public static int ChestHair { get; set; }
        //public static int BodyBlemish { get; set; }
    }
}
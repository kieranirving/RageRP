using GTANetworkAPI;
using System;

namespace RageRP.Server.Helpers
{
    public static class ErrorHelper
    {
        public static void Register(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            c.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }
}

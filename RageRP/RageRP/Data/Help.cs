using System;
using System.Collections.Generic;
using System.Reflection;
using GTANetworkAPI;
using RageRP.Server.Helpers;

namespace RageRP.Server.Data
{
    public class Help : Script
    {
        public static List<string> GetPlayerCommands()
        {
            c.WriteLine("Getting Player Commands");
            return GetCommands(typeof(Commands.Player));
        }

        public static List<string> GetAdminCommands()
        {
            c.WriteLine("Getting Admin Commands");
            return GetCommands(typeof(Commands.Admin));
        }

        public static List<string> GetFactionCommands()
        {
            c.WriteLine("Getting Faction Commands");
            return GetCommands(typeof(Commands.Faction));
        }

        public static List<string> GetGroupCommands()
        {
            c.WriteLine("Getting Group Commands");
            return GetCommands(typeof(Commands.Group));
        }

        private static List<string> GetCommands(Type T)
        {
            var result = new List<string>();
            MethodInfo[] methodInfos = T.GetMethods();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                var m = methodInfos[i];
                if (m.ReturnType != typeof(void))
                    continue;
                var cmd = m.GetCustomAttribute<CommandAttribute>(true);
                result.Add($"/{cmd.CommandString}");
                if (!string.IsNullOrEmpty(cmd.Alias))
                    result.Add($"/{cmd.Alias}");
            }
            return result;
        }
    }
}

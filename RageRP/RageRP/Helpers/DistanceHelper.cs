using GTANetworkAPI;

namespace RageRP.Server.Helpers
{
    class DistanceHelper : Script
    {
        /// <summary>
        /// Used to return the correct colour hex for local chat messages
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static string getDistanceChatColour(float distance)
        {
            string colour;
            if (distance <= 10.0f)
                colour = "#ffffff";
            else if (distance <= 10.0f)
                colour = "#ffffff";
            else if (distance <= 20.0f)
                colour = "#cccccc";
            else if (distance <= 30.0f)
                colour = "#999999";
            else if (distance <= 40.0f)
                colour = "#666666";
            else if (distance <= 50.0f)
                colour = "#333333";
            else
                colour = "#ffffff";
            return colour;
        }

        /// <summary>
        /// Used to return the correct colour hex for local emotes /me
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static string getDistanceEmoteColour(float distance)
        {
            string colour;
            if (distance <= 10.0f)
                colour = "#C2A2DA";
            else if (distance <= 15.0f)
                colour = "#a689bc";
            else if (distance <= 20.0f)
                colour = "#987cad";
            else if (distance <= 25.0f)
                colour = "#7e6491";
            else if (distance <= 30.0f)
                colour = "#50405b";
            else
                colour = "#C2A2DA";
            return colour;
        }
    }
}

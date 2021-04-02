using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using static RageRP.Data.Events.Types;

namespace RageRP.Server.Helpers
{
    public class Markers
    {
        private const float normMarker = 0.75f;
        public static bool Render(int Id, MarkerType type, Vector3 location, EventTypes eventType, dynamic parameter2, string markerMessage)
        {
            bool result;
            try
            {
                ColShape colShape = NAPI.ColShape.CreateCylinderColShape(location, normMarker, 4f, 0);
                Marker markerPDMShop = NAPI.Marker.CreateMarker(MarkerType.VerticalCylinder, location, new Vector3(), new Vector3(), normMarker, new GTANetworkAPI.Color(255, 0, 0, 100), false, 0);

                TextLabel text = null;
                if (!string.IsNullOrEmpty(markerMessage))
                    text = NAPI.TextLabel.CreateTextLabel(markerMessage, location.Add(new Vector3(0,0,0.5)), 3, 1f, 4, new Color(255, 255, 255, 255), dimension:0);

                colShape.SetSharedData("Id", Id);
                colShape.SetSharedData("Type", eventType);
                colShape.SetSharedData("p2", parameter2);
                colShape.SetSharedData("text", text);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                ErrorHelper.Register(ex);
            }
            return result;
        }
    }
}

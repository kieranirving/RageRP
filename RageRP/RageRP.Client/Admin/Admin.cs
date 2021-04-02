using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageRP.Client.Admin
{
    public class Admin : Events.Script
    {
        private int camera;
        private int entityid;
        private Vector3 oldPosition;
        public Admin()
        {
            Events.Add("spectate", Spectate);
            Events.Add("stopSpectating", StopSpectate);
            Events.Add("freezeHandler", freezeHandler);
        }

        public async void Spectate(object[] args)
        {
            RAGE.Elements.Entity entity = (RAGE.Elements.Entity)args[0];

            oldPosition = RAGE.Elements.Player.LocalPlayer.Position;

            RAGE.Elements.Player.LocalPlayer.Position = entity.Position;

            entityid = entity.RemoteId;
            camera = Cam.CreateCameraWithParams(Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), entity.Position.X, entity.Position.Y, entity.Position.Z, 0f, 0f, 0f, 90.0f, true, 0);
            Cam.AttachCamToEntity(camera, entity.RemoteId, 0f, 0f, 0f, true);
            Cam.SetCamActive(camera, true);
            Cam.RenderScriptCams(true, false, 0, true, false, 0);

            //RAGE.Game.Streaming.SetHdArea(entity.Position.X, entity.Position.Y, entity.Position.Z, 0f);
        }

        public async void StopSpectate(object[] args)
        {
            RAGE.Elements.Player.LocalPlayer.Position = oldPosition;

            Cam.DestroyCam(camera, true);
            Cam.SetCamActive(camera, false);
            Cam.RenderScriptCams(false, false, 0, true, false, 0);

            camera = -1;
            entityid = -1;
            oldPosition = null;

            RAGE.Game.Streaming.ClearHdArea();
        }

        public async void freezeHandler(object[] args)
        {
            bool isFrozen = Convert.ToBoolean(RAGE.Elements.Player.LocalPlayer.GetSharedData("isFrozen"));
            RAGE.Elements.Player.LocalPlayer.FreezePosition(isFrozen);
        }
    }
}

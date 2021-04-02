using RAGE;
using RageRP.Client.Helpers;
using RageRP.Client.Models;
using System;
using System.Collections.Generic;
using static RAGE.Events;

//Ported from https://github.com/Flatracer/RadarWhileDriving
namespace RageRP.Client.Player
{
    class HudHandler : Events.Script
    {
        private MinimapModel minimap;
        private string helpText;
        private bool showHelpText;

        private int res_x = 0;
        private int res_y = 0;

        public HudHandler()
        {
            //Nametags.Enabled = false;
            showHelpText = false;
            helpText = "";
            minimap = new MinimapModel();

            Events.Tick += Tick;
        }

        public void UpdateMinimapValues()
        {
            float safezone = RAGE.Game.Graphics.GetSafeZoneSize();
            float safezone_x = 1.0f / 20.0f;
            float safezone_y = 1.0f / 20.0f;

            float aspect_ratio = RAGE.Game.Graphics.GetAspectRatio(false);

            RAGE.Game.Graphics.GetActiveScreenResolution(ref res_x, ref res_y);
            float xscale = 1.0f / res_x;
            float yscale = 1.0f / res_y;

            minimap.width = xscale * (res_x / (4 * aspect_ratio));
            minimap.height = yscale * (res_y / 5.674f);
            minimap.left_x = xscale * (res_x * (safezone_x * ((Math.Abs(safezone - 1.0f)) * 10)));
            minimap.bottom_y = 1.0f - yscale * (res_y * (safezone_y * ((Math.Abs(safezone - 1.0f)) * 10)));
            minimap.right_x = minimap.left_x + minimap.width;
            minimap.top_y = minimap.bottom_y - minimap.height;
            minimap.x = minimap.left_x;
            minimap.y = minimap.top_y;
            minimap.xunit = xscale;
            minimap.yunit = yscale;
        }

        public async void Tick(List<TickNametagData> nametags)
        {
            if (!Globals.isSpawned)
            {
                #region healthArmourMap
                if (RAGE.Elements.Player.LocalPlayer.IsInAnyVehicle(false))
                {
                    RAGE.Game.Ui.DisplayRadar(true);
                }
                else
                {
                    RAGE.Game.Ui.DisplayRadar(false);

                    int PlayerHealth = RAGE.Elements.Player.LocalPlayer.GetHealth();
                    int PlayerArmour = RAGE.Elements.Player.LocalPlayer.GetArmour();
                    float PlayerStamina = RAGE.Game.Player.GetPlayerSprintStaminaRemaining();

                    UpdateMinimapValues();
                    float BarY = minimap.bottom_y - ((minimap.yunit * 18.0f) * 0.5f);
                    float BackgroundBarH = minimap.yunit * 18.0f;
                    float BarH = BackgroundBarH / 2;
                    float BarSpacer = minimap.xunit * 3.0f;

                    RGBA BackgroundBar = new RGBA(0, 0, 0, 125);// {['R'] = 0, ['G'] = 0, ['B'] = 0, ['A'] = 125, ['L'] = 0}

                    RGBA HealthBaseBar = new RGBA(57, 102, 57, 172);// {['R'] = 57, ['G'] = 102, ['B'] = 57, ['A'] = 175, ['L'] = 1}
                    RGBA HealthBar = new RGBA(114, 204, 114, 175);// {['R'] = 114, ['G'] = 204, ['B'] = 114, ['A'] = 175, ['L'] = 2}

                    RGBA HealthHitBaseBar = new RGBA(112, 25, 25, 175);// ['R'] = 112, ['G'] = 25, ['B'] = 25, ['A'] = 175}
                    RGBA HealthHitBar = new RGBA(224, 50, 50, 175);// {['R'] = 224, ['G'] = 50, ['B'] = 50, ['A'] = 175}

                    RGBA ArmourBaseBar = new RGBA(47, 92, 115, 175);// {['R'] = 47, ['G'] = 92, ['B'] = 115, ['A'] = 175, ['L'] = 1}
                    RGBA ArmourBar = new RGBA(93, 182, 229, 175);// {['R'] = 93, ['G'] = 182, ['B'] = 229, ['A'] = 175, ['L'] = 2}

                    RGBA AirBaseBar = new RGBA(67, 106, 130, 175);// {['R'] = 67, ['G'] = 106, ['B'] = 130, ['A'] = 175, ['L'] = 1}
                    RGBA AirBar = new RGBA(174, 219, 242, 175);// {['R'] = 174, ['G'] = 219, ['B'] = 242, ['A'] = 175, ['L'] = 2}

                    float BackgroundBarW = minimap.width;
                    float BackgroundBarX = minimap.x + (minimap.width / 2);
                    DrawRectangle(BackgroundBarX, BarY, BackgroundBarW, BackgroundBarH, BackgroundBar.Red, BackgroundBar.Green, BackgroundBar.Blue, BackgroundBar.Alpha, 0);

                    float HealthBaseBarW = (minimap.width / 2) - (BarSpacer / 2);
                    float HealthBaseBarX = minimap.x + (HealthBaseBarW / 2);
                    float HealthBaseBarR = HealthBaseBar.Red,
                          HealthBaseBarG = HealthBaseBar.Green,
                          HealthBaseBarB = HealthBaseBar.Blue,
                          HealthBaseBarA = HealthBaseBar.Alpha;
                    var HealthBarW = (minimap.width / 2) - (BarSpacer / 2);
                    if (PlayerHealth < 175 && PlayerHealth > 100)
                    {
                        HealthBarW = ((minimap.width / 2) - (BarSpacer / 2)) / 75 * (PlayerHealth - 100);
                    }
                    else if (PlayerHealth < 100)
                    {
                        HealthBarW = 0;
                    }
                    float HealthBarX = minimap.x + (HealthBarW / 2);

                    float HealthBarR = HealthBar.Red,
                          HealthBarG = HealthBar.Green,
                          HealthBarB = HealthBar.Blue,
                          HealthBarA = HealthBar.Alpha;


                    if (PlayerHealth <= 118 || (PlayerStamina >= 90.0 && (RAGE.Elements.Player.LocalPlayer.IsRunning() || RAGE.Elements.Player.LocalPlayer.IsSprinting())))
                    {

                        HealthBaseBarR = HealthHitBaseBar.Red;
                        HealthBaseBarG = HealthHitBaseBar.Green;
                        HealthBaseBarB = HealthHitBaseBar.Blue;
                        HealthBaseBarA = HealthHitBaseBar.Alpha;

                        HealthBarR = HealthHitBar.Red;
                        HealthBarG = HealthHitBar.Green;
                        HealthBarB = HealthHitBar.Blue;
                        HealthBarA = HealthHitBar.Alpha;

                    }

                    DrawRectangle(HealthBaseBarX, BarY, HealthBaseBarW, BarH, HealthBaseBar.Red, HealthBaseBar.Green, HealthBaseBar.Blue, HealthBaseBar.Alpha, 1);
                    DrawRectangle(HealthBarX, BarY, HealthBarW, BarH, HealthBar.Red, HealthBar.Green, HealthBar.Blue, HealthBar.Alpha, 2);


                    if (!RAGE.Elements.Player.LocalPlayer.IsSwimmingUnderWater())
                    {
                        float ArmourBaseBarW = (minimap.width / 2) - (BarSpacer / 2);
                        float ArmourBaseBarX = minimap.right_x - (ArmourBaseBarW / 2);
                        float ArmourBarW = ((minimap.width / 2) - (BarSpacer / 2)) / 100 * PlayerArmour;
                        float ArmourBarX = minimap.right_x - ((minimap.width / 2) - (BarSpacer / 2)) + (ArmourBarW / 2);

                        DrawRectangle(ArmourBaseBarX, BarY, ArmourBaseBarW, BarH, ArmourBaseBar.Red, ArmourBaseBar.Green, ArmourBaseBar.Blue, ArmourBaseBar.Alpha, 1);
                        DrawRectangle(ArmourBarX, BarY, ArmourBarW, BarH, ArmourBar.Red, ArmourBar.Green, ArmourBar.Blue, ArmourBar.Alpha, 2);
                    }
                    else
                    {
                        float ArmourBaseBarW = (((minimap.width / 2) - (BarSpacer / 2)) / 2) - (BarSpacer / 2);
                        float ArmourBaseBarX = minimap.right_x - (((minimap.width / 2) - (BarSpacer / 2)) / 2) - (ArmourBaseBarW / 2) - (BarSpacer / 2);
                        float ArmourBarW = ((((minimap.width / 2) - (BarSpacer / 2)) / 2) - (BarSpacer / 2)) / 100 * PlayerArmour;
                        float ArmourBarX = minimap.right_x - ((minimap.width / 2) - (BarSpacer / 2)) + (ArmourBarW / 2);

                        DrawRectangle(ArmourBaseBarX, BarY, ArmourBaseBarW, BarH, ArmourBaseBar.Red, ArmourBaseBar.Green, ArmourBaseBar.Blue, ArmourBaseBar.Alpha, 1);
                        DrawRectangle(ArmourBarX, BarY, ArmourBarW, BarH, ArmourBar.Red, ArmourBar.Green, ArmourBar.Blue, ArmourBar.Alpha, 2);

                        float AirBaseBarW = (((minimap.width / 2) - (BarSpacer / 2)) / 2) - (BarSpacer / 2);
                        float AirBaseBarX = minimap.right_x - (AirBaseBarW / 2);

                        float Air = RAGE.Game.Player.GetPlayerUnderwaterTimeRemaining();
                        if (Air < 0.0f)
                        {
                            Air = 0.0f;
                        }
                        float AirBarW = ((((minimap.width / 2) - (BarSpacer / 2)) / 2) - (BarSpacer / 2)) / 10.0f * Air;
                        float AirBarX = minimap.right_x - ((((minimap.width / 2) - (BarSpacer / 2)) / 2) - (BarSpacer / 2)) + (AirBarW / 2);

                        DrawRectangle(AirBaseBarX, BarY, AirBaseBarW, BarH, AirBaseBar.Red, AirBaseBar.Green, AirBaseBar.Blue, AirBaseBar.Alpha, 1);
                        DrawRectangle(AirBarX, BarY, AirBarW, BarH, AirBar.Red, AirBar.Green, AirBar.Blue, AirBar.Alpha, 2);
                    }
                }
                #endregion
            }
        }

        private void DrawRectangle(float x, float y, float width, float height, uint r, uint g, uint b, uint a, int layer)
        {
            RAGE.Game.Graphics.Set2dLayer(layer);
            RAGE.Game.Graphics.DrawRect(x, y, width, height, (int)r, (int)g, (int)b, (int)a, 0);
        }
    }
}
﻿using MelonLoader;
using HarmonyLib;

using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Main.Scenes;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Unity.Bridge;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Simulation.Track;
using Assets.Scripts.Unity.Map;
using UnityEngine;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Utils;
using System;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Simulation.Towers.Projectiles;
using System.IO;
using BTD_Mod_Helper.Api.Towers;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using UnityEngine.UI;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu.TowerSelectionMenuThemes;
using System.Collections.Generic;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using System.Reflection;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Assets.Scripts.Unity.UI_New.InGame.RightMenu;

[assembly: MelonInfo(typeof(minicustomtowersv2.Main), "Mini Custom Towers", "1.0.4", "Greenphx")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace minicustomtowersv2
{
    public class Main : BloonsTD6Mod
    {

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Mini Custom Towers loaded!");
        }
        public override string MelonInfoCsURL => "https://raw.githubusercontent.com/Greenphx9/BTD6Mods/main/MiniCustomTowersV2/Main.cs";
        public override string LatestURL => "https://github.com/Greenphx9/BTD6Mods/blob/main/MiniCustomTowersV2/minicustomtowersv2.dll?raw=true";
        public override string IDPrefix => "MiniCustomTowers_V2_";
        public override void OnUpdate()
        {
            base.OnUpdate();

            bool inAGame = InGame.instance != null && InGame.instance.bridge != null;
            if (inAGame)
            {
                try
                {
                    foreach (TowerToSimulation tts in InGame.instance.GetAllTowerToSim())
                    {
                        if (tts.tower.towerModel.baseId.Contains("EnergyShooter") && tts.tower.display.scaleOffset != new Assets.Scripts.Simulation.SMath.Vector3(0.5f, 0.5f, 0.5f))
                        {
                            tts.tower.display.scaleOffset = new Assets.Scripts.Simulation.SMath.Vector3(0.5f, 0.5f, 0.5f);
                        }
                    }

                }
                catch
                {

                }

            }

        }
        static Texture2D makeReadable(Texture texture)
        {
            // Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary(
                                texture.width,
                                texture.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);

            // Blit the pixels on texture to the RenderTexture
            UnityEngine.Graphics.Blit(texture, tmp);
            // Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;
            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;
            // Create a new readable Texture2D to copy the pixels to it
            Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
            // Copy the pixels from the RenderTexture to the new Texture
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            // Reset the active RenderTexture
            RenderTexture.active = previous;
            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);
            return myTexture2D;
        }
        //Had to remove (for now)
        [HarmonyPatch(typeof(StandardTowerPurchaseButton), nameof(StandardTowerPurchaseButton.UpdateIcon))]
        public class SetTower
        {
            [HarmonyPrefix]
            internal static bool Prefix(ref StandardTowerPurchaseButton __instance)
            {
                __instance.bg = __instance.gameObject.GetComponent<Image>();
                var towerModel = __instance.baseTowerModel;
                if(towerModel.baseId.Contains("MiniCustomTowers_V2_"))
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerCustom"));
                }
                return true;
            }
        }
        /*[HarmonyPatch(typeof(TSMThemeDefault), nameof(TSMThemeAmbidextrousRangs.UpdateFromSimInfo))]
        [HarmonyPatch]
        public class TSMTheme_Patch
        {

            [HarmonyPostfix]
            public static void Postfix(TSMThemeAmbidextrousRangs __instance, TowerToSimulation tower)
            {
                var texture2D = ModContent.GetTexture<Main>("TowerContainerCustom");
                var sprite = Sprite.Create(texture2D, new UnityEngine.Rect(0, 0, texture2D.width, texture2D.height), default(Vector2), 100f, 0U, SpriteMeshType.Tight);
                if (tower.tower.towerModel.baseId.Contains("MiniCustomTowers_V2"))
                {
                    __instance.towerBackgroundImage.sprite = sprite;
                }
            }
        }*/
    }

}
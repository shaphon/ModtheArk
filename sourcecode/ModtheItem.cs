using BepInEx;
using GameDataEditor;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Management.Instrumentation;
using System.Reflection.Emit;
using System.Reflection;
using UnityEngine;
using System.Linq;
using I2.Loc;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Security.Policy;
namespace SHAPHON
{

    [BepInPlugin(GUID, "ModtheItem", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModtheItemPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModtheItem";
        public const string version = "1.0.0";

        private static readonly Harmony harmony = new Harmony(GUID);
        void Awake()
        {
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }




        [HarmonyPatch(typeof(GDEItem_ActiveData))]
        private class GDEItem_ActiveDataPlugin
        {


            [HarmonyPatch(nameof(GDEItem_ActiveData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEItem_ActiveData __instance, string dataKey)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                GDEDataManager.Get(key, out dict);
                dict.TryGetBool("Moded", out bool moded, key);
                if (!moded)
                {
                    ;
                }
                else
                {
                    dict.TryGetString("_image_Moded_Path", out string _image_Moded_Path, key);
                    Texture2D _image = LoadPNG.LoadPNG.ModTexture2D(_image_Moded_Path);
                    Traverse.Create(__instance).Field("_image").SetValue(_image);
                }
            }

        }
        [HarmonyPatch(typeof(GDEItem_ConsumeData))]
        private class GDEItem_ConsumeDataPlugin
        {


            [HarmonyPatch(nameof(GDEItem_ConsumeData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEItem_ConsumeData __instance, string dataKey)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                GDEDataManager.Get(key, out dict);
                dict.TryGetBool("Moded", out bool moded, key);
                if (!moded)
                {
                    ;
                }
                else
                {
                    dict.TryGetString("_image_Moded_Path", out string _image_Moded_Path, key);
                    Texture2D _image = LoadPNG.LoadPNG.ModTexture2D(_image_Moded_Path);
                    Traverse.Create(__instance).Field("_image").SetValue(_image);
                }
            }

        }
        [HarmonyPatch(typeof(GDEItem_EquipData))]
        private class GDEItem_EquipDataPlugin
        {


            [HarmonyPatch(nameof(GDEItem_EquipData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEItem_EquipData __instance, string dataKey)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                GDEDataManager.Get(key, out dict);
                dict.TryGetBool("Moded", out bool moded, key);
                if (!moded)
                {
                    ;
                }
                else
                {
                    dict.TryGetString("_image_Moded_Path", out string _image_Moded_Path, key);
                    Texture2D _image = LoadPNG.LoadPNG.ModTexture2D(_image_Moded_Path);
                    Traverse.Create(__instance).Field("_image").SetValue(_image);
                }
            }

        }
        [HarmonyPatch(typeof(GDEItem_MiscData))]
        private class GDEItem_MiscDataPlugin
        {


            [HarmonyPatch(nameof(GDEItem_MiscData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEItem_MiscData __instance, string dataKey)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                GDEDataManager.Get(key, out dict);
                dict.TryGetBool("Moded", out bool moded, key);
                if (!moded)
                {
                    ;
                }
                else
                {
                    dict.TryGetString("_image_Moded_Path", out string _image_Moded_Path, key);
                    Texture2D _image = LoadPNG.LoadPNG.ModTexture2D(_image_Moded_Path);
                    Traverse.Create(__instance).Field("_image").SetValue(_image);
                }
            }

        }
        [HarmonyPatch(typeof(GDEItem_PassiveData))]
        private class GDEItem_PassiveDataPlugin
        {


            [HarmonyPatch(nameof(GDEItem_PassiveData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEItem_PassiveData __instance, string dataKey)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                GDEDataManager.Get(key, out dict);
                dict.TryGetBool("Moded", out bool moded, key);
                if (!moded)
                {
                    ;
                }
                else
                {
                    dict.TryGetString("_image_Moded_Path", out string _image_Moded_Path, key);
                    Texture2D _image = LoadPNG.LoadPNG.ModTexture2D(_image_Moded_Path);
                    Traverse.Create(__instance).Field("_image").SetValue(_image);
                }
            }

        }
        [HarmonyPatch(typeof(GDEItem_PotionsData))]
        private class GDEItem_PotionsDataPlugin
        {


            [HarmonyPatch(nameof(GDEItem_PotionsData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEItem_PotionsData __instance, string dataKey)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                GDEDataManager.Get(key, out dict);
                dict.TryGetBool("Moded", out bool moded, key);
                if (!moded)
                {
                    ;
                }
                else
                {
                    dict.TryGetString("_image_Moded_Path", out string _image_Moded_Path, key);
                    Texture2D _image = LoadPNG.LoadPNG.ModTexture2D(_image_Moded_Path);
                    Traverse.Create(__instance).Field("_image").SetValue(_image);
                }
            }

        }



    }
}

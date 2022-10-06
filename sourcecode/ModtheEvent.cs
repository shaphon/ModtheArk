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

    [BepInPlugin(GUID, "ModtheEvent", version)] 
    [BepInProcess("ChronoArk.exe")]
    public class ModtheEventPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModtheEvent";
        public const string version = "1.0.0";
        
        private static readonly Harmony harmony = new Harmony(GUID);
        public static Dictionary<string, Texture2D> eventTex = new Dictionary<string, Texture2D>();
        void Awake()
        {
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }


        [HarmonyPatch(typeof(GDERandomEventData))]
        private class GDERandomEventDataPlugin
        {


            [HarmonyPatch(nameof(GDERandomEventData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDERandomEventData __instance, string dataKey)
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
                    dict.TryGetString("_MainImage_Moded_Path", out string _MainImage_Moded_Path, key);
                    Sprite _MainImage = LoadPNG.LoadPNG.ModSprite(_MainImage_Moded_Path);
                    Traverse.Create(__instance).Field("_MainImage").SetValue(_MainImage);
                    dict.TryGetString("_MainImage_2Stage_Moded_Path", out string _MainImage_2Stage_Moded_Path, key);
                    Sprite _MainImage_2Stage = LoadPNG.LoadPNG.ModSprite(_MainImage_2Stage_Moded_Path);
                    Traverse.Create(__instance).Field("_MainImage_2Stage").SetValue(_MainImage_2Stage);
                    dict.TryGetString("_MainImage_3Stage_Moded_Path", out string _MainImage_3Stage_Moded_Path, key);
                    Sprite _MainImage_3Stage = LoadPNG.LoadPNG.ModSprite(_MainImage_3Stage_Moded_Path);
                    Traverse.Create(__instance).Field("_MainImage_3Stage").SetValue(_MainImage_3Stage);
                }
            }

        }

       



    }
}

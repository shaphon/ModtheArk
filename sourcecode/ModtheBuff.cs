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

    [BepInPlugin(GUID, "ModtheBuff", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModtheBuffPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModtheBuff";
        public const string version = "1.0.0";

        private static readonly Harmony harmony = new Harmony(GUID);
        void Awake()
        {
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }




        [HarmonyPatch(typeof(GDEBuffData))]
        private class GDEBuffDataPlugin
        {
            
            
            [HarmonyPatch(nameof(GDEBuffData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDEBuffData __instance, string dataKey)
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
                    dict.TryGetString("_Icon_Moded_Path", out string _Icon_Moded_Path, key);
                    Texture2D _Icon = LoadPNG.LoadPNG.ModTexture2D(_Icon_Moded_Path);
                    Traverse.Create(__instance).Field("_Icon").SetValue(_Icon);
                }
                }

        }



    }
    }

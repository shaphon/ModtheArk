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
                harmony.UnpatchSelf();
        }
        private static Dictionary<string, GameObject> moded_AllyBuffEffect = new Dictionary<string, GameObject>();



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
            [HarmonyPatch(nameof(GDEBuffData.AllyBuffEffect), MethodType.Getter)]
            [HarmonyPostfix]
            private static void AllyBuffEffect_Postfix(GDEBuffData __instance,GameObject __result)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                if (GDEDataManager.Get(key, out dict))
                {
                    dict.TryGetBool("Moded", out bool moded, key);
                    if (moded)
                    {
                        if(__result != null)
                        {
                            if (moded_AllyBuffEffect.Keys.Contains(key))
                            {
                                try
                                {
                                    
                                    if (moded_AllyBuffEffect[key] == null)
                                    {
                                        throw new Exception("重新读取");
                                    }
                                    else
                                    {
                                        __result = moded_AllyBuffEffect[key];
                                    }
                                }
                                catch
                                {
                                    __result = GameObject.Instantiate(__result);
                                    __result.SetActive(false);
                                    moded_AllyBuffEffect[key] = __result;
                                }
                               
                            }
                            else
                            {
                                __result = GameObject.Instantiate(__result);
                                __result.SetActive(false);
                                try
                                {
                                    moded_AllyBuffEffect.Add(key, __result);
                                }
                                catch
                                {

                                }
                                
                            }
                            
                        }
                    }
                   
                }
                
            }

        }



    }
    }

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
using BepInEx.Configuration;
using I2.Loc;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Security.Policy;
using System.IO;
using System.Xml.Linq;

namespace SHAPHON
{

    [BepInPlugin(GUID, "ModtheCharactor", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModtheCharactorPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModtheCharactor";
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
        public static Dictionary<string, Texture2D> texDict;
        void Start()
        {


        }
        void Update()
        {
            ;

        }
        void FixedUpdate()
        {

        }
        [HarmonyPatch(typeof(StartPartySelect))]
        private class StartPartySelectPlugin
        {
            [HarmonyPatch(nameof(StartPartySelect.Init))]
            [HarmonyPostfix]
            private static void Init_Postfix(StartPartySelect __instance, int select)
            {
                if (__instance.Chars.Count > 20)
                {
                    if (select == 2)
                    {
                        __instance.gameObject.transform.Find("CharAlign").localScale = new Vector3((float)0.5, (float)0.5, (float)0);
                        __instance.gameObject.transform.Find("CharAlign").GetComponent<RectTransform>().sizeDelta = new Vector2(3900, 800);
                    }
                    else if (select == 1)
                    {
                        __instance.gameObject.transform.Find("CharAlign").localScale = new Vector3((float)1, (float)1, (float)0);
                        __instance.gameObject.transform.Find("CharAlign").GetComponent<RectTransform>().sizeDelta = new Vector2(1950, 350);
                    }
                }


            }
        }

        [HarmonyPatch(typeof(GDECharacterData))]
        private class GDECharacterDataPlugin
        {

            [HarmonyPatch(nameof(GDECharacterData.LoadFromSavedData))]
            [HarmonyPostfix]
            private static void LoadFromSavedData_Postfix(GDECharacterData __instance, string dataKey)
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

                    dict.TryGetString("_face_Path", out string _face_Path, key);
                    Texture2D _face = LoadPNG.LoadPNG.ModTexture2D(_face_Path);
                    Traverse.Create(__instance).Field("_face").SetValue(_face);
                    dict.TryGetString("_PassiveIcon_Path", out string _PassiveIcon_Path, key);
                    Texture2D _PassiveIcon = LoadPNG.LoadPNG.ModTexture2D(_PassiveIcon_Path);
                    Traverse.Create(__instance).Field("_PassiveIcon").SetValue(_PassiveIcon);
                    dict.TryGetString("_CollectionSprite_Cover_Path", out string _CollectionSprite_Cover_Path, key);
                    Sprite _CollectionSprite_Cover = LoadPNG.LoadPNG.ModSprite(_CollectionSprite_Cover_Path);
                    Traverse.Create(__instance).Field("_CollectionSprite_Cover").SetValue(_CollectionSprite_Cover);
                    dict.TryGetStringList("_CampSD_Path", out List<string> _CampSD_Path, key);
                    List<Sprite> _CampSD = new List<Sprite>();
                    foreach (string path in _CampSD_Path)
                    {
                        _CampSD.Add(LoadPNG.LoadPNG.ModSprite(path));
                    }
                    Traverse.Create(__instance).Field("CampSD").SetValue(_CampSD);
                    
                }
            }

        }



    }




}



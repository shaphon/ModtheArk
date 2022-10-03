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

    [BepInPlugin(GUID, "ReadStringList", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ReadStringListPlugin : BaseUnityPlugin
    {
        public const string GUID = "ReadStringList";
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
                harmony.UnpatchAll(GUID);
        }


        [HarmonyPatch(typeof(GDEDataManager))]
        private class GDEDataManagerPlugin
        {


            [HarmonyPatch(nameof(GDEDataManager.LocalizeArrayUpdate))]
            [HarmonyPrefix]
            private static bool LocalizeArrayUpdate_Prefix(string Key, string ValueName, List<string> MainList, Dictionary<string, object> LocalizeText)
            {
                List<object> list = new List<object>();
                for (int i = 0; i < MainList.Count; i++)
                {
                    string temp =  LocalizeManager.DBFile.GetTranslation(string.Concat(new object[]
                    {
                    LocalizeText["_gdeSchema"],
                    "/",
                    Key,
                    "_",
                    ValueName,
                    "_",
                    i
                    }), null, null, false, false);
                    if (string.IsNullOrEmpty(temp))
                    {
                        list.Add(MainList[i]);
                    }
                    else
                    {
                        list.Add(temp);
                    }
                }
                LocalizeText[ValueName] = list;
                return false;
            }

        }





    }
}

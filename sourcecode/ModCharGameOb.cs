using BepInEx;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SHAPHON
{

    [BepInPlugin(GUID, "ModCharGameOb", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModCharGameObPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModCharGameOb";
        public const string version = "1.0.0";
        private static readonly Harmony harmony = new Harmony(GUID);
        public static int count  = -1;
        private static Dictionary<string, GameObject> moded_FaceOriChar = new Dictionary<string, GameObject>();
        private static Dictionary<string, GameObject> moded_BattleChar = new Dictionary<string, GameObject>();
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
       [HarmonyPatch(typeof(GDECharacterData))]
        private class GDECharacterDataPlugin
        {


            [HarmonyPatch(nameof(GDECharacterData.FaceOriginChar), MethodType.Getter)]
            [HarmonyPrefix]
            private static bool FaceOriginChar_Prefix(GDECharacterData __instance)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                if (GDEDataManager.Get(key, out dict))
                {
                    dict.TryGetBool("Moded", out bool moded, key);
                    if (!moded)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            [HarmonyPatch(nameof(GDECharacterData.FaceOriginChar), MethodType.Getter)]
            [HarmonyPostfix]
            private static void FaceOriginChar_Postfix(GDECharacterData __instance, ref GameObject __result)
            {

                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                if (GDEDataManager.Get(key, out dict))
                {
                    dict.TryGetBool("Moded", out bool moded, key);
                    if (!moded)
                    {
                        ;
                    }
                    else
                    {
                        if (false && moded_FaceOriChar.ContainsKey(key))
                        {
                            try
                            {
                                __result = moded_FaceOriChar[key];
                            }
                            catch
                            {
                                GameObject face_ori = Resources.Load<GameObject>("CharImagePrefebs/Face/Face_Johan");

                                GameObject face = Object.Instantiate(face_ori);
                                Image face_img = face.GetComponent<Image>();
                                string _Image_FaceOriginChar_Path;
                                dict.TryGetString("_Image_FaceOriginChar_Path", out _Image_FaceOriginChar_Path, key);
                                face_img.sprite = LoadPNG.LoadPNG.ModSprite(_Image_FaceOriginChar_Path);
                                RectTransform face_rect = face.GetComponent<RectTransform>();
                                Vector2 offsetMax;
                                Vector2 offsetMin;
                                Vector2 pivot;
                                dict.TryGetVector2("_Image_FaceOriginChar_offsetMax", out offsetMax, key);
                                dict.TryGetVector2("_Image_FaceOriginChar_offsetMin", out offsetMin, key);
                                dict.TryGetVector2("_Image_FaceOriginChar_pivot", out pivot, key);
                                face_rect.offsetMax = offsetMax;
                                face_rect.offsetMin = offsetMin;
                                face_rect.pivot = pivot;
                                face.name = _Image_FaceOriginChar_Path;
                                moded_FaceOriChar[key]= face;
                                __result = face;
                            }
                            
                        }
                        else
                        {
                            GameObject face_ori = Resources.Load<GameObject>("CharImagePrefebs/Face/Face_Johan");

                            GameObject face = Object.Instantiate(face_ori);
                            Image face_img = face.GetComponent<Image>();
                            string _Image_FaceOriginChar_Path;
                            dict.TryGetString("_Image_FaceOriginChar_Path", out _Image_FaceOriginChar_Path, key);
                            face_img.sprite = LoadPNG.LoadPNG.ModSprite(_Image_FaceOriginChar_Path);
                            RectTransform face_rect = face.GetComponent<RectTransform>();
                            Vector2 offsetMax;
                            Vector2 offsetMin;
                            Vector2 pivot;
                            dict.TryGetVector2("_Image_FaceOriginChar_offsetMax", out offsetMax, key);
                            dict.TryGetVector2("_Image_FaceOriginChar_offsetMin", out offsetMin, key);
                            dict.TryGetVector2("_Image_FaceOriginChar_pivot", out pivot, key);
                            face_rect.offsetMax = offsetMax;
                            face_rect.offsetMin = offsetMin;
                            face_rect.pivot = pivot;
                            face.name = _Image_FaceOriginChar_Path;
                            try
                            {
                                moded_FaceOriChar.Add(key, face);
                            }
                            catch
                            {

                            }
                            
                            __result = face;
                        }

                        
                    }

                }


            }
            
            [HarmonyPatch(nameof(GDECharacterData.BattleChar), MethodType.Getter)]
            [HarmonyPrefix]
            private static bool BattleChar_Prefix(GDECharacterData __instance)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                
                Dictionary<string, object> dict;
                if (GDEDataManager.Get(key, out dict))
                {
                    dict.TryGetBool("Moded", out bool moded, key);
                    if (!moded)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }


            [HarmonyPatch(nameof(GDECharacterData.BattleChar), MethodType.Getter)]
            [HarmonyPostfix]
            private static void BattleChar_Postfix(GDECharacterData __instance, ref GameObject __result)
            {
                string key = Traverse.Create(__instance).Field("_key").GetValue<string>();
                Dictionary<string, object> dict;
                if (GDEDataManager.Get(key, out dict))
                {
                    dict.TryGetBool("Moded", out bool moded, key);
                    if (!moded)
                    {
                        ;
                    }
                    else
                    {
                        if (false&&moded_BattleChar.ContainsKey(key))
                        {
                            try
                            {
                                __result = moded_BattleChar[key];
                            }
                            catch
                            {
                                GameObject char_ori = Resources.Load<GameObject>("CharImagePrefebs/JohanBattle");

                                GameObject char_new = Object.Instantiate(char_ori);

                                Image char_img = char_new.GetComponent<Image>();
                                string _Image_BattleChar_Path;
                                dict.TryGetString("_Image_BattleChar_Path", out _Image_BattleChar_Path, key); ;
                                char_img.sprite = LoadPNG.LoadPNG.ModSprite(_Image_BattleChar_Path);

                                RectTransform char_rect = char_img.GetComponent<RectTransform>();
                                Vector2 offsetMax;
                                Vector2 offsetMin;
                                Vector2 pivot;
                                dict.TryGetVector2("_Image_BattleChar_offsetMax", out offsetMax, key);
                                dict.TryGetVector2("_Image_BattleChar_offsetMin", out offsetMin, key);
                                dict.TryGetVector2("_Image_BattleChar_pivot", out pivot, key);
                                char_rect.offsetMax = offsetMax;
                                char_rect.offsetMin = offsetMin;
                                char_rect.pivot = pivot;
                                char_new.name = _Image_BattleChar_Path;
                                moded_BattleChar[key]= char_new;
                                __result = char_new;
                            }
                        }
                        else
                        {
                            GameObject char_ori = Resources.Load<GameObject>("CharImagePrefebs/JohanBattle");

                            GameObject char_new = Object.Instantiate(char_ori);

                            Image char_img = char_new.GetComponent<Image>();
                            string _Image_BattleChar_Path;
                            dict.TryGetString("_Image_BattleChar_Path", out _Image_BattleChar_Path, key); ;
                            char_img.sprite = LoadPNG.LoadPNG.ModSprite(_Image_BattleChar_Path);

                            RectTransform char_rect = char_img.GetComponent<RectTransform>();
                            Vector2 offsetMax;
                            Vector2 offsetMin;
                            Vector2 pivot;
                            dict.TryGetVector2("_Image_BattleChar_offsetMax", out offsetMax, key);
                            dict.TryGetVector2("_Image_BattleChar_offsetMin", out offsetMin, key);
                            dict.TryGetVector2("_Image_BattleChar_pivot", out pivot, key);
                            char_rect.offsetMax = offsetMax;
                            char_rect.offsetMin = offsetMin;
                            char_rect.pivot = pivot;
                            char_new.name = _Image_BattleChar_Path;
                            try
                            {
                                moded_BattleChar.Add(key, char_new);
                            }
                            catch
                            {

                            }
                            
                            __result = char_new;
                        }
                       
                    }

                }
            }
        }



    }



}



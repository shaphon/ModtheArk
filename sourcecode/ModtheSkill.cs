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
using System.Runtime.ConstrainedExecution;

namespace SHAPHON
{

    [BepInPlugin(GUID, "ModtheSkill", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModtheSkillPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModtheSkill";
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




        [HarmonyPatch(typeof(GDESkillData))]
        private class GDESkillDataPlugin
        {


            [HarmonyPatch(nameof(GDESkillData.LoadFromSavedData))]
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
                    dict.TryGetString("_Image_0_Moded_Path", out string _Image_0_Moded_Path, key);
                    Sprite _Image_0 = Load_Skill_Image(_Image_0_Moded_Path);
                    Traverse.Create(__instance).Field("_Image_0").SetValue(_Image_0);
                    dict.TryGetString("_Image_1_Moded_Path", out string _Image_1_Moded_Path, key);
                    Sprite _Image_1 = Load_Skill_Image(_Image_1_Moded_Path);
                    Traverse.Create(__instance).Field("_Image_1").SetValue(_Image_1);
                    dict.TryGetString("_Image_2_Moded_Path", out string _Image_2_Moded_Path, key);
                    Sprite _Image_2 =  Load_Skill_Image(_Image_2_Moded_Path);
                    Traverse.Create(__instance).Field("_Image_2").SetValue(_Image_2);
                }
            }

        }
        static Sprite Load_Skill_Image(string path)
        {
            if (LoadPNG.LoadPNG.ContainsSprite(path))
            {
                return LoadPNG.LoadPNG.ModSprite(path);
            }
            Texture2D ori_tex = LoadPNG.LoadPNG.ModTexture2D(path);
            if(ori_tex== null)
            {
                return null;
            }
            int width = 440;
            int height = 280;
            int ori_width = ori_tex.width;
            int ori_height = ori_tex.height;
            int dx = (width - ori_width) / 2;
            int dy = (height - ori_height) / 2;
            if (( dx > 0 && dy > 0 )||( dx == 0 && dy > 0 )||( dx > 0 && dy == 0 ))
            {
                Texture2D new_tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
                Color clr_bg = Texture2D.blackTexture.GetPixel(0, 0);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (i >= dx && i < dx + ori_width && j >= dy && j < dy + ori_height)
                        {
                            Color clr = ori_tex.GetPixel(i - dx, j - dy);
                            new_tex.SetPixel(i, j, clr);

                        }
                        else
                        {
                            new_tex.SetPixel(i, j, clr_bg);
                        }

                    }

                }
                new_tex.Apply();
                ori_tex = new_tex;
                LoadPNG.LoadPNG.ChangeTexture2D(path, new_tex);
                UnityEngine.Debug.Log("正在重构" + path);
            }
            Sprite sp = Sprite.Create(ori_tex, new Rect(0, 0, ori_tex.width, ori_tex.height), new Vector2(0, 0), 100f, 0U, SpriteMeshType.Tight);
            LoadPNG.LoadPNG.ChangeSprite(path, sp);
            return sp;
        }



    }
}

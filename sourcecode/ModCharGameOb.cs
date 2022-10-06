using BepInEx;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System;
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
                harmony.UnpatchSelf();
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
                        if (moded_FaceOriChar.ContainsKey(key))
                        {
                            try
                            {
                                __result = moded_FaceOriChar[key];
                                if (__result == null)
                                {

                                    throw new Exception("重新读取");
                                }
                            }
                            catch
                            {
                                GameObject face_ori = Resources.Load<GameObject>("CharImagePrefebs/Face/Face_Johan");

                                GameObject face = UnityEngine.Object.Instantiate(face_ori);
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

                            GameObject face = UnityEngine.Object.Instantiate(face_ori);
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
                        if (moded_BattleChar.ContainsKey(key))
                        {
                            try
                            {
                                __result = moded_BattleChar[key];
                                if (__result == null)
                                {
                                    throw new Exception("重新读取");
                                }
                            }
                            catch
                            {
                                GameObject char_ori = Resources.Load<GameObject>("CharImagePrefebs/JohanBattle");

                                GameObject char_new = UnityEngine.Object.Instantiate(char_ori);


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

                            GameObject char_new = UnityEngine.Object.Instantiate(char_ori);

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



/*
       private class SkillComparer : IComparer<GameObject>
       {
           // Token: 0x06003A5C RID: 14940 RVA: 0x001BB4E3 File Offset: 0x001B98E3
           public int Compare(GameObject x, GameObject y)
           {
               if (x.GetComponent<SkillPrefab>().ClassNum < y.GetComponent<SkillPrefab>().ClassNum)
               {
                   return -1;
               }
               return 0;
           }
       }
       */
/*
[HarmonyPatch(typeof(SKillCollection))]
private class SKillCollectionPlugin
{


    [HarmonyPatch("NowCharShow")]
    [HarmonyPostfix]
    private static void NowCharShow_Postfix(SKillCollection __instance,int num)
    { 
        if(num == count)
        {
            Traverse.Create(__instance).Field("Nowchar").SetValue(CameraKey);
        }
    }
    [HarmonyPatch("InitPageNum")]
    [HarmonyPostfix]
    private static void InitPageNum_Postfix(SKillCollection __instance, int num)
    {
        string nowchar =  Traverse.Create(__instance).Field("Nowchar").GetValue<string>();
        List<GameObject> Align = Traverse.Create(__instance).Field("Align").GetValue<List<GameObject>>();
        if (nowchar == CameraKey)
        {
            Traverse.Create(__instance).Field("TempAlign").SetValue(Align[count]);
        }




    }
    [HarmonyPatch("Init")]
    [HarmonyPostfix]
    private static void Init_Postfix(SKillCollection __instance)
    {

        GDEDataManager.GetAllDataKeysBySchema(GDESchemaKeys.Skill, out List<string> SkillKeys);
        List<GameObject> Align = Traverse.Create(__instance).Field("Align").GetValue<List<GameObject>>();
        List<GDESkillData> AllSkill = new List<GDESkillData>();
        count = Align.Count;
        foreach (string text in SkillKeys)
        {
            if (text != string.Empty)
            {
                GDESkillData gdeskillData = new GDESkillData(text);
                gdeskillData.KeyID = gdeskillData.Key;
                AllSkill.Add(gdeskillData);
            }
        }
        GameObject align = GameObject.Instantiate(__instance.Align_Azar,__instance.Align_Azar.transform.parent.transform);

        for (int i = 0; i < AllSkill.Count; i++)
        {
            if (AllSkill[i].User == CameraKey || AllSkill[i].LucyPartyDraw == CameraKey)
            {

                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(__instance.SkillPrefab, align.transform);
                gameObject.name = CameraKey;
                gameObject.GetComponent<SkillPrefab>().MPNum.text = AllSkill[i].UseAp.ToString();
                if (AllSkill[i].Image_0 != null)
                {
                    gameObject.GetComponent<SkillPrefab>().SkillImage.sprite = AllSkill[i].Image_0;
                }
                gameObject.GetComponent<SkillPrefab>().SkillName.text = AllSkill[i].Name;
                gameObject.GetComponent<SkillPrefab>().SkillTargetText.text = __instance.SkillTarget(AllSkill[i].Target);
                gameObject.GetComponent<SkillPrefab>().Index = i;
                if (AllSkill[i].LucyPartyDraw == CameraKey)
                {
                    gameObject.GetComponent<SkillPrefab>().LucyFace.SetActive(true);
                }


                gameObject.GetComponent<SkillPrefab>().CharIndex = Align.Count;
                if (AllSkill[i].NotCount)
                {
                    gameObject.GetComponent<SkillPrefab>().Crystal.sprite = __instance.MP_Quick;
                }


                if (AllSkill[i].Rare || AllSkill[i].User == "Lucy")
                {
                    gameObject.GetComponent<SkillPrefab>().SkillClassImage.gameObject.SetActive(true);
                    gameObject.GetComponent<SkillPrefab>().SkillClassImage.sprite = __instance.RareSkillLine;
                    gameObject.GetComponent<SkillPrefab>().ClassNum = 2;
                }
                if (AllSkill[i].Target.Key == GDEItemKeys.s_targettype_all_enemy || AllSkill[i].Target.Key == GDEItemKeys.s_targettype_enemy || AllSkill[i].Target.Key == GDEItemKeys.s_targettype_enemy_PlusRandom || AllSkill[i].Target.Key == GDEItemKeys.s_targettype_random_enemy)
                {
                    gameObject.GetComponent<SkillPrefab>().SkillTargetImage.sprite = __instance.EnemyTargetBG;
                    gameObject.GetComponent<SkillPrefab>().SkillTargetText.color = Color.white;
                }
                if (AllSkill[i].Target.Key == GDEItemKeys.s_targettype_Misc)
                {
                    gameObject.GetComponent<SkillPrefab>().SkillTargetImage.gameObject.SetActive(false);
                }
            }

        }

        List<GameObject> list = new List<GameObject>();
        for (int m = 0; m < align.transform.childCount; m++)
        {
            list.Add(align.transform.GetChild(m).gameObject);
        }
        list.Sort(new SkillComparer());
        for (int j = 0; j < align.transform.childCount; j++)
        {
            align.transform.GetChild(j).parent = null;
            j--;
        }
        foreach (GameObject gameObject in list)
        {
            gameObject.transform.parent = align.transform;
        }
        for (int k = 0; k < align.transform.childCount; k++)
        {
            if (align.transform.GetChild(k).GetComponent<SkillPrefab>().LucyFace.activeSelf)
            {
                Transform child = align.transform.GetChild(k);
                child.SetParent(null);
                child.SetParent(align.transform);
            }
        }

        list.Clear();

        Align.Add(align);


        AllSkill.Clear();
        List<GameObject> Align_Char = Traverse.Create(__instance).Field("Align_Char").GetValue<List<GameObject>>();

        GameObject gameObjectchar = UnityEngine.Object.Instantiate<GameObject>(__instance.Skill_CharSelectPrefab, __instance.Align_CharSelect.transform);
        gameObjectchar.GetComponent<Skill_CharSelect>().CharacterImage.sprite = Align[count].GetComponent<SKill_Align>().CharImage;
        gameObjectchar.GetComponent<Skill_CharSelect>().index = count;
        gameObjectchar.GetComponent<Skill_CharSelect>().Category = "Deal";
        gameObjectchar.GetComponent<Skill_CharSelect>().Key = CameraKey;
        Align_Char.Add(gameObjectchar);



    }
*/
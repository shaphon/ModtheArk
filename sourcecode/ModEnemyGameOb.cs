using BepInEx;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SHAPHON
{

    [BepInPlugin(GUID, "ModEnemyGameOb", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModEnemyGameObPlugin : BaseUnityPlugin
    {
        public const string GUID = "ModEnemyGameOb";
        public const string version = "1.0.0";
        private static readonly Harmony harmony = new Harmony(GUID);
        public static int count = -1;
        private static Dictionary<string, GameObject> moded_BattleObject = new Dictionary<string, GameObject>();
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
        
        [HarmonyPatch(typeof(GDEEnemyData))]
        private class GDEEnemyDataPlugin
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


                    dict.TryGetString("_CollectionSprite_Cover_Path", out string _CollectionSprite_Cover_Path, key);
                    Sprite _CollectionSprite_Cover = LoadPNG.LoadPNG.ModSprite(_CollectionSprite_Cover_Path);
                    Traverse.Create(__instance).Field("_CollactionSprite_Cover").SetValue(_CollectionSprite_Cover);


                }
            }
            [HarmonyPatch(nameof(GDEEnemyData.BattleObject), MethodType.Getter)]
            [HarmonyPostfix]
            private static void BattleObject_Postfix(GDEEnemyData __instance, ref GameObject __result)
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
                        if (moded_BattleObject.ContainsKey(key))
                        {
                            try
                            {
                                __result = moded_BattleObject[key];
                                if (__result == null)
                                {
                                    throw new Exception("重新读取");
                                }
                            }
                            catch
                            {
                                string _Base_BattleObject;
                                dict.TryGetString("_Base_BattleObject", out _Base_BattleObject, key);
                                if (string.IsNullOrEmpty(_Base_BattleObject))
                                {
                                    _Base_BattleObject = "BattleEnemy/Dochi";
                                }
                                UnityEngine.Debug.Log(_Base_BattleObject);
                                GameObject battleob_ori = Resources.Load<GameObject>(_Base_BattleObject);
                                string _Image_BattleObject_Path;
                                dict.TryGetString("_Image_BattleObject_Path", out _Image_BattleObject_Path, key);
                                if (string.IsNullOrEmpty(_Image_BattleObject_Path))
                                {
                                    _Image_BattleObject_Path = "Camera_OriFace2.png";
                                }
                                GameObject battleob = UnityEngine.Object.Instantiate(battleob_ori);
                                UnityEngine.Debug.Log(_Image_BattleObject_Path);
                                SpriteRenderer battleob_img = battleob.GetComponentInChildren<SpriteRenderer>();
                                battleob_img.sprite = Load_EnemyBattle_Image(_Image_BattleObject_Path);
                                battleob.name = _Image_BattleObject_Path;
                                moded_BattleObject[key]= battleob;
                                __result = battleob;
                            }
                        }
                        else
                        {
                            string _Base_BattleObject;
                            dict.TryGetString("_Base_BattleObject", out _Base_BattleObject, key);
                            if (string.IsNullOrEmpty(_Base_BattleObject))
                            {
                                _Base_BattleObject = "BattleEnemy/Dochi";
                            }
                            UnityEngine.Debug.Log(_Base_BattleObject);
                            GameObject battleob_ori = Resources.Load<GameObject>(_Base_BattleObject);
                            string _Image_BattleObject_Path;
                            dict.TryGetString("_Image_BattleObject_Path", out _Image_BattleObject_Path, key);
                            if (string.IsNullOrEmpty(_Image_BattleObject_Path))
                            {
                                _Image_BattleObject_Path = "Camera_OriFace2.png";
                            }
                            GameObject battleob = UnityEngine.Object.Instantiate(battleob_ori);
                            UnityEngine.Debug.Log(_Image_BattleObject_Path);
                            SpriteRenderer battleob_img = battleob.GetComponentInChildren<SpriteRenderer>();
                            battleob_img.sprite = Load_EnemyBattle_Image(_Image_BattleObject_Path);
                            battleob.name = _Image_BattleObject_Path;
                            try
                            {
                                moded_BattleObject.Add(key, battleob);
                            }
                            catch
                            {

                            }
                            

                            __result = battleob;
                        }
                        


                    }

                }


            }
            static Sprite Load_EnemyBattle_Image(string path)
            {
                if (LoadPNG.LoadPNG.ContainsSprite(path))
                {
                    return LoadPNG.LoadPNG.ModSprite(path);
                }
                Texture2D ori_tex = LoadPNG.LoadPNG.ModTexture2D(path);
                Sprite sp = Sprite.Create(ori_tex, new Rect(0, 0, ori_tex.width, ori_tex.height), new Vector2(0.5f, 0), 100f, 0U, SpriteMeshType.Tight);
                LoadPNG.LoadPNG.ChangeSprite(path, sp);
                return sp;
            }



        }
    }
}
        
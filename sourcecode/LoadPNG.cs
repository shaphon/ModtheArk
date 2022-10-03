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
using static System.Net.Mime.MediaTypeNames;
using Microsoft.SqlServer.Server;

namespace LoadPNG
{
    [BepInPlugin(GUID, "LoadPNG", version)]
    [BepInProcess("ChronoArk.exe")]
    public class LoadPNG : BaseUnityPlugin
    {
        public const string GUID = "LoadPNG";
        public const string version = "1.0.0";

        private static readonly Harmony harmony = new Harmony(GUID);

        private static Dictionary<string, Texture2D> moded_texture = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Sprite> moded_sprite = new Dictionary<string, Sprite>();
        void Awake()
        {
            LoadTextureDir(moded_texture, Paths.PluginPath, "");
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }
        public static void LoadTextureDir(Dictionary<string, Texture2D> texDict, string dir_path,string inner_path = "")
        {
            
            DirectoryInfo directoryInfo = new DirectoryInfo(dir_path);
            if (inner_path != "")
            {
                directoryInfo = new DirectoryInfo(dir_path + "\\" + inner_path);
            }
            Queue<FileInfo> queue = new Queue<FileInfo>();
            foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories())
            {
                string new_inner_path = dirInfo.Name;
                if (inner_path != "")
                {
                    new_inner_path = inner_path + "\\" + dirInfo.Name;
                }
                LoadTextureDir(texDict, dir_path, new_inner_path);
            }
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                bool isPNG = fileInfo.Extension.Equals(".png");
                if (isPNG)
                {
                    queue.Enqueue(fileInfo);
                }
            }
            while (queue.Count > 0)
            {
                FileInfo fileInfo2 = queue.Dequeue();
                string filename;
                if (inner_path=="")
                {
                    filename = fileInfo2.Name;
                }
                else
                {
                    filename = inner_path + "\\" + fileInfo2.Name;
                }
                Texture2D tex = LoadTexture(fileInfo2.FullName);
                texDict.Add(filename, tex);
                UnityEngine.Debug.Log("读取完毕:" + filename);
            }
        }
        public static Texture2D LoadTexture(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bys = new byte[fs.Length];
            try
            {
                fs.Read(bys, 0, bys.Length);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
            fs.Close();
            Texture2D texture = new Texture2D(4096,4096,TextureFormat.ARGB32,false);
            texture.LoadImage(bys);
            texture.Apply();
            return texture;
        }
        public static Texture2D ModTexture2D(string key)
        {
            string[] keys = key.Split('+');
            if (keys[0] == "GameResource")
            {
                try
                {
                    string schemaname = keys[1];
                    string keyname = keys[2];
                    string fieldname = keys[3];
                    Texture2D tex = GDEDataManager.GDEResourcesData.Schemas[schemaname][keyname][fieldname].MainTexture;
                    return tex;
                }
                catch
                {
                    ;
                }
            }   
            if (moded_texture.ContainsKey(key))
            {
                return moded_texture[key];
            }
            else
            {
                return null;
            }
            
        }
        public static Sprite ModSprite(string key)
        {
            string[] keys = key.Split('+');
            if (keys[0] == "GameResource")
            {
                try
                {
                    string schemaname = keys[1];
                    string keyname = keys[2];
                    string fieldname = keys[3];
                    Sprite sp = GDEDataManager.GDEResourcesData.Schemas[schemaname][keyname][fieldname].MainSprite;
                    return sp;
                }
                catch
                {
                    ;
                }
                
            }
            if (moded_texture.ContainsKey(key))
            {
                if (moded_sprite.ContainsKey(key))
                {
                    return moded_sprite[key];
                }
                else
                {
                    Texture2D tempTexture2D = moded_texture[key];

                    Sprite sp = Sprite.Create(tempTexture2D, new Rect(0, 0, tempTexture2D.width, tempTexture2D.height), new Vector2(0, 0), 100f, 0U, SpriteMeshType.Tight);
                    moded_sprite.Add(key, sp);
                    return sp;
                }
                
            }
            else
            {
                return null;
            }
           
        }
        public static void  ChangeTexture2D(string key,Texture2D new_tex)
        {
            moded_texture[key] = new_tex;
        }
        public static void ChangeSprite(string key, Sprite new_sp)
        {
            moded_sprite[key] = new_sp;
        }
        public static bool ContainsSprite(string key)
        {
            return moded_sprite.ContainsKey(key);
        }
        public static bool ContainsTexture2D(string key)
        {
            return moded_texture.ContainsKey(key);
        }
        public static Dictionary<string, Texture2D> All_Moded_Texture2D()
        {
            return moded_texture;
        }

    }

}

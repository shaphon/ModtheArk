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
using Microsoft.SqlServer.Server;
using DarkTonic.MasterAudio;

namespace LoadAsset
{
    [BepInPlugin(GUID, "LoadAsset", version)]
    [BepInProcess("ChronoArk.exe")]
    public class LoadAsset : BaseUnityPlugin
    {
        public const string GUID = "LoadAsset";
        public const string version = "1.0.0";

        private static readonly Harmony harmony = new Harmony(GUID);

        public static Dictionary<string, AssetBundle> moded_asset = new Dictionary<string, AssetBundle>();
        void Awake()
        {
           
            LoadAssetDir(moded_asset, Paths.PluginPath, "");
            
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }
        public static void LoadAssetDir(Dictionary<string, AssetBundle> assetDict, string dir_path, string inner_path = "")
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
                LoadAssetDir(assetDict, dir_path, new_inner_path);
            }
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                bool isAsset = fileInfo.Extension.Equals(".asset");
                if (isAsset)
                {
                    queue.Enqueue(fileInfo);
                }
            }
            while (queue.Count > 0)
            {
                FileInfo fileInfo2 = queue.Dequeue();
                string filename;
                if (inner_path == "")
                {
                    filename = fileInfo2.Name;
                }
                else
                {
                    filename = inner_path + "\\" + fileInfo2.Name;
                }
                string url = "file:" + fileInfo2.FullName.Replace("\\", "/");
                WWW www = WWW.LoadFromCacheOrDownload(url, 1);
                if (www != null)
                {
                    AssetBundle asset = www.assetBundle;
                    if (asset != null)
                    {
                        assetDict.Add(filename, asset);
                        UnityEngine.Debug.Log("资源包读取完毕:" + filename);
                        UnityEngine.Object[] objects= asset.LoadAllAssets();
                        foreach(object ob in objects)
                        {
                            UnityEngine.Debug.Log(ob);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("资源包读取错误:" + filename);
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("资源包路径读取错误:" + filename);
                }



            }
        }
    }

}

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
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace LoadType
{
    [BepInPlugin(GUID, "LoadType", version)]
    [BepInProcess("ChronoArk.exe")]
    public class LoadType : BaseUnityPlugin
    {
        public const string GUID = "LoadType";
        public const string version = "1.0.0";

        private static readonly Harmony harmony = new Harmony(GUID);

        private static Dictionary<string, Assembly> moded_assembly_dict = new Dictionary<string, Assembly>();
        private static Dictionary<string, Dictionary<string, Type>> moded_type_dict = new Dictionary<string, Dictionary<string, Type>>();
        
        void Awake()
        {
            LoadAssemblyDir(moded_assembly_dict, Paths.PluginPath, "");
            LoadNewTypes(moded_type_dict, moded_assembly_dict);
            
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }
        public static void LoadNewTypes(Dictionary<string, Dictionary<string, Type>> moded_type_dict, Dictionary<string, Assembly> moded_assembly_dict)
        {
            foreach(string asmb_key in moded_assembly_dict.Keys)
            {
                Assembly asmb = moded_assembly_dict[asmb_key];
                Dictionary<string, Type> asmb_typ = new Dictionary<string, Type>();
                foreach (Type typ in asmb.GetTypes())
                {
                    asmb_typ.Add(typ.FullName, typ);
                    UnityEngine.Debug.Log("类读取完毕:" + asmb_key+"  "+typ.FullName);
                }
                moded_type_dict.Add(asmb_key, asmb_typ);
            }
        }
        public static void LoadAssemblyDir(Dictionary<string, Assembly> assembly_dict, string dir_path, string inner_path = "")
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
                LoadAssemblyDir(assembly_dict, dir_path, new_inner_path);
            }
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                bool isDLL = fileInfo.Extension.Equals(".dll");
                if (isDLL)
                {
                    string[] namesplite = fileInfo.Name.Split('&');
                    int size = namesplite.Length ;
                   if ( namesplite[size-1] == "NewType.dll")
                   {
                        queue.Enqueue(fileInfo);
                   }
                    
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
                Assembly asmb = Assembly.LoadFrom(fileInfo2.FullName);
                assembly_dict.Add(filename, asmb);
                UnityEngine.Debug.Log("类文件读取完毕:" + filename);
            }
        }

        [HarmonyPatch(typeof(Type))]
        private class TypePlugin
        {
            [HarmonyPatch("internal_from_name")]
            [HarmonyPrefix]
            private static bool internal_from_nameprefix(ref Type __result, string name)
            {
                string[] keys = name.Split('+');
                if (keys[0].Length - 5 < 0)
                {
                    return true;
                }
                if (keys[0].Substring(keys[0].Length-5) == "Moded")
                {

                    __result = moded_type_dict[keys[1]][keys[2]];
                    return false;
                }
                else
                {
                    return true;
                }
                
            }
        }

        


        public void readSave(string filePath,  string targetpath)
        {
            DESCryptoServiceProvider Secretkey = new DESCryptoServiceProvider();
            Secretkey.Key = Encoding.ASCII.GetBytes("dkrldkwj");
            Secretkey.IV = Encoding.ASCII.GetBytes("dkrldkwj");
            using (FileStream fileStream2 = File.Open(filePath, FileMode.Open))
            {
                using (CryptoStream cryptoStream = new CryptoStream(fileStream2, Secretkey.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                    {
                        using(StreamWriter streamWriter = new StreamWriter(targetpath))
                        {
                            try
                            {
                                streamWriter.Write(streamReader.ReadToEnd());
                            }
                            catch
                            {
                            }
                            try
                            {
                                streamReader.Close();
                                cryptoStream.Close();
                                fileStream2.Close();
                                streamWriter.Close();
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

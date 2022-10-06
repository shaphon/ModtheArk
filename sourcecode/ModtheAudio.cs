using BepInEx;
using DarkTonic.MasterAudio;
using HarmonyLib;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace SHAPHON
{
    [BepInPlugin(GUID, "ModtheAudio", version)]
    [BepInProcess("ChronoArk.exe")]
    public class ModtheAudio : BaseUnityPlugin
    {
        public const string GUID = "ModtheAudio";
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



        [HarmonyPatch(typeof(MasterAudio))]
        private class TypePlugin
        {
            [HarmonyPatch("Awake")]
            [HarmonyPostfix]
            private static void AwakePostfix()
            {
                foreach (string assetkey in LoadAsset.LoadAsset.moded_asset.Keys)
                {
                    AssetBundle asset = LoadAsset.LoadAsset.moded_asset[assetkey];
                    AudioClip[] audioClips = asset.LoadAllAssets<AudioClip>();
                    foreach (AudioClip audioClip in audioClips)
                    {
                        string name = assetkey + ":" + audioClip.name;
                        if (MasterAudio.GetGroupInfo(name) == null)
                        {
                            CreateSoundGroupFromSoundGroup("TW_BlackMoon", name);
                            MasterAudio.ChangeVariationClip(name, true, name, audioClip);
                            MasterAudio.AudioGroupInfo audioGroupInfo = MasterAudio.GetGroupInfo(name);
                            audioGroupInfo.Sources[0].Source.transform.name = name;
                        }



                    }

                }


            }
        }
        public static int GetBusIndex(string busName)
        {
            
            for (int i = 0; i < MasterAudio.GroupBuses.Count; i++)
            {
                if (MasterAudio.GroupBuses[i].busName == busName)
                {
                    return i + 2;
                }
            }
            MasterAudio.LogWarning("Could not find bus '" + busName + "'.");
            return -1;
        }
        public static void ChangeGroupBus(string sType,string NewBusName)
        {
            UnityEngine.Debug.Log("???");

            int newBusIndex = GetBusIndex(NewBusName);
            MasterAudioGroup masterAudioGroup = MasterAudio.GetGroupInfo(sType).Group;
            masterAudioGroup.busIndex = newBusIndex;

        }
        public static AudioSource getAudioSource(string sType)
        {
            return MasterAudio.GetGroupInfo(sType).Sources[0].Source;
        }
        public static void ChangeLoop(string sType,bool loop)
        {
            
            Traverse.Create(MasterAudio.GetAllVariationsOfGroup(sType)[0].Variation).Field("_audioLoops").SetValue(loop);


        }









        public static Transform CreateSoundGroupFromSoundGroup(string oldname, string newtransformname,bool errorOnExisting = true)
        {
            bool SceneHasMasterAudio = Traverse.Create(MasterAudio.Instance).Field("SceneHasMasterAudio").GetValue<bool>();
            if (SceneHasMasterAudio)
            {
                return null;
            }
            if (!MasterAudio.SoundsReady)
            {
                Debug.LogError("MasterAudio not finished initializing sounds. Cannot create new group yet.");
                return null;
            }
            string name = newtransformname;
            MasterAudio instance = MasterAudio.Instance;
            if (MasterAudio.Instance.Trans.GetChildTransform(name) != null)
            {
                if (errorOnExisting)
                {
                    Debug.LogError("Cannot add a new Sound Group named '" + name + "' because there is already a Sound Group of that name.");
                }
                return null;
            }
           
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(MasterAudio.Instance.Trans.GetChildTransform(oldname).gameObject);
            Transform transform = gameObject.transform;
            transform.name = UtilStrings.TrimSpace(name);
            transform.parent = MasterAudio.Instance.Trans;
            transform.gameObject.layer = MasterAudio.Instance.gameObject.layer;
            MasterAudioGroup component3 = gameObject.GetComponent<MasterAudioGroup>();
            float? groupVolume = PersistentAudioSettings.GetGroupVolume(name);
            List<MasterAudio.AudioInfo> list = new List<MasterAudio.AudioInfo>();
            List<int> list2 = new List<int>();
            for (int k = 0; k < gameObject.transform.childCount; k++)
            {
                list2.Add(k);
                Transform child = gameObject.transform.GetChild(k);
                AudioSource component4 = child.GetComponent<AudioSource>();
                SoundGroupVariation component = child.GetComponent<SoundGroupVariation>();
                list.Add(new MasterAudio.AudioInfo(component, component4, component4.volume));
                component.DisableUpdater();
            }
            MasterAudio.AudioGroupInfo groupInfo = new MasterAudio.AudioGroupInfo(list, component3);
            //Traverse.Create(MasterAudio.Instance).Method("AddRuntimeGroupInfo").GetValue(new object[] { name, new MasterAudio.AudioGroupInfo(list, component3) });
            Traverse.Create(MasterAudio.Instance).Field("AudioSourcesBySoundType").GetValue<Dictionary<string, MasterAudio.AudioGroupInfo>>().Add(name, groupInfo);
            List<AudioSource> listauds = new List<AudioSource>(groupInfo.Sources.Count);
            for (int i = 0; i < groupInfo.Sources.Count; i++)
            {
                listauds.Add(groupInfo.Sources[i].Source);
            }
            MasterAudio.TrackRuntimeAudioSources(listauds);
            if (component3.curVariationSequence == MasterAudioGroup.VariationSequence.Randomized)
            {
                ArrayListUtil.SortIntArray(ref list2);
            }
            Traverse.Create(MasterAudio.Instance).Field("_randomizer").GetValue<Dictionary<string, List<int>>>().Add(name, list2);
            List<int> list3 = new List<int>(list2.Count);
            list3.AddRange(list2);
            Traverse.Create(MasterAudio.Instance).Field("_randomizerOrigin").GetValue<Dictionary<string, List<int>>>().Add(name, list3);
            Traverse.Create(MasterAudio.Instance).Field("_randomizerLeftovers").GetValue<Dictionary<string, List<int>>>().Add(name, new List<int>(list2.Count)); ;
            Traverse.Create(MasterAudio.Instance).Field("_randomizerLeftovers").GetValue<Dictionary<string, List<int>>>()[name].AddRange(list2);
            Traverse.Create(MasterAudio.Instance).Field("_clipsPlayedBySoundTypeOldestFirst").GetValue<Dictionary<string, List<int>>>().Add(name, new List<int>(list2.Count));
            MasterAudio.RescanGroupsNow();
            if (component3.BusForGroup != null && component3.BusForGroup.isMuted)
            {
                MasterAudio.MuteGroup(component3.name, false);
            }
            else if (MasterAudio.Instance.mixerMuted)
            {
                MasterAudio.MuteGroup(component3.name, false);
            }
            return transform;
        }

        public static void FadeBusToVolume(string busName, float newVolume, float fadeTime, bool willStopAfterFade = false, bool willResetVolumeAfterFade = false)
        {
            if (newVolume < 0f || newVolume > 1f)
            {
                UnityEngine.Debug.LogError("Illegal volume passed to FadeBusToVolume: '" + newVolume + "'. Legal volumes are between 0 and 1");
                return;
            }
            if (fadeTime <= 0.1f)
            {
                MasterAudio.SetBusVolumeByName(busName, newVolume);
                if (willStopAfterFade)
                {
                    MasterAudio.StopBus(busName);
                }
                return;
            }
            GroupBus groupBus = MasterAudio.GrabBusByName(busName);
            if (groupBus == null)
            {
                UnityEngine.Debug.Log("Could not find bus '" + busName + "' to fade it.");
                return;
            }
            UnityEngine.Debug.Log("????????");
            List<BusFadeInfo> BusFades = Traverse.Create(MasterAudio.Instance).Field("BusFades").GetValue<List<BusFadeInfo>>();
            UnityEngine.Debug.Log(BusFades.Count);
            UnityEngine.Debug.Log("????????2");
            foreach(BusFadeInfo obj in BusFades)
            {
                UnityEngine.Debug.Log(obj.NameOfBus);
            }
            BusFadeInfo busFadeInfo = BusFades.Find((BusFadeInfo obj) => obj.NameOfBus == busName);
            UnityEngine.Debug.Log("??????3");
            if (busFadeInfo != null)
            {
                busFadeInfo.IsActive = false;
            }
            BusFadeInfo busFadeInfo2 = new BusFadeInfo
            {
                NameOfBus = busName,
                ActingBus = groupBus,
                StartVolume = groupBus.volume,
                TargetVolume = newVolume,
                StartTime = AudioUtil.Time,
                CompletionTime = AudioUtil.Time + fadeTime,
                WillStopGroupAfterFade = willStopAfterFade,
                WillResetVolumeAfterFade = willResetVolumeAfterFade
            };

            BusFades.Add(busFadeInfo2);
            
        }
    }
}

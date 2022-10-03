# ModtheArk
A tool for Chronoark mods

## For Player

Put the ModtheArk.dll in  `Chrono Ark\x64\Master\BepInEx\plugins` folder.

## For Modder

This is a tool to:

1. Load Unity AssetBundle files

2. Load .png pictures

3. Load .dll assemblies( and new Types)

4. Play AudioClips loaded from AssetBundles（at game volume）

5. Fix the bug about `List<string>` that if you don't have strings in .csv files，you will load nothing from gdata.json

5. Load Moded gdata.json including

① Buff icons

② Skill images（3 image for 1 Skill）

③ Charator face and battle GameObject images

④ Charator Passive icon ，collection cover images

⑤ Enemy battle GameObject images and collection cover images

⑥ Item icons （except for Scrolls）

⑦ Event Images

### LoadAsset
1.You can put Unity AssetBundle under `Chrono Ark\x64\Master\BepInEx\plugins` folder and its subfolders

2.There must be `.asset`   at the end of your AssetBundle filename.

3.You can use LoadAsset.LoadAsset.moded_asset to get the Dictionary<string, AssetBundle>. ModtheArk Will read AssetBundle into this Dictionary when game starts.
The Keys are paths of `.asset` after `Chrono Ark\x64\Master\BepInEx\plugins`

eg: the Key of `Chrono Ark\x64\Master\BepInEx\plugins\mydir\myassetbundle.asset` is `"mydir\\myassetbundle.asset"`

4.Please Use Unity of version `2018.4.32f1` to make AssetBundles. You Can download it from https://unity3d.com/get-unity/download/archive

### LoadPNG
If you want to Load .png file through gdata.json, you can skip this section.

The following is for Modders who want to LoadPNG manually in their mods.

All functions below has a namespace   `LoadPNG` and a classname `LoadPNG`.

Everything you do to the returned pictures of these functions will change them forever.(until you restart the game)

1. public static Texture2D ModTexture2D(string key)

It will return the picture under `Chrono Ark\x64\Master\BepInEx\plugins`, `key` is the path of .png file after `Chrono Ark\x64\Master\BepInEx\plugins`.

eg: the key of `Chrono Ark\x64\Master\BepInEx\plugins\mydir\mypicture.png` is `"mydir\\mypicture.png"`

If you want to use the pictures inside ChronoArk ,you can use `"GameResource+A+B+C"` to get `GDEDataManager.GDEResourcesData.Schemas[A][B][C].MainTexture` 

2. public static Sprite ModSprite(string key)

It is similar to Texture2D










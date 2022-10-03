# ModtheArk
A tool for Chronoark mods

## For Player

Put the ModtheArk.dll in  `Chrono Ark\x64\Master\BepInEx\plugins` folder.

## For Modder

This is a tool to:

1.Load Unity AssetBundle files

2.Load .png pictures

3.Load .dll assemblies( and new Types)

4.Play AudioClips loaded from AssetBundles（at game volume）

5.Fix the bug about `List<string>` that if you don't have strings in .csv files，you will load nothing from gdata.json

6.Load Moded gdata.json including

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
You can put .png files under `Chrono Ark\x64\Master\BepInEx\plugins` folder and its subfolders.

the `ImagePath` in the following texts means 
1.the path of .png files after `Chrono Ark\x64\Master\BepInEx\plugins`

eg: If you want to load `Chrono Ark\x64\Master\BepInEx\plugins\mydir\mypicture.png`, you have `ImagePath = "mydir\\mypicture.png"`

2. a string in the format of  `"GameResource+A+B+C"` to read the ChronoArk picture in `GDEDataManager.GDEResourcesData.Schemas["A"]["B"]["C"]`

You can directly load .png file through gdata.json (see About gdata.json). The following is for modders who want to LoadPNG manually in their mods.

All functions below have a namespace `LoadPNG` and a classname `LoadPNG`.

Everything you do to the returned pictures of these functions will change them forever.(until you restart the game)

1.`public static Texture2D ModTexture2D(string ImagePath)`

2.`public static Sprite ModSprite(string ImagePath)`

### LoadType
1.You can put your .dll file under `Chrono Ark\x64\Master\BepInEx\plugins` folder and its subfolders.

2.There must be `&NewType.dll`   at the end of your assembly filename.

3.You can use `Type.GetType(string name)` with name in the format of  `"Moded+PathName+namespace.ClassName"`to get your class in .dll files

eg:`Type.GetType("Moded+mydir\\mydll&NewType.dll+SHAPHON.S_MyChar_0")`will return the type defined in `Chrono Ark\x64\Master\BepInEx\plugins\mydir\mydll&NewType.dll` with namespace  `SHAPHON` and a classname `S_MyChar_0`

4.You can write strings in the format above in gdata.json,to induce ChronoArk to read your class

eg:
```json
{		
    "SomeSkill": {
            ......
           "SkillExtended": [
          "Moded+mydir\\mydll&NewType.dll+SHAPHON.S_MyChar_0"
          ],
       	    ......

                }
}		
```
This .json means a Skill called "SomeSkill" will have a SkillExtended of class `SHAPHON.S_MyChar_0` written in Chrono Ark\x64\Master\BepInEx\plugins\mydir\mydll&NewType.dll`

Buff,Passive,Potions,... Other classes mentioned in gata.json can also be written in this way.


### ModtheAudio
1.You first need to load AudioClips through AssetBundles 

2.You can use MasterAudio.PlaySound(string AudioName) to play audios (and other MasterAudio functions is the similar)

3.`AudioName` is in the format of `"key of AssetBundle"+":"+"path of AudioClip in AssetBundle"`

4.You can use functions under   `SHAPHON.ModtheAudio` to do some easy works

`public static void ChangeLoop(string AudioName,bool loop)` to make the audio loop(or not)

`public static void ChangeGroupBus(string AudioName,string NewBusName)`to change audio into different Buses like
 `"BattleBGM"` and `FieldBGM`.
 
 5.Some of the MasterAudio functions cannot be used directly because of  the bug on System.Action, please rewrite it if you really need it.
 
 6.This mod may be related to the .Net FrameWork versions. If you use other version, please check whether this mod can be used.

### About gdata.json
This mod may become more useful if you use ArkLib at the same time.

If you don't know what `ImagePath` is, see LoadPNG. `"Here is ImagePath"` should be all replaced by  `ImagePath`.

Every .json means what you should add to gdata.json. Do not delete the useless part of the original gdata format.

eg: `"Icon":""` is nessary although you have `"_Icon_Moded_Path": "Here is ImagePath"`


#### Buff
```json
{		
    "SomeSkill": {
            ......
          "Moded": true,
          "_gdeType_Moded": "Bool",

          "_gdeType__Icon_Moded_Path": "String",
          "_Icon_Moded_Path": "Here is ImagePath"
  
       	    ......

                }
}	
```
#### Enemy
```json
{		
    "SomeEnemy": {
            ......
          "Moded": true,
          "_gdeType_Moded": "Bool",
          //This is a base GameObject for changing the Enemy battle image,it can be found in gdata of other enemies
          //The default GameObject is the BattleObject of a hedgehog
          //If you want to change the battle image of some enemy with special GameObject,such as Godo, you should not use the default GameObject as a base GameObject.
          //Do not use Enemy GameObject with more than 1 image as a base GameObject.
          "_gdeType__Base_BattleObject": "string",
          "_Base_BattleObject": "BattleEnemy/SR_Outlaw",
          "_gdeType__Image_BattleObject_Path": "string",
          "_Image_BattleObject_Path": "Here is ImagePath",
          "_CollectionSprite_Cover_Path": "Here is ImagePath",
          "_gdeType__CollectionSprite_Cover_Path": "String"
  
       	    ......

                }
}	
```
#### Enemy
```json
{		
    "技能名": {
            .....
        "Moded": true,
        "_gdeType_Moded": "Bool",
        //The .png must have the same size and shape with ChronoArk Skills.
        //Please use AssetStudio to check it.
        //Fixed Skill Image
        "_Image_2_Moded_Path": "Here is ImagePath",
        "_gdeType_Image_2_Moded_Path": "String",
        //Skill Button Image
        "_Image_1_Moded_Path": "Here is ImagePath",
        "_gdeType_Image_1_Moded_Path": "String",
        //Skill Image
        "_Image_0_Moded_Path": "Here is ImagePath",
        "_gdeType_Image_0_Moded_Path": "String"
            ......


  }
}	
```




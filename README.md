# ModtheArk
A tool for Chronoark mods

## For Player

Put the ModtheArk.dll in  `Chrono Ark\x64\Master\BepInEx\plugins` folder.

How to check a simple Mod Character Skill Encyclopedia: press Tab in the Field and click the name of the character after expanding his/her full image

## For Modder

This is a tool to:

1.Load Unity AssetBundle files

2.Load .png pictures

3.Load .dll assemblies( and new Types)

4.Play AudioClips loaded from AssetBundles（at game volume）

5.Fix the bug about `List<string>` that if you don't have strings in .csv files，you will load nothing from gdata.json

6.Check a simple Mod Character Skill Encyclopedia(press Tab in the Field and click the name of the character after expanding his/her full image)

7.Load Moded gdata.json including

① Buff icons

② Skill images（3 image for 1 Skill）

③ Charator face and battle GameObject images

④ Charator Passive icon ，collection cover images

⑤ Enemy battle GameObject images and collection cover images

⑥ Item icons （except for Scrolls）

⑦ Event Images

⑧ SkillExtended Icon
### Before you start modding
1.Create a folder with  `arklib_config.json`
```json
{
  "Modname": "Yourmodname",
  "UseModtheArk": true
}    
```
Put ModtheArk.dll and ArklibAPI.dll in plugins folder Then start the game once.

2.You can find your folder contains:

- Yourmodname-ModtheArk
- arklib_config.json

You can put your .asset, .png or .dll files(see LoadAsset,LoadPNG,LoadType)  in `Yourmodname-ModtheArk` folder, and ModtheArk will read them all.

3. When you start to make your mod, you may need to LoadAsset,LoadPNG,LoadType through a `filepath`

The `filepath` is the path of your file that begins with `Yourmodname-ModtheArk\...`

eg: the `filepath` of `Chrono Ark\x64\Master\BepInEx\plugins\mydir\Yourmodname-ModtheArk\mydir2\myassetbundle.asset` is

`"Yourmodname-ModtheArk\\mydir2\\myassetbundle.asset"`

4. When the mod needs to be released, it should be that:
    - Yourmodname-ModtheArk
    - arklib_config.json 
    - icon.png
    - manifest.json
    - README.md


### LoadAsset
1.You can put Unity AssetBundle under `Yourmodname-ModtheArk` folder

2.There must be `.asset`   at the end of your AssetBundle filename.

3.You can use LoadAsset.LoadAsset.moded_asset to get the Dictionary<string, AssetBundle>. ModtheArk Will read AssetBundle into this Dictionary when game starts.
The Keys are `filepath` of `.asset`(See :Before you start modding)

4.Please Use Unity of version `2018.4.32f1` to make AssetBundles. You Can download it from https://unity3d.com/get-unity/download/archive

### LoadPNG
You can put .png files under `Yourmodname-ModtheArk` folder

the `ImagePath` in the following texts means 

1.the `filepath` of .png files (See :Before you start modding)

2.a string in the format of  `"GameResource+A+B+C"` to read the ChronoArk picture in `GDEDataManager.GDEResourcesData.Schemas["A"]["B"]["C"]`

You can directly load .png file through gdata.json (see About gdata.json). The following is for modders who want to LoadPNG manually in their mods.

All functions below have a namespace `LoadPNG` and a classname `LoadPNG`.

Everything you do to the returned pictures of these functions will change them forever.(until you restart the game)

1.`public static Texture2D ModTexture2D(string ImagePath)`

2.`public static Sprite ModSprite(string ImagePath)`

### LoadType
1.You can put your .dll file under `Yourmodname-ModtheArk` folder, the Assembly name (not .dll filename) should not contain characters except for alphanumeric characters and the underscore _.

2.There must be `NewType.dll`   at the end of your assembly filename.  `&`in your filename will be ignored.

3.You can write strings in the format above in gdata.json,to induce ChronoArk to read your class

eg:
```json
{		
    "SomeSkill": {
            ......
           "SkillExtended": [
           \\& in filename will be ignored
          "Moded+Yourmodname-ModtheArk\\mydir2\\mydll&NewType.dll+SHAPHON.S_MyChar_0"
          ],
       	    ......

                }
}		
```
This .json means a Skill called "SomeSkill" will have a SkillExtended of class `SHAPHON.S_MyChar_0` written in `Chrono Ark\x64\Master\BepInEx\plugins\NoMatterWhatItNames\Yourmodname-ModtheArk\mydir2\mydllNewType.dll`

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

If you don't know what `ImagePath` is, see LoadPNG. `"Here is ImagePath"` should be all replaced by  `ImagePath`.Some of the Image has a default value in ChronoArk.

Every .json means what you should add to gdata.json. Do not delete the useless part of the original gdata format.


eg: `"Icon":""` is nessary although you have `"_Icon_Moded_Path": "Here is ImagePath"`

Is `"_gdeType_..."` actually used in ChronoArk? I am not sure. 
#### Buff
```json
{		
    "SomeSkill": {
            ......
          "Moded": true,
          "_gdeType_Moded": "Bool",

          "_gdeType_Icon_Moded_Path": "String",
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
          "_gdeType_Base_BattleObject": "string",
          "_Base_BattleObject": "BattleEnemy/SR_Outlaw",
          "_gdeType_Image_BattleObject_Path": "string",
          "_Image_BattleObject_Path": "Here is ImagePath",
          "_CollectionSprite_Cover_Path": "Here is ImagePath",
          "_gdeType_CollectionSprite_Cover_Path": "String"
  
       	    ......

                }
}	
```
#### Skill
```json
{		
    "SomeSkill": {
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
#### Charactor
```json
{		
    "SomeCharactor": {
            .....
        "Moded": true,
        "_gdeType_Moded": "Bool",
        "_face_Path": "Here is ImagePath",
        "_gdeType_face_Path": "String",
        "_PassiveIcon_Path": "Here is ImagePath",
        "_gdeType_PassiveIcon_Path": "String",
        "_CollectionSprite_Cover_Path": "Here is ImagePath",
        "_gdeType_CollectionSprite_Cover_Path": "String",
        "_gdeIsList_CampSD_Path": 1,
        "_gdeType_CampSD_Path": "String",
        "_CampSD_Path": [ 
          "Here is ImagePath",
          "Here is ImagePath",
          "Here is ImagePath",
          "Here is ImagePath"
        ],
        //BattleChar means the image you see when you choose your team at the begining of game or when you press Tab during the game.
        //Here are 3 parameters to change the position and size of image
        //Maybe in some place of the game, only `Max-Min` will be actually used
        "_Image_BattleChar_Path": "Here is ImagePath",
        "_gdeType_Image_BattleChar_Path": "String",
        "_gdeType_Image_BattleChar_offsetMax": "Vector2",
        "_Image_BattleChar_offsetMax": {
          "x": 461,
          "y": 84
        },
        "_gdeType_Image_BattleChar_offsetMin": "Vector2",
        "_Image_BattleChar_offsetMin": {
          "x": -520,
          "y": -1394
        },
        "_gdeType_Image_BattleChar_pivot": "Vector2",
        "_Image_BattleChar_pivot": {
          "x": 0.4,
          "y": 0.9
        },
        //FaceOriginChar means the image you see in the left of the Skill button, in the Collection ,at the bottom of your screen during the game and not in the battle ,in the little rectangle when you choose new team members.
        "_Image_FaceOriginChar_Path": "Here is ImagePath",
        "_gdeType_Image_FaceOriginChar_Path": "String",
        "_gdeType_Image_FaceOriginChar_offsetMax": "Vector2",
        "_Image_FaceOriginChar_offsetMax": {
          "x": 375,
          "y": 241
        },
        "_gdeType_Image_FaceOriginChar_offsetMin": "Vector2",
        "_Image_FaceOriginChar_offsetMin": {
          "x": -375,
          "y": -1179
        },
        "_gdeType_Image_FaceOriginChar_pivot": "Vector2",
        "_Image_FaceOriginChar_pivot": {
          "x": 0.5,
          "y": 0.85
        },
            ......
        //Something irrelevant to ModtheArk
        //In order to make sure the game won't be destroyed when your charactor is controlled by the 
boss PharosLeader. Please ensure you have "Text_PharosLeader" like this(A 3 string list. The string can be either empty or not):
        "Text_PharosLeader": [
          "",
          "",
          ""
        ]
  }
}	
```
#### Event
```json
{		
    "SomeRandomEvent": {
            .....
       "_gdeType_moded": "Bool",
        "Moded": "true",
        "_MainImage_Moded_Path": "Here is ImagePath",
        "_gdeType_MainImage_Moded_Path": "String",
        "_MainImage_2Stage_Moded_Path": "Here is ImagePath",
        "_gdeType_MainImage_2Stage_Moded_Path": "String",
        "_MainImage_3Stage_Moded_Path": "Here is ImagePath",
        "_gdeType_MainImage_3Stage_Moded_Path": "String"
            ......

  }
}	
```
#### Item
```json
{		
    "SomeItem": {
            .....
        "Moded": true,
        "_gdeType_Moded": "Bool",
        //Item_Active,Item_Consume,Item_Equip,Item _Misc,Item_Passive,Item_Potions(No Scrolls)
        "_image_Moded_Path": "Here is ImagePath",
        "_gdeType_image_Moded_Path": "String"
            ......

  }
}	
```
#### SkillExtended
```json
{		
    "SomeSkillExtended": {
            .....
        "Moded": true,
        "_gdeType_Moded": "Bool",
        "_gdeType_IsIcon_Moded_Path": "String",
        "_IsIcon_Moded_Path": "Here is ImagePath",
            ......

  }
}	
```


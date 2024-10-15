using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public static string BgName {
        get {
            if (PlayerPrefs.HasKey("RiddleCurrentBgName")) {
                return PlayerPrefs.GetString("RiddleCurrentBgName");
            }

            return "1 6.5";
        }
        set {
            PlayerPrefs.SetString("RiddleCurrentBgName", value);
        }
    }

    public static int PlayerFirstEnter {
        get {
            if (PlayerPrefs.HasKey("RiddlePlayerIsFirstEntered")) {
                return  PlayerPrefs.GetInt("RiddlePlayerIsFirstEntered");
            }
            return 0;
        }
        set {
            PlayerPrefs.SetInt("RiddlePlayerIsFirstEntered",value);
        }
    }

    public static int LastLaunchedGameIndex {
        get {
            if (PlayerPrefs.HasKey("RiddlePlayerLastLaunchedGameIndexSaveKey")) {
                return PlayerPrefs.GetInt("RiddlePlayerLastLaunchedGameIndexSaveKey");
            }
            return 0;
        }
        set {
            PlayerPrefs.SetInt("RiddlePlayerLastLaunchedGameIndexSaveKey",value);
        }
    }
    
    public static int PlayerCurrentPointsCount {
        get {
            if (PlayerPrefs.HasKey("RiddlePlayerCurrentPointsCountSaveKey")) {
                return PlayerPrefs.GetInt("RiddlePlayerCurrentPointsCountSaveKey");
            }
            return 0;
        }
        set {
            PlayerPrefs.SetInt("RiddlePlayerCurrentPointsCountSaveKey",value);
        }
    }

    public static float MusicVolume {
        get {
            if (PlayerPrefs.HasKey("RiddleMusicVolumeSaveKey")) {
                return PlayerPrefs.GetFloat("RiddleMusicVolumeSaveKey");
            }
            return 1;
        }
        set {
            PlayerPrefs.SetFloat("RiddleMusicVolumeSaveKey",value);
        }
    }

    public static float SfxVolume {
        get {
            if (PlayerPrefs.HasKey("RiddleSfxVolumeSaveKey")) {
                return PlayerPrefs.GetFloat("RiddleSfxVolumeSaveKey");
            }
            return 1;
        }
        set {
            PlayerPrefs.SetFloat("RiddleSfxVolumeSaveKey",value);
        }
    }
}

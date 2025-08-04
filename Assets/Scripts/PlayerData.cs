using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class PlayerData {
    public enum Currency {
        coin,
        diamond
    }

    public string name;
    public List<string> skinUnlockedID = new List<string>();
    public int currentSkinID;
    public int coins;
    public int diamonds;
    public int highScore;
    public int totaltimePlayed;

    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
}
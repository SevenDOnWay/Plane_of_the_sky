using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "ScriptableObjects/Skin", order = 1)]
public class Skin_ScriptableObject : ScriptableObject {
    public string skinID;
    public Sprite icon; //UI 
    public Sprite backGround;
    public GameObject skinPrefab; // 2D model
    public List ObstacleID;
    public int price;
    public bool unlockedByDefault;
}

using System;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour { 

    public static PlayerDataManager Instance { get; private set; }

    public PlayerData playerData;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() {
        playerData = SecureSaveSystem.Load();

        StateManager.Instance.OnGameEnded += Save;
    }

    private void Load() {
        playerData = SecureSaveSystem.Load();

        Debug.Log(playerData);
    }

    public void Save() {
        SecureSaveSystem.Save(playerData);
        Debug.Log("Player data saved successfully. on path " + Path.Combine(Application.persistentDataPath, "save.dat"));
    }

}

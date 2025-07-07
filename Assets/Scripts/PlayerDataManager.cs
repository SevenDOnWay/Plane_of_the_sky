using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour {

    public static PlayerDataManager Instance { get; private set; }

    public PlayerData playerData;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        Load();
    }

    private void OnEnable() {
        playerData = SecureSaveSystem.Load();
    }

    private void Load() {
        playerData = SecureSaveSystem.Load();
    }

    private void Save() {
        SecureSaveSystem.Save(playerData);
    }
}

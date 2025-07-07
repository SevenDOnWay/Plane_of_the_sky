using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class SecureSaveSystem {

    private const string Password = "S@m3-P07-C@rn_@@#M1n65-%TiD^4";
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("16Mkw3#$871b"); // 8-16 bytes is good

    private const string SaveFileName = "save.dat";


    private static byte[] GenerateKey(string password, byte[] salt, int keySize = 32) {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000); // Iteration count: 100,000
        return pbkdf2.GetBytes(keySize);
    }

    public static void Save(PlayerData data) {
        string json = JsonUtility.ToJson(data);
        byte[] plainBytes = Encoding.UTF8.GetBytes(json);
        byte[] key = GenerateKey(Password, Salt);

        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV(); // Random IV

        byte[] encrypted;

        using (MemoryStream ms = new MemoryStream()) {
            // First write IV (16 bytes)
            ms.Write(aes.IV, 0, aes.IV.Length);

            // Then write encrypted content
            using System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(plainBytes, 0, plainBytes.Length);
            cs.FlushFinalBlock();

            encrypted = ms.ToArray();
        }

        File.WriteAllBytes(GetFullPath(), encrypted);
        Debug.Log("Encrypted save written to: " + GetFullPath());
    }

    public static PlayerData Load() {
        try {
            string path = GetFullPath();
            if (!File.Exists(path)) {
                Debug.LogWarning("Save file not found");
                return null;
            }

            byte[] fileBytes = File.ReadAllBytes(path);
            byte[] key = GenerateKey(Password, Salt);

            using Aes aes = Aes.Create();
            aes.Key = key;

            // Read IV from first 16 bytes
            byte[] iv = new byte[16];
            Array.Copy(fileBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream(fileBytes, iv.Length, fileBytes.Length - iv.Length))
            using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (MemoryStream result = new MemoryStream()) {
                cs.CopyTo(result);
                decryptedBytes = result.ToArray();
            }

            string json = Encoding.UTF8.GetString(decryptedBytes);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        catch(Exception ex) {
            Debug.LogError("Failed to load save file: " + ex.Message);
            return null;
        }
    }

    private static string GetFullPath() => Path.Combine(Application.persistentDataPath, SaveFileName);

}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-3)]
public class ASavingManager : Singleton<ASavingManager>
{
    private string _saveFilePath;
    
    [System.Serializable]
    public class AGameData
    {
        public bool IsLastGameOver;
        public bool IsLastGameAWin;
        public int TotalScore;
        public int TotalTurns;
        public int Combos;
        public List<ACardSlot.ACardSlotData> RemainingCards;
    }
    
    public AGameData GameData { get; private set; }
    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        _saveFilePath = Application.persistentDataPath + "/GameData.json";
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += SaveData;
        SceneManager.sceneLoaded += LoadData;
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(_saveFilePath))
        {
            string json = File.ReadAllText(_saveFilePath);
            GameData = JsonUtility.FromJson<AGameData>(json);
            Debug.Log("Loading Data...");
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= SaveData;
        SceneManager.sceneLoaded -= LoadData;
        SaveData();
    }

    private void SaveData()
    {
        if(!AGameManager.Instance) return;
        if(!AScoringSystem.Instance) return;
        GameData = new AGameData
        {
            IsLastGameOver = AGameManager.Instance.IsGameOver,
            IsLastGameAWin = AGameManager.Instance.IsWin,
            RemainingCards = AGameManager.Instance.CardGrid.GetRemainingCards(),
            TotalScore = AScoringSystem.Instance.Score,
            TotalTurns = AScoringSystem.Instance.TurnsCounter,
            Combos = AScoringSystem.Instance.ComboCounter
        };
        
        string json = JsonUtility.ToJson(GameData);
        
        File.WriteAllText(_saveFilePath, json);
        
        Debug.Log("Saving Data...");
        Debug.Log(Application.persistentDataPath);
    }

    private void LoadData(Scene scene, LoadSceneMode loadMode)
    {
        LoadData();
    }

    private void SaveData(Scene scene)
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}

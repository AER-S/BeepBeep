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
        public float RemainingTime;
        public int TotalScore;
        public int TotalTurns;
        public int Combos;
        public List<ACardSlot.ACardSlotData> RemainingCards;
        public ACardsGrid.ACardsGridData CardsGridData;
        public AMainMenuController.AGameMode GameMode;
    }
    
    public AGameData GameData { get; private set; }
    new void Awake()
    {
        
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        _saveFilePath = Application.persistentDataPath + "/GameData.json";
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
        if(this == Instance)SaveData();
    }

    private void SaveData()
    {
        if (GameData == null) GameData = new AGameData();


        if (AGameManager.Instance)
        {
            GameData.IsLastGameOver = AGameManager.Instance.IsGameOver;
            GameData.IsLastGameAWin = AGameManager.Instance.IsWin;
            GameData.RemainingCards = AGameManager.Instance.CardGrid.GetRemainingCards();
            GameData.CardsGridData = AGameManager.Instance.CardGrid.CardsGridData;
            GameData.RemainingTime = AGameManager.Instance.RemainingTime;
        }

        if (AScoringSystem.Instance)
        {
            GameData.TotalScore = AScoringSystem.Instance.Score;
            GameData.TotalTurns = AScoringSystem.Instance.TurnsCounter;
            GameData.Combos = AScoringSystem.Instance.ComboCounter;
        }
        
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
        if(this == Instance) SaveData();
    }
}

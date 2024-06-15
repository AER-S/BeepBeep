
using System.Collections.Generic;
using System.IO;
using UnityEngine;


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
    }

    public void LoadData()
    {
        if (File.Exists(_saveFilePath))
        {
            string json = File.ReadAllText(_saveFilePath);
            GameData = JsonUtility.FromJson<AGameData>(json);
            Debug.Log("Loading Data...");
        }

        GameData ??= new AGameData
        {
            IsLastGameOver = true,
            IsLastGameAWin = true,
            CardsGridData = new ACardsGrid.ACardsGridData()
        };
    }

    private void OnDisable()
    {
        if(this == Instance)SaveData();
    }

    public void SaveData()
    {
        if (AGameManager.Instance)
        {
            GameData.IsLastGameOver = AGameManager.Instance.IsGameOver;
            GameData.IsLastGameAWin = AGameManager.Instance.IsWin;
            GameData.RemainingCards = AGameManager.Instance.CardGrid.GetRemainingCards();
            GameData.CardsGridData = AGameManager.Instance.CardGrid.CardsGridData;
            GameData.RemainingTime = AGameManager.Instance.RemainingTime;
            Debug.Log("Saving Game Manager....");
        }

        if (AScoringSystem.Instance)
        {
            GameData.TotalScore = AScoringSystem.Instance.Score;
            GameData.TotalTurns = AScoringSystem.Instance.TurnsCounter;
            GameData.Combos = AScoringSystem.Instance.ComboCounter;
            Debug.Log("SavingScore");
        }
        
        string json = JsonUtility.ToJson(GameData);
        
        File.WriteAllText(_saveFilePath, json);
        
        Debug.Log("Saving Data...");
        Debug.Log(Application.persistentDataPath);
    }

    private void OnApplicationQuit()
    {
        if(this == Instance) SaveData();
    }
}

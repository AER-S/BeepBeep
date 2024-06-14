using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASavingManager : Singleton<ASavingManager>
{
    private string _saveFilePath;
    public class AGameData
    {
        public bool IsLastGameAWin;
        public int TotalScore;
        public int TotalTurns;
        public int Combos;
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
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= SaveData;
        SceneManager.sceneLoaded -= LoadData;
    }

    private void LoadData(Scene scene, LoadSceneMode loadMode)
    {
        if (File.Exists(_saveFilePath))
        {
            string json = File.ReadAllText(_saveFilePath);
            GameData = JsonUtility.FromJson<AGameData>(json);
            Debug.Log("Loading Data...");
        }
    }

    private void SaveData(Scene scene)
    {
        if(!AGameManager.Instance) return;
        if(!AScoringSystem.Instance) return;
        GameData = new AGameData
        {
            IsLastGameAWin = AGameManager.Instance.IsWin,
            TotalScore = AScoringSystem.Instance.Score,
            TotalTurns = AScoringSystem.Instance.TurnsCounter,
            Combos = AScoringSystem.Instance.ComboCounter
        };
        
        string json = JsonUtility.ToJson(GameData);
        
        File.WriteAllText(_saveFilePath, json);
        
        Debug.Log("Saving Data...");
    }
    
}

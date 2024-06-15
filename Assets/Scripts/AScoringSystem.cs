using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-1)]
public class AScoringSystem : Singleton<AScoringSystem>
{
    [SerializeField] private int ComboFactor;

    public int TurnsCounter => _turnsCounter;
    public int Score => _score;
    public int ComboCounter => _combosCounter;

    public Action ScoreUpdated;

    private int _turnsCounter;
    private int _score;
    private int _combosCounter;

    new void Awake()
    {
        base.Awake();
        _turnsCounter = 0;
        _score = 0;
        _combosCounter = 0;
    }

    private void OnEnable()
    {
        LoadData();
        AGameManager.Instance.MatchingSuccess += ProcessMatchingSuccess;
        AGameManager.Instance.MatchingFailed += ProcessMatchingFail;
    }



    private void OnDisable()
    {
        AGameManager.Instance.MatchingSuccess -= ProcessMatchingSuccess;
        AGameManager.Instance.MatchingFailed -= ProcessMatchingFail;
    }

    private void ProcessMatchingSuccess()
    {
        _turnsCounter++;
        _combosCounter++;
        _score += _combosCounter * ComboFactor;
        ScoreUpdated?.Invoke();
    }
    
    private void ProcessMatchingFail()
    {
        _turnsCounter++;
        _combosCounter = 0;
        ScoreUpdated?.Invoke();
    }
    
    private void LoadData()
    {
        if (ASavingManager.Instance.GameData.IsLastGameAWin || ASavingManager.Instance.GameData.GameMode == AMainMenuController.AGameMode.Continue)
        {
            _turnsCounter = ASavingManager.Instance.GameData.TotalTurns;
            _score = ASavingManager.Instance.GameData.TotalScore;
            _combosCounter = ASavingManager.Instance.GameData.Combos;
        }
    }
    
}

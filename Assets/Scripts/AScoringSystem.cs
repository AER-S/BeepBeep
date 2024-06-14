using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-1)]
public class AScoringSystem : Singleton<AScoringSystem>
{
    [SerializeField] private int ComboFactor;
    
    public int TurnsCounter { get; private set; }

    public int Score { get; private set; }

    public int ComboCounter { get; private set; }

    public Action ScoreUpdated;

    new void Awake()
    {
        base.Awake();
        TurnsCounter = 0;
        Score = 0;
        ComboCounter = 0;
    }

    private void OnEnable()
    {
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
        TurnsCounter++;
        ComboCounter++;
        Score += ComboCounter * ComboFactor;
        ScoreUpdated?.Invoke();
    }
    
    private void ProcessMatchingFail()
    {
        TurnsCounter++;
        ComboCounter = 0;
        ScoreUpdated?.Invoke();
    }
    
}

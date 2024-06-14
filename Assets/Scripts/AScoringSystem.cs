using System;
using Unity.Collections;
using UnityEngine;

public class AScoringSystem : Singleton<AScoringSystem>
{
    [SerializeField] private int ComboFactor;
    
    [SerializeField] private int _turnsCounter;

    [SerializeField] private int _score;

    [SerializeField] private int _comboCounter;
    
    private void Awake()
    {
        base.Awake();
        _turnsCounter = 0;
        _score = 0;
        _comboCounter = 0;
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
        _turnsCounter++;
        _comboCounter++;
        _score += _comboCounter * ComboFactor;
    }
    
    private void ProcessMatchingFail()
    {
        _turnsCounter++;
        _comboCounter = 0;
    }
    
}

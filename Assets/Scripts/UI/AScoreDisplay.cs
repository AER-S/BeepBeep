using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;

public class AScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text TurnsValueDisplay;
    [SerializeField] private TMP_Text ScoreValueDisplay;
    [SerializeField] private TMP_Text CombosValueDisplay;
    [SerializeField] private TMP_Text TimerValueDisplay;

    
    private void OnEnable()
    {
        AScoringSystem.Instance.ScoreUpdated += UpdateDisplay;
    }

    private void OnDisable()
    {
        AScoringSystem.Instance.ScoreUpdated -= UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        TurnsValueDisplay.text = AScoringSystem.Instance.TurnsCounter.ToString("0000");
        ScoreValueDisplay.text = AScoringSystem.Instance.Score.ToString("0000");
        CombosValueDisplay.text = AScoringSystem.Instance.ComboCounter.ToString("0000");
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void Update()
    {
        TimerValueDisplay.text = AGameManager.Instance.RemainingTime.ToString("00:00");
    }
}

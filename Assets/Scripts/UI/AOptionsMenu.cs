using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AOptionsMenu : Singleton<AOptionsMenu>
{
    [SerializeField] private Slider RowsSlider;

    [SerializeField] private Slider ColumnsSlider;

    [SerializeField] private Slider Variations;

    [SerializeField] private TMP_Text RowsValue;
    [SerializeField] private TMP_Text ColumnsValue;
    [SerializeField] private TMP_Text VariationsValue;

    [SerializeField] private Button SaveButton;
    [SerializeField] private Button BackButton;
    

    


    private void OnEnable()
    {
        SaveButton.onClick.AddListener(SaveOptions);
        BackButton.onClick.AddListener(BackToMainMenu);
        
        RowsSlider.onValueChanged.AddListener(OnRowsSliderChanged);
        ColumnsSlider.onValueChanged.AddListener(OnColumnsSliderChanged);
        ColumnsSlider.wholeNumbers = false;
        
        SetupVariations();
        Variations.onValueChanged.AddListener(OnVariationsSliderChanged);

        LoadData();
    }

    private void LoadData()
    {
        if (ASavingManager.Instance.GameData.CardsGridData!=null)
        {
            RowsSlider.value = ASavingManager.Instance.GameData.CardsGridData.Rows;
            ColumnsSlider.value = ASavingManager.Instance.GameData.CardsGridData.Columns;
            Variations.value = ASavingManager.Instance.GameData.CardsGridData.Columns;
        }
    }

    private void OnDisable()
    {
        SaveButton.onClick.RemoveListener(SaveOptions);
        BackButton.onClick.RemoveListener(BackToMainMenu);
        RowsSlider.onValueChanged.RemoveListener(OnRowsSliderChanged);
        ColumnsSlider.onValueChanged.RemoveListener(OnColumnsSliderChanged);
        Variations.onValueChanged.RemoveListener(OnVariationsSliderChanged);
    }

    private void BackToMainMenu()
    {
        AMainMenuController.Instance.CloseOptionsMenu();
    }

    private void SaveOptions()
    {
        if (Math.Abs(ASavingManager.Instance.GameData.CardsGridData.Columns - ColumnsSlider.value) > 0.1f ||
            Math.Abs(ASavingManager.Instance.GameData.CardsGridData.Rows - RowsSlider.value) > 0.1f ||
            Math.Abs(ASavingManager.Instance.GameData.CardsGridData.Variations - Variations.value) > 0.1f)
        {
            ASavingManager.Instance.GameData.CardsGridData.Rows = (int)RowsSlider.value;
            ASavingManager.Instance.GameData.CardsGridData.Columns = (int)ColumnsSlider.value;
            ASavingManager.Instance.GameData.CardsGridData.Variations = (int)Variations.value;
            ASavingManager.Instance.GameData.IsLastGameOver = true;
            ASavingManager.Instance.GameData.IsLastGameAWin = false;
        }
        BackToMainMenu();
    }

    private void OnRowsSliderChanged(float value)
    {
        RowsValue.text = RowsSlider.value.ToString("00");
        SetupVariations();
    }

    private void OnVariationsSliderChanged(float value)
    {
        VariationsValue.text = Variations.value.ToString("00");
    }

    private void SetupVariations()
    {
        Variations.maxValue = RowsSlider.value * ColumnsSlider.value / 2;
        
    }

    private void OnColumnsSliderChanged(float value)
    {
        ColumnsSlider.value = Mathf.Round(value / 2) * 2;
        ColumnsValue.text = ColumnsSlider.value.ToString("00");
        SetupVariations();
    }

    private void UpdateDisplay()
    {
        VariationsValue.text = Variations.value.ToString("00");
        RowsValue.text = RowsSlider.value.ToString("00");
        ColumnsValue.text = ColumnsSlider.value.ToString("00");
    }
    
}

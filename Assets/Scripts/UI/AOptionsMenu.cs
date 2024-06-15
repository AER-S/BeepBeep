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
    

    [SerializeField] private VisualProvider VisualProvider;


    private void OnEnable()
    {
        RowsSlider.onValueChanged.AddListener(OnRowsSliderChanged);
        ColumnsSlider.onValueChanged.AddListener(OnColumnsSliderChanged);
        ColumnsSlider.wholeNumbers = false;
        
        SetupVariations();
        Variations.onValueChanged.AddListener(OnVariationsSliderChanged);
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


    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

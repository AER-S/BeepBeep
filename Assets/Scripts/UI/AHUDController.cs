using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AHUDController : Singleton<AHUDController>
{
    [SerializeField] private Button HomeButton;

    [SerializeField] private Button QuitButton;


    public void GoHome()
    {
        SceneManager.LoadScene(0);
    }
    

    private void OnEnable()
    {
        HomeButton.onClick.AddListener(GoHome);
        QuitButton.onClick.AddListener(Application.Quit);
    }

    private void OnDisable()
    {
        HomeButton.onClick.RemoveListener(GoHome);
        QuitButton.onClick.RemoveListener(Application.Quit);
    }
    
}

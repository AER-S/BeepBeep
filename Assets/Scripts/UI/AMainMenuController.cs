using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AMainMenuController : Singleton<AMainMenuController>
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;
    
    [Serializable]
    public enum AGameMode
    {
        None,
        Continue,
        NewGame
    }
    
    private void OnEnable()
    {
        if(ASavingManager.Instance.GameData.IsLastGameOver) ContinueButton.gameObject.SetActive(false);
        else ContinueButton.onClick.AddListener(ContinueGame);
        NewGameButton.onClick.AddListener(NewGame);
        OptionsButton.onClick.AddListener(OpenOptionsMenu);
        QuitButton.onClick.AddListener(Application.Quit);
    }

    private void OnDisable()
    {
        ContinueButton.onClick.RemoveListener(ContinueGame);
        NewGameButton.onClick.RemoveListener(NewGame);
        OptionsButton.onClick.RemoveListener(OpenOptionsMenu);
        QuitButton.onClick.RemoveListener(Application.Quit);
    }

    private void OpenOptionsMenu()
    {
        throw new NotImplementedException();
    }

    private void NewGame()
    {
        ASavingManager.Instance.GameData.GameMode = AGameMode.NewGame;
        SceneManager.LoadScene(1);
    }

    private void ContinueGame()
    {
        ASavingManager.Instance.GameData.GameMode = AGameMode.Continue;
        SceneManager.LoadScene(1);
    }

    
}

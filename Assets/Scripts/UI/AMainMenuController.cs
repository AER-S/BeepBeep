using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AMainMenuController : Singleton<AMainMenuController>
{
    [SerializeField] private GameObject OptionsMenuPanel;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;
    
    [Serializable]
    public enum AGameMode
    {
        None,
        Continue,
        NewGame,
        Restart,
        WinStrike
    }
    
    private void OnEnable()
    {
        ASavingManager.Instance.LoadData();
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
        ASavingManager.Instance.SaveData();
    }

    private void Start()
    {
        ShowHideContinueButton();
    }

    private void OpenOptionsMenu()
    {
        OptionsMenuPanel.SetActive(true);
    }

    private void ShowHideContinueButton()
    {
        if(ASavingManager.Instance.GameData.IsLastGameOver && ASavingManager.Instance.GameData.GameMode != AGameMode.WinStrike) ContinueButton.gameObject.SetActive(false);
        else ContinueButton.onClick.AddListener(ContinueGame);
    }

    private void NewGame()
    {
        ASavingManager.Instance.GameData.GameMode = AGameMode.NewGame;
        SceneManager.LoadScene(1);
    }

    private void ContinueGame()
    {
        if(ASavingManager.Instance.GameData.GameMode != AGameMode.WinStrike || !ASavingManager.Instance.GameData.IsLastGameOver)ASavingManager.Instance.GameData.GameMode = AGameMode.Continue;
        Debug.Log("Game Mode "+Enum.GetName(typeof(AGameMode),ASavingManager.Instance.GameData.GameMode));
        SceneManager.LoadScene(1);
    }


    public void CloseOptionsMenu()
    {
        ShowHideContinueButton();
        OptionsMenuPanel.SetActive(false);
    }
}

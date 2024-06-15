
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AEndGamePanelController : MonoBehaviour
{
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button HomeButton;
    [SerializeField] private Button QuitButton;


    private void OnEnable()
    {
        AGameManager.Instance.GameOver += ProcessEndGame;
        QuitButton.onClick.AddListener(Application.Quit);
        RestartButton.onClick.AddListener(Restart);
        ContinueButton.onClick.AddListener(Continue);
        HomeButton.onClick.AddListener(GoHome);
    }
    

    private void OnDisable()
    {
        AGameManager.Instance.GameOver -= ProcessEndGame;
        QuitButton.onClick.RemoveListener(Application.Quit);
        RestartButton.onClick.RemoveListener(Restart);
        ContinueButton.onClick.RemoveListener(Continue);
        HomeButton.onClick.AddListener(GoHome);
    }

    
    private void ProcessEndGame(bool gameWon)
    {
        if(gameWon)OnGameWon();
        else OnGameLost();
        GameOverPanel.SetActive(true);
    }

    private void OnGameWon()
    {
        Title.text = "GAME WON !!!";
        Title.color = Color.green;
    }

    private void OnGameLost()
    {
        Title.text = "GAME LOST !!!";
        Title.color = Color.red;
        ContinueButton.gameObject.SetActive(false);
    }
    
    private void GoHome()
    {
        if (AGameManager.Instance.IsWin)
            ASavingManager.Instance.GameData.GameMode = AMainMenuController.AGameMode.WinStrike;
        else ASavingManager.Instance.GameData.GameMode = AMainMenuController.AGameMode.NewGame;
        AHUDController.Instance.GoHome();
    }

    private void Restart()
    {
        ASavingManager.Instance.GameData.GameMode = AMainMenuController.AGameMode.Restart;
        ASavingManager.Instance.GameData.IsLastGameAWin = false;
        SceneManager.LoadScene(1);
    }

    private void Continue()
    {
        ASavingManager.Instance.GameData.GameMode = AMainMenuController.AGameMode.WinStrike;
        SceneManager.LoadScene(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

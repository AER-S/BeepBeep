using System;
using System.Collections;
using System.Collections.Generic;
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
        QuitButton.onClick.AddListener(QuitGame);
        RestartButton.onClick.AddListener(Restart);
    }


    private void OnDisable()
    {
        AGameManager.Instance.GameOver -= ProcessEndGame;
        QuitButton.onClick.RemoveListener(QuitGame);
        RestartButton.onClick.RemoveListener(Restart);
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

    private void QuitGame()
    {
        Application.Quit();
    }

    private void Restart()
    {
        var level = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level);
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

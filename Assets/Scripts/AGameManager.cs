using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-2)]
public class AGameManager : Singleton<AGameManager>
{
    [SerializeField] private ACardsGrid CardsGrid;
    [SerializeField] private float CardsShowingTime;
    [SerializeField] private float LevelTime;

    public float RemainingTime => _levelTime;

    public bool IsGameOver { get; private set; }
    public bool IsWin { get; private set; }

    

    public ACardsGrid CardGrid => CardsGrid;

    private Queue<CardsCouple> _cardsCouples;

    private CardsCouple CurrentCouple;

    public Action MatchingSuccess;
    public Action MatchingFailed;
    public Action<bool> GameOver;

    [SerializeField]private int _unmatchedCards;
    private float _levelTime;
    

    public struct CardsCouple
    {
        public ACardSlot CardSlotA;
        public ACardSlot CardSlotB;
    }

    public void TakeSlot(ACardSlot newSlot)
    {
        if (CurrentCouple.CardSlotA == null)
        {
            CurrentCouple.CardSlotA = newSlot;
            return;
        }
        else
        {
            CurrentCouple.CardSlotB = newSlot;
            _cardsCouples.Enqueue(CurrentCouple);
            CurrentCouple = new CardsCouple();
            CurrentCouple.CardSlotA = null;
            CurrentCouple.CardSlotB = null;
        }
    }

    

    // Start is called before the first frame update
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0) return;
        CardsGrid.Populate();
        CurrentCouple.CardSlotA = null;
        CurrentCouple.CardSlotB = null;
        _cardsCouples = new Queue<CardsCouple>();
        IsGameOver = false;
        IsWin = false;
        _levelTime = (!ASavingManager.Instance.GameData.IsLastGameOver)? ASavingManager.Instance.GameData.RemainingTime:LevelTime;
        Debug.Log("Game Start...");
        StartCoroutine(ShowHideCards());
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex!=0) return;
        if (_cardsCouples.Count > 0)
        {
            var cardsCouple = _cardsCouples.Dequeue();
            ProcessResult(cardsCouple);
        }

        if (CardsGrid.UnmatchedCards <= 0)
        {
            GameOver?.Invoke(true);
            IsGameOver = true;
            IsWin = true;
            return;
        }
        
        _levelTime = Mathf.Max(_levelTime-Time.deltaTime,0);
        if (_levelTime <= 0 && CardsGrid.UnmatchedCards > 0 && !IsGameOver)
        {
            GameOver?.Invoke(false);
            IsGameOver = true;
            IsWin = false;
        }
        
        
        
        
    }

    IEnumerator FlipCardsCouple(CardsCouple cardCouple)
    {
        MatchingFailed?.Invoke();
        yield return new WaitForSecondsRealtime(1);
        cardCouple.CardSlotA.Card.Flip();
        cardCouple.CardSlotB.Card.Flip();

    }

    IEnumerator DestroyCards(CardsCouple cardsCouple)
    {
        MatchingSuccess?.Invoke();
        yield return new WaitForSecondsRealtime(1);
        cardsCouple.CardSlotA.ClearSlot();
        cardsCouple.CardSlotB.ClearSlot();
    }

    private void ProcessResult(CardsCouple cardsCouple)
    {
        StartCoroutine(CheckMatchingCards(cardsCouple) ? "DestroyCards" : "FlipCardsCouple", cardsCouple);
    }

    private bool CheckMatchingCards(CardsCouple cardsCouple)
    {
        return (cardsCouple.CardSlotA.Card.Value == cardsCouple.CardSlotB.Card.Value);
    }

    IEnumerator ShowHideCards()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        CardsGrid.FlipAllCards();
        yield return new WaitForSecondsRealtime(CardsShowingTime);
        CardsGrid.FlipAllCards();
    }
    

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-2)]
public class AGameManager : Singleton<AGameManager>
{
    #region SerializeField
        [field:SerializeField] public ACardsGrid CardGrid { get; private set; }
        [SerializeField] private float CardsShowingTime;
        [SerializeField] private float LevelTime;
    
    #endregion

    #region Public Actions
    
        public Action MatchingSuccess;
        public Action MatchingFailed;
        public Action<bool> GameOver;
    
    #endregion

    #region Public Getters

        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }
        public float RemainingTime => _levelTime;

    #endregion

    #region Private members

        private Queue<CardsCouple> _cardsCouples;
        private CardsCouple _currentCouple;
        private int _unmatchedCards;
        private float _levelTime;

    #endregion

    private struct CardsCouple
    {
        public ACardSlot CardSlotA;
        public ACardSlot CardSlotB;
    }

    #region Unity Events

    private void OnEnable()
    {
        ASavingManager.Instance.LoadData();
    }

    private void OnDisable()
    {
        ASavingManager.Instance.SaveData();
    }
    
    private void Start()
    {
        CardGrid.Populate();
        _currentCouple.CardSlotA = null;
        _currentCouple.CardSlotB = null;
        _cardsCouples = new Queue<CardsCouple>();
        IsGameOver = false;
        IsWin = false;
        _levelTime = (ASavingManager.Instance.GameData.GameMode == AMainMenuController.AGameMode.Continue)
            ? ASavingManager.Instance.GameData.RemainingTime
            : LevelTime;
        Debug.Log("Game Start...");
        StartCoroutine(ShowHideCards());
    }
    
    void Update()
    {
        if (IsGameOver) return;

        _levelTime = Mathf.Max(_levelTime - Time.deltaTime, 0);


        if (_cardsCouples.Count > 0)
        {
            var cardsCouple = _cardsCouples.Dequeue();
            ProcessResult(cardsCouple);
        }

        if (CardGrid.UnmatchedCards <= 0)
        {
            GameOver?.Invoke(true);
            IsGameOver = true;
            IsWin = true;
            return;
        }


        if (_levelTime <= 0 && CardGrid.UnmatchedCards > 0)
        {
            GameOver?.Invoke(false);
            IsGameOver = true;
            IsWin = false;
        }
    }

    #endregion
    
    public void TakeSlot(ACardSlot newSlot)
    {
        if (_currentCouple.CardSlotA == null)
        {
            _currentCouple.CardSlotA = newSlot;
        }
        else
        {
            _currentCouple.CardSlotB = newSlot;
            _cardsCouples.Enqueue(_currentCouple);
            _currentCouple = new CardsCouple
            {
                CardSlotA = null,
                CardSlotB = null
            };
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
        CardGrid.FlipAllCards();
        yield return new WaitForSecondsRealtime(CardsShowingTime);
        CardGrid.FlipAllCards();
    }
    
}

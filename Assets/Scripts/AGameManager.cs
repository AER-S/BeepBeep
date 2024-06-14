using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class AGameManager : Singleton<AGameManager>
{
    [SerializeField] private ACardsGrid CardsGrid;
    [SerializeField] private float CardsShowingTime;
    [field:SerializeField] public float LevelTime { get; private set; }

    private Queue<CardsCouple> _cardsCouples;

    private CardsCouple CurrentCouple;

    public Action MatchingSuccess;
    public Action MatchingFailed;
    public Action<bool> GameOver;

    private int _unmatchedCards;

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
    void Start()
    {
        CurrentCouple.CardSlotA = null;
        CurrentCouple.CardSlotB = null;
        _cardsCouples = new Queue<CardsCouple>();
        _unmatchedCards = CardsGrid.Columns * CardsGrid.Rows;
        StartCoroutine(ShowHideCards());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_cardsCouples.Count > 0)
        {
            var cardsCouple = _cardsCouples.Dequeue();
            ProcessResult(cardsCouple);
        }

        if (_unmatchedCards <= 0)
        {
            GameOver?.Invoke(true);
            return;
        }
        
        LevelTime = Mathf.Max(LevelTime-Time.deltaTime,0);
        if(LevelTime<=0 && _unmatchedCards>0) GameOver?.Invoke(false);
        
        
        
        
    }

    IEnumerator FlipCardsCouple(CardsCouple cardCouple)
    {
        MatchingFailed?.Invoke();
        yield return new WaitForSecondsRealtime(1);
        cardCouple.CardSlotA.Card.Flip();
        cardCouple.CardSlotB.Card.Flip();
        yield return new WaitForSecondsRealtime(1);
        
    }

    IEnumerator DestroyCards(CardsCouple cardsCouple)
    {
        MatchingSuccess?.Invoke();
        yield return new WaitForSecondsRealtime(1);
        cardsCouple.CardSlotA.ClearSlot();
        cardsCouple.CardSlotB.ClearSlot();
        yield return new WaitForSecondsRealtime(1);
    }

    private void ProcessResult(CardsCouple cardsCouple)
    {
        if (CheckMatchingCards(cardsCouple)) _unmatchedCards -= 2;
        StartCoroutine(CheckMatchingCards(cardsCouple) ? "DestroyCards" : "FlipCardsCouple", cardsCouple);
    }

    private bool CheckMatchingCards(CardsCouple cardsCouple)
    {
        return (cardsCouple.CardSlotA.Card.Value == cardsCouple.CardSlotB.Card.Value);
    }

    IEnumerator ShowHideCards()
    {
        CardsGrid.FlipAllCards();
        yield return new WaitForSecondsRealtime(CardsShowingTime);
        CardsGrid.FlipAllCards();
    }

}

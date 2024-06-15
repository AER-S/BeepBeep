using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ACardsGrid : MonoBehaviour
{
    [Serializable]
    public class ACardsGridData
    {
        public int Width;
        public int Height;
        public int Rows;
        public int Columns;
        public int Variations;
    }

    [SerializeField] private ACardsGridData GridData;
    [SerializeField] private ACard CardPrefab;
    [SerializeField] private ACardSlot CardSlotPrefab;

    [SerializeField]private VisualProvider VisualProvider;

    public ACardsGridData CardsGridData => GridData;
    public int UnmatchedCards => _unmatchedCards;
    


    private ACardSlot[] _cardSlots;
    private Vector3 _scale;
    private int _unmatchedCards;


    private List<ACardSlot.ACardSlotData> _savedSlots;
    
    

    private void OnEnable()
    {
        AGameManager.Instance.MatchingSuccess += OnMatchingSuccess;
        if(ASavingManager.Instance.GameData == null) return;
        if (!ASavingManager.Instance.GameData.IsLastGameOver)
        {
            _savedSlots = ASavingManager.Instance.GameData.RemainingCards;
        }

        if(ASavingManager.Instance.GameData.CardsGridData!=null && ASavingManager.Instance.GameData.CardsGridData.Rows!=0) GridData = ASavingManager.Instance.GameData.CardsGridData;
    }

    private void OnDisable()
    {
        AGameManager.Instance.MatchingSuccess += OnMatchingSuccess;
    }

    private void OnMatchingSuccess()
    {
        _unmatchedCards -= 2;
    }

    public void Populate()
    {
        _unmatchedCards = 0;
        _scale = GetScale();
        var cardsCount = GridData.Rows * GridData.Columns;
        _cardSlots = new ACardSlot[cardsCount];
        if (_savedSlots is { Count: > 0 })
        {
            foreach (var savedSlot in _savedSlots)
            {
                SpawnACard(savedSlot.Index,savedSlot.CardValue);
            }
            
            return;
        }
        var baseWeight = GetWeight();
        var weightsDistribution = GetDistribution(baseWeight);
        for (int i = 0; i < _cardSlots.Length; i++)
        {
            var cardValue = GetCardValueFrom(weightsDistribution);
            SpawnACard(i,cardValue);
        }
        
    }

    private int GetWeight()
    {
        return (GridData.Rows * GridData.Columns) / GridData.Variations;
    }

    private Dictionary<int, int> GetDistribution(int baseWeight)
    {
        var upperWeightCount = (GridData.Rows * GridData.Columns) % GridData.Variations;
        //Debug.Log("UpperWeightCount= "+upperWeightCount);
        Dictionary<int, int> Distributions = new Dictionary<int, int>();
        for (int i = 0; i < GridData.Variations; i++)
        {
            int value = GetRandomValue(Distributions);
            int weight = (i < upperWeightCount) ? (baseWeight + 1) : baseWeight;
            Distributions.Add(value,weight);
            //Debug.Log("value= "+value+ " weight: "+ weight);
        }

        return Distributions;
    }

    private int GetRandomValue(Dictionary<int,int> Distributions)
    {
        int randomValue = 0;
        do
        {
            randomValue = Random.Range(0, VisualProvider.GetTextures().Length);
        } while (Distributions.ContainsKey(randomValue));

        return randomValue;
    }

    private int GetCardValueFrom(Dictionary<int, int> weightsDistributions)
    {

        int value = weightsDistributions.ElementAt(Random.Range(0, weightsDistributions.Count)).Key;
        int weight=0;
        weightsDistributions.TryGetValue(value, out weight);
        --weight;
        if (weight <= 0) weightsDistributions.Remove(value);
        else weightsDistributions[value] = weight;
        return value;
    }

    private void SpawnACard(int position, int cardValue)
    {
        var spawnPosition = GetPosition(position);
        _cardSlots[position] = Instantiate<ACardSlot>(CardSlotPrefab,spawnPosition,Quaternion.identity);
        _cardSlots[position].FillSlot(Instantiate<ACard>(CardPrefab, spawnPosition, Quaternion.identity));
        _cardSlots[position].Card.transform.SetParent(_cardSlots[position].transform);
        _cardSlots[position].Card.Value = cardValue;
        _cardSlots[position].transform.localScale = _scale;
        _unmatchedCards++;
    }

    Vector3 GetPosition(int i)
    {
        
        int rowIndex = i / GridData.Columns;
        int columnIndex =  i % GridData.Columns;
        float rowPosition = (GridData.Height / (float)GridData.Rows) * rowIndex;
        float columnPosition = (GridData.Width /(float) GridData.Columns) * columnIndex;
        var spawnLocation = new Vector3(columnPosition, rowPosition, 0);
        return spawnLocation;
    }

    Vector3 GetScale()
    {
        float WidthScale = (GridData.Width) / ((float)GridData.Columns * 3);
        float HeightScale = (GridData.Height) / ((float)GridData.Rows * 5);
        return new Vector3(WidthScale*0.9f, HeightScale*0.9f, 1);
    }

    public void FlipAllCards()
    {
        foreach (var cardSlot in _cardSlots)
        {
            if(cardSlot != null)cardSlot.Card.Flip();
        }
    }

    public List<ACardSlot.ACardSlotData> GetRemainingCards()
    {
        var remainingCards = new List<ACardSlot.ACardSlotData>();
        for (int i = 0; i < _cardSlots.Length; i++)
        {
            if(_cardSlots[i] ==null || _cardSlots[i].IsEmpty) continue;
            Debug.Log("SavingSlot...");
            var cardData = new ACardSlot.ACardSlotData()
            {
                CardValue = _cardSlots[i].Card.Value,
                Index = i
            };
            remainingCards.Add(cardData);
        }

        return remainingCards;
    }
}

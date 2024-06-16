using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ACardsGrid : MonoBehaviour
{
    [Serializable]
    public class ACardsGridData
    {
        
        public int Rows;
        public int Columns;
        public int Variations;
    }

    #region SerializeField

    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private ACardsGridData GridData;
    [SerializeField] private ACard CardPrefab;
    [SerializeField] private ACardSlot CardSlotPrefab;
    [SerializeField] private VisualProvider VisualProvider;

    #endregion

    public ACardsGridData CardsGridData => GridData;
    public int UnmatchedCards => _unmatchedCards;
    


    private ACardSlot[] _cardSlots;
    private Vector3 _scale;
    private int _unmatchedCards;
    private List<ACardSlot.ACardSlotData> _savedSlots;
    
    

    private void OnEnable()
    {
        AGameManager.Instance.MatchingSuccess += OnMatchingSuccess;
        if(ASavingManager.Instance.GameData.CardsGridData!=null && ASavingManager.Instance.GameData.CardsGridData.Rows!=0) 
            GridData = ASavingManager.Instance.GameData.CardsGridData;
    }

    private void OnDisable()
    {
        AGameManager.Instance.MatchingSuccess -= OnMatchingSuccess;
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

        if (ASavingManager.Instance.GameData.GameMode == AMainMenuController.AGameMode.Continue)
        {
            PopulateWithSavedGrid();
            return;
        }
        
        PopulateWithNewGrid();
        
    }

    private void PopulateWithSavedGrid()
    {
        _savedSlots = ASavingManager.Instance.GameData.RemainingCards;
        
        for (int i = 0; i < _cardSlots.Length; i++)
        {
            if (i == _savedSlots[0].Index)
            {
                SpawnACard(_savedSlots[0].Index, _savedSlots[0].CardValue);
                _savedSlots.Remove(_savedSlots[0]);
                continue;
            }
            
            SpawnSlot(i);
            _cardSlots[i].ClearSlot();
        }
    }

    private void PopulateWithNewGrid()
    {
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
        
        Dictionary<int, int> distributions = new Dictionary<int, int>();
        
        for (int i = 0; i < GridData.Variations; i++)
        {
            int value = GetRandomValue(distributions);
            int weight = (i < upperWeightCount/2) ? (baseWeight + 2) : baseWeight;
            distributions.Add(value,weight);
        }

        return distributions;
    }

    private int GetRandomValue(Dictionary<int,int> distributions)
    {
        int randomValue = 0;
        do
        {
            randomValue = Random.Range(0, VisualProvider.GetTextures().Length);
        } while (distributions.ContainsKey(randomValue));

        return randomValue;
    }

    private int GetCardValueFrom(Dictionary<int, int> weightsDistributions)
    {

        int value = weightsDistributions.ElementAt(Random.Range(0, weightsDistributions.Count)).Key;
        int weight = weightsDistributions[value];
        --weight;
        if (weight <= 0) weightsDistributions.Remove(value);
        else weightsDistributions[value] = weight;
        return value;
    }

    private void SpawnSlot(int position)
    {
        var spawnPosition = GetPosition(position);
        _cardSlots[position] = Instantiate<ACardSlot>(CardSlotPrefab,spawnPosition,Quaternion.identity);
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
        float rowPosition = (Height / (float)GridData.Rows) * rowIndex - Width/2;
        float columnPosition = (Width /(float) GridData.Columns) * columnIndex - Height/2;
        var spawnLocation = new Vector3(columnPosition, rowPosition, 0);
        return spawnLocation;
    }

    Vector3 GetScale()
    {
        float WidthScale = (Width) / ((float)GridData.Columns * 3);
        float HeightScale = (Height) / ((float)GridData.Rows * 5);
        return new Vector3(WidthScale*0.9f, HeightScale*0.9f, 1);
    }

    public void FlipAllCards()
    {
        foreach (var cardSlot in _cardSlots)
        {
            if(cardSlot.Card)cardSlot.Card.Flip();
        }
    }

    public List<ACardSlot.ACardSlotData> GetRemainingCards()
    {
        var remainingCards = new List<ACardSlot.ACardSlotData>();
        for (int i = 0; i < _cardSlots.Length; i++)
        {
            if(_cardSlots[i].IsEmpty) continue;
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

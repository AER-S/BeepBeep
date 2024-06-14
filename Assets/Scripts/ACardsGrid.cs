using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ACardsGrid : MonoBehaviour
{
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private ACard CardPrefab;
    [SerializeField] private ACardSlot CardSlotPrefab;
    [field:SerializeField]public int Rows { get; private set; }
    [field:SerializeField]public int Columns { get; private set; }

    [SerializeField]private VisualProvider VisualProvider;

    [SerializeField] private int Variations;


    private ACardSlot[] _cardSlots;
    private Vector3 _scale;
    
    public void Populate()
    {
        _scale = GetScale();
        var cardsCount = Rows * Columns;
        _cardSlots = new ACardSlot[cardsCount];
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
        return (Rows * Columns) / Variations;
    }

    private Dictionary<int, int> GetDistribution(int baseWeight)
    {
        var upperWeightCount = (Rows * Columns) % Variations;
        //Debug.Log("UpperWeightCount= "+upperWeightCount);
        Dictionary<int, int> Distributions = new Dictionary<int, int>();
        for (int i = 0; i < Variations; i++)
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
    }

    Vector3 GetPosition(int i)
    {
        
        int rowIndex = i / Columns;
        int columnIndex =  i % Columns;
        float rowPosition = (Height / (float)Rows) * rowIndex;
        float columnPosition = (Width /(float) Columns) * columnIndex;
        var spawnLocation = new Vector3(columnPosition, rowPosition, 0);
        return spawnLocation;
    }

    Vector3 GetScale()
    {
        float WidthScale = (Width) / ((float)Columns * 3);
        float HeightScale = (Height) / ((float)Rows * 5);
        return new Vector3(WidthScale*0.9f, HeightScale*0.9f, 1);
    }

    public void FlipAllCards()
    {
        foreach (var cardSlot in _cardSlots)
        {
            cardSlot.Card.Flip();
        }
    }
}

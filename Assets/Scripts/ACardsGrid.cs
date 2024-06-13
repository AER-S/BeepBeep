using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
    
    

    void Start()
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
        _cardSlots[position].transform.localScale = _scale;
        _cardSlots[position].FillSlot(Instantiate<ACard>(CardPrefab, spawnPosition, Quaternion.identity));
        _cardSlots[position].Card.Value = cardValue;
        _cardSlots[position].Card.transform.localScale = _scale;
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
        float WidthScale = (Width-1) / ((float)Columns * 3);
        float HeightScale = (Height-1) / ((float)Rows * 5);
        return new Vector3(WidthScale, HeightScale, 1);
    }
}

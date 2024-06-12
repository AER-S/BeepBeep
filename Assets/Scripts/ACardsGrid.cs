using System.Collections;
using System.Collections.Generic;
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
    private ACardSlot[] _cardSlots;
    

    void Start()
    {
        _cardSlots = new ACardSlot[Rows * Columns];
        for (int i = 0; i < _cardSlots.Length; i++)
        { 
            SpawnACard(i);
        }
    }

    private void SpawnACard(int position)
    {
        var SpawnPosition = GetPosition(position);
        _cardSlots[position] = Instantiate<ACardSlot>(CardSlotPrefab,SpawnPosition,Quaternion.identity);
        _cardSlots[position].FillSlot(Instantiate<ACard>(CardPrefab, SpawnPosition, Quaternion.identity));
        _cardSlots[position].Card.Value = position % VisualProvider.GetTextures().Length;
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
}

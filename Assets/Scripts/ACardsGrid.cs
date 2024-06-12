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
    private Vector3 _scale;

    void Start()
    {
        _scale = GetScale();
        _cardSlots = new ACardSlot[Rows * Columns];
        for (int i = 0; i < _cardSlots.Length; i++)
        { 
            SpawnACard(i);
        }
    }

    private void SpawnACard(int position)
    {
        var spawnPosition = GetPosition(position);
        _cardSlots[position] = Instantiate<ACardSlot>(CardSlotPrefab,spawnPosition,Quaternion.identity);
        _cardSlots[position].transform.localScale = _scale;
        _cardSlots[position].FillSlot(Instantiate<ACard>(CardPrefab, spawnPosition, Quaternion.identity));
        _cardSlots[position].Card.Value = position % VisualProvider.GetTextures().Length;
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

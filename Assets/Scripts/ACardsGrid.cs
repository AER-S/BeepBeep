using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACardsGrid : MonoBehaviour
{
    [SerializeField] private ACard CardPrefab;
    [SerializeField] private ACardSlot CardSlotPrefab;
    public uint Raws { get; private set; }
    public uint Columns { get; private set; }

    private ACardSlot[] _cardSlots;
    

    void Start()
    {
        _cardSlots = new ACardSlot[Raws * Columns];
        for (int i = 0; i < _cardSlots.Length; i++)
        { 
            _cardSlots[i] = Instantiate<ACardSlot>(CardSlotPrefab);
            _cardSlots[i].FillSlot(Instantiate(CardPrefab));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ACardSlot : MonoBehaviour
{
    private ACard _card;
    public ACard Card
    {
        get => _card;
        private set
        {
            _card = value;
            IsEmpty = false;
        }
    }

    public bool IsEmpty { get; private set; }

    public void FillSlot(ACard card)
    {
        Card = card;
    }

    public void ClearSlot()
    {
        Destroy(Card);
        IsEmpty = true;
    }
    
    void Start()
    {
        
    }
    
}

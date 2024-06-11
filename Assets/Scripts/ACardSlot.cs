using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ACardSlot : MonoBehaviour
{
    public ACard Card
    {
        get => Card;
        private set
        {
            if(!value) return;
            Card = value;
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

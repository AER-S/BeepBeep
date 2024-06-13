using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ACardSlot : MonoBehaviour
{
    public ACard Card
    { get; private set; }

    

    public void FillSlot(ACard card)
    {
        Card = card;
    }

    public void ClearSlot()
    {
        Destroy(Card.gameObject);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if(!Card.Isflipped) return;
        AGameManager.Instance.TakeSlot(this);
        Card.Flip();
    }
    
    
}

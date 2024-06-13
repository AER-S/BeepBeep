using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ACardSlot : MonoBehaviour
{
    public ACard Card
    { get; private set; }

    public bool IsEmpty { get; private set; }

    public void FillSlot(ACard card)
    {
        Card = card;
        IsEmpty = false;
    }

    public void ClearSlot()
    {
        Card.Destroy();
        IsEmpty = true;
    }

    private void OnMouseDown()
    {
        if(IsEmpty) return;
        if(!Card.Isflipped) return;
        AGameManager.Instance.TakeSlot(this);
        Card.Flip();
    }
    
}

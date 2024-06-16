
using UnityEngine;
using UnityEngine.EventSystems;

public class ACardSlot : MonoBehaviour
{
    [System.Serializable]
    public class ACardSlotData
    {
        public int CardValue;
        public int Index;
    }
    public ACard Card { get; private set; }
    public bool IsEmpty { get; private set; }
    
    public void FillSlot(ACard card)
    {
        Card = card;
        IsEmpty = false;
    }

    public void ClearSlot()
    {
        if(Card)Card.Destroy();
        IsEmpty = true;
    }

    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(IsEmpty) return;
        if(!Card.IsFlipped) return;
        AGameManager.Instance.TakeSlot(this);
        Card.Flip();
    }
    
}

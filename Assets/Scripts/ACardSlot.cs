
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
    public ACard Card
    { get; private set; }
    

    public bool IsEmpty => _isEmpty;

    private bool _isEmpty;

    public void FillSlot(ACard card)
    {
        Card = card;
        _isEmpty = false;
    }

    public void ClearSlot()
    {
        if(Card)Card.Destroy();
        _isEmpty = true;
    }

    private void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(_isEmpty) return;
        if(!Card.IsFlipped) return;
        AGameManager.Instance.TakeSlot(this);
        Card.Flip();
    }
    
}

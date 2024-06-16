
using UnityEngine;

[RequireComponent(typeof(ACard))]
public class ACardSoundController : MonoBehaviour
{
    private ACard _card;
    
    void Awake()
    {
        _card = GetComponent<ACard>();
    }

    private void OnEnable()
    {
        _card.FlipCard += PlayFlippingSound;
    }

    private void OnDisable()
    {
        _card.FlipCard -= PlayFlippingSound;
    }

    private void PlayFlippingSound()
    {
        ASoundsManager.Instance.PlayFlippingSound();
    }
    
}

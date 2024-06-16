
using UnityEngine;

[RequireComponent(typeof(ACard))]
[RequireComponent(typeof(Animator))]
public class ACardAnimationsController : MonoBehaviour
{
    private ACard _card;
    private Animator _animator;
    
    #region Unity Events

    private void Awake()
    {
        _card = GetComponent<ACard>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _card.FlipCard += PlayFlipAnimation;
        _card.DestroyCard += PlayDestroyAnimation;
    }

    private void OnDisable()
    {
        _card.FlipCard -= PlayFlipAnimation;
        _card.DestroyCard -= PlayDestroyAnimation;
    }

    #endregion

    void PlayFlipAnimation()
    {
        _animator.SetTrigger("Flip");
    }

    void PlayDestroyAnimation()
    {
        _animator.SetTrigger("Destroy");
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ACard))]
[RequireComponent(typeof(Animator))]
public class ACardAnimationsController : MonoBehaviour
{
    private ACard _card;

    private Animator _animator;
    
    // Start is called before the first frame update

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

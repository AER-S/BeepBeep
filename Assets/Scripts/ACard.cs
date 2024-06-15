
using System;
using UnityEngine;
using UnityEngine.Events;


public class ACard : MonoBehaviour
{
    [SerializeField] private Texture backTexture;
    [SerializeField] private Texture FrontTexture;
    [SerializeField] private MeshRenderer Back;
    [SerializeField] private MeshRenderer Front;
    [SerializeField] private VisualProvider VisualProvider;

    public Action FlipCard;
    public Action DestroyCard;
    

    [field: SerializeField]
    public int Value { get; set; }

    public bool Isflipped { get; private set; }

    public void Flip()
    {
        Isflipped= !Isflipped;
        FlipCard?.Invoke();
    }

    public void Destroy()
    {
        DestroyCard?.Invoke();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        Isflipped = true;
        FrontTexture = VisualProvider.GetTexture(Value);
        Front.material.mainTexture = FrontTexture;
    }
}

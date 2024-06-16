
using System;
using UnityEngine;


public class ACard : MonoBehaviour
{
    #region SerializeField
    
    [SerializeField] private Texture FrontTexture;
    [SerializeField] private MeshRenderer Front;
    [SerializeField] private VisualProvider VisualProvider;
    [field: SerializeField] public int Value { get; set; }

    #endregion

    #region Public Actions

    public Action FlipCard;
    public Action DestroyCard;

    #endregion
    
    public bool IsFlipped { get; private set; }
    
    public void Flip()
    {
        IsFlipped= !IsFlipped;
        FlipCard?.Invoke();
    }

    public void Destroy()
    {
        DestroyCard?.Invoke();
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        IsFlipped = true;
        FrontTexture = VisualProvider.GetTexture(Value);
        Front.material.mainTexture = FrontTexture;
    }
}

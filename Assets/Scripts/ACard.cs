using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ACard : MonoBehaviour
{
    [SerializeField] private Texture backTexture;
    [SerializeField] private Texture FrontTexture;
    [SerializeField] private MeshRenderer Back;
    [SerializeField] private MeshRenderer Front;
    [SerializeField] private VisualProvider VisualProvider;
    

    [field: SerializeField]
    public int Value { get; set; }

    public bool Isflipped { get; private set; }

    public ACard()
    {
        Value = 0;
        Isflipped = false;
    }

    public ACard(int value) : this()
    {
        Value = value;
    }

    public void Flip()
    {
        Isflipped= !Isflipped;
    }
    // Start is called before the first frame update
    void Start()
    {
        FrontTexture = VisualProvider.GetTexture(Value);
        Front.material.mainTexture = FrontTexture;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACard : MonoBehaviour
{
    [field:SerializeField] public uint Value { get; set; }
    
    public bool Isflipped { get; private set; }

    public ACard()
    {
        Value = 0;
        Isflipped = false;
    }

    public ACard(uint value) : this()
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
        
    }
}

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

    public void Flip()
    {
        Isflipped= !Isflipped;
        var rotation = transform.eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y + 180,rotation.z);
        transform.eulerAngles = rotation;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Isflipped = true;
        FrontTexture = VisualProvider.GetTexture(Value);
        Front.material.mainTexture = FrontTexture;
    }
}

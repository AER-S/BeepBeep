using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VisualProvider : ScriptableObject
{
    [SerializeField] private Texture2D[] Textures;

    public Texture2D[] GetTextures() => Textures;

    public Texture2D GetTexture(int index)
    {
        return Textures[index];
    }
}

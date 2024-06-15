
using UnityEngine;


public class Singleton<T>: MonoBehaviour where T: Singleton<T>
{
    private static T _instance; 
    public static T Instance
    {
        get => _instance;
    }

    protected void Awake()
    {
        if (_instance==null) _instance = (T)this;
        else if(gameObject)Destroy(gameObject);
        
    }
}

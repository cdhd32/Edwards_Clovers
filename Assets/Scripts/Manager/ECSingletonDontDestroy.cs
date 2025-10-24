using Unity.VisualScripting;
using UnityEngine;

public class ECSingletonDontDestroy<T> : ECSingleton<T> where T : MonoBehaviour
{
    protected new void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(_instance);
            Debug.Log("ECSingletonDontDestroy load");
        }
    }
}

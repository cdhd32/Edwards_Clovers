using UnityEngine;

public class ECSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance => _instance;

    protected static T _instance;

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            Debug.Log("ECSingleton load");
        }
    }
}

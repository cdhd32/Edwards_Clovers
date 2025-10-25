using UnityEngine;

//Àü¿ª ½Ì±ÛÅæ
public class ECSingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    var singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = "[Singleton]" + typeof(T).ToString();

                    DontDestroyOnLoad(_instance);
                    Debug.Log("ECSingletonDontDestroy Created");
                }
            }

            return _instance;
        }
    }
}

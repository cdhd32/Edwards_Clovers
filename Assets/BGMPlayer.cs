using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer instance;
    private AudioSource audioSource;

    private void Awake()
    {
        Debug.Log("dgs");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;       
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}

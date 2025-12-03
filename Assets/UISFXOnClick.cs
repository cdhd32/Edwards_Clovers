using UnityEngine;
using UnityEngine.EventSystems;

public class UISFXOnClick : MonoBehaviour, IPointerDownHandler
{
    public AudioClip clickSFX;     // 재생할 효과음
    private AudioSource audioSource;

    void Awake()
    {
        // AudioSource 자동 추가 (없으면)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.playOnAwake = false;
    }

    // 클릭 시 실행됨
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSFX != null)
        {
            audioSource.PlayOneShot(clickSFX);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ECEnglishVocaBox : MonoBehaviour
{
    public Image vocaImage;
    public TextMeshProUGUI vocaWord;
    public TextMeshProUGUI vocaMeaning;

    public void SetVocaImage(ECEnglishCardInfo info)
    {
        vocaImage.sprite = info.spr;
        vocaWord.text = info.word;
        vocaMeaning.text = info.meaning;
    }
}

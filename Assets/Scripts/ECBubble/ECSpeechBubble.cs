using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ECBubbleTextTable
{
    public ECBubbleText[] texts;
}
[Serializable]
public class ECBubbleText
{
    public int eventType;
    public int key;
    public string value;
    public int bubbleSprType;
    public int edwardSprType;
}

public struct ECBubbleKey
{
    public int eventType;
    public int key;
}

public enum SpeechType : int
{
    KOR,
    ENG,
    MATH,
    SCI,
    IDLE,
    _MAX
}

public class ECSpeechBubble : MonoBehaviour
{
    public TextAsset speechText;
    public Dictionary<ECBubbleKey, ECBubbleText> tableDictionary;
    public TextMeshProUGUI tmp;
    public Image bubbleImage;
    public Image edwardImage;
    public Sprite[] bubbleSprites; // 이미지 받고 추가
    public Sprite[] edwardSprites; // 이미지 받고 추가
    private Tweener tween;
    public float typingSpeed = 2f;
    private const int maxIdleNum = 10007;

    private void Awake()
    {
        ECBubbleTextTable table = JsonUtility.FromJson<ECBubbleTextTable>(speechText.text);
        tableDictionary = new Dictionary<ECBubbleKey, ECBubbleText>();
        for (int i = 0; i < table.texts.Length; ++i)
        {
            ECBubbleText text = table.texts[i];
            ECBubbleKey key = new ECBubbleKey();
            key.eventType = text.eventType;
            key.key = text.key;
            tableDictionary.Add(key, text);
        }
    }

    private void OnEnable()
    {
        ChangeSpeechBubble();
    }

    public void ChangeSpeechBubble(EventType type, EResultState state)
    {
        tmp.text = string.Empty;
        int intType = (int)type;
        if(intType > (int) SpeechType.IDLE)
        {
            intType = (int)SpeechType.IDLE;
        }
        int resultType = state == EResultState.Bad ? 10002 : 10001;
        if(intType == (int)SpeechType.IDLE)
        {
            resultType = UnityEngine.Random.Range(10001, maxIdleNum);
            int befType = PlayerPrefs.GetInt("idle");
            if(resultType == befType)
            {
                while (befType != resultType)
                {
                    resultType = UnityEngine.Random.Range(10001, maxIdleNum);
                }

            }
            PlayerPrefs.SetInt("idle", resultType);
        }
        ECBubbleKey key = new ECBubbleKey();
        key.eventType = intType;
        key.key = resultType;
        ECBubbleText b = tableDictionary[key];
        string val = b.value;
        //이미지 추가 후 변경
        //bubbleImage.sprite = bubbleSprites[b.bubbleSprType];
        //edwardImage.sprite = edwardSprites[b.edwardSprType];
        if (intType == (int)SpeechType.IDLE && resultType == 10001)
        {
            ECPlayerStatManager manager = ECPlayerStatManager.Instance;
            val = manager.GetLowestStatSubject() + val;
        }
        tmp.text = val;
        TMPDOText(tmp, typingSpeed);
    }

    private void ChangeSpeechBubble()
    {
        int state = PlayerPrefs.GetInt("state");
        if (state == 0)
        {
            state = (int)SpeechType._MAX;
        }
        else
        {
            state--;
        }
        int result = PlayerPrefs.GetInt("result");
        if (result > 0)
        {
            result--;
        }
        ChangeSpeechBubble((EventType)state, (EResultState)result);
    }

    public void OnClickBtnSpeechBubble()
    {
        DOTween.Kill(tween);
        ChangeSpeechBubble();
    }
    private void TMPDOText(TextMeshProUGUI tmp, float duration)
    {
        tmp.maxVisibleCharacters = 0;
        tween = DOTween.To(x => tmp.maxVisibleCharacters = (int)x, 0, tmp.text.Length, duration);
    }

    
}

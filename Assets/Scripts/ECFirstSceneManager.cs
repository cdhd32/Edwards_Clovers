using UnityEngine;

public class ECFirstSceneManager : MonoBehaviour
{
    public Texture2D cursorTex;
    float spotWidthval = 0.1f;
    float spotHeightval = 0.3f;
    void Awake()
    {
        //���� �Ŵ��� �ʱ�ȭ
        ECPlayerStatManager.Instance.Init();
        PlayerPrefs.SetInt("examResult", -1);
        PlayerPrefs.SetInt("state", 0);
        cursorSet(cursorTex);
    }

    void cursorSet(Texture2D tex)
    {
        //.Reinitialize(144, 144);
        CursorMode mode = CursorMode.ForceSoftware;
        float xspot = tex.width * spotWidthval;
        float yspot = tex.height * spotHeightval;
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(tex, hotSpot, mode);
    }

}

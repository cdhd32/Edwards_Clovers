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
        //Texture2D tex = ResizeTexture(cursorTex, 150, 150);
        cursorSet(cursorTex);
        //Cursor.sc
        //Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.ForceSoftware);
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

    public Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        // 임시 RenderTexture 생성
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Bilinear;

        // 원본 텍스처를 RT로 블릿
        Graphics.Blit(source, rt);

        // 결과를 Texture2D로 읽어오기
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D newTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return newTexture;
    }
}

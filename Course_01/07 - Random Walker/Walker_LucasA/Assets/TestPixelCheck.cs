using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TestPixelCheck : MonoBehaviour
{
    public WalkerTest walkerTest;
    RawImage test;
    float timer;
    Vector2Int pos;
    // Start is called before the first frame update
    void Start()
    {
        test = GetComponent<RawImage>();
        //GetCameraTexture();
        timer = 10;
    }

    public Texture2D GetCameraTexture(Camera camera = null)
    {
        if (camera == null)
            camera = Camera.main;

        //pos.x = Mathf.RoundToInt((walkerTest.walkerPos.x * 5.4f) - 64);
        //pos.y = Mathf.RoundToInt((walkerTest.walkerPos.y * 5.4f) - 64);

        RenderTexture renderTexture = new RenderTexture(camera.scaledPixelWidth, camera.scaledPixelHeight, 32);
        camera.targetTexture = renderTexture;
        camera.Render();

        Texture2D tex2d = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        RenderTexture.active = renderTexture;
       // Debug.Log(pos);

        tex2d.ReadPixels(new Rect(0, 0, 128, 128), pos.x, pos.y);

        //tex2d.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        //Debug.Log(tex2d.GetPixel(pos.x,pos.y));

        //tex2d.ReadPixels(new Rect(pos.x * walkerTest.scaleFactor, pos.y * walkerTest.scaleFactor, 16, 16), 0, 0);

        //var colors = tex2d.GetPixels().ToList();

        //List<Color> uniqueColors = colors.Select(x => new Color(x.r, x.g, x.b, x.a)).Distinct().ToList();
            
        //Debug.Log("Total different colors: " + uniqueColors.Count);
        //int i = 0;
        //foreach (Color color in uniqueColors)
        //{
        //    i++;
        //    Debug.Log("num: " + i + $"R:{color.r} G:{color.g} B:{color.b}");
        //}

        //test.texture = renderTexture;
        tex2d.Apply();
        test.texture = tex2d;
        camera.targetTexture = null;
        camera.Render();

        return tex2d;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 0.5f)
        {
            timer = 0;
            //GetCameraTexture();
            GetCameraTexture();
        }

    }
}

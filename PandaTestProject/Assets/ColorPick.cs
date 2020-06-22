using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nexweron.Core.MSK;
using UnityEngine.UI; 
public class ColorPick : MonoBehaviour
{

    public ChromaKey_Alpha_General chromakeycontroller;
    public Image displayimage;
    Texture2D tex;
    public static ColorPick instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        tex = new Texture2D(1, 1); 
    }

	private void Update()
	{
        displayimage.color = chromakeycontroller.keyColor;
    }


	void OnDestroy()
    {
        Destroy(tex);
    }

    public void selectcolortoTransparent() 
    {
        StartCoroutine(pickcolor());
    }
    IEnumerator pickcolor() 
    {
        yield return new WaitForEndOfFrame();
    //    Vector2 pos = UICamera.lastEventPosition;
    //    tex.ReadPixels(new Rect(pos.x, pos.y, 1, 1), 0, 0);
        tex.Apply();
        Color color = tex.GetPixel(0, 0);
   //     Debug.Log(ColorToHex(color));

    }

    void OnColorChange(HSBColor color)
    {
        chromakeycontroller.keyColor = color.ToColor();
    }



    public void setcolor(Color32 color) 
    {
        chromakeycontroller.keyColor = color;
    }
}

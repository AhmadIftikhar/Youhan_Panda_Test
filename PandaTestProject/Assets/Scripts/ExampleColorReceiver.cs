using UnityEngine;

public class ExampleColorReceiver : MonoBehaviour {
	
   public  Color color;

	void OnColorChange(HSBColor color) 
	{
        this.color = color.ToColor();
		this.color.a = 1;
		ColorPick.instance.setcolor(this.color);
	}

}

using UnityEngine;
using System.Collections;

public class CGButton : CGElement {

	public string style = "";
	public string text = "";
	bool clicked = false;
	
	public override void Render(){
		clicked = GUI.Button(GetActualRect(),text,GetStyle(GUI.skin.button,style));
	}
	public bool GetClicked(){
		return clicked;
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "B";}
	#endif
}

using UnityEngine;
using System.Collections;

public class CGTextField : CGElement {

	public string style = "";
	public string text = "text field";
	
	public override void Render(){
		text = GUI.TextField(GetActualRect(),text,GetStyle(GUI.skin.textField,style));
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "TF";}
	#endif
}

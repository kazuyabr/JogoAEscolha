using UnityEngine;
using System.Collections;

public class CGLabel : CGElement {

	public string style = "";
	public string text = "label";
	
	public override void Render(){
		GUI.Label(GetActualRect(),text,GetStyle(GUI.skin.label,style));
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "L";}
	#endif
}

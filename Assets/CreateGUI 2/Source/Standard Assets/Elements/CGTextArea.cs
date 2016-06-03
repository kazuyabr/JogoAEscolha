using UnityEngine;
using System.Collections;

public class CGTextArea : CGElement {

	public string style = "";
	public string text = "text area";
	
	public override void Render(){
		text = GUI.TextArea(GetActualRect(),text,GetStyle(GUI.skin.textArea,style));
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "TA";}
	#endif
}

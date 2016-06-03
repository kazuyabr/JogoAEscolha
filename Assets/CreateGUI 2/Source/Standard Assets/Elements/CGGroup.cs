using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGGroup : CGElement {

	string style = "";

	public override void Render(){
		GUI.BeginGroup(GetActualRect(),GetStyle(null,style));
		RenderChildren();
		GUI.EndGroup();
	}
	public override bool SupportsChildren(){
		return true;
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "G";}
	#endif
}

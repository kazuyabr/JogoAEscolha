using UnityEngine;
using System.Collections;

public class CGRoot : CGElement{
	
	public float UIScale = 1;
	public override bool SupportsChildren(){
		return true;
	}
	public override Rect GetActualRect(){
		return new Rect(0,0,Screen.width,Screen.height);
	}
	#if UNITY_EDITOR
	public bool drawBounds = false;
	public CGElement selected = null;
	#endif
}

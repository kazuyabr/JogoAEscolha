using UnityEngine;
using System.Collections;

public class CGScrollBar : CGElement {
	
	public enum ScrollbarOrientation{Horizontal,Vertical}
	public string style = "";
	public ScrollbarOrientation scrollbarOrientation = ScrollbarOrientation.Horizontal;
	public float scrollbarValue = 0;
	public float size = 3;
	public float leftTopLimit = 0;
	public float rightBottomLimit = 10;
	public string thumbStyle = "";
	
	public override void Render(){
		if(scrollbarOrientation == ScrollbarOrientation.Horizontal){
			scrollbarValue = GUI.HorizontalScrollbar(GetActualRect(),scrollbarValue,size,leftTopLimit,rightBottomLimit,GetStyle(GUI.skin.horizontalScrollbar,style));
		}else{
			scrollbarValue = GUI.VerticalScrollbar(GetActualRect(),scrollbarValue,size,leftTopLimit,rightBottomLimit,GetStyle(GUI.skin.verticalScrollbar,style));
		}
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "SB";}
	#endif
}

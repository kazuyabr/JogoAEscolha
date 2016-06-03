using UnityEngine;
using System.Collections;

public class CGWindow : CGElement {
	
	public string title = "Window";
	public string style = "";
	int id = 0;
	public Rect dragArea = new Rect(0,0,100,5);
	public RelativePosition dragAreaPosition = RelativePosition.TopLeft;
	CGRoot Root;
	
	public override void Render(){
		if(id == 0) id = GetHashCode();
		if(visible){
			if(skin != null) GUI.skin = skin;
			Rect orgRect = GetActualRect();
			Rect newRect = new Rect();
			newRect = GUI.Window(id,orgRect,RenderWindow,title,GetStyle(GUI.skin.window,style));
			if(orgRect != newRect) rect = ScalingTool.GetPercentageRect(root.UIScale,new Vector2(Screen.width,Screen.height),newRect,relativePosition);
			#if UNITY_EDITOR                               
			if(root.drawBounds){
				Rect actualDragWindowRect = ScalingTool.GetActualRect(root.UIScale,new Vector2(GetActualRect().width,GetActualRect().height),
				                                                      dragArea,dragAreaPosition);
				DrawBounds(new Rect(actualDragWindowRect.x+GetActualRect().x,actualDragWindowRect.y+GetActualRect().y,actualDragWindowRect.width,actualDragWindowRect.height));
			}
			#endif
		}
	}
	
	public void RenderWindow(int windowID){
		Rect dragAreaActualRect = ScalingTool.GetActualRect(root.UIScale,new Vector2(GetActualRect().width,GetActualRect().height),dragArea,dragAreaPosition);
		GUI.DragWindow(dragAreaActualRect);
		#if UNITY_EDITOR
		if(root.drawBounds) DrawBounds(dragAreaActualRect);
		#endif
		RenderChildren();
	}
	public override bool SupportsChildren(){
		return true;
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "W";}
	#endif
}
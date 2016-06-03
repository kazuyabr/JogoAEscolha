using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGScrollArea : CGElement {
	
	public Vector2 scrollPosition = new Vector2(0,0);
	public string horizontalStyle = "";
	public string verticalStyle = "";
	
	public override void Render(){
		Vector2 actualViewAreaSize = GetBoundsOfChildrenRecursively(children,new Vector2());
		Rect actualViewArea = new Rect(0,0,actualViewAreaSize.x,actualViewAreaSize.y);
		scrollPosition = GUI.BeginScrollView(GetActualRect(),scrollPosition,actualViewArea,GetStyle(GUI.skin.horizontalScrollbar,horizontalStyle),GetStyle(GUI.skin.verticalScrollbar,verticalStyle));
		RenderChildren();
		GUI.EndScrollView();
	}

	public Vector2 GetBoundsOfChildrenRecursively(List<CGElement> elements,Vector2 bounds){
		foreach(CGElement element in elements){
			bounds.x = Mathf.Max(bounds.x,element.GetActualRect().x+element.GetActualRect().width);
			bounds.y = Mathf.Max(bounds.y,element.GetActualRect().y+element.GetActualRect().height);
			bounds = GetBoundsOfChildrenRecursively(element.children,bounds);
		}
		return bounds;
	}

	public override bool SupportsChildren(){
		return true;
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "SA";}
	#endif
}
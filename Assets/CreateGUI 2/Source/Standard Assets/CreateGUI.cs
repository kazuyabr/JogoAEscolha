using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum RelativePosition{TopLeft,TopCenter,TopRight,MiddleLeft,MiddleCenter,MiddleRight,BottomLeft,BottomCenter,BottomRight}
[ExecuteInEditMode()]
public class CreateGUI : MonoBehaviour{
	public CGRoot root;
	#if UNITY_EDITOR
	public void OnEnable(){
		if(root == null){
			root = ScriptableObject.CreateInstance<CGRoot>();
			root.SetActualRect(new Rect(0,0,Screen.width,Screen.height));
			root.root = root;
		}
	}
	#endif
	void OnGUI(){
		root.SetActualRect(new Rect(0,0,Screen.width,Screen.height));
		root.RenderChildren();
	}
}

public class ScalingTool{
	//Converting Percentage Values to Actual Points
	public static Rect GetActualRect(float UIScale,Vector2 localArea,Rect rect,RelativePosition relativePosition){
		return GetActualRect(UIScale,localArea,rect,relativePosition,true);
	}
	public static Rect GetActualRect(float UIScale,Vector2 localArea,Rect rect,RelativePosition relativePosition,bool sizeRelativeToScreen){
		
		float localScaleFactor = GetScaleFactor(UIScale,localArea);
		float globalScaleFactor = (sizeRelativeToScreen) ? GetScaleFactor(UIScale,new Vector2(Screen.width,Screen.height)) : localScaleFactor;
		
		rect = new Rect(rect.x*localScaleFactor,rect.y*localScaleFactor,rect.width*globalScaleFactor,rect.height*globalScaleFactor);
		
		Vector2 position = new Vector2();
		
		switch(relativePosition){
		
			case RelativePosition.TopCenter:
				position = new Vector2(localArea.x/2-rect.width/2,0);
				break;
				
			case RelativePosition.TopRight: 
				position = new Vector2(localArea.x-rect.width,0);
				break;
				
			case RelativePosition.MiddleLeft: 
				position = new Vector2(0,localArea.y/2-rect.height/2);
				break;
				
			case RelativePosition.MiddleCenter: 
				position = new Vector2(localArea.x/2-rect.width/2,localArea.y/2-rect.height/2);
				break;
				
			case RelativePosition.MiddleRight: 
				position = new Vector2(localArea.x-rect.width,localArea.y/2-rect.height/2);
				break;
				
			case RelativePosition.BottomLeft:
				position = new Vector2(0,localArea.y-rect.height);
				break;
				
			case RelativePosition.BottomCenter:
				position = new Vector2(localArea.x/2-rect.width/2,localArea.y-rect.height);
				break;
				
			case RelativePosition.BottomRight:
				position = new Vector2(localArea.x-rect.width,localArea.y-rect.height);
				break;
		}
		
		position = new Vector2(position.x+rect.x,position.y+rect.y);
		
		return new Rect(position.x,position.y,rect.width,rect.height);
	}
	//Convert Actual Points to Percentage Values
	public static Rect GetPercentageRect(float UIScale,Vector2 localArea,Rect rect, RelativePosition relativePosition){

		Vector2 position = new Vector2();
		
		switch(relativePosition){
		
			case RelativePosition.TopCenter:
				position = new Vector2(localArea.x/2-rect.width/2,0);
				break;
				
			case RelativePosition.TopRight: 
				position = new Vector2(localArea.x-rect.width,0);
				break;
				
			case RelativePosition.MiddleLeft: 
				position = new Vector2(0,localArea.y/2-rect.height/2);
				break;
				
			case RelativePosition.MiddleCenter:
				position = new Vector2(localArea.x/2-rect.width/2,localArea.y/2-rect.height/2);
				break;
				
			case RelativePosition.MiddleRight: 
				position = new Vector2(localArea.x-rect.width,localArea.y/2-rect.height/2);
				break;
				
			case RelativePosition.BottomLeft:
				position = new Vector2(0,localArea.y-rect.height);
				break;
				
			case RelativePosition.BottomCenter:
				position = new Vector2(localArea.x/2-rect.width/2,localArea.y-rect.height);
				break;
				
			case RelativePosition.BottomRight:
				position = new Vector2(localArea.x-rect.width,localArea.y-rect.height);
				break;
		}
		
		position = new Vector2(rect.x-position.x,rect.y-position.y);
		
		float sf = GetScaleFactor(UIScale,localArea);
		
		return new Rect(position.x/sf,position.y/sf,rect.width/sf,rect.height/sf);
	}
	
	public static float GetScaleFactor(float UIScale,Vector2 localArea){
		
		return (localArea.x+localArea.y)/200*UIScale;
	}
}

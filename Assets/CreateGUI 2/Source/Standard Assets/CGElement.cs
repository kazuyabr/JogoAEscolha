using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public abstract class CGElement : ScriptableObject{
	/// <summary>Should this element and it's children be rendered?</summary>
	public bool visible = true;
	/// <summary>GUI.skin set to this before rendering and after rendering each child</summary>
	public GUISkin skin;
	/// <summary>Location relative to parent where element is anchored</summary>
	public RelativePosition relativePosition = RelativePosition.TopLeft;
	/// <summary>X and Y as a percentage of Parent Width and Height a percentage of Screen</summary>
	public Rect rect = new Rect(0,0,10,10);
	
	//These are Hidden in the Inspector:
	
	/// <summary>"rect" (percentage based) converted to pixels according to screen or parent element size</summary>
	Rect actualRect = new Rect();
	/// <summary>the same as "actualRect" but counters the effects of "BeginArea","BeginScrollArea" and similar GUI commands</summary>
	Rect globalRect = new Rect();
	public CGRoot root;
	public List<CGElement> children = new List<CGElement>();
	public CGElement parent = null;
	#if UNITY_EDITOR
	public bool foldout = false;
	public bool GetFoldout(){return foldout;}
	public void SetFoldout(bool f){foldout = f;}
	/// <summary>Is this Element above the the specified Element (used in SetParent function)</summary>
	public bool IsAboveInHierarchy(CGElement element){
		if(parent == element.parent) return false;
		while(true){
			if(element.parent == null) return false;
			if(element.parent == this) return true;
			element = element.parent;
		}
	}
	public virtual string ShortName(){return "?";}
	#endif
	public virtual bool SupportsChildren(){return false;}
	public virtual Rect GetActualRect(){return actualRect;}
	public void SetActualRect(Rect a){actualRect = a;}
	public Rect GetGlobalRect(){return globalRect;}
	/// <summary>Overriden by each element class</summary>
	public virtual void Render(){}
	/// <summary>Overriden by each element class</summary>
	public void RenderChildren(){
		GUISkin parentSkin = (skin) ? skin : GUI.skin;
		foreach(CGElement child in children){
			GUI.skin = parentSkin;
			child.RenderRecursively(new Vector2(actualRect.width,actualRect.height));
		}
	}
	public bool MouseOver(){
		return globalRect.Contains(new Vector2(Input.mousePosition.x,Screen.height-Input.mousePosition.y));
	}
	/// <summary>Element and it's Children are Rendered if it is Visible (Other Essential Functions like UpdateRenderBounds can also be called)</summary>
	public void RenderRecursively(Vector2 totalArea){
		if(visible){
			if(skin != null) GUI.skin = skin;
			Vector2 offset = GUIUtility.GUIToScreenPoint(new Vector2(actualRect.x,actualRect.y));
			actualRect = ScalingTool.GetActualRect(root.UIScale,totalArea,rect,relativePosition);
			globalRect = new Rect(offset.x,offset.y,actualRect.width,actualRect.height);

			Render();
			#if UNITY_EDITOR
			if(root.drawBounds) DrawBounds(actualRect);
			#endif
		}
	}
	#if UNITY_EDITOR
	public void DrawBounds(Rect actualRect){
		GUI.DrawTexture(new Rect(actualRect.x,actualRect.y,actualRect.width,1),EditorGUIUtility.whiteTexture);
		GUI.DrawTexture(new Rect(actualRect.x,actualRect.y+actualRect.height,actualRect.width+1,1),EditorGUIUtility.whiteTexture);
		GUI.DrawTexture(new Rect(actualRect.x,actualRect.y,1,actualRect.height),EditorGUIUtility.whiteTexture);
		GUI.DrawTexture(new Rect(actualRect.x+actualRect.width,actualRect.y,1,actualRect.height),EditorGUIUtility.whiteTexture); 
	}
	#endif
	/// <summary>An Element is removed from it's previous Parent and Parented to this Element</summary>
	public CGElement AddChild(CGElement element){
		if(!SupportsChildren()){
			Debug.LogWarning("Element \""+name+"\": Element Type \""+GetType()+"\" does not support children");
			return null;
		}
		if(element.parent != null) element.parent.children.Remove(element);
		element.parent = this;
		children.Add(element);
		return element;
	}

	/// <summary>New Element is Created through "ScriptableObject.CreateInstance(type) as CGElement" and it's skin set to that of the current element</summary>
	public CGElement AddNew(System.Type T){
		if(!SupportsChildren()){
			Debug.LogWarning("Element \""+name+"\": Element Type \""+GetType()+"\" does not support children");
			return null;
		}
		CGElement element = ScriptableObject.CreateInstance(T) as CGElement;
		element.root = root;
		element.parent = this;
		children.Add(element);
		return element;
	}
	/// <summary>New Element is Created through "ScriptableObject.CreateInstance(type) as CGElement" and it's skin set to that of the current element</summary>
	public CGElement AddNew(CGElement copyThis){
		if(!SupportsChildren()){
			Debug.LogWarning("Element \""+name+"\": Element Type \""+GetType()+"\" does not support children");
			return null;
		}
		CGElement element = copyThis.Clone();
		AddChild(element);
		return element;
	}
	/// <summary>Copies this Element along with the Elements below it</summary>
	public CGElement Clone(){
		CGElement element = Instantiate(this) as CGElement;
		element.name = name;
		element.children = new List<CGElement>();
		for(int i=0;i<children.Count;i++){
			CGElement clone = children[i].Clone();
			element.AddChild(clone);
		}
		if(parent != null) parent.children.Add(element);
		return element;
	}
	/// <summary>Destroys this Element and also Removes it from the Children of it's Parent</summary>
	public void DestroyThis(){
		parent.children.Remove(this);
		for(int i = 0; i < children.Count; i++){
			children[i].DestroyThis();
		}
		if(Application.isPlaying){
			Destroy(this);
		}else{
			DestroyImmediate(this);
		}
	}
	public void DestroyChildren(bool destroyHidden = true){
		for(int i = children.Count - 1; i >= 0; i--){
			if(destroyHidden | children[i].visible) children[i].DestroyThis();
		}
	}
	/// <summary>Handles Automatic Font Scaling</summary>
	public GUIStyle GetStyle(GUIStyle defaultStyle,string styleName){
		GUIStyle renderStyle = null;
		if(styleName != ""){
			try{
				renderStyle = new GUIStyle(GUI.skin.FindStyle(styleName));
			}catch(System.NullReferenceException){}
		}
		if(renderStyle == null){
			if(defaultStyle == null){
				renderStyle = new GUIStyle();
			}else{
				renderStyle = new GUIStyle(defaultStyle);
			}
		}
		int oldFontSize = (renderStyle.fontSize == 0) ? 15 : renderStyle.fontSize;
		renderStyle.fontSize = Mathf.RoundToInt(((float)Screen.width+Screen.height)/1500*oldFontSize*root.UIScale);
		return renderStyle;
	}
	/// <summary>Describes the Hierarchal Relationship between Elements through a List of Indices</summary>
	public List<int> GetRelativePath(CGElement ancestor,CGElement descendant){
		List<int> indices = new List<int>();
		CGElement current = descendant;
		while(current.parent != null && current != ancestor){
			indices.Add(current.parent.children.IndexOf(current));
			current = current.parent;
		}
		if(current == null) return null;
		return indices;
	}
	/// <summary>Generate a Relative Path from the given Elements then use that Path starting at this Element</summary>
	public CGElement GetEquivalent(CGElement ancestor,CGElement descendant){
		List<int> indices = GetRelativePath(ancestor,descendant);
		indices.Reverse();
		CGElement current = this;
		foreach(int i in indices){
			if(i >= current.children.Count || i < 0){
				Debug.LogWarning("CreateGUI: Invalid Path ["+name+"]");
				return null;
			}
			current = current.children[i];
		}
		if(current != null){
			return current;
		}else return null;
	}
}
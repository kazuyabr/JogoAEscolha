using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGTexture : CGElement {
	
	public Texture2D texture;
	public ScaleMode scaleMode = ScaleMode.ScaleAndCrop;
	
	public override void Render(){
		if(texture != null) GUI.DrawTexture(GetActualRect(),texture,scaleMode);
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "TX";}
	#endif
}

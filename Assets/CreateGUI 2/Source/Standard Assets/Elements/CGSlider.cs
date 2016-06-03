using UnityEngine;
using System.Collections;

public class CGSlider : CGElement {
	
	public enum SliderOrientation{Horizontal,Vertical}
	public SliderOrientation sliderOrientation = SliderOrientation.Horizontal;
	public float sliderValue = 0;
	public float leftTopLimit = 0;
	public float rightBottomLimit = 10;
	public string style = "";
	public string thumbStyle = "";
	
	public override void Render(){
		if(sliderOrientation == SliderOrientation.Horizontal){
			sliderValue = GUI.HorizontalSlider(GetActualRect(),sliderValue,leftTopLimit,rightBottomLimit
			                                   ,GetStyle(GUI.skin.horizontalSlider,style),GetStyle(GUI.skin.horizontalSliderThumb,thumbStyle));
		}else{
			sliderValue = GUI.VerticalSlider(GetActualRect(),sliderValue,leftTopLimit,rightBottomLimit
			                                   ,GetStyle(GUI.skin.verticalSlider,style),GetStyle(GUI.skin.verticalSliderThumb,thumbStyle));
		}
	}
	#if UNITY_EDITOR
	public override string ShortName(){return "SL";}
	#endif
}

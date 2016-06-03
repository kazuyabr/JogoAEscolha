using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CreateGUIExample : MonoBehaviour {

	public int spaceBetweenItems = 5;
	public float errorShowDuration = 3;
	float errorShowTime = -100;
	public List<string> items =  new List<string>();
	public CGScrollArea itemList;
	//The Input Box
	public CGTextField newItemName;
	public CGButton newItemAdd;
	public CGLabel newItemError;
	//The Template
	public CGGroup itemT;
	public CGLabel itemNumberT;
	public CGLabel itemNameT;
	public CGButton itemRemoveT;
	//Input to Scan for Changes
	List<CGButton> itemRemoveButtons = new List<CGButton>();

	void Start () {
		itemT.visible = false;
		RefreshItemList();
	}
	void Update(){

		if(Time.time-errorShowDuration > errorShowTime){
			newItemError.visible = false;
		}

		if(newItemAdd.GetClicked()){
			string text = newItemName.text;
			if(text == ""){
				newItemError.visible = true;
				errorShowTime = Time.time;
			}else{
				newItemError.visible = false;
				items.Add(newItemName.text);
				RefreshItemList();
				itemList.scrollPosition.y = Mathf.Infinity;
			}
		}
		for(int i=0;i<itemRemoveButtons.Count;i++){
			if(itemRemoveButtons[i].GetClicked()){
				items.RemoveAt(i);
				RefreshItemList();
				break;
			}
		}
	}
	void RefreshItemList(){
		itemList.DestroyChildren(false);
		itemRemoveButtons = new List<CGButton>();
		for(int i=0;i<items.Count;i++){
			CGGroup item = (CGGroup) itemT.Clone();
			((CGLabel)item.GetEquivalent(itemT,itemNumberT)).text = (i+1)+"";
			((CGLabel)item.GetEquivalent(itemT,itemNameT)).text = items[i];
			itemRemoveButtons.Add((CGButton)item.GetEquivalent(itemT,itemRemoveT));
			item.visible = true;
			item.rect.y = i*spaceBetweenItems;
		}
	}
}

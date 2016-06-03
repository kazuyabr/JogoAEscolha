#pragma warning disable 618
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CreateGUI)),CanEditMultipleObjects]
public class CreateGUIEditor : Editor{
	public static Color lightBlue = new Color(0,0.2f,1,1);
	public static string[] hideAttributes = new string[]{"saveType","root","children","foldout","parent"};
	static float elementButtonHeight = 20;
	Vector2 scrollPosition_ElementView = new Vector2();
	Vector2 scrollPosition_ElementList = new Vector2();
	List<System.Type> elementTypes = new List<System.Type>();
	CreateGUI main;
	CGElement addChild = null;
	CGElement destroyThis = null;
	CGElement cloneThis = null;
	
	[MenuItem ("GameObject/Create Other/CreateGUI")]
	static void AddCreateGUI(){
		GameObject go = new GameObject();
		go.name = "CreateGUI";
		go.AddComponent(typeof(CreateGUI));
		Selection.activeGameObject = go;
	}

	public void OnEnable(){
		elementTypes = new List<System.Type>();
		foreach(System.Type type in Assembly.GetAssembly(typeof(CGElement)).GetTypes()){
			if(type.IsSubclassOf(typeof(CGElement))){
				elementTypes.Add(type);
			}
		}
	}

	override public void OnInspectorGUI(){

		main = target as CreateGUI;
		main.OnEnable();
		
		if(Application.isPlaying){
			GUILayout.Space(10);
			GUIStyle warning = new GUIStyle();
			warning.normal.textColor = Color.red;
			warning.alignment = TextAnchor.MiddleCenter;
			warning.fontStyle = FontStyle.Bold;
			GUILayout.BeginHorizontal("dockareaStandalone");
			GUILayout.Label("Changes will NOT be kept! (In Play Mode)",warning);
			GUILayout.EndHorizontal();
		}
		
		GUI.SetNextControlName("unfocus");
		GUI.TextField(new Rect(-3,0,0,0),"");
		
		if(main.root.selected == null || main.root.selected.GetType() == typeof(CGRoot)){
			main.root.name = "CreateGUIRoot";
			main.root.selected = main.root;
		}
		if(destroyThis != null){
			Undo.RegisterSceneUndo("Remove CreateGUI Element");
			destroyThis.DestroyThis();
			main.root.selected = main.root;
		}
		if(addChild != null && main.root.selected != null){
			main.root.selected.AddChild(addChild);
			main.root.selected.SetFoldout(true);
			addChild = null;
		}
		if(cloneThis != null){
			CGElement Clone = cloneThis.Clone();
			Clone.name = cloneThis.name+" (Clone)";
			main.root.selected = Clone;
			Undo.RegisterCreatedObjectUndo(Clone,"Clone CreateGUI Element");
			cloneThis = null;
		}
		GUILayout.Space(5);
		
		scrollPosition_ElementView = GUILayout.BeginScrollView(scrollPosition_ElementView,"CN Box",GUILayout.Height(200));
		GUILayout.Space(5);
		//Show Options for main.root.selected Element
		if(main.root.selected == main.root){
			GUILayout.Label("Create GUI","BoldLabel");
			GUILayout.Space(5);
			main.root.UIScale = (float) UndoIfChanged(main.root,main.root.UIScale,EditorGUILayout.FloatField("UI Scale",main.root.UIScale));
			GUILayout.Space(5);
			main.root.drawBounds = (bool) UndoIfChanged(main.root,main.root.drawBounds,EditorGUILayout.Toggle("Draw Bounds (Editor)",main.root.drawBounds));
			GUILayout.Space(5);
			main.root.skin = (GUISkin) UndoIfChanged(main.root,main.root.skin,EditorGUILayout.ObjectField(main.root.skin,typeof(GUISkin),true));
			GUILayout.Space(5);
		}else{
			string newName = EditorGUILayout.TextField("Name",main.root.selected.name);
			if(newName != main.root.selected.name){
				Undo.RegisterUndo(main.root.selected,"CreateGUI Element Name Changed");
			}
			main.root.selected.name = newName;
			FieldInfo [] fields = main.root.selected.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
			foreach(FieldInfo fi in fields){
				bool show = !fi.IsNotSerialized;
				foreach(string attributeName in hideAttributes){
					if(attributeName == fi.Name) show = false;
				}
				if(show){
					char[] propertyName = fi.Name.ToCharArray();
					if(propertyName.Length>0) propertyName[0] = char.ToUpper(propertyName[0]);
					SerializedObject tempSerializedObj = new SerializedObject(main.root.selected);
					SerializedProperty targetProperty = tempSerializedObj.FindProperty(fi.Name);
					if(EditorGUILayout.PropertyField(targetProperty,true)){
						Undo.RegisterUndo(tempSerializedObj.targetObjects,"CreateGUI Element Modified");
					}
					tempSerializedObj.ApplyModifiedProperties();
					tempSerializedObj.Dispose();
				}
			}
		}
		GUILayout.EndScrollView();
		GUILayout.Space(2);
		scrollPosition_ElementList = GUILayout.BeginScrollView(scrollPosition_ElementList,"grey_border",GUILayout.MinHeight(200));
		//List all of the Elements
		//Root element
		GUILayout.BeginHorizontal();
		if(ElementButton(main.root)){
			if(Event.current.shift && main.root.selected != main.root){
				Debug.LogWarning("The main.root \"CREATE GUI\" cannot become the child of Element \""+main.root.selected.name+"\"");
			}else{
				GUI.FocusControl("unfocus");
				main.root.selected = main.root;
			}
		}
		GUILayout.Space(1);
		AddChildrenPopup(main.root);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		//List children of root
		ListElements(main.root.children);
		GUILayout.EndScrollView();
	
		//To get the Game View GUI to Repaint
		if(GUI.changed){
			main.transform.position = main.transform.position;
		}
	}
	
	void ListElements(List<CGElement> elements){
		CGElement removeThis = null;
		foreach(CGElement e in elements){
			if(e == null){
				removeThis = e;
				continue;
			}
			GUILayout.Space(2);
			GUILayout.BeginHorizontal();
			GUILayout.Space(22);
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			if(e.children.Count > 0){
				string symbol = (e.GetFoldout())? "-" : "+";
				if(ElementOption(symbol,e == main.root.selected)) e.SetFoldout(!e.GetFoldout());
				GUILayout.Space(1);
			}
			if(ElementButton(e)){
				if(Event.current.shift){
					if(main.root.selected != e ){
						addChild = e;
					}
				}else{
					GUI.FocusControl("unfocus");
					main.root.selected = e;
				}
			}
			GUILayout.Space(1);
			if(ElementOption("X",main.root.selected == e)){
				destroyThis = e;
			}
			GUILayout.Space(1);
			if(ElementOption("C",main.root.selected == e)){
				cloneThis = e;
			}
			if(e.SupportsChildren()){
				GUILayout.Space(1);
				AddChildrenPopup(e);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			if(e!=null && e.GetFoldout() && e.children.Count>0) ListElements(e.children);
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}
		if(removeThis != null) elements.Remove(removeThis);
	}
	
	bool ElementButton(CGElement e){
		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.alignment = TextAnchor.MiddleLeft;
		if(!e.visible) style.fontStyle = FontStyle.Italic;
		if(main.root.selected == e) style.normal.textColor = lightBlue;
		Rect dragArea = GUILayoutUtility.GetRect(150,elementButtonHeight,GUILayout.ExpandWidth(true));
		
		if(Event.current.type == EventType.MouseDrag && dragArea.Contains(Event.current.mousePosition)){
			DragAndDrop.PrepareStartDrag();
			DragAndDrop.StartDrag(e.name);
			DragAndDrop.visualMode = DragAndDropVisualMode.Link;
			DragAndDrop.objectReferences = new Object[]{e};
		}
		string text = (e.GetType() == typeof(CGRoot)) ? "Create GUI" : "["+e.ShortName()+"] "+e.name;
		if(text.Length > 20) text = text.Remove(20)+"..";
		return GUI.Button(dragArea,text,style);
	}
	bool ElementOption(string text,bool selected){
		GUIStyle style = new GUIStyle(GUI.skin.box);
		style.alignment = TextAnchor.MiddleCenter;
		if(selected) style.normal.textColor = lightBlue;
		return GUI.Button(GUILayoutUtility.GetRect(20,elementButtonHeight,GUILayout.ExpandWidth(true)),text,style);
	}
	void AddChildrenPopup(CGElement e){
		string[] options = new string[elementTypes.Count+1];
		options[0] = "Add";
		int count = 1;
		foreach(System.Type elementType in elementTypes){
			options[count] = elementType.Name;
			count++;
		}
		GUIStyle popupStyle = new GUIStyle(GUI.skin.box);
		popupStyle.alignment = TextAnchor.MiddleCenter;
		if(main.root.selected == e) popupStyle.normal.textColor = lightBlue;
		int action = EditorGUI.Popup(GUILayoutUtility.GetRect(30,elementButtonHeight,GUILayout.ExpandWidth(true)),0,options,popupStyle);
		if(action != 0){
			CGElement addition = e.AddNew(elementTypes[action-1]);
			addition.name = addition.GetType().Name+" "+FindObjectsOfType(addition.GetType()).Length;
			e.SetFoldout(true);
			main.root.selected = addition;
			Undo.RegisterCreatedObjectUndo(addition,"Undo add CreateGUI Element");
		}
	}
	object UndoIfChanged(Object parent,object org,object obj){
		if(org!=obj) Undo.RegisterUndo(parent,"CreateGUI Property Changed");
		return obj;
	}
}
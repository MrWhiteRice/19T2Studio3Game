using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowController
{
	public static void CreateActorList()
	{
		ActorListEditorWindow window = (ActorListEditorWindow)EditorWindow.GetWindow(typeof(ActorListEditorWindow), false, "Actor List");
		window.minSize = new Vector2(510, 450);
		window.Show();
	}

	public static void CreateNewActor()
	{
		NewActorEditorWindow window = (NewActorEditorWindow)EditorWindow.GetWindow(typeof(NewActorEditorWindow), true, "Actor Editor");
		window.minSize = new Vector2(300, 130);
		window.maxSize = new Vector2(300, 130);
		window.Show();
	}
}
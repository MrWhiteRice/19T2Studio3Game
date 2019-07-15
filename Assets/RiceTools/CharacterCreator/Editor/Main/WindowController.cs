using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WindowController
{
	public static void CreateCharacterList()
	{
		CharacterEditorWindow window = (CharacterEditorWindow)EditorWindow.GetWindow(typeof(CharacterEditorWindow), false, "Characters");
		window.minSize = new Vector2(510, 450);
		window.Show();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewActorEditorWindow : EditorWindow
{
	Sprite icon;
	string actorName = "New Actor";
	Actor.WeightClass weight;
	int rarity;

	[MenuItem("Rice Tools/Dialogue System/New Actor", priority = 0000)]
	static void Init()
	{
		WindowController.CreateNewActor();
	}

	void OnGUI()
	{
		GUILayout.Label("New Actor", EditorStyles.boldLabel);
		icon = (Sprite)EditorGUILayout.ObjectField("Actor Face", icon, typeof(Sprite), false);
		actorName = EditorGUILayout.TextField("Actor Name", actorName);
		weight = (Actor.WeightClass)EditorGUILayout.EnumPopup("Weight Class", weight);

		if(GUILayout.Button("Create!"))
		{
			Actor asset = ScriptableObject.CreateInstance<Actor>();

			asset.Name = actorName;
			asset.Icon = icon;
			asset.ID = Random.Range(0, 99999);

			AssetDatabase.CreateAsset(asset, "Assets/RiceTools/CharacterCreator/Resources/Actors/Actor_" + asset.ID + ".asset");
			AssetDatabase.SaveAssets();
		}
	}
}
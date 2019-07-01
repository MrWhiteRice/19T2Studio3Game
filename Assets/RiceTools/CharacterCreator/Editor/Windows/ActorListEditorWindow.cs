using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ActorListEditorWindow : EditorWindow
{
	List<Actor>actors = new List<Actor>();
	Vector2 scrollPos;

	GUISkin skin;

	[MenuItem("Rice Tools/Dialogue System/Actor List", priority = 1000)]
	static void Init()
	{
		WindowController.CreateActorList();
	}

	void OnGUI()
	{
		AddActor();

		LoadActors();
		DrawActors();
	}

	void AddActor()
	{
		if(GUILayout.Button("New Actor", GUILayout.Height(30)))
		{
			WindowController.CreateNewActor();
		}
	}

	void LoadActors()
	{
		actors.Clear();

		Object[] loadedAssets = Resources.LoadAll("Actors/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			if(loadedAssets[x].name.Contains("Actor_"))
			{
				Actor actor = (Actor)loadedAssets[x];

				actors.Add(actor);
			}
		}
	}

	void DrawActors()
	{
		skin = (GUISkin)Resources.Load("RiceSkin");

		EditorGUILayout.BeginVertical();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width));

		for(int x = 0; x < actors.Count; x++)
		{
			DrawTile(x);
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	void DrawTile(int id)
	{
		GUILayout.BeginHorizontal(skin.box);
		actors[id].Icon = (Sprite)EditorGUILayout.ObjectField("Actor Face", actors[id].Icon, typeof(Sprite), false);

		GUILayout.BeginVertical();
		actors[id].name = EditorGUILayout.TextField("Actor Name", actors[id].name);

		if(GUILayout.Button("Delete"))
		{
			AssetDatabase.DeleteAsset("Assets/RiceTools/DialogueSystem/Resources/Actors/Actor_" + actors[id].ID + ".asset");
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
	}
}
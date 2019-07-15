using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterEditorWindow : EditorWindow
{
	List<Actor>actors = new List<Actor>();
	Vector2 scrollPos;

	GUISkin skin;

	[MenuItem("Rice Tools/Character Creator System/Characters", priority = 0)]
	static void Init()
	{
		WindowController.CreateCharacterList();
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
			CreateActor();
		}
	}

	void LoadActors()
	{
		actors.Clear();

		Object[] loadedAssets = Resources.LoadAll("RiceStuff/Actors/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			Debug.Log(loadedAssets[x].name);
			if(loadedAssets[x].name.Contains("Actor_"))
			{
				Actor actor = (Actor)loadedAssets[x];

				actors.Add(actor);
			}
		}
	}

	void CreateActor()
	{
		Random.Range(0, 99999); Actor asset = ScriptableObject.CreateInstance<Actor>();

		int id = Random.Range(0, 99999);

		asset.CharacterName = "Actor_" + id;
		asset.Icon = null;
		asset.ID = id;

		AssetDatabase.CreateAsset(asset, "Assets/RiceTools/CharacterCreator/Resources/RiceStuff/Actors/Actor_" + asset.ID + ".asset");
		AssetDatabase.SaveAssets();
	}

	void DrawActors()
	{
		skin = (GUISkin)Resources.Load("RiceStuff/RiceSkin");

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
		actors[id].Icon = (Sprite)EditorGUILayout.ObjectField("Icon", actors[id].Icon, typeof(Sprite), false);

		GUILayout.BeginVertical();
		actors[id].CharacterName = EditorGUILayout.TextField("Name", actors[id].CharacterName);

		actors[id].Weight = (Actor.WeightClass)EditorGUILayout.EnumPopup("Weight Class", actors[id].Weight);
		actors[id].Rarity = EditorGUILayout.IntSlider("Rarity", actors[id].Rarity, 1, 5);
		actors[id].Initiative = EditorGUILayout.IntSlider("Initiative", actors[id].Initiative, 1, 100);

		if(GUILayout.Button("Delete"))
		{
			AssetDatabase.DeleteAsset("Assets/RiceTools/CharacterCreator/Resources/RiceStuff/Actors/Actor_" + actors[id].ID + ".asset");
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
	}
}
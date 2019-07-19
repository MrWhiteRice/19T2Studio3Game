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
		Color c = GUI.color;
		GUI.color = Color.green;
		if(GUILayout.Button("New Actor", GUILayout.Height(30)))
		{
			CreateActor();
		}
		GUI.color = c;
	}

	void LoadActors()
	{
		actors.Clear();

		Object[] loadedAssets = Resources.LoadAll("RiceStuff/Actors/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			if(loadedAssets[x].name.Contains("Actor_"))
			{
				Actor actor = (Actor)loadedAssets[x];

				actors.Add(actor);
			}
		}
	}

	void CreateActor()
	{
		Actor asset = ScriptableObject.CreateInstance<Actor>();

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
		var SO = new SerializedObject(actors[id]);

		GUILayout.BeginHorizontal(skin.box);
		//icon
		SO.FindProperty("icon").objectReferenceValue = (Sprite)EditorGUILayout.ObjectField("Icon", actors[id].Icon, typeof(Sprite), false);
		
		GUILayout.BeginVertical();
		//name
		SO.FindProperty("characterName").stringValue = EditorGUILayout.TextField("Name", actors[id].CharacterName);

		//weight
		Actor.WeightClass weight = (Actor.WeightClass)EditorGUILayout.EnumPopup("Weight Class", actors[id].Weight);
		SO.FindProperty("weight").enumValueIndex = (int)weight;

		//rarity
		SO.FindProperty("rarity").intValue = EditorGUILayout.IntSlider("Rarity", actors[id].Rarity, 1, 5);
		//initiative
		SO.FindProperty("initiative").intValue = EditorGUILayout.IntSlider("Initiative", actors[id].Initiative, 1, 100);

		Weapon[] loadedWeapons = Resources.LoadAll<Weapon>("RiceStuff/Weapons");

		string[] names = new string[loadedWeapons.Length];
		for(int x = 0; x < names.Length; x++)
		{
			names[x] = loadedWeapons[x].WeaponName;
		}

		actors[id].StartingItem = EditorGUILayout.Popup("Selected Weapon", actors[id].StartingItem, names);

		Color c = GUI.color;
		GUI.color = Color.red;

		if(GUILayout.Button("Delete"))
		{
			AssetDatabase.DeleteAsset("Assets/RiceTools/CharacterCreator/Resources/RiceStuff/Actors/Actor_" + actors[id].ID + ".asset");
		}
		GUI.color = c;

		GUILayout.EndVertical();

		GUILayout.EndHorizontal();

		SO.ApplyModifiedProperties();
	}
}
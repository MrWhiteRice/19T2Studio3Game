using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunEditorWindow : EditorWindow
{
	List<Weapon>weapons = new List<Weapon>();
	Vector2 scrollPos;

	GUISkin skin;

	[MenuItem("Rice Tools/Character Creator System/Weapons", priority = 1)]
	static void Init()
	{
		WindowController.CreateWeaponList();
	}

	void OnGUI()
	{
		AddWeapon();

		LoadWeapons();
		DrawWeapons();
	}

	void AddWeapon()
	{
		Color c = GUI.color;
		GUI.color = Color.green;
		if(GUILayout.Button("New Weapon", GUILayout.Height(30)))
		{
			CreateWeapon();
		}
		GUI.color = c;
	}

	void LoadWeapons()
	{
		weapons.Clear();

		Object[] loadedAssets = Resources.LoadAll("RiceStuff/Weapons/");

		for(int x = 0; x < loadedAssets.Length; x++)
		{
			if(loadedAssets[x].name.Contains("Weapon_"))
			{
				Weapon wep = (Weapon)loadedAssets[x];

				weapons.Add(wep);
			}
		}
	}

	void CreateWeapon()
	{
		Weapon asset = ScriptableObject.CreateInstance<Weapon>();

		int id = Random.Range(0, 99999);

		asset.WeaponName = "Weapon_" + id;
		asset.Icon = null;
		asset.ID = id;

		AssetDatabase.CreateAsset(asset, "Assets/RiceTools/CharacterCreator/Resources/RiceStuff/Weapons/Weapon_" + asset.ID + ".asset");
		AssetDatabase.SaveAssets();
	}

	void DrawWeapons()
	{
		skin = (GUISkin)Resources.Load("RiceStuff/RiceSkin");

		EditorGUILayout.BeginVertical();
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width));

		for(int x = 0; x < weapons.Count; x++)
		{
			DrawTile(x);
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	void DrawTile(int id)
	{
		GUILayout.BeginHorizontal(skin.box);
		weapons[id].Icon = (Sprite)EditorGUILayout.ObjectField("Icon", weapons[id].Icon, typeof(Sprite), false);

		GUILayout.BeginVertical();
		weapons[id].WeaponName = EditorGUILayout.TextField("Name", weapons[id].WeaponName);

		weapons[id].Weight = (Weapon.WeightClass)EditorGUILayout.EnumPopup("Weight Class", weapons[id].Weight);
		weapons[id].Rarity = EditorGUILayout.IntSlider("Rarity", weapons[id].Rarity, 1, 5);

		Color c = GUI.color;
		GUI.color = Color.red;

		if(GUILayout.Button("Delete"))
		{
			AssetDatabase.DeleteAsset("Assets/RiceTools/CharacterCreator/Resources/RiceStuff/Weapons/Weapon_" + weapons[id].ID + ".asset");
		}
		GUI.color = c;

		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
	}
}
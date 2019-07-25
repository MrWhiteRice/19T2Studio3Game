using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartySelector : MonoBehaviour
{
	public Transform panel;
	public int selected;

	private void Start()
	{
		LoadActors();
	}

	void LoadActors()
	{
		GameObject button = (GameObject)Resources.Load("PlayerButton");

		for(int c = 0; c < panel.transform.childCount; c++)
		{
			Destroy(panel.transform.GetChild(c).transform.gameObject);
		}

		for(int x = 0; x < FindObjectOfType<LootBox>().data.unlockedCharacters.Count; x++)
		{
			if(FindObjectOfType<LootBox>().data.unlockedCharacters[x].unlocked)
			{
				GameObject b = Instantiate(button, panel);
				b.GetComponent<PlayerButtonData>().id = FindObjectOfType<LootBox>().data.unlockedCharacters[x].ID;
				b.GetComponent<Button>().onClick.AddListener(b.GetComponent<PlayerButtonData>().Click);
			}
		}
	}

	public void SelectCharacter(int id)
	{
		FindObjectOfType<LootBox>().data.party[selected] = new CharacterParty(id);
		print("selected: " + id);
		Destroy(gameObject);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonData : MonoBehaviour
{
	public int id;

	public void Click()
	{
		FindObjectOfType<PartySelector>().SelectCharacter(id);
	}
}
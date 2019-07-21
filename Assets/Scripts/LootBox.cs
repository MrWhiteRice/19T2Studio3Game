using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : MonoBehaviour
{
	public Text text;

    void Start()
    {
		text.text = "";
    }

	public void Roll()
	{
		int roll = Random.Range(1, 101);

		//1 Star weapon
		if(roll > 30 && roll <= 100)
		{
			text.text += roll + ": 1 Star Weapon \n";
		}

		//2 Star weapon
		if(roll > 5 && roll <= 25)
		{
			text.text += roll + ": 2 Star Weapon \n";
		}

		//3 star weapon
		if(roll > 1 && roll <= 4)
		{
			text.text += roll + ": 3 Star Weapon \n";
		}

		//character
		if(roll == 1)
		{
			text.text += roll + ": Character \n";
		}
	}
}
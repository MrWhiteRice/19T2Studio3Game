using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationData : MonoBehaviour
{
	public Image icon;
	public Text info;

	public void Init(Sprite i, string t)
	{
		icon.sprite = i;
		info.text = t;
	}

	public void Quit()
	{
		Destroy(gameObject);
	}
}
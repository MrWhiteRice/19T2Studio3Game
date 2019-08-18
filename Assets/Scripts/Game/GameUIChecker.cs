using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIChecker : MonoBehaviour
{
	public GameObject pc;
	public GameObject mobile;

    void Start()
    {
		bool set = PlayerPrefs.GetInt("Player" + 0 + "Controller", -1) == -1 ? true : false;
		Debug.LogError(PlayerPrefs.GetInt("Player" + 0 + "Controller", -1));
		pc.SetActive(!set);
		mobile.SetActive(set);
    }
}
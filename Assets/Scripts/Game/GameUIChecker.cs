using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIChecker : MonoBehaviour
{
	public GameObject pc;
	public GameObject mobile;

    void Start()
    {
		bool set = PlayerPrefs.GetInt("Player" + 1 + "Controller") == -1 ? true : false;
		pc.SetActive(!set);
		mobile.SetActive(set);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFixer : MonoBehaviour
{
    void Start()
    {
		GetComponent<Camera>().orthographicSize = (26f) * (float)Screen.height / (float)Screen.width * 0.5f;
	}
}

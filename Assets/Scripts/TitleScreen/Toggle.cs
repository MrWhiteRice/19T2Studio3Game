using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public GameObject userInterfaceObject;
    public GameObject userInterfaceObject2;
    public void toggle()
    {
        if (userInterfaceObject.activeSelf == true)
        {
            userInterfaceObject.SetActive(false);
            userInterfaceObject2.SetActive(true);
            return;
        }

        if (userInterfaceObject.activeSelf == false)
        {
            userInterfaceObject.SetActive(true);
            userInterfaceObject2.SetActive(false);
        }
    }
}

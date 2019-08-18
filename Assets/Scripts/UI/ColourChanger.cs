using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour
{
    public void Colour()
    {
        GetComponent<Image>().color = new Color(0.2623f,0.8301f,0.7862f);
    }

    public void ResetColour()
    {
        GetComponent<Image>().color = new Color(0.2392f, 1, 0.9411f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject start;
    public GameObject options;
    public GameObject gacha;
    public GameObject party;
    
    //Going from main menu to each option
    public void LaunchStart()
    {
        mainMenu.SetActive(false);
        start.SetActive(true);
    }

    public void LaunchOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void LaunchGacha()
    {
        mainMenu.SetActive(false);
        gacha.SetActive(true);
    }

    public void LaunchParty()
    {
        mainMenu.SetActive(false);
        party.SetActive(true);
    }

    //Return from option to main menu
    

}

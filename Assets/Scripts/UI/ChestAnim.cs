using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnim : MonoBehaviour
{
    public GameObject animCanvas;
    public GameObject chestObject;
    public GameObject lootObject;
    public bool animRun;
    bool rollOne;
    bool rollTen;
    string rollType;
    LootBox lootScript;
    Animator animatorChest;
    //set 1 roll
    public void SingleRoll(string weight)
    {
        ChestAnimation();
        rollOne = true;
        rollType = weight;
    }

    //set 10 roll
    public void TenRoll(string weight)
    {
        ChestAnimation();
        rollTen = true;
        rollType = weight;
    }

    void ChestAnimation()
    {
        animCanvas.SetActive(true);
        animatorChest = chestObject.GetComponent<Animator>();
        animRun = true;
    }

    private void Update()
    {
        if(animRun == true)
        {
            if (Input.GetMouseButton(0) && animatorChest.GetCurrentAnimatorStateInfo(0).IsName("Struggle")|| animatorChest.GetCurrentAnimatorStateInfo(0).IsName("still"))
            {
                animatorChest.SetTrigger("Open");
                animRun = false;
            }
        }

        if (animatorChest.GetCurrentAnimatorStateInfo(0).IsName("openstill"))
        {
            StartCoroutine(wait());
            lootScript = lootObject.GetComponent<LootBox>();
            if(rollOne == true)
            {
                lootScript.Roll(rollType);
                animCanvas.SetActive(false);
            }

            if (rollTen == true)
            {
                lootScript.MultiRoll(rollType);
                animCanvas.SetActive(false);
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }
}

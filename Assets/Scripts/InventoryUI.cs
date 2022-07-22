using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private bool active = true;
    public void TurnVisible()
    {
        if(active == true)
        {
            active = false;
            GetComponent<Image>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
        else
        {
            active = true;
            GetComponent<Image>().enabled = true;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<Image>().enabled = true;
        }
    }
}

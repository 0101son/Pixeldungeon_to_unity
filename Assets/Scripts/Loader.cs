using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject GameManager;
    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("Loader Awake");
        Instantiate(GameManager);
    }
}

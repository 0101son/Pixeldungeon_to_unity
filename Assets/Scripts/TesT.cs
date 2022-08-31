using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Bundle bundle = new Bundle();
        bundle.Put("num",3);
        Debug.Log(bundle.Get<int>("num"));

        int[,] arr = new int[2, 3];
        arr[0, 0] = 1;
        arr[1, 1] = 4;
        bundle.Put("arr", arr);
        int[,] w = bundle.Get2DArray<int>("arr");
        Debug.Log(w[0,0] + ", " + w[1,1] + ", " + w[1,2]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

        }
    }
}

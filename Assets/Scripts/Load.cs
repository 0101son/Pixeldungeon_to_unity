using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    static private Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();
    // Start is called before the first frame update
    static public void LoadTexture(string subfolder)
    {
        object[] t0 = Resources.LoadAll(subfolder, typeof(Texture2D));

        Debug.Log(t0.Length);
        

        for (int i = 0; i < t0.Length ; i++ ){
            Texture2D t1 = (Texture2D)t0[i];
            cache[t1.name] = t1;
            Debug.Log(t1.name);
        }

        
    }
    static public Texture2D Get(string key)
    {
        return cache[key];
    }
}

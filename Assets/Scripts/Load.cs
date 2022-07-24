using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    static private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    // Start is called before the first frame update
    static public void LoadTexture(string subfolder)
    {
        object[] t0 = Resources.LoadAll(subfolder, typeof(Texture2D));

        Debug.Log(t0.Length);
        

        for (int i = 0; i < t0.Length ; i++ ){
            Texture2D t1 = (Texture2D)t0[i];
            Rect rect = new Rect(0, 0, t1.width, t1.height);
            spriteCache[t1.name] = Sprite.Create(t1, rect, new Vector2(0.5f, 0.5f), 32);
            //Debug.Log(t1.name);
        }

        
    }
    static public Sprite Get(string key)
    {
        return spriteCache[key];
    }
}

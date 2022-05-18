using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{

    public Sprite[] sprites;
    public GameObject CharPrefab;
    private readonly string[] names = {"Hero","Enemy"};

    public CharSprite GetNew()
    {
        GameObject newChar = Instantiate(CharPrefab);
        CharSprite sprite = newChar.GetComponent<CharSprite>();
        sprite.Initiate();
        return sprite;
    }
}

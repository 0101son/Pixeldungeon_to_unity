using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2Int input;


    void Update()
    {
        if (!GameScene.instance.onControll || !Input.anyKeyDown || GameScene.instance.spriteActing == true) return;

        if (Input.GetKey(KeyCode.Keypad1))
        {
            input = new Vector2Int(-1, -1);
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            input = new Vector2Int(0, -1);
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            input = new Vector2Int(1, -1);
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            input = new Vector2Int(-1, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            input = new Vector2Int(1, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad7))
        {
            input = new Vector2Int(-1, 1);
        }
        else if (Input.GetKey(KeyCode.Keypad8))
        {
            input = new Vector2Int(0, 1);
        }
        else if (Input.GetKey(KeyCode.Keypad9))
        {
            input = new Vector2Int(1, 1);
        }
        else
        {
            input = Vector2Int.zero;
        }

        if (input != Vector2Int.zero)
        {
            if (Dungeon.hero.MoveToward(input) == true)
            {
                
                GameScene.instance.onControll = false;
                return;
            }
            foreach (Char mob in Actor.All())
            {
                if (Dungeon.hero.position + input == mob.position)
                {
                    Dungeon.hero.Attack(mob);
                    GameScene.instance.onControll = false;
                    return;
                }
            }
        }

        if (Input.GetKey(KeyCode.Keypad5))
        {
            Dungeon.hero.Rest();
            GameScene.instance.onControll = false;
            return;
        }
    }
}

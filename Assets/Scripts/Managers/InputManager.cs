using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputType input;

    public class InputType
    {
        public class Direction : InputType
        {
            public Vector2Int direction;
            public Direction(int x, int y)
            {
                direction = new Vector2Int(x, y);
            }
            
        }

        public class Character : InputType
        {
            public char character;
            public Character(char input)
            {
                character = input;
            }

        }
    }

    void Update()
    {
        if (!GameScene.instance.onControll || !Input.anyKeyDown || GameScene.instance.spriteActing == true) return;

        input = null;

        if (Input.GetKey(KeyCode.Keypad1))
        {
            input = new InputType.Direction(-1, -1);
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            input = new InputType.Direction(0, -1);
        }
        else if (Input.GetKey(KeyCode.Keypad3))
        {
            input = new InputType.Direction(1, -1);
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            input = new InputType.Direction(-1, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            input = new InputType.Direction(1, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad7))
        {
            input = new InputType.Direction(-1, 1);
        }
        else if (Input.GetKey(KeyCode.Keypad8))
        {
            input = new InputType.Direction(0, 1);
        }
        else if (Input.GetKey(KeyCode.Keypad9))
        {
            input = new InputType.Direction(1, 1);
        }
        else if (Input.GetKey(KeyCode.Keypad5))
        {
            input = new InputType.Character('5');
        }
        else if (Input.GetKey(KeyCode.G))
        {
            input = new InputType.Character('G');
        }
        else if (Input.GetKey(KeyCode.E))
        {
            input = new InputType.Character('E');
        }
        else if (Input.GetKey(KeyCode.D))
        {
            input = new InputType.Character('D');
        }
        else if (Input.GetKey(KeyCode.W))
        {
            input = new InputType.Character('W');
        }
        else if (Input.GetKey(KeyCode.R))
        {
            input = new InputType.Character('R');
        }

        if (input == null) return;

        if (input is InputType.Direction dirInput)
        {
            if (Dungeon.hero.MoveToward(dirInput.direction))
            {
                GameScene.instance.onControll = false;
                return;
            }
            else
            {
                foreach (Char mob in Actor.All())
                {
                    Vector2Int attackTile = Dungeon.hero.position + dirInput.direction;
                    if (attackTile == mob.position)
                    {
                        Dungeon.hero.Attack(mob);
                        GameScene.instance.onControll = false;
                        return;
                    }
                }
            }
            
        }

        if (input is InputType.Character charInput)
        {
            if(charInput.character == '5')
            {
                Dungeon.hero.Rest();
                GameScene.instance.onControll = false;
                return;
            }

            
            if (charInput.character == 'G')
            {
                if(Dungeon.hero.ActPickUp())
                    GameScene.instance.onControll = false;
                return;
            }

            if (charInput.character == 'E')
            {
                if (Dungeon.hero.ActConsume())
                    GameScene.instance.onControll = false;
                return;
            }

            if (charInput.character == 'D')
            {
                if (Dungeon.hero.ActDrop())
                    GameScene.instance.onControll = false;
                return;
            }

            if (charInput.character == 'W')
            {
                if (Dungeon.hero.ActEquip())
                    GameScene.instance.onControll = false;
                return;
            }

            if (charInput.character == 'R')
            {
                if (Dungeon.hero.ActUnequip())
                    GameScene.instance.onControll = false;
                return;
            }
        }


    }
}

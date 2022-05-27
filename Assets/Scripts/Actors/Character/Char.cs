using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Char : Actor
{
    
    // StaticSetup

    public static SpriteManager spriteScript;
    public static GameScene gameScript;



    public Vector2Int position = Vector2Int.zero;

    public CharSprite sprite;

    public int HT;
    public int HP;

    public int attackSpeed;
    public int damage;

    public int moveSpeed;

    public bool visible;

    // Don't know

    public int charID;


    public static void StaticSetup()
    {
        //Debug.Log("Character StaticSetup");
        gameScript = GameScene.instance;
        spriteScript = GameScene.instance.GetComponent<SpriteManager>();
        //Debug.Log("Character StaticSetup Sucess");
    }

    public static void UpdateFOV()
    {
        //Debug.Log("CharSprites to play: " + All().Count);
        foreach (Char character in All())
        {
            character.SetVisible(Dungeon.level.heroFOV[character.position.x, character.position.y]);
        }
    }

    public virtual void Initiate(Vector2Int initPosition)
    {
        //Debug.Log("Init");
        Add(this);
        position = initPosition;
        sprite = spriteScript.GetNew();
        sprite.Link(this);
        //Debug.Log("IsSpriteNullAfterInit??? :" + (sprite == null));
        Dungeon.level.blocking[position.x, position.y] = true;
    }

    public virtual bool MoveToward(Vector2Int dir)
    {
        return TryMoveTo(position + dir);
    }

    public virtual bool TryMoveTo(Vector2Int target)
    {

        if (!Dungeon.level.solid[target.x, target.y] && !Dungeon.level.blocking[target.x, target.y])
        {
            MoveTo(target);

            return true;
        }
        else
            return false;
    }

    public virtual void Attack(Char target)
    {
        Debug.Log("P: " + this + "Attack" + target);
        target.HP -= damage;
        if (target.HP <= 0)
        {
            target.Die();
        }
        Spend(attackSpeed);
        sprite.EnqueueClip(new AttackClip(this, target, damage));
        
    }

    public virtual void Drop(int quantity)
    {
        Debug.Log("P: " + this + "Drops item");
        Dungeon.level.items[position.x,position.y] += quantity;
        Spend(10);

    }

    public virtual void PickUp(int quantity)
    {
        Debug.Log("P: " + this + "PickUp Item");
        Dungeon.level.items[position.x, position.y] -= quantity;
        Spend(10);

    }

    public virtual void Die()
    {
        Debug.Log("P: " + this + "Die");

        HP = 0;
        Dungeon.level.blocking[position.x, position.y] = false;
        Remove(this);
    }

    public virtual void MoveTo(Vector2Int target)
    {
        Debug.Log("P: " + this + "Moves to " + target);
        Spend(moveSpeed);
        sprite.EnqueueClip(new MovingClip(position, target, Dungeon.level.heroFOV[target.x,target.y]));
        Dungeon.level.blocking[position.x, position.y] = false;
        position = target;
        Dungeon.level.blocking[position.x, position.y] = true;

    }

    public Vector2Int NextStepTo(Vector2Int target)
    {
        List<PathNode> pass = Dungeon.level.PassPath(this.position, target);
        List<PathNode> detour = Dungeon.level.DetourPath(this.position, target);
        if(detour != null)
        {
            if (pass.Count + 4 < detour.Count)
                return pass[1].Vector;
            else
                return detour[1].Vector;
        }
        else
        {
            return pass[1].Vector;
        }
    }
        

    public void SetVisible(bool visible)
    {
        this.visible = visible;
        sprite.SetVisible(visible);
    }
}

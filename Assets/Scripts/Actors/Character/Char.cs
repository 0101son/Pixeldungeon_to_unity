using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Char : Actor
{

    // StaticSetup

    public string texture = "ghost_crab";

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

    public static void UpdateFOV()
    {
        //Debug.Log("CharSprites to play: " + All().Count);
        foreach (Char character in All())
        {
            character.SetVisible(Dungeon.level.heroFOV[character.position.x, character.position.y]);
        }
    }


    public virtual bool MoveToward(Vector2Int dir)
    {
        return TryMoveTo(position + dir);
    }

    public virtual bool TryMoveTo(Vector2Int target)
    {

        if (!Dungeon.level.losBlocking[target.x, target.y] && !Dungeon.level.blocking[target.x, target.y])
        {
            MoveTo(target);

            return true;
        }
        else
            return false;
    }

    protected static readonly string POSX       = "posx";
    protected static readonly string POSY       = "posy";
    protected static readonly string TAG_HP     = "HP";
	protected static readonly string TAG_HT     = "HT";

    public override void StoreInBundle(Bundle bundle)
    {

        base.StoreInBundle(bundle);

        bundle.Put(POSX, position.x);
        bundle.Put(POSY, position.y);
        bundle.Put(TAG_HP, HP);
        bundle.Put(TAG_HT, HT);
    }

    public override void RestoreFromBundle( Bundle bundle )
    {
        base.RestoreFromBundle(bundle);

        position = new Vector2Int(bundle.Get<int>(POSX), bundle.Get<int>(POSY));
        HP = bundle.Get<int>(TAG_HP);
        HT = bundle.Get<int>(TAG_HT);
    }

    public virtual void Attack(Char target)
    {
        Debug.Log("P: " + this + "Attack" + target);
        if(this is Hero hero && hero.belongings.weapon != null)
        {
            target.HP -= (damage + hero.belongings.weapon.damageBonus);
            sprite.EnqueueClip(new AttackClip(this, target, damage + hero.belongings.weapon.damageBonus));
        }
        else
        {
            target.HP -= damage;
            sprite.EnqueueClip(new AttackClip(this, target, damage));
        }
        
        if (target.HP <= 0)
        {
            target.Die();
        }
        Spend(attackSpeed);
        
        
    }

    public int Heal(int heal)
    {
        //Debug.Log("heal amount: " + heal);

        if (HP + heal > HT)
        {
            sprite.EnqueueClip(new RecoveryClip(this, HT - HP));
            HP = HT;
            return HT - HP;
        }
        else
        {
            sprite.EnqueueClip(new RecoveryClip(this, heal));
            HP += heal;
            return heal;
        }
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

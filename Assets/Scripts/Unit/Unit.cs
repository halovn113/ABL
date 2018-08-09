using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public UnitType unitType;
    public UnitState unitState;

    //public UnitType type;
    //UnitState state;

    //public UnitState GetState()
    //{
    //    return state;
    //}

    public virtual void HealthApply(float number)
    {
        if (unitType == UnitType.UntouchNPC) return;
        if (health + number <= 0)
        {
            Dead();
            return;
        }
        if (health + number > maxHealth)
        {
            health = maxHealth;
            return;
        }
        health += number;
    }

    public virtual void Dead()
    {
        health = 0;
        unitState = UnitState.Dead;
        Debug.Log("Unit Dead");
    }

    public UnitState GetUnitState()
    {
        return unitState;
    }
}

public enum UnitType
{
    Player,
    Enemy,
    Allies,
    NPC,
    UntouchNPC,
}

public enum UnitState
{
    Alive,
    Dead,
    Stun,
    Idle,
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public UnitType unitType;
    public UnitState unitState;
    public Facing unitFacing;

    public virtual void HealthUpdate(float number)
    {
        if (unitType == UnitType.UntouchNPC) return;
        if (currentHealth + number <= 0)
        {
            Dead();
            return;
        }
        if (currentHealth + number > maxHealth)
        {
            currentHealth = maxHealth;
            return;
        }
        currentHealth += number;
    }

    public virtual void Dead()
    {
        currentHealth = 0;
        unitState = UnitState.Dead;
        //Debug.Log("Unit Dead");
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

public enum Facing
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,

}

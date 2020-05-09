using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpVisitor
{
    public virtual void alterBehaviour() { }
}

public class PowerUpHealth : PowerUpVisitor
{
    public float alterBehaviour(float health) {
        Debug.Log(health);
        health = health + 10f;
        return health;
    }
}

public class PowerUpGold : PowerUpVisitor
{
    public float alterBehaviour(float gold)
    {
        gold = gold + 100;
        return gold;
    }
}

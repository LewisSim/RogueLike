using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpVisitor
{
    public virtual void alterBehaviour() { }
}

public class PowerUpHealth : PowerUpVisitor
{
    public int alterBehaviour(int health) {
        Debug.Log(health);
        health = health + 10;
        return health;
    }
}

public class PowerUpGold : PowerUpVisitor
{
    public int alterBehaviour(int gold)
    {
        gold = gold + 100;
        return gold;
    }
}

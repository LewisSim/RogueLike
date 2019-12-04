using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    //Can be picked up
    public enum ItemType {Armour, Weapon, Potion};

    public enum ItemTier {Common, Uncommon, Rare};

}

public class Equipment : Item
{
    //Can be placed in equipment slots
    static I_ItemAffix[] m_prefixes = {
        new I_ItemAffix("Healthy", I_ItemAffix.StatTarget.Health, new float[]{5,10}, new float[]{10,20}, new float[]{20, 40}),
        new I_ItemAffix("Hermes", I_ItemAffix.StatTarget.Speed, new float[]{0.1f,0.3f}, new float[]{0.3f,0.5f}, new float[]{0.5f, 0.8f}),
        new I_ItemAffix("Empowering", I_ItemAffix.StatTarget.Damage, new float[]{5,10}, new float[]{10,20}, new float[]{20, 40}),
        new I_ItemAffix("Lumbersome", I_ItemAffix.StatTarget.Speed, new float[]{-1,-0.5f}, new float[]{-1.5f,-1f}, new float[]{-1.7f, -1.5f})
    };
    public static I_ItemAffix[] Prefixes
    {
        get { return m_prefixes; }
    }
    static I_ItemAffix[] m_suffixes = {
        new I_ItemAffix("Constitution", I_ItemAffix.StatTarget.Health, new float[]{5,10}, new float[]{10,20}, new float[]{20, 40}),
        new I_ItemAffix("Speed", I_ItemAffix.StatTarget.Speed, new float[]{0.1f,0.3f}, new float[]{0.3f,0.5f}, new float[]{0.5f, 0.8f}),
        new I_ItemAffix("Damaging", I_ItemAffix.StatTarget.Damage, new float[]{5,10}, new float[]{10,20}, new float[]{20, 40}),
    };
    public static I_ItemAffix[] Suffixes
    {
        get { return m_suffixes; }
    }
}

public class Armour : Equipment
{
    static I_ItemBase[] m_base = {
        new I_ItemBase("Light Armour", 20, 5, 10, 0.15f),
        new I_ItemBase("Medium Armour", 40, 5, 15, 0f),
        new I_ItemBase("Heavy Armour", 60, 5, 20, -0.2f),
        new I_ItemBase("Masterwork Light Armour", 40, 5, 10, 0.15f),
        new I_ItemBase("Masterwork Medium Armour", 60, 5, 15, 0f),
        new I_ItemBase("Masterwork Heavy Armour", 80, 5, 20, -0.2f)

    };
    public static I_ItemBase[] Base
    {
        get { return m_base; }
    }
}

public class Weapon : Equipment
{
    static I_ItemBase[] m_base = {
        new I_ItemBase("Spear", 0, 15, 5, 0),
        new I_ItemBase("Sword", 0, 10, 5, 0),
        new I_ItemBase("Bow", 0, 10, 5, 0.5f)

    };
    public static I_ItemBase[] Base
    {
        get { return m_base; }
    }
}

public class Consumable : Item
{
    //Can be stored in inventory and used
}

public class Potion : Consumable
{
    static I_ItemBase[] m_base = {
        new I_ItemBase("Health Potion", 0, 0, 5, 0),
        new I_ItemBase("Speed Potion", 0, 0, 0, 0.5f),
        new I_ItemBase("Damage Potion", 0, 5, 0, 0)

    };
    public static I_ItemBase[] Base
    {
        get { return m_base; }
    }
    //Potency is a multiplier applied to the base properties, this will vary based on tier
    static float[] m_potency =
    {
        0.5f,
        1f,
        1.5f
    };
    public static float[] Potency
    {
        get { return m_potency; }
    }
}

public class I_ItemBase
{
    public string m_name { get; set; }
    public float m_armourRating { get; set; }
    public float m_damage { get; set; }
    public float m_health { get; set; }
    public float m_speed { get; set; }
    //public float m_potency { get; set; } = 1f;
    public I_ItemBase(string name, float armour, float damage, float health, float speed)
    {
        m_name = name;
        m_armourRating = armour;
        m_damage = damage;
        m_health = health;
        m_speed = speed;
    }
}

public class I_ItemAffix
{
    public enum StatTarget {Health, Armour, Damage, Speed};
    public string m_name { get; set; }
    //public Item.ItemTier m_tier { get; set; }
    //All stat ranges for tiers
    public float[] m_t0_minMaxValue { get; set; } = new float[2];
    public float[] m_t1_minMaxValue { get; set; } = new float[2];
    public float[] m_t2_minMaxValue { get; set; } = new float[2];

    public StatTarget m_targetStat { get; set; }

    public I_ItemAffix(string name, StatTarget targetStat, float[] t0_mValues, float[] t1_mValues, float[] t2_mValues)
    {
        m_name = name;
        m_targetStat = targetStat;
        m_t0_minMaxValue = t0_mValues;
        m_t1_minMaxValue = t1_mValues;
        m_t2_minMaxValue = t2_mValues;
    }
}

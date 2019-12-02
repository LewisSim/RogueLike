using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats : MonoBehaviour
{
    //In game stat component of an item in the game world

    public Item.ItemType type;
    public float armourRating, damage, health, speed;
    public string baseName, prefix, suffix, fullName;
    public Item.ItemTier tier;

    //Only for Potions
    [Header("Only for Potions")]
    public float potency;

    private void Awake()
    {
        AssignStats();
    }

    private void Update()
    {
        //Test Live
        //AssignStats();
    }

    //For assigning fresh stats
    public void AssignStats()
    {
        //Assign base stats based on type
        switch (type)
        {
            case Item.ItemType.Armour:
                PullFromBase(Armour.Base[Random.Range(0, Armour.Base.Length - 1)]);
                break;
            case Item.ItemType.Weapon:
                PullFromBase(Weapon.Base[Random.Range(0, Armour.Base.Length - 1)]);
                break;
            case Item.ItemType.Potion:
                PullFromBase(Potion.Base[Random.Range(0, Armour.Base.Length - 1)]);
                break;
        }

        //Choose prefix and suffix based on base
        //Assign tier- then add stats accordingly
        //Armour
        if (type == Item.ItemType.Armour)
        {
            //Assign prefix
            AssignAffix(Armour.Prefixes[Random.Range(0, Armour.Prefixes.Length - 1)], true);
            //Assign suffix
            AssignAffix(Armour.Suffixes[Random.Range(0, Armour.Suffixes.Length - 1)], false);

        }
        //Weapon
        if (type == Item.ItemType.Weapon)
        {
            AssignAffix(Weapon.Prefixes[Random.Range(0, Weapon.Prefixes.Length - 1)], true);
            AssignAffix(Weapon.Suffixes[Random.Range(0, Weapon.Suffixes.Length - 1)], false);
        }

        if(type == Item.ItemType.Potion)
        {
            switch (tier)
            {
                case Item.ItemTier.Common:
                    potency = Potion.Potency[0];
                    break;
                case Item.ItemTier.Uncommon:
                    potency = Potion.Potency[1];
                    break;
                case Item.ItemTier.Rare:
                    potency = Potion.Potency[2];
                    break;
            }
        }

        //Set Item Name
        string n = prefix + " " + baseName + " of " + suffix;
        if(type == Item.ItemType.Potion)
        {
            switch (tier)
            {
                case Item.ItemTier.Common:
                    prefix = "Weak";
                    break;
                case Item.ItemTier.Uncommon:
                    prefix = "Regular";
                    break;
                case Item.ItemTier.Rare:
                    prefix = "Strong";
                    break;
            }
            n = prefix + " " + baseName;
        }
        gameObject.name = n;
        fullName = n;
    }

    void PullFromBase(I_ItemBase b)
    {
        baseName = b.m_name;
        armourRating = b.m_armourRating;
        damage = b.m_damage;
        health = b.m_health;
        speed = b.m_speed;
    }

    void AssignAffix(I_ItemAffix a, bool isPrefix)
    {
        if (isPrefix)
        {
            prefix = a.m_name;
        }
        else
        {
            suffix = a.m_name;
        }

        switch (a.m_targetStat)
        {
            case I_ItemAffix.StatTarget.Armour:
                if(tier == Item.ItemTier.Common)
                {
                    armourRating += Random.Range(a.m_t0_minMaxValue[0], a.m_t0_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Uncommon)
                {
                    armourRating += Random.Range(a.m_t1_minMaxValue[0], a.m_t1_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Rare)
                {
                    armourRating += Random.Range(a.m_t2_minMaxValue[0], a.m_t2_minMaxValue[1]);
                }
                break;
            case I_ItemAffix.StatTarget.Damage:
                if (tier == Item.ItemTier.Common)
                {
                    damage += Random.Range(a.m_t0_minMaxValue[0], a.m_t0_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Uncommon)
                {
                    damage += Random.Range(a.m_t1_minMaxValue[0], a.m_t1_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Rare)
                {
                    damage += Random.Range(a.m_t2_minMaxValue[0], a.m_t2_minMaxValue[1]);
                }
                break;
            case I_ItemAffix.StatTarget.Health:
                if (tier == Item.ItemTier.Common)
                {
                    health += Random.Range(a.m_t0_minMaxValue[0], a.m_t0_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Uncommon)
                {
                    health += Random.Range(a.m_t1_minMaxValue[0], a.m_t1_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Rare)
                {
                    health += Random.Range(a.m_t2_minMaxValue[0], a.m_t2_minMaxValue[1]);
                }
                break;
            case I_ItemAffix.StatTarget.Speed:
                if (tier == Item.ItemTier.Common)
                {
                    speed += Random.Range(a.m_t0_minMaxValue[0], a.m_t0_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Uncommon)
                {
                    speed += Random.Range(a.m_t1_minMaxValue[0], a.m_t1_minMaxValue[1]);
                }
                if (tier == Item.ItemTier.Rare)
                {
                    speed += Random.Range(a.m_t2_minMaxValue[0], a.m_t2_minMaxValue[1]);
                }
                break;
        }
    }

    public void LoadStats()
    {

    }

    public void GetStats()
    {

    }
}

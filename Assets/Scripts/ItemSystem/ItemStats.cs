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

    public Item.ItemSubType subType;

    public bool custom = false;

    //Item Meshes
    public Mesh armourMesh, weaponMesh, potionMesh;
    MeshFilter currentMeshFilter;

    //Rigidbody
    private Rigidbody rb;

    //Only for Potions
    [Header("Only for Potions")]
    public float potency;

    [Header("The text pop up for the item")]
    public ItemNamePopup INP;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentMeshFilter = GetComponentInChildren<MeshFilter>();
        AssignStats();
        GetComponentInChildren<Animation>().Play();
    }

    private void Update()
    {
        //Test Live
        //AssignStats();
    }

    int ReturnIndexForSubTypeToBase(Item.ItemSubType itemSubType)
    {
        int returnVal = 0;
        if(type == Item.ItemType.Armour)
        {
            switch (itemSubType)
            {
                case Item.ItemSubType.LightArmour:
                    returnVal = 0;
                    break;
                case Item.ItemSubType.MediumArmour:
                    returnVal = 1;
                    break;
                case Item.ItemSubType.HeavyArmour:
                    returnVal = 2;
                    break;
            }
        }

        if (type == Item.ItemType.Weapon)
        {
            switch (itemSubType)
            {
                case Item.ItemSubType.Spear:
                    returnVal = 0;
                    break;
                case Item.ItemSubType.Sword:
                    returnVal = 1;
                    break;
                case Item.ItemSubType.Bow:
                    returnVal = 2;
                    break;
            }
        }
        if (type == Item.ItemType.Potion)
        {
            switch (itemSubType)
            {
                case Item.ItemSubType.HealthPot:
                    returnVal = 0;
                    break;
                case Item.ItemSubType.SpeedPot:
                    returnVal = 1;
                    break;
                case Item.ItemSubType.DamagePot:
                    returnVal = 2;
                    break;
            }
        }
        //print("return val: " + returnVal);
        return returnVal;
    }

    //For assigning fresh stats
    public void AssignStats()
    {
        //Assign base stats based on type
        switch (type)
        {
            case Item.ItemType.Armour:
                var rng = Random.Range(0, Armour.Base.Length);
                if (!custom)
                {
                    PullFromBase(Armour.Base[rng]);
                    MessyAssignSubType(rng, type);
                }
                else
                {
                    PullFromBase(Armour.Base[ReturnIndexForSubTypeToBase(subType)]);
                }
                currentMeshFilter.mesh = armourMesh;
                break;
            case Item.ItemType.Weapon:
                var rng1 = Random.Range(0, Weapon.Base.Length);
                if (!custom)
                {
                    PullFromBase(Weapon.Base[rng1]);
                    MessyAssignSubType(rng1, type);
                }
                else
                {
                    PullFromBase(Weapon.Base[ReturnIndexForSubTypeToBase(subType)]);
                }
                currentMeshFilter.mesh = weaponMesh;
                currentMeshFilter.transform.localScale = new Vector3(4f, 4f, 4f);
                break;
            case Item.ItemType.Potion:
                var rng2 = Random.Range(0, Potion.Base.Length);
                if (!custom)
                {
                    PullFromBase(Potion.Base[rng2]);
                    MessyAssignSubType(rng2, type);
                }
                else
                {
                    PullFromBase(Potion.Base[ReturnIndexForSubTypeToBase(subType)]);
                }
                currentMeshFilter.mesh = potionMesh;
                currentMeshFilter.transform.localScale = new Vector3(15f, 15f, 15f);
                break;
        }

        //Choose prefix and suffix based on base
        //Assign tier- then add stats accordingly
        //Armour
        if (type == Item.ItemType.Armour)
        {
            //Assign prefix
            AssignAffix(Armour.Prefixes[Random.Range(0, Armour.Prefixes.Length)], true);
            //Assign suffix
            AssignAffix(Armour.Suffixes[Random.Range(0, Armour.Suffixes.Length)], false);

        }
        //Weapon
        if (type == Item.ItemType.Weapon)
        {
            AssignAffix(Weapon.Prefixes[Random.Range(0, Weapon.Prefixes.Length)], true);
            AssignAffix(Weapon.Suffixes[Random.Range(0, Weapon.Suffixes.Length)], false);
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

        //Update pop up
        INP.AssignTextToPopUp(fullName);
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

    public void LoadStats(Item itemObject)
    {
        tier = itemObject.Tier;
        type = itemObject.Type;
        armourRating = itemObject.ArmourRating;
        health = itemObject.Health;
        damage = itemObject.Damage;
        speed = itemObject.Speed;
        baseName = itemObject.BaseName;
        prefix = itemObject.Prefix;
        suffix = itemObject.Suffix;
        potency = itemObject.Potency;
        subType = itemObject.SubType;
        switch (type)
        {
            case Item.ItemType.Armour:
                currentMeshFilter.mesh = armourMesh;
                break;
            case Item.ItemType.Weapon:
                currentMeshFilter.mesh = weaponMesh;
                currentMeshFilter.transform.localScale = new Vector3(4f, 4f, 4f);
                break;
            case Item.ItemType.Potion:
                currentMeshFilter.mesh = potionMesh;
                currentMeshFilter.transform.localScale = new Vector3(15f, 15f, 15f);
                break;
        }

        //Set Item Name
        string n = prefix + " " + baseName + " of " + suffix;
        if (type == Item.ItemType.Potion)
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

        //Update pop up
        INP.AssignTextToPopUp(fullName);
    }

    public Item GetStats()
    {
        Item itemOut = new Item();

        //Assign all variables
        itemOut.Tier = tier;
        itemOut.Type = type;
        itemOut.ArmourRating = armourRating;
        itemOut.Health = health;
        itemOut.Damage = damage;
        itemOut.Speed = speed;
        itemOut.Potency = potency;
        itemOut.BaseName = baseName;
        itemOut.Prefix = prefix;
        itemOut.Suffix = suffix;
        itemOut.SubType = subType;
        //Debug.Log(subType);
        return itemOut;
    }

    void MessyAssignSubType(int index, Item.ItemType parentType)
    {
        switch (parentType)
        {
            case Item.ItemType.Armour:
                switch (index)
                {
                    case 0:
                        subType = Item.ItemSubType.LightArmour;
                        break;
                    case 1:
                        subType = Item.ItemSubType.MediumArmour;
                        break;
                    case 2:
                        subType = Item.ItemSubType.HeavyArmour;
                        break;
                    case 3:
                        subType = Item.ItemSubType.LightArmour;
                        break;
                    case 4:
                        subType = Item.ItemSubType.MediumArmour;
                        break;
                    case 5:
                        subType = Item.ItemSubType.HeavyArmour;
                        break;
                }
                break;
            case Item.ItemType.Potion:
                switch (index)
                {
                    case 0:
                        subType = Item.ItemSubType.HealthPot;
                        break;
                    case 1:
                        subType = Item.ItemSubType.SpeedPot;
                        break;
                    case 2:
                        subType = Item.ItemSubType.DamagePot;
                        break;
                }
                break;
            case Item.ItemType.Weapon:
                switch (index)
                {
                    case 0:
                        subType = Item.ItemSubType.Spear;
                        break;
                    case 1:
                        subType = Item.ItemSubType.Sword;
                        break;
                    case 2:
                        subType = Item.ItemSubType.Bow;
                        break;
                }
                break;
        }
    }


    //public Item GetStats()
    //{
    //    Item itemOut = new Item();
    //    switch (type)
    //    {
    //        case Item.ItemType.Armour:
    //            itemOut = new Armour();
    //            break;
    //        case Item.ItemType.Weapon:
    //            itemOut = new Weapon();
    //            break;
    //        case Item.ItemType.Potion:
    //            itemOut = new Potion();
    //            break;
    //        default:
    //            break;
    //    }
    //    //Assign all variables
    //    itemOut.Tier = tier;
    //    itemOut.Type = type;
    //    itemOut.ArmourRating = armourRating;
    //    itemOut.Health = health;
    //    itemOut.Damage = damage;
    //    itemOut.Speed = speed;
    //    itemOut.Potency = potency;
    //    itemOut.BaseName = baseName;
    //    itemOut.Prefix = prefix;
    //    itemOut.Suffix = suffix;
    //    return itemOut;
    //}
}

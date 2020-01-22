using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
public abstract class PItemBase
{
    //Variables
    abstract public string FullName { get; set; }
    abstract public string Description { get; set; }
    abstract public string Affix { get; set; }
    abstract public string Suffix { get; set; }
    abstract public int Type { get; set; }
    abstract public int Tier { get; set; }
}
class PArmour : PItemBase
{

    //Base Variables 
    public override string FullName { get; set; }
    public override string Description { get; set; }
    public override int Type { get; set; }
    public override int Tier { get; set; }
    public override string Affix { get; set; }
    public override string Suffix { get; set; }

    //Specific variables
    public float ArmourRating { get; set; }
    public float Speed { get; set; }
    public float Health { get; set; }
}
class PWeapon : PItemBase
{

    //Base Variables 
    public override string FullName { get; set; }
    public override string Description { get; set; }
    public override string Affix { get; set; }
    public override string Suffix { get; set; }
    public override int Type { get; set; }
    public override int Tier { get; set; }

    //Specific variables
    public float ArmourRating { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
}
class PPotion : PItemBase
{
    //With the potions I wasnt sure which fields to add, so for now I've added Damage, Health, and Armour as I assume they're going to be the greatest variables effected.

    //Base Variables 
    public override string FullName { get; set; }
    public override string Description { get; set; }
    public override string Affix { get; set; }
    public override string Suffix { get; set; }
    public override int Type { get; set; }
    public override int Tier { get; set; }

    //Specific variables
    public float Damage { get; set; }
    public float Health { get; set; }
    public float ArmourRating { get; set; }
    public float Potency { get; set; }
}



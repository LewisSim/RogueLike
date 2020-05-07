using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables
    public int health = 100;

    //Methods
    public void AddDamage(int damage)
    {
            health -= damage;
            print(damage.ToString() + " Damage taken!");
            checkHealth();
        gameObject.GetComponent<SoundAtSource>().indexOverride = 7;
        gameObject.GetComponent<SoundAtSource>().TriggerSound();
    }
    public void checkHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            gameObject.GetComponent<SoundAtSource>().indexOverride = 6;
            gameObject.GetComponent<SoundAtSource>().TriggerSoundAtUI();
            gameObject.GetComponent<Drop>().DropItem();
        }
    }
}

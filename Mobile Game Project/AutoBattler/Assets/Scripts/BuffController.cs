using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{

    [SerializeField] int cost;
    [SerializeField] int health;
    [SerializeField] int damage;

    public void UpdateCharacter(ICharacter character)
    {
        character.GiveDamage(damage);
        character.GiveHealth(health);
    }

    public int GetCost()
    {
        return cost;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{

    [SerializeField] string title;
    [TextArea(4,8)]
    [SerializeField] string description;
    [SerializeField] int cost;
    [SerializeField] int health;
    [SerializeField] int damage;

    public void UpdateCharacter(ICharacter character)
    {
        character.GiveDamage(damage);
        character.GiveHealth(health);
    }

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetCost()
    {
        return cost;
    }

}

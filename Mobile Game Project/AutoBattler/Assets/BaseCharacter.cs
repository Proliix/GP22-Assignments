using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour, ICharacter
{
    public int charNumber;
    public CharacterStats baseStats;
    public int extraHealth = 0;
    public int extraDamage = 0;

    bool isDead = false;
    int currentHealth = 0;
    int currentDamage = 0;

    protected void Start()
    {
        currentHealth = baseStats.Health + extraHealth;
        currentDamage = baseStats.Damage + extraDamage;
    }
    public void Attack(ICharacter character)
    {
        Debug.Log("Deals " + currentDamage + " damage!");
        if (!isDead)
            character.TakeDamage(currentDamage);
    }

    public void AttackEffect()
    {
        throw new System.NotImplementedException();
    }

    public void Death()
    {
        Debug.Log(gameObject.name + " IS DEAD");
        isDead = true;
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void GiveHealth(int health)
    {
        currentHealth += health;
    }

    public void PassiveEffect()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Death();

    }

    public bool GetIsDead()
    {
        return isDead;
    }
}

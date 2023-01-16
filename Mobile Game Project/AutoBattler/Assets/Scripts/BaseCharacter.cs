using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class BaseCharacter : MonoBehaviour, ICharacter
{
    public int charNumber;
    public CharacterStats baseStats;

    [SerializeField] int extraHealth = 0;
    [SerializeField] int extraDamage = 0;
    [SerializeField] TextMeshPro hpText;
    [SerializeField] TextMeshPro damageText;

    bool isDead = false;
    int currentHealth = 0;
    int currentDamage = 0;

    Animator anim;

    protected void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sprite = baseStats.CharacterSprite;
        StatDisplayManager.Instance.GetStatDisplayer(transform, new Vector3(0, -1.5f, 0), ref hpText, ref damageText, false);
        UpdateStats();
    }
    public void Attack(ICharacter character)
    {
        Debug.Log("Deals " + currentDamage + " damage!");
        character.TakeDamage(currentDamage);
        anim.SetTrigger("Attack");
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
        UpdateStats();
    }

    public void PassiveEffect()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HitNumbers.Instance.GetHitNumber(gameObject.transform.position, damage);
        hpText.text = currentHealth.ToString();
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public bool CheckDead()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
        return isDead;
    }

    public void UpdateStats()
    {
        currentHealth = baseStats.Health + extraHealth;
        currentDamage = baseStats.Damage + extraDamage;
        hpText.text = currentHealth.ToString();
        damageText.text = currentDamage.ToString();
    }
}

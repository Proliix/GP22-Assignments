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
    [SerializeField] string key;

    bool isActive = false;
    bool isDead = false;
    int currentHealth = 0;
    int currentDamage = 0;

    GameController gameController;
    SpriteRenderer sr;
    Animator anim;
    int displayIndex = -1;
    protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        UpdateCharacter();
        //if (hpText == null)
        //    StartCoroutine(FindStatDisplayer());
    }

    public void UpdateCharacter()
    {
        anim = gameObject.GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sprite = baseStats.CharacterSprite;
        UpdateStats();

    }


    IEnumerator FindStatDisplayerEnum()
    {
        while (hpText == null)
        {
            if (StatDisplayManager.Instance != null)
            {
                StatDisplayManager.Instance?.GetStatDisplayer(transform, new Vector3(0, -1.5f, 0), ref hpText, ref damageText, ref displayIndex, false);
                if (hpText != null)
                {
                    UpdateStats();
                }
            }
            yield return null;
        }
    }


    public void Attack(ICharacter character)
    {
        if (gameObject.activeSelf)
        {
            Debug.Log("Deals " + currentDamage + " damage!");
            character.TakeDamage(currentDamage);
            anim.SetTrigger("Attack");
        }
    }

    public void AttackEffect()
    {
        throw new System.NotImplementedException();
    }

    public void Death()
    {
        Debug.Log(gameObject.name + " IS DEAD");
        isDead = true;
        StatDisplayManager.Instance.ResetDisplay(displayIndex);
        displayIndex = -1;
        gameObject.SetActive(false);
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void GiveHealth(int health)
    {
        extraHealth += health;
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
        if (hpText != null)
            hpText.text = currentHealth.ToString();
        else
        {
            Debug.Log("Is here");
            StatDisplayManager.Instance?.GetStatDisplayer(transform, new Vector3(0, -1.5f, 0), ref hpText, ref damageText, ref displayIndex, false);
            UpdateStats();
        }
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
        if (hpText != null)
        {
            hpText.text = currentHealth.ToString();
            damageText.text = currentDamage.ToString();
        }
        key = GetCharacterKey();
    }

    public void GiveDamage(int damage)
    {
        extraDamage += damage;
        UpdateStats();
    }

    public void ChangeStats(CharacterStats newStats)
    {
        baseStats = newStats;
        UpdateCharacter();
    }

    public string GetCharacterKey()
    {
        string charKey = "";

        charKey += isActive ? baseStats.Index : -1;
        charKey += "|";
        charKey += extraHealth;
        charKey += "|";
        charKey += extraDamage;

        return charKey;
    }


    public void InitializeFromKey(string key)
    {
        int dividerIndex = 0;
        string indexStr = "";
        int index = 0;
        char[] keyarr = key.ToCharArray();
        for (int i = 0; i < keyarr.Length; i++)
        {
            if (keyarr[i] == '|')
            {
                dividerIndex = i;
                break;
            }
            else
            {
                indexStr += keyarr[i];
            }
        }
        index = int.Parse(indexStr);

        ChangeIsActive(index >= 0 ? true : false);

        int health = 0;
        string healthStr = "";
        for (int i = dividerIndex + 1; i < keyarr.Length; i++)
        {
            if (keyarr[i] == '|')
            {
                dividerIndex = i;
                break;
            }
            else
            {
                healthStr += keyarr[i];
            }
        }
        health = int.Parse(healthStr);
        string damageStr = "";
        int damage = 0;
        for (int i = dividerIndex + 1; i < keyarr.Length; i++)
        {
            damageStr += keyarr[i];
        }

        damage = int.Parse(damageStr);

        if (gameController == null)
        {
            gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        }

        if (index >= 0)
        {
            baseStats = gameController.GetStatsFromIndex(index);
            extraHealth = health;
            extraDamage = damage;
            UpdateCharacter();
        }
    }

    public void FindStatDisplayer()
    {
        if (displayIndex < 0)
        {
            StatDisplayManager.Instance.GetStatDisplayer(transform, new Vector3(0, -1.5f, 0), ref hpText, ref damageText, ref displayIndex, false);
            Debug.Log(hpText.gameObject.name);
            if (hpText == null)
                Debug.Log("Is here");
        }

        UpdateStats();
    }

    public void ResetCharacter()
    {
        gameObject.SetActive(true);
        currentHealth = baseStats.Health + extraHealth;
        currentDamage = baseStats.Damage + extraDamage;
        isDead = false;
        hpText = null;
        damageText = null;
        displayIndex = -1;
    }

    public CharacterStats GetStats()
    {
        return baseStats;
    }

    public int GetCost()
    {
        return baseStats.Cost;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public void ChangeIsActive(bool value)
    {
        gameObject.SetActive(value);
        isActive = value;
    }
}

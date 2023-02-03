using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum CharacterType { Normal, Splitter }

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
    int level = 0;

    GameObject currentStatDisplayer;

    CharacterHolder holder;
    GameController gameController;
    SpriteRenderer sr;
    Animator anim;
    CharacterType characterType = CharacterType.Normal;
    int displayIndex = -1;
    protected void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        holder ??= transform.parent.GetComponent<CharacterHolder>();
        UpdateCharacter();
        //if (hpText == null)
        //    StartCoroutine(FindStatDisplayer());
    }

    public void UpdateCharacter()
    {
        anim = gameObject.GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sprite = baseStats.CharacterSpriteLevel1;
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
        if (character.GetIsActive() && isActive)
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
        switch (characterType)
        {

            case CharacterType.Splitter:
                StatDisplayManager.Instance.ResetDisplay(displayIndex);
                displayIndex = -1;
                transform.localScale = Vector3.one * 0.75f;
                FindStatDisplayer();
                currentDamage = 1;
                currentHealth = 1;
                characterType = CharacterType.Normal;
                UpdateDisplayer();
                break;
            default:
                isDead = true;
                StatDisplayManager.Instance.ResetDisplay(displayIndex);
                displayIndex = -1;
                gameObject.SetActive(false);
                break;
        }
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
            FindStatDisplayer();
        }
    }

    public bool GetIsDead()
    {
        bool returnBool = isActive ? isDead : true;

        return returnBool;
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
        int statDamage = Mathf.CeilToInt((baseStats.Damage * level) / 2);
        int statHealth = Mathf.CeilToInt((baseStats.Health * level) / 2);
        currentHealth = baseStats.Health + level + extraHealth;
        currentDamage = baseStats.Damage + level + extraDamage;
        UpdateDisplayer();
        key = GetCharacterKey();
    }

    public void GiveDamage(int damage)
    {
        extraDamage += damage;
        UpdateStats();
    }

    public void ChangeStats(CharacterStats newStats)
    {
        ChangeIsActive(true);
        baseStats = newStats;
        characterType = baseStats.Type;
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
        charKey += "|";
        charKey += level;

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
            if (keyarr[i] == '|')
            {
                dividerIndex = i;
                break;
            }
            else
            {
                damageStr += keyarr[i];
            }
        }
        damage = int.Parse(damageStr);
        string levelStr = "";
        int newlevel = 0;
        for (int i = dividerIndex + 1; i < keyarr.Length; i++)
        {
            levelStr += keyarr[i];
        }
        newlevel = int.Parse(levelStr);
        if (gameController == null)
        {
            gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        }

        if (index >= 0)
        {
            baseStats = gameController.GetStatsFromIndex(index);
            extraHealth = health;
            extraDamage = damage;
            level = newlevel;
            characterType = baseStats.Type;
            UpdateCharacter();
        }
    }

    public void FindStatDisplayer()
    {
        if (displayIndex < 0)
        {
            currentStatDisplayer = StatDisplayManager.Instance.GetStatDisplayer(transform, new Vector3(0, -1.5f, 0), ref hpText, ref damageText, ref displayIndex, false);
            if (hpText == null)
                Debug.Log("Is here");
        }

        UpdateStats();

    }

    public void ResetCharacter()
    {
        gameObject.SetActive(true);
        characterType = baseStats.Type;
        transform.localScale = Vector3.one;
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

        if (holder != null)
            holder.hasCharacter = value;
        else if (holder = transform.parent.GetComponent<CharacterHolder>())
            holder.hasCharacter = value;


        gameObject.SetActive(value);
        isActive = value;
    }

    void UpdateDisplayer()
    {
        if (hpText != null)
        {
            hpText.text = currentHealth.ToString();
            damageText.text = currentDamage.ToString();
        }
    }

    public void Upgrade()
    {
        level++;
        UpdateStats();
    }
}

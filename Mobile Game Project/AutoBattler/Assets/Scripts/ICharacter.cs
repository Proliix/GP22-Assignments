using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public void Attack(ICharacter character);
    public void ChangeStats(CharacterStats newStats);
    public void UpdateStats();
    public void UpdateCharacter();
    public void AttackEffect();
    public void Death();
    public void TakeDamage(int damage);
    public void PassiveEffect();
    public void GiveHealth(int health);
    public void GiveDamage(int damage);
    public bool CheckDead();
    public void ResetCharacter();
    public int GetHealth();
    public bool GetIsDead();
    public void InitializeFromKey(string key);
    public string GetCharacterKey();
    public void FindStatDisplayer();
    public CharacterStats GetStats();
    public int GetCost();
}

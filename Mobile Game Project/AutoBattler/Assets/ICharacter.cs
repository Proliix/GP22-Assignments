using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public void Attack(ICharacter character);
    public void AttackEffect();
    public void Death();
    public void TakeDamage(int damage);
    public void PassiveEffect();
    public void GiveHealth(int health);
    public int GetHealth();
}

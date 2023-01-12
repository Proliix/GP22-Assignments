using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public int Damage;
    public int Health;
    public Sprite CharacterSprite;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [HideInInspector]
    public int Index;
    public int Cost = 2;
    public int Damage;
    public int Health;
    public Sprite CharacterSprite;
}

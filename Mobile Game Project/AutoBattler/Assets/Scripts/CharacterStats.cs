using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [HideInInspector]
    public int Index;
    public string Name;
    [TextArea(4,8)]
    public string Description;
    public int Cost = 2;
    public int Damage;
    public int Health;
    public Sprite CharacterSpriteLevel1;
    public Sprite CharacterSpriteLevel2;
    public Sprite CharacterSpriteLevel3;
    public CharacterType Type = CharacterType.Normal;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    public bool hasCharacter;
    [SerializeField] GameObject characterObj;

    ICharacter character;

    // Start is called before the first frame update
    void Start()
    {
        character = characterObj.GetComponent<ICharacter>();
    }
    public GameObject GetCharacterObj()
    {
        return characterObj;
    }

    public void SetCharacterObjActive(bool value)
    {
        characterObj.SetActive(value);
        hasCharacter = value;
    }

    public ICharacter GetCharacter()
    {
        if (character == null)
            character = characterObj.GetComponent<ICharacter>();
        
        return character;
    }
}

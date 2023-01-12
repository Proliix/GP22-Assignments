using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] List<GameObject> allCharacters;

    [SerializeField] List<GameObject> playerBoard;
    [SerializeField] List<GameObject> enemyBoard;

    ICharacter[] playerCharacters;
    ICharacter[] enemyCharacters;
    int playerIndex = 0;
    int enemyIndex = 0;

    float timer = 0;

    private void Start()
    {
        LoadData();
    }

    void LoadData()
    {
        //IMPLEMENT DATA READING

        UpdateCharacters();
    }

    void UpdateCharacters()
    {
        playerCharacters = new ICharacter[playerBoard.Count];
        enemyCharacters = new ICharacter[enemyBoard.Count];
        for (int i = 0; i < playerBoard.Count; i++)
        {
            if (playerBoard[i].GetComponent<ICharacter>() != null)
                playerCharacters[i] = playerBoard[i].GetComponent<ICharacter>();
            else
                Debug.LogError("<color=red>ERROR:</color> No ICharacter on playerBoard: " + playerBoard[i].name + " in slot " + i);
        }
        for (int i = 0; i < enemyBoard.Count; i++)
        {
            if (enemyBoard[i].GetComponent<ICharacter>() != null)
                enemyCharacters[i] = enemyBoard[i].GetComponent<ICharacter>();
            else
                Debug.LogError("<color=red>ERROR:</color> No ICharacter on enemyBoard: " + enemyBoard[i].name + " in slot " + i);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            timer = 0;
            Attacks();
        }
    }

    void Attacks()
    {
        Debug.Log(playerBoard[playerIndex].name + " Attacks " + enemyBoard[enemyIndex].name);
        playerCharacters[playerIndex].Attack(enemyCharacters[enemyIndex]);
        Debug.Log(enemyBoard[enemyIndex].name + " Attacks " + playerBoard[playerIndex].name);
        enemyCharacters[enemyIndex].Attack(playerCharacters[playerIndex]);

        if (playerCharacters[playerIndex].GetHealth() <= 0)
            playerIndex = (playerIndex + 1) >= playerCharacters.Length ? playerCharacters.Length - 1 : playerIndex + 1;
        if (enemyCharacters[enemyIndex].GetHealth() <= 0)
            enemyIndex = (enemyIndex + 1) >= enemyCharacters.Length ? enemyCharacters.Length - 1 : enemyIndex + 1;

    }

}

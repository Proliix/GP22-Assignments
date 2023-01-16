using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] CharacterStats[] allCharacters;

    [SerializeField] List<GameObject> playerBoard;
    [SerializeField] List<GameObject> enemyBoard;
    [SerializeField] TextMeshProUGUI winText;

    [Header("UI")]
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Image buttonImage;

    ICharacter[] playerCharacters;
    ICharacter[] enemyCharacters;
    int playerIndex = 0;
    int enemyIndex = 0;


    bool playerDead;
    bool enemyDead;

    int round = 0;
    bool hasPlayedRound;
    bool hasStarted = false;

    float timer = 0;

    private void Start()
    {
        for (int i = 0; i < allCharacters.Length; i++)
        {
            allCharacters[i].Index = i;
        }

        winText.text = "";
        LoadData();
    }


    public void ToggleGameState()
    {
        hasStarted = !hasStarted;
        buttonImage.sprite = hasStarted ? pauseSprite : playSprite;
    }

    public void StartGame()
    {
        hasStarted = true;
        buttonImage.sprite = pauseSprite;
    }

    public void StopGame()
    {
        hasStarted = false;
        buttonImage.sprite = playSprite;
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
        if (timer >= attackCooldown && hasStarted)
        {
            timer = 0;
            PlayRound();
        }

    }

    void PlayRound()
    {
        Attacks();
        CheckDeaths();
    }

    void CheckDeaths()
    {
        playerCharacters[playerIndex].CheckDead();
        enemyCharacters[enemyIndex].CheckDead();
        if (playerCharacters[playerIndex].GetIsDead())
            playerIndex = (playerIndex + 1) >= playerCharacters.Length ? playerCharacters.Length - 1 : playerIndex + 1;
        if (enemyCharacters[enemyIndex].GetIsDead())
            enemyIndex = (enemyIndex + 1) >= enemyCharacters.Length ? enemyCharacters.Length - 1 : enemyIndex + 1;

        playerDead = playerCharacters[playerCharacters.Length - 1].GetIsDead();
        enemyDead = enemyCharacters[enemyCharacters.Length - 1].GetIsDead();

        if (playerDead || enemyDead)
        {
            winText.text = playerDead ? "<color=red>YOU LOOSE!</color>" : "<color=yellow>YOU WIN!</color>";
            winText.text = playerDead == enemyDead ? "DRAW" : winText.text;
        }

    }

    void Attacks()
    {
        if (!playerDead && !enemyDead)
        {

            Debug.Log(playerBoard[playerIndex].name + " Attacks " + enemyBoard[enemyIndex].name);
            playerCharacters[playerIndex].Attack(enemyCharacters[enemyIndex]);
            Debug.Log(enemyBoard[enemyIndex].name + " Attacks " + playerBoard[playerIndex].name);
            enemyCharacters[enemyIndex].Attack(playerCharacters[playerIndex]);
        }
    }

}

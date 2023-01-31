using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum GameState { Shop, Duel, Intermission }

public class GameController : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] CharacterStats[] allCharacters;
    [SerializeField] TextMeshProUGUI roundText, healthText, winsText;

    [Header("Shop")]
    [SerializeField] int startCoins = 6;
    [SerializeField] int shopRefreshCost = 1;
    [SerializeField] GameObject shopHolder;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] List<GameObject> playerBoardShop;
    [SerializeField] List<GameObject> shopBoard;
    [SerializeField] List<GameObject> playerHolderShop;

    [Header("Duel")]
    [SerializeField] int health = 5;
    [SerializeField] GameObject duelHolder;
    [SerializeField] List<GameObject> playerBoard;
    [SerializeField] List<GameObject> enemyBoard;
    [SerializeField] TextMeshProUGUI winText;

    [Header("UI")]
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Image playButtonImage;
    [SerializeField] Sprite lockedLock, openLock;
    [SerializeField] Image LockButtonImage;

    [SerializeField]
    GameState currentState, nextState;
    ICharacter[] playerCharacters;
    ICharacter[] enemyCharacters;

    FireBaseManager fbm;

    ICharacter[] playerCharactersShop;
    ICharacter[] charactersShop;
    int playerIndex = 0;
    int enemyIndex = 0;
    int coins = 0;
    bool lockedShop = false;
    string lockedShopKey;

    bool playerDead;
    bool enemyDead;

    bool loadingScene;

    int round = 0;
    int wins = 0;
    int lastActivePlayer;
    int lastActiveEnemy;
    bool hasPlayedRound;
    bool hasStarted = false;

    public string debugString;
    string currentTeamKey;
    float timer = 0;

    private void Start()
    {
        fbm = gameObject.GetComponent<FireBaseManager>();

        for (int i = 0; i < allCharacters.Length; i++)
        {
            allCharacters[i].Index = i;
        }

        winText.text = "";
        UpdateCoinText();
        //LoadData();
        LoadShop();
    }


    public void ToggleRoundState()
    {
        if (currentState == GameState.Duel)
            hasStarted = !hasStarted;
        playButtonImage.sprite = hasStarted ? pauseSprite : playSprite;
    }

    public void StartGame()
    {
        hasStarted = true;
        playButtonImage.sprite = pauseSprite;
    }

    public void StopGame()
    {
        hasStarted = false;
        playButtonImage.sprite = playSprite;
    }

    void LoadData()
    {
        //IMPLEMENT DATA READING

        UpdateDuelCharacters();
    }

    void UpdateDuelCharacters()
    {
        playerCharacters = new ICharacter[playerBoard.Count];
        enemyCharacters = new ICharacter[enemyBoard.Count];
        for (int i = 0; i < playerBoard.Count; i++)
        {
            if (playerBoard[i].GetComponent<CharacterHolder>().GetCharacter() != null)
            {
                playerCharacters[i] = playerBoard[i].GetComponent<CharacterHolder>().GetCharacter();
            }
            else
                Debug.LogError("<color=red>ERROR:</color> No ICharacter on playerBoard: " + playerBoard[i].name + " in slot " + i);
        }
        for (int i = 0; i < enemyBoard.Count; i++)
        {
            if (enemyBoard[i].GetComponent<CharacterHolder>().GetCharacter() != null)
            {
                enemyCharacters[i] = enemyBoard[i].GetComponent<CharacterHolder>().GetCharacter();
            }
            else
                Debug.LogError("<color=red>ERROR:</color> No ICharacter on enemyBoard: " + enemyBoard[i].name + " in slot " + i);
        }

        if (round > 0)
            ResetDuel();

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            playerCharacters[i].FindStatDisplayer();
        }

        CreateNewEnemyTeam(round > 3 ? true : false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(GetTeamKey(playerCharacters));
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            InitializeTeamFromTeamKey(debugString, playerBoard.ToArray(), playerCharacters);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            fbm.SaveTeam(1, 1, GetTeamKey(playerCharacters));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            fbm.LoadTeam(1, 1);
        }

        if (nextState == currentState)
        {

            switch (currentState)
            {
                case GameState.Shop:
                    break;
                case GameState.Duel:
                    timer += Time.deltaTime;
                    if (timer >= attackCooldown && hasStarted)
                    {
                        timer = 0;
                        PlayRound();
                    }
                    break;
            }
        }
    }

    bool IsPlayerTeamEmpty()
    {
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (playerCharacters[i].GetIsActive())
                return false;
        }
        return true;
    }


    void UpdateText()
    {
        healthText.text = "x" + health;
        winsText.text = "x" + wins;
        roundText.text = "x" + round;
    }

    void PlayRound()
    {
        Attacks();
        CheckDeaths();
    }

    public GameState GetGameState()
    {
        return currentState;
    }

    public void ChangeGameState(GameState newState)
    {
        if (currentState != GameState.Intermission)
        {

            if (newState != GameState.Intermission)
            {
                currentState = GameState.Intermission;
                nextState = newState;
                FadeController.Instance.FadeOut();
            }
            else
            {
                Debug.LogError("<color=red>ERROR: </color> YOU CAN'T SWITCH STATE TO INTERMISSION MANUALLY");
            }
        }
    }

    public void ChangeGameState(GameState newState, float fadeWait)
    {
        if (currentState != GameState.Intermission)
        {

            if (newState != GameState.Intermission)
            {
                currentState = GameState.Intermission;
                nextState = newState;
                StartCoroutine(CallFadeOutAfterSec(fadeWait));
            }
            else
            {
                Debug.LogError("<color=red>ERROR: </color> YOU CAN'T SWITCH STATE TO INTERMISSION MANUALLY");
            }
        }
    }
    public void ChangeGameState(int newStateNum)
    {
        if (currentState != GameState.Intermission)
        {

            GameState newState = (GameState)newStateNum;
            if (newState != GameState.Intermission)
            {
                currentState = GameState.Intermission;
                nextState = newState;
                FadeController.Instance.FadeOut();
            }
            else
            {
                Debug.LogError("<color=red>ERROR: </color> YOU CAN'T SWITCH STATE TO INTERMISSION MANUALLY");
            }
        }
    }

    IEnumerator CallFadeOutAfterSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        FadeController.Instance.FadeOut();
    }


    public void IntermissionComplete()
    {
        if (currentState == GameState.Intermission)
        {
            currentState = nextState;
            switch (currentState)
            {
                case GameState.Shop:
                    ResetDuel();
                    LoadShop();
                    break;
                case GameState.Duel:
                    ResetShop();
                    LoadDuel();
                    break;
            }
            FadeController.Instance.FadeIn();
        }
        else
        {
            Debug.LogError("<color=red>ERROR: </color> INTERMISSSION COMPLETE WHEN currentState = " + currentState);
        }
    }
    private void ResetDuel()
    {
        lastActiveEnemy = 0;
        lastActivePlayer = 0;
        for (int i = 0; i < playerBoard.Count; i++)
        {
            playerCharacters[i].ResetCharacter();
        }

        for (int i = 0; i < enemyBoard.Count; i++)
        {
            enemyCharacters[i].ResetCharacter();
        }
    }

    void LoadDuel()
    {
        currentTeamKey = GetTeamKey(playerCharactersShop);
        playerDead = false;
        enemyDead = false;
        winText.text = "";
        timer = -1;
        hasStarted = false;
        playButtonImage.sprite = hasStarted ? pauseSprite : playSprite;
        StatDisplayManager.Instance.ResetAll();
        shopHolder.SetActive(false);
        duelHolder.SetActive(true);
        LoadData();
        InitializeTeamFromTeamKey(currentTeamKey, playerBoard.ToArray(), playerCharacters);
        Debug.Log("Playerboard empty: " + IsPlayerTeamEmpty());
        FindLastActiveCharacters();
        FindStartIndex();
        CheckDeaths();
    }

    void ResetShop()
    {
        if (lockedShop)
            lockedShopKey = GetTeamKey(charactersShop, shopBoard.ToArray());

        for (int i = 0; i < playerCharactersShop.Length; i++)
        {
            playerCharactersShop[i].ResetCharacter();
        }
        for (int i = 0; i < charactersShop.Length; i++)
        {
            charactersShop[i].ResetCharacter();
        }

    }

    void LoadShop()
    {
        shopHolder.SetActive(true);
        duelHolder.SetActive(false);
        UpdateText();
        StatDisplayManager.Instance?.ResetAll();
        coins = startCoins;
        UpdateCoinText();
        playerCharactersShop = new ICharacter[playerBoardShop.Count];
        for (int i = 0; i < playerBoardShop.Count; i++)
        {
            playerCharactersShop[i] = playerBoardShop[i].GetComponent<CharacterHolder>().GetCharacter();
        }
        charactersShop = new ICharacter[shopBoard.Count];
        for (int i = 0; i < shopBoard.Count; i++)
        {
            charactersShop[i] = shopBoard[i].GetComponent<CharacterHolder>().GetCharacter();
        }
        if (round > 0)
            ResetShop();
        if (!lockedShop)
            ReloadShopItems();
        else
            InitializeTeamFromTeamKey(lockedShopKey, shopBoard.ToArray(), charactersShop);


        for (int i = 0; i < charactersShop.Length; i++)
        {
            charactersShop[i].FindStatDisplayer();
        }
        for (int i = 0; i < playerCharactersShop.Length; i++)
        {
            playerCharactersShop[i].FindStatDisplayer();
        }

        if (round > 0)
        {
            InitializeTeamFromTeamKey(currentTeamKey, playerBoardShop.ToArray(), playerCharactersShop);
            FadeController.Instance.FadeIn();
        }
        else
        {
            for (int i = 0; i < playerCharactersShop.Length; i++)
            {
                playerCharactersShop[i].ChangeIsActive(false);
            }
        }
    }

    public void RefreshShop()
    {
        if (BuyObject(shopRefreshCost))
        {
            ReloadShopItems();
        }
    }

    public void LockShopToggle()
    {
        lockedShop = !lockedShop;
        LockButtonImage.sprite = lockedShop ? lockedLock : openLock;
    }

    void ReloadShopItems()
    {

        for (int i = 0; i < shopBoard.Count; i++)
        {
            shopBoard[i].GetComponent<CharacterHolder>().SetCharacterObjActive(true);
            shopBoard[i].GetComponent<CharacterHolder>().hasCharacter = true;
        }
        for (int i = 0; i < charactersShop.Length; i++)
        {
            charactersShop[i].ChangeStats(allCharacters[Random.Range(0, allCharacters.Length)]);
            charactersShop[i].ChangeIsActive(true);
        }

    }

    public void RemoveCoins(int amount)
    {
        if (coins - amount >= 0)
            coins -= amount;

        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        coinText.text = "x" + coins;
    }

    public bool BuyObject(int Cost)
    {
        bool returnValue = true;
        if (coins - Cost >= 0)
        {
            returnValue = true;
            RemoveCoins(Cost);
        }
        else
        {
            returnValue = false;
        }
        return returnValue;
    }

    void CheckDeaths()
    {
        playerCharacters[playerIndex].CheckDead();
        enemyCharacters[enemyIndex].CheckDead();
        if (playerCharacters[playerIndex].GetIsDead())
            playerIndex = (playerIndex + 1) >= playerCharacters.Length ? playerCharacters.Length - 1 : playerIndex + 1;
        if (enemyCharacters[enemyIndex].GetIsDead())
            enemyIndex = (enemyIndex + 1) >= enemyCharacters.Length ? enemyCharacters.Length - 1 : enemyIndex + 1;

        if (!playerCharacters[playerIndex].GetIsActive())
        {
            for (int i = playerIndex; i < playerCharacters.Length; i++)
            {
                if (playerCharacters[i].GetIsActive())
                {
                    playerIndex = i;
                    break;
                }
                else
                {
                    playerCharacters[i].Death();
                }
            }
        }

        if (!enemyCharacters[enemyIndex].GetIsActive())
        {
            for (int i = enemyIndex; i < enemyCharacters.Length; i++)
            {
                if (enemyCharacters[i].GetIsActive())
                {
                    enemyIndex = i;
                    break;
                }
                else
                {
                    enemyCharacters[i].Death();
                }
            }
        }

        playerDead = playerCharacters[lastActivePlayer].GetIsDead();
        enemyDead = enemyCharacters[lastActiveEnemy].GetIsDead();

        if (playerDead || enemyDead)
        {
            winText.text = playerDead ? "<color=red>YOU LOSE!</color>" : "<color=yellow>YOU WIN!</color>";
            winText.text = playerDead == enemyDead ? "DRAW" : winText.text;
            hasStarted = false;
            round++;
            if (playerDead && !enemyDead)
                health--;
            else
                wins++;

            currentTeamKey = GetTeamKey(playerCharacters);
            ChangeGameState(GameState.Shop, 2f);
        }

    }


    public CharacterStats GetStatsFromIndex(int index)
    {
        return allCharacters[index];
    }

    public void SwapKeys(ICharacter char1, ICharacter char2)
    {
        string key1 = char1.GetCharacterKey();
        string key2 = char2.GetCharacterKey();
        char1.InitializeFromKey(key2);
        char2.InitializeFromKey(key1);
    }

    public string GetTeamKey(ICharacter[] characters)
    {
        string teamKey = "";
        for (int i = 0; i < characters.Length; i++)
        {

            teamKey += characters[i].GetCharacterKey();
            teamKey += '|';
        }
        return teamKey;
    }
    /// <summary>
    /// Creates a teamkey with the active characters
    /// </summary>
    /// <param name="characters">The characters that will have their keys taken</param>
    /// <param name="board">The board of gameobjects that is needed to check if they are active or not</param>
    /// <returns>The teamkey of active characters</returns>
    public string GetTeamKey(ICharacter[] characters, GameObject[] board)
    {
        string teamKey = "";
        for (int i = 0; i < characters.Length; i++)
        {
            if (board[i].GetComponent<CharacterHolder>().hasCharacter == true)
            {
                teamKey += characters[i].GetCharacterKey();
                teamKey += '|';
            }
        }
        return teamKey;
    }
    public void InitializeTeamFromTeamKey(string teamKey, GameObject[] board, ICharacter[] characters)
    {
        char[] teamArr = teamKey.ToCharArray();
        int numOfDividers = 0;
        int currentIndex = 0;
        string charKey = "";
        for (int i = 0; i < teamArr.Length; i++)
        {
            if (teamArr[i] == '|')
                numOfDividers++;

            if (numOfDividers == 3)
            {
                numOfDividers = 0;
                board[currentIndex].GetComponent<CharacterHolder>().SetCharacterObjActive(true);
                characters[currentIndex].InitializeFromKey(charKey);
                charKey = "";
                currentIndex++;
            }
            else
            {
                charKey += teamArr[i];
            }
        }

        if (currentIndex < characters.Length)
        {
            if (lockedShop && teamKey == lockedShopKey)
            {
                for (int i = currentIndex; i < characters.Length; i++)
                {
                    characters[i].ChangeStats(allCharacters[Random.Range(0, allCharacters.Length)]);
                    board[i].GetComponent<CharacterHolder>().SetCharacterObjActive(true);
                }
            }
            else
            {
                for (int i = currentIndex; i < board.Length; i++)
                {
                    board[i].GetComponent<CharacterHolder>().SetCharacterObjActive(false);
                }
            }
        }

    }

    void CreateNewEnemyTeam(bool scrambledPlayer)
    {
        if (scrambledPlayer)
        {
            InitializeTeamFromTeamKey(currentTeamKey, enemyBoard.ToArray(), enemyCharacters);

            int r1, r2;

            for (int i = 0; i < 5; i++)
            {
                r1 = Random.Range(0, enemyCharacters.Length);
                r2 = Random.Range(1, enemyCharacters.Length);
                if (r2 >= enemyCharacters.Length)
                    r2 -= enemyCharacters.Length;

                SwapKeys(enemyCharacters[r1], enemyCharacters[r2]);

            }
        }

        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            enemyCharacters[i].FindStatDisplayer();

            if (!scrambledPlayer)
                enemyCharacters[i].ChangeStats(allCharacters[Random.Range(0, allCharacters.Length)]);
        }
        FindStartIndex();
    }

    void ReloadScene()
    {
        loadingScene = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FindStartIndex()
    {
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (playerCharacters[i].GetIsActive())
            {
                playerIndex = i;
                break;
            }
        }
        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            if (enemyCharacters[i].GetIsActive())
            {
                enemyIndex = i;
                break;
            }
        }
    }

    void FindLastActiveCharacters()
    {
        for (int i = 0; i < enemyCharacters.Length; i++)
        {
            if (enemyCharacters[i].GetIsActive())
            {
                lastActiveEnemy = i;
            }
        }
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (playerCharacters[i].GetIsActive())
            {
                lastActivePlayer = i;
            }
        }
    }

    void Attacks()
    {
        if (!playerDead && !enemyDead)
        {

            //Debug.Log(playerBoard[playerIndex].name + " Attacks " + enemyBoard[enemyIndex].name);
            playerCharacters[playerIndex].Attack(enemyCharacters[enemyIndex]);
            //Debug.Log(enemyBoard[enemyIndex].name + " Attacks " + playerBoard[playerIndex].name);
            enemyCharacters[enemyIndex].Attack(playerCharacters[playerIndex]);
        }
    }

}

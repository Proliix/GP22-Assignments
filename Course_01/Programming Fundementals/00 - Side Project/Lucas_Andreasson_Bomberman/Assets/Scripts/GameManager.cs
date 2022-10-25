using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0, 100)]
    public int boxChanceRemoval = 10;
    public GameObject bombObject;
    public bool spawningBombs;

    private float timer;
    private float cooldown = 5;
    private GameObject[] boxes;
    private GameObject[] ground;
    private UIManager uiManager;
    private PlayerManager[] pManagers;
    private string winnerName = "";
    private bool playerHasWon = false;
    private int bombsize = 1;
    private int iterations = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        pManagers = new PlayerManager[players.Length];
        timer = 2;

        for (int i = 0; i < players.Length; i++)
        {
            pManagers[i] = players[i].GetComponent<PlayerManager>();

        }

        uiManager = GetComponent<UIManager>();
        ground = GameObject.FindGameObjectsWithTag("Ground");

        //create random holes in boxes
        boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject item in boxes)
        {
            int r = Random.Range(0, 100);

            if (r >= 100 - boxChanceRemoval)
            {
                item.SetActive(false);
            }

        }

    }

    //sets the winner text to the winner by determening the player that loses
    void SetLooser(int loser)
    {

        bool isDraw = false;
        if (!playerHasWon)
        {
            for (int i = 0; i < pManagers.Length; i++)
            {
                if (i != loser)
                {
                    playerHasWon = true;
                    winnerName = "Player " + (pManagers[i].playerNum + 1);
                }
            }
        }
        else
        {
            isDraw = true;
        }
        uiManager.GameIsOver(winnerName, isDraw);
    }

    //Finds empty spot over ground and spawns bomb
    public void SpawnRandomBombs()
    {
        int r = Random.Range(0, ground.Length);

        if (!Physics.CheckBox(ground[r].transform.position + Vector3.up, Vector3.one * 0.25f))
        {
            if (iterations % 5 == 0 && bombsize < 10)
                bombsize++;

            if (iterations >= 10 && cooldown != 2.5f && cooldown != 1f)
                cooldown = 2.5f;
            else if (iterations >= 30 && cooldown != 1f)
                cooldown = 1f;

            GameObject bomb = Instantiate(bombObject, ground[r].transform.position + Vector3.up, bombObject.transform.rotation);
            bomb.GetComponent<BombManager>().size = bombsize;

            timer = cooldown;
            iterations++;
        }
    }

    public void UpdatePlayerHP()
    {
        for (int i = 0; i < pManagers.Length; i++)
        {
            if (pManagers[i].hp <= 0)
            {
                SetLooser(i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (spawningBombs)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnRandomBombs();
            }
        }
    }
}

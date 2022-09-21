using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0, 100)]
    public int boxChanceRemoval = 10;

    private GameObject[] boxes;
    private UIManager uiManager;
    private PlayerManager[] pManagers;
    private string winnerName = "";
    private bool playerHasWon = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        pManagers = new PlayerManager[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            pManagers[i] = players[i].GetComponent<PlayerManager>();
            
        }

        uiManager = GetComponent<UIManager>();

        //create random holes in boxes
        boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject item in boxes)
        {
            int r = UnityEngine.Random.Range(0, 100);

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

    public void UpdatePlayerHP()
    {
        for (int i = 0; i < pManagers.Length; i++)
        {
            if (pManagers[i].hp <= 0)
            {
                Debug.Log(i);
                Debug.Log("t1 " + pManagers[i].hp + " | " + pManagers[i].name);
                SetLooser(i);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

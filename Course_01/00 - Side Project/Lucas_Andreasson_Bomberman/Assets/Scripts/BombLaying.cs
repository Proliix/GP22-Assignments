using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class BombLaying : MonoBehaviour
{
    public GameObject bomb;
    public Transform SpawnPosition;
    public float cooldown;
    public int explosionSize = 1;
    public int maxBomb = 2;

    private int lastMaxBomb, lastSize;
    private PlayerManager pManager;
    private UIManager uIManager;
    private int playerNum;
    private bool canPutDown = true;
    private float bombAmmo;
    private Vector3 spawnPos;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        uIManager = GameObject.FindWithTag("GameController").GetComponent<UIManager>();
        pManager = gameObject.GetComponent<PlayerManager>();
        playerNum = pManager.playerNum;
        bombAmmo = maxBomb;
        lastMaxBomb = maxBomb;
        lastSize = explosionSize;
    }

    public void changeBombAmmo(bool willAdd = true)
    {
        if (willAdd)
        {
            if (bombAmmo + 1 <= maxBomb)
                bombAmmo++;
        }
        if (!willAdd)
        {
            if (bombAmmo > 0)
            {
                bombAmmo--;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (maxBomb < 1)
        {
            maxBomb = 1;
        }

        if (bombAmmo > maxBomb)
        {
            bombAmmo = maxBomb;
        }
        
        if(lastMaxBomb != maxBomb || lastSize != explosionSize)
        {
            lastMaxBomb = maxBomb;
            lastSize = explosionSize;
            uIManager.UpdatePlayerStats(playerNum, maxBomb, explosionSize);
        }

        if (explosionSize <= 0)
        {
            explosionSize = 1;
        }
        timer += Time.deltaTime;

        if (Input.GetButtonDown(pManager.GetInput(PlayerManager.PlayerInput.Fire,playerNum)) && bombAmmo > 0 && canPutDown)
        {
            timer = 0;
            bombAmmo--;

            pManager.SetInvulerability(false);

            // Bit shift the index of the layer (6) to get a bit mask
            int layerMask = 1 << 6;

            // This would cast rays only against colliders in layer 6.

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
            {
                spawnPos = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
            }

            GameObject playerBomb;
            playerBomb = Instantiate(bomb, spawnPos, bomb.transform.rotation);
            playerBomb.GetComponent<BombManager>().size = explosionSize;
            playerBomb.GetComponent<BombManager>().SetPlayerGameobject(gameObject,playerNum);
            //Destroy(playerBomb, 5);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Bomb")
        {
            canPutDown = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Bomb")
        {
            canPutDown = true;
        }
    }

}

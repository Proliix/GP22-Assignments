using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerInput
    {
        Fire,
        Vertical,
        Horizontal,
    }

    [Range(0, 1)]
    public int playerNum = 0;
    public int hp = 3;
    public float invulnerabilityTime = 5;
    public bool canKickBombs = false;

    public GameObject kickTriggerX;
    public GameObject kickTriggerZ;


    private bool invulnerable = false;
    private bool isDead = false;
    private UIManager uIManager;
    private GameManager gameManager;
    private MeshRenderer mRenderer;
    private Material mat;
    private float alphaChangeRate = 0.75f;
    private float alpha = 0.5f;
    private float timer;
    private Vector3 pInput;

    // Start is called before the first frame update
    void Start()
    {
        mRenderer = gameObject.GetComponent<MeshRenderer>();
        mat = mRenderer.material;

        GameObject gControll = GameObject.FindWithTag("GameController");
        uIManager = gControll.GetComponent<UIManager>();
        gameManager = gControll.GetComponent<GameManager>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        pInput = new Vector3(Input.GetAxis(GetInput(PlayerInput.Horizontal, playerNum)), 0.0f, Input.GetAxis(GetInput(PlayerInput.Vertical, playerNum)));

        if (canKickBombs)
        {
            if (kickTriggerX.GetComponent<KickTrigger>().isTouchingBomb && Input.GetButtonDown(GetInput(PlayerInput.Fire, playerNum)))
            {
                kickTriggerX.GetComponent<KickTrigger>().KickBomb();
            }
            else if (kickTriggerZ.GetComponent<KickTrigger>().isTouchingBomb && Input.GetButtonDown(GetInput(PlayerInput.Fire, playerNum)))
            {
                kickTriggerZ.GetComponent<KickTrigger>().KickBomb();
            }
        }


        if (invulnerable)
        {
            if (timer >= invulnerabilityTime)
            {
                SetInvulerability(false);
            }

            if (alpha >= 1 && alphaChangeRate > 0 || alpha <= 0.5f && alphaChangeRate < 0)
            {
                alphaChangeRate *= -1;
            }
            //else if (alpha <= 0.5f && alphaChangeRate < 0)
            //{
            //    alphaChangeRate *= -1;
            //}
            alpha += alphaChangeRate * Time.deltaTime;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, alpha);

        }
        else
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        }
    }

    public bool GetKickTriggerCol()
    {
        bool touch = false;

        if (kickTriggerX.GetComponent<KickTrigger>().isTouchingBomb)
            touch = true;

        if (kickTriggerZ.GetComponent<KickTrigger>().isTouchingBomb && touch == false)
            touch = true;

        return touch;
    }
    public void SetInvulerability(bool isActive)
    {
        timer = 0;

        if (!isActive)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1);
        }

        invulnerable = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Explosion" && !invulnerable)
        {
            SetInvulerability(true);
            hp--;
            uIManager.UpdatePlayerLives(playerNum, hp);

        }

        if (hp <= 0 && !isDead)
        {
            gameManager.UpdatePlayerHP();
            isDead = true;
        }
    }

    public string GetInput(PlayerInput pInput, int pNum)
    {
        string returnString = "";
        if (pInput == PlayerInput.Fire && pNum == 0)
        {
            returnString = "Jump";
        }
        else if (pInput == PlayerInput.Fire && pNum == 1)
        {
            returnString = "Jump1";
        }
        else if (pInput == PlayerInput.Horizontal && pNum == 0)
        {
            returnString = "Horizontal";
        }
        else if (pInput == PlayerInput.Horizontal && pNum == 1)
        {
            returnString = "Horizontal1";
        }
        else if (pInput == PlayerInput.Vertical && pNum == 0)
        {
            returnString = "Vertical";
        }
        else if (pInput == PlayerInput.Vertical && pNum == 1)
        {
            returnString = "Vertical1";
        }

        if (returnString == "")
        {
            Debug.LogError("GetInput returned null in " + name);
        }

        return returnString;
    }

}

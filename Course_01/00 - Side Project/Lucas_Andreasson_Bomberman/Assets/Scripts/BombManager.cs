using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public float explosionTime = 5;
    public float size = 2;
    public GameObject explosionObject;
    public GameObject startExplosionObject;
    public GameObject powerUp;
    public GameObject bombModel;

    private float timePerExplosion = 1.25f;
    private float timeToNext = 0.05f;
    private float timer;
    private float explodeNext = 0;
    private float currentSize = 0;
    private float step = 1;
    private float collisionStep = 0;
    private Vector2 posStep;
    private Vector2 negStep;
    private GameObject player;
    private int playerNum;
    [HideInInspector]
    public bool hasExploded = false;
    private int playerInTrigger = 0;
    private Rigidbody rBody;
    private Vector3 moveDir;
    private float moveSpeed = 4;
    private bool isMoving = false;


    private void Start()
    {
        rBody = gameObject.GetComponent<Rigidbody>();
        //player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > explosionTime)
        {
            if (timer > explodeNext)
            {

                Explode();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 0.45f);
            foreach (Collider col in colliders)
            {
                if (col.tag == "Box" || col.tag == "Wall" || col.tag == "Bomb" && col.gameObject != this.gameObject)
                {
                    Debug.Log(col.name);
                    isMoving = false;
                    SetGridPos();
                }
            }
            rBody.MovePosition(transform.position + (moveDir * moveSpeed) * Time.deltaTime);
        }
    }

    public void Explode(bool forceExplode = false)
    {
        hasExploded = true;

        if (isMoving)
        {
            SetGridPos();
        }

        if (forceExplode)
        {
            timer = explosionTime + 1;
        }


        if (currentSize >= size)
        {

            timer = 0;
            hasExploded = false;
            Destroy(gameObject);
        }
        else
        {

            if (currentSize == 0)
            {
                GameObject startExp = Instantiate(startExplosionObject, transform.position, startExplosionObject.transform.rotation);
                Destroy(startExp, timePerExplosion);
            }


            IncreaseSteps();
            InstantiateExplosion(new Vector3(posStep.x, 0, 0), false);
            InstantiateExplosion(new Vector3(0, 0, posStep.y));
            InstantiateExplosion(new Vector3(negStep.x, 0, 0), false);
            InstantiateExplosion(new Vector3(0, 0, negStep.y));


            if (bombModel.GetComponent<MeshRenderer>().enabled == true)
            {
                bombModel.transform.localScale = Vector3.zero;
                bombModel.GetComponent<Animator>().enabled = false;
                bombModel.GetComponent<MeshRenderer>().enabled = false;
                player.GetComponent<BombLaying>().changeBombAmmo();
            }

            explodeNext = timer + timeToNext;
            currentSize++;
        }
    }

    public void MoveDirection(Vector3 direction)
    {
        moveDir = direction;
        isMoving = true;
    }

    public void SetPlayerGameobject(GameObject newPLayer, int newPLayerNum)
    {
        player = newPLayer;
        playerNum = newPLayerNum;
    }

    void SetGridPos()
    {
        int layerMask = 1 << 6;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
        }
    }
    void InstantiateExplosion(Vector3 stepFromBomb, bool facingZ = true)
    {
        GameObject bombExplosion;

        bombExplosion = Instantiate(explosionObject, new Vector3(transform.position.x + stepFromBomb.x, transform.position.y + stepFromBomb.y, transform.position.z + stepFromBomb.z), explosionObject.transform.rotation);

        if (!facingZ)
            bombExplosion.transform.Rotate(0, 90, 0);

        Destroy(bombExplosion, timePerExplosion);
    }

    void IncreaseSteps()
    {
        collisionStep = posStep.x / step;
        if (!PointCollisionCheck(new Vector3(transform.position.x + collisionStep + 1, transform.position.y, transform.position.z)))
        {
            posStep.x += step;
        }

        collisionStep = posStep.y / step;
        if (!PointCollisionCheck(new Vector3(transform.position.x, transform.position.y, transform.position.z + collisionStep + 1)))
        {
            posStep.y += step;
        }

        collisionStep = (negStep.x * -1) / step;
        collisionStep *= -1;
        if (!PointCollisionCheck(new Vector3(transform.position.x + collisionStep - 1, transform.position.y, transform.position.z)))
        {
            negStep.x -= step;
        }

        collisionStep = (negStep.y * -1) / step;
        collisionStep *= -1;
        if (!PointCollisionCheck(new Vector3(transform.position.x, transform.position.y, transform.position.z + collisionStep - 1)))
        {
            negStep.y -= step;
        }

    }

    bool PointCollisionCheck(Vector3 pos)
    {
        bool returnValue = false;
        Collider[] colliders = Physics.OverlapBox(pos, Vector3.one * 0.25f, Quaternion.Euler(0, 0, 0));

        //*** DEBUG STUFF ***
        //GameObject testSphere = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //testSphere.transform.position = pos;
        //testSphere.transform.localScale = Vector3.one * 0.5f;

        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].tag == "Wall")
            {
                returnValue = true;
            }
            else if (colliders[i].tag == "Box")
            {
                float r = Random.Range(0, 3);
                if (r == 0)
                {
                    Instantiate(powerUp, colliders[i].transform.position, powerUp.transform.rotation);
                }
                Destroy(colliders[i].gameObject);
                returnValue = false;
            }
            else if (colliders[i].tag == "PowerUp")
            {
                Destroy(colliders[i].gameObject);
                returnValue = false;
            }
            else if (colliders[i].tag == "Bomb")
            {
                if (colliders[i].GetComponent<BombManager>().hasExploded == false)
                {
                    colliders[i].gameObject.GetComponent<BombManager>().Explode(true);
                }
            }
        }


        return returnValue;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger--;
        }

        if (playerInTrigger <= 0)
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInTrigger++;
        }
    }

}
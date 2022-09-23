using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public enum PowerupType { sizeUp, sizeDown, bombUp, bombDown, bombKick }
    public GameObject bombUpModel;
    public GameObject badCross;
    public GameObject bombKickObj;

    [SerializeField]
    PowerupType powerupType;
    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        powerupType = (PowerupType)Random.Range(0, System.Enum.GetNames(typeof(PowerupType)).Length);
        mat = gameObject.GetComponent<MeshRenderer>().material;

        if (powerupType == PowerupType.sizeUp)
        {
            mat.SetColor("_EmissionColor", new Color(191, 36, 0, 1f) * 0.05f);
        }
        else if (powerupType == PowerupType.sizeDown)
        {
            mat.SetColor("_EmissionColor", new Color(0, 101, 191, 1f) * 0.05f);
        }
        else if (powerupType == PowerupType.bombUp)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            bombUpModel.SetActive(true);
        }
        else if (powerupType == PowerupType.bombDown)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            bombUpModel.SetActive(true);
            badCross.SetActive(true);
        }
        else if (powerupType == PowerupType.bombKick)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            bombKickObj.SetActive(true);
        }
        Destroy(gameObject, 10);
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (powerupType == PowerupType.sizeUp)
            {
                other.gameObject.GetComponent<BombLaying>().explosionSize++;
                Destroy(gameObject);
            }
            else if (powerupType == PowerupType.sizeDown)
            {
                other.gameObject.GetComponent<BombLaying>().explosionSize--;
                Destroy(gameObject);
            }
            else if (powerupType == PowerupType.bombUp)
            {
                other.gameObject.GetComponent<BombLaying>().maxBomb++;
                other.gameObject.GetComponent<BombLaying>().changeBombAmmo();
                Destroy(gameObject);
            }
            else if (powerupType == PowerupType.bombDown)
            {
                other.gameObject.GetComponent<BombLaying>().maxBomb--;
                Destroy(gameObject);
            }
            else if (powerupType == PowerupType.bombKick)
            {
                other.gameObject.GetComponent<PlayerManager>().canKickBombs = true;
                Destroy(gameObject);
            }
        }
    }
}

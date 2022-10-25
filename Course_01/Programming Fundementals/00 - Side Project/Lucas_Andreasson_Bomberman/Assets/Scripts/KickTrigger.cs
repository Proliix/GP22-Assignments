using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickTrigger : MonoBehaviour
{
    public bool isTouchingBomb = false;
    GameObject touchedBomb;

    public void KickBomb(Vector3 dir)
    {
        touchedBomb.GetComponent<BombManager>().MoveDirection(dir);
    }
    public void KickBomb()
    {
        Vector3 dir = new Vector3(gameObject.transform.localPosition.x, 0, gameObject.transform.localPosition.z);
        if (touchedBomb != null)
        {
            touchedBomb.GetComponent<BombManager>().MoveDirection(dir);
        }
        else
        {
            isTouchingBomb = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        isTouchingBomb = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bomb")
        {
            isTouchingBomb = true;
            touchedBomb = other.gameObject;
        }

    }
}

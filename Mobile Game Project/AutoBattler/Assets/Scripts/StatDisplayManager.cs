using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayManager : MonoBehaviour
{
    const int POOL_SIZE = 8;

    [SerializeField] GameObject statPrefab;

    public static StatDisplayManager Instance;

    GameObject[] objectPool = new GameObject[POOL_SIZE];

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            for (int i = 0; i < objectPool.Length; i++)
            {
                objectPool[i] = Instantiate(statPrefab, gameObject.transform);
                objectPool[i].SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetStatDisplayer(Transform parent, Vector3 localPos, ref TextMeshPro leftText, ref TextMeshPro rightText, bool flipX = false)
    {
        GameObject returnObj = null;

        for (int i = 0; i < objectPool.Length; i++)
        {
            if (!objectPool[i].activeSelf)
            {
                returnObj = objectPool[i];
                returnObj.SetActive(true);
                returnObj.transform.SetParent(parent);
                returnObj.transform.localPosition = localPos;
                leftText = returnObj.transform.GetChild(0).GetComponent<TextMeshPro>();
                rightText = returnObj.transform.GetChild(1).GetComponent<TextMeshPro>();
                break;
            }
        }
        returnObj.GetComponent<SpriteRenderer>().flipX = flipX;
        return returnObj;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayManager : MonoBehaviour
{
    const int POOL_SIZE = 8;

    [SerializeField] GameObject statPrefab;

    private static StatDisplayManager _instance;
    public static StatDisplayManager Instance { get { return _instance; } }

    GameObject[] objectPool = new GameObject[POOL_SIZE];

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
        DontDestroyOnLoad(gameObject);
    }

    public GameObject GetStatDisplayer(Transform parent, Vector3 localPos, ref TextMeshPro leftText, ref TextMeshPro rightText, ref int index, bool flipX = false)
    {
        GameObject returnObj = null;

        for (int i = 0; i < objectPool.Length; i++)
        {
            if (objectPool[i] != null)
            {
                if (objectPool[i].transform.parent == transform)
                {
                    returnObj = objectPool[i];
                    leftText = returnObj.transform.GetChild(0).GetComponent<TextMeshPro>();
                    rightText = returnObj.transform.GetChild(1).GetComponent<TextMeshPro>();
                    returnObj.SetActive(true);
                    returnObj.transform.SetParent(parent);
                    returnObj.transform.localPosition = localPos;
                    index = i;
                    break;
                }
            }
            else
            {
                objectPool[i] = Instantiate(statPrefab, gameObject.transform);
                objectPool[i].SetActive(false);
                i--;
            }
        }
        if (returnObj != null)
            returnObj.GetComponent<SpriteRenderer>().flipX = flipX;

        if (leftText == null || rightText == null)
        {
            leftText = returnObj.transform.GetChild(0).GetComponent<TextMeshPro>();
            rightText = returnObj.transform.GetChild(1).GetComponent<TextMeshPro>();
        }
        return returnObj;
    }

    public void ResetAll()
    {
        for (int i = 0; i < objectPool.Length; i++)
        {
            objectPool[i].transform.parent = transform;
            objectPool[i].SetActive(false);
        }
    }

    public void ResetDisplay(int displayIndex)
    {
        objectPool[displayIndex].transform.parent = transform;
        objectPool[displayIndex].SetActive(false);
    }
}

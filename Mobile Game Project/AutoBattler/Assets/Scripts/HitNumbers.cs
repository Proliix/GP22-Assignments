using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitNumbers : MonoBehaviour
{
    private static HitNumbers _instance;
    public static HitNumbers Instance { get { return _instance; } }
    [SerializeField] int startSize = 5;
    [SerializeField] GameObject textObjPrefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] float returnTime = 1;
    List<GameObject> textObjects;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            textObjects = new List<GameObject>();
            StartCoroutine(InitializeObjectPool(startSize));
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    IEnumerator InitializeObjectPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject textObj = Instantiate(textObjPrefab, gameObject.transform.position, textObjPrefab.transform.rotation, gameObject.transform);
            textObjects.Add(textObj);
            textObj.name = "Hit Text: " + (textObjects.Count - 1);
            textObj.SetActive(false);
        }
        yield return null;
    }

    void UpdateText(GameObject obj, Vector3 position, int damage)
    {
        obj.transform.position = position;
        obj.transform.SetParent(Canvas.transform);
        obj.GetComponent<TextMeshProUGUI>().text = damage >= 0 ? "<color=red>" + damage : "<color=green>" + Mathf.Abs(damage);
        obj.SetActive(true);
        StartCoroutine(ReturnAfterTime(returnTime, obj));
    }

    IEnumerator ReturnAfterTime(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.transform.SetParent(gameObject.transform);
        obj.SetActive(false);
    }
    IEnumerator GetInactiveTextObj(Vector3 position, int damage)
    {
        bool textGiven = false;
        for (int i = 0; i < textObjects.Count; i++)
        {
            if (textObjects[i].activeSelf == false)
            {
                UpdateText(textObjects[i], position, damage);
                textGiven = true;
                break;
            }
        }
        if (!textGiven)
        {
            int newNum = textObjects.Count;
            StartCoroutine(InitializeObjectPool(5));
            UpdateText(textObjects[newNum], position, damage);
        }
        yield return null;
    }

    public void GetHitNumber(Vector3 position, int damage)
    {
        StartCoroutine(GetInactiveTextObj(position, damage));
    }

}

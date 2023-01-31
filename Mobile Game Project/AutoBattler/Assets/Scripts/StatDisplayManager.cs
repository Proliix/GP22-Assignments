using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatDisplayManager : MonoBehaviour
{
    const int POOL_SIZE = 8;

    [SerializeField] GameObject statPrefab;
    [SerializeField] GameObject descriptionPrefab;


    GameObject descriptionObj = null;
    TextMeshProUGUI title;
    TextMeshProUGUI description;
    TextMeshProUGUI cost;
    Vector3 startScale;

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
            startScale = statPrefab.transform.localScale;
            InitializeDescriptionObject();
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void InitializeDescriptionObject()
    {
        if (descriptionObj == null)
        {
            descriptionObj = Instantiate(descriptionPrefab, transform.position, descriptionPrefab.transform.rotation, transform);
            if (!(title = descriptionObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>()))
            {
                Debug.LogError("<color=red>Error:</color> Cant find title textobject in description prefab. Make sure there is a textmeshproui component on child 2");
            }

            if (!(description = descriptionObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>()))
            {
                Debug.LogError("<color=red>Error:</color> Cant find description textobject in description prefab. Make sure there is a textmeshproui component on child 3");
            }

            if (!(cost = descriptionObj.transform.GetChild(4).GetComponent<TextMeshProUGUI>()))
            {
                Debug.LogError("<color=red>Error:</color> Cant find cost textobject in description prefab. Make sure there is a textmeshproui component on child 4 ");
            }

            if (title == null || description == null || cost == null)
                Debug.LogError("<b>Prefab setup should be: </b>\n(extend message to se setup)\n Canvas(world canvas)\n background(image)\n coin(image)\n title(text)\n description(text)\n cost(text)");

            descriptionObj.SetActive(false);
        }
        else
            Debug.LogError("<color=red>Error:</color> DescriptionObject already exists this function should not be called");
    }

    public void GetDescriptionObject(CharacterStats stats, Vector3 position)
    {
        title.text = stats.Name;
        description.text = stats.Description;
        cost.text = "x" + stats.Cost;
        descriptionObj.transform.position = position;
        descriptionObj.SetActive(true);
    }

    public void GetDescriptionObject(string titleText, string descriptionText, int costValue, Vector3 position)
    {
        title.text = titleText;
        description.text = descriptionText;
        cost.text = "x" + costValue;
        descriptionObj.transform.position = position;
        descriptionObj.SetActive(true);
    }

    public void ResetDescriptionObject()
    {
        descriptionObj.SetActive(false);
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
                    returnObj.transform.localScale = startScale;
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

    /// <summary>
    /// Resets all the display objects
    /// </summary>
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
        objectPool[displayIndex].transform.localScale = startScale;
    }
}

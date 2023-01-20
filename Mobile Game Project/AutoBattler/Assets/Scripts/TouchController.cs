using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public enum TouchState { Buff, Character, ShopCharacter, Nothing }

    [SerializeField] float yPosBezierCurve = 2;
    [SerializeField]
    float switchScale = 1.25f;
    [SerializeField] GameObject arrow;
    float xPos = 0;
    LineRenderer line;
    bool UpdateLineRenderer = false;
    Vector3 camPos;
    Vector3 lineStartPos;

    Touch touch;
    TouchState state = TouchState.Nothing;
    Potion currentBuff;
    GameController gameController;
    ICharacter char1, char2;
    GameObject obj1, obj2;
    Vector3 oldScale;
    bool hasReset = false;
    bool usingDescription = false;
    private void Start()
    {
        gameController = GetComponent<GameController>();
        line = GetComponent<LineRenderer>();
        arrow.SetActive(false);
    }
    void Update()
    {
        if (gameController.GetGameState() == GameState.Shop)
        {

            if ((Input.touchCount > 0))
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    hasReset = false;
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Buff"))
                        {
                            StartLine(hit.transform.position);

                            state = TouchState.Buff;
                            currentBuff = hit.collider.gameObject.GetComponent<Potion>();
                            obj1 = hit.collider.gameObject;
                            StatDisplayManager.Instance.GetDescriptionObject(currentBuff.GetTitle(), currentBuff.GetDescription(), currentBuff.GetCost(), obj1.transform.position + (Vector3.up * 2));
                            usingDescription = true;

                        }
                        if (hit.collider.CompareTag("Character"))
                        {
                            StartLine(hit.transform.position);
                            state = TouchState.Character;
                            obj1 = hit.collider.gameObject;
                            char1 = hit.collider.GetComponent<ICharacter>();
                            StatDisplayManager.Instance.GetDescriptionObject(char1.GetStats(), obj1.transform.position + (Vector3.up * 2));
                            usingDescription = true;
                        }
                        if (hit.collider.CompareTag("ShopCharacter"))
                        {
                            StartLine(hit.transform.position);
                            state = TouchState.ShopCharacter;
                            obj1 = hit.collider.gameObject;
                            char1 = hit.collider.GetComponent<ICharacter>();
                            StatDisplayManager.Instance.GetDescriptionObject(char1.GetStats(), obj1.transform.position + (Vector3.up * 2));
                            usingDescription = true;
                        }

                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (state != TouchState.Nothing)
                    {

                        if (usingDescription)
                            StatDisplayManager.Instance.ResetDescriptionObject();

                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);
                        if (hit.collider != null)
                        {
                            Debug.Log("Hit something");
                            if (hit.collider.CompareTag("Character"))
                            {
                                if (state == TouchState.Buff)
                                {
                                    if (gameController.BuyObject(currentBuff.GetCost()))
                                        currentBuff.UpdateCharacter(hit.collider.gameObject.GetComponent<ICharacter>());
                                }
                                else if (state == TouchState.Character)
                                {
                                    char2 = hit.collider.GetComponent<ICharacter>();
                                    gameController.SwapKeys(char1, char2);
                                }
                                else if (state == TouchState.ShopCharacter)
                                {
                                    char2 = hit.collider.GetComponent<ICharacter>();
                                    if (gameController.BuyObject(char2.GetCost()))
                                    {
                                        char2.InitializeFromKey(char1.GetCharacterKey());
                                        obj1.SetActive(false);
                                    }
                                }

                            }

                        }


                    }

                    if (!hasReset)
                    {
                        hasReset = true;
                        usingDescription = false;
                        ResetObjs();
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (state != TouchState.Nothing)
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                        if (hit.collider != null)
                        {
                            switch (state)
                            {
                                case TouchState.Buff:
                                    OverSelectableItem(hit, "Character");
                                    break;
                                case TouchState.Character:
                                    OverSelectableItem(hit, "Character");
                                    break;
                                case TouchState.ShopCharacter:
                                    OverSelectableItem(hit, "Character");
                                    break;
                                default:
                                    break;
                            }

                        }
                        else if (obj2 != null && oldScale != Vector3.zero)
                        {
                            obj2.transform.localScale = oldScale;
                            obj2 = null;
                        }
                    }
                }

                if (UpdateLineRenderer)
                {
                    camPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y, 0);
                    xPos = Mathf.Lerp(lineStartPos.x, camPos.x, 0.5f);
                    DrawQuadraticBezierCurve(lineStartPos, new Vector3(xPos, lineStartPos.y + yPosBezierCurve, 0), camPos);
                    arrow.transform.position = camPos;
                    arrow.transform.up = line.GetPosition(199) - camPos;
                }
            }
        }
    }

    private void OverSelectableItem(RaycastHit2D hit, string checkTag)
    {
        if (hit.collider.CompareTag(checkTag))
        {
            if (hit.collider.gameObject != obj1 && hit.collider.gameObject != obj2)
            {
                if (obj2 != null)
                    obj2.transform.localScale = oldScale;

                obj2 = hit.collider.gameObject;
                oldScale = obj2.transform.localScale;
                obj2.transform.localScale = Vector3.one * switchScale;
            }
        }
    }
    private void StartLine(Vector3 startPos)
    {
        UpdateLineRenderer = true;
        lineStartPos = startPos;
        line.enabled = true;
        arrow.SetActive(true);
    }

    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        line.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < line.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            line.SetPosition(i, B);
            t += (1 / (float)line.positionCount);
        }
    }

    private void ResetObjs()
    {
        line.enabled = false;
        UpdateLineRenderer = false;
        arrow.SetActive(false);
        state = TouchState.Nothing;

        if (obj2 != null && oldScale != Vector3.zero)
            obj2.transform.localScale = oldScale;

        oldScale = Vector3.zero;
        obj1 = null;
        obj2 = null;
        char1 = null;
        char2 = null;
        currentBuff = null;
    }
}

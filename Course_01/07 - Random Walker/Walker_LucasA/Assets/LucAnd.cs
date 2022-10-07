using System.Collections.Generic;
using UnityEngine;
class LucAnd : IRandomWalker
{
    //Add your own variables here.
    //Do not use processing variables like width or height

    Vector2 walkerPos;
    Vector2 screenSize;
    int lowScore = 2;
    List<Vector2> positions;
    [Tooltip("0 = newPos,1 = oldPos, 2 = OutsideOfScreen")]
    [Range(0, 2)] int[] canMove = new int[4];
    float[] moveScore = new float[4];
    [Tooltip("0 = up, 1 = right, 2 = down, 3 = left")]
    Vector2[] moveDir = new Vector2[4];

    int stuckFix;

    string cameraName = "We're no strangers to love You know the rules and so do I (do I) A full commitment's what I'm thinking of You wouldn't get this from any other guy I just wanna tell you how I'm feeling Gotta make you understand Never gonna give you up Never gonna let you down Never gonna run around and desert you Never gonna make you cry Never gonna say goodbye Never gonna tell a lie and hurt you We've known each other for so long Your heart's been aching, but you're too shy to say it(say it) Inside, we both know what's been going on (going on) We know the game and we're gonna play it And if you ask me how I'm feeling Don't tell me you're too blind to see Never gonna give you up Never gonna let you down Never gonna run around and desert you Never gonna make you cry Never gonna say goodbye Never gonna tell a lie and hurt you Never gonna give you up Never gonna let you down Never gonna run around and desert you Never gonna make you cry Never gonna say goodbye Never gonna tell a lie and hurt you We've known each other for so long Your heart's been aching, but you're too shy to say it(to say it) Inside, we both know what's been going on (going on) We know the game and we're gonna play it I just wanna tell you how I'm feeling Gotta make you understand Never gonna give you up Never gonna let you down Never gonna run around and desert you Never gonna make you cry Never gonna say goodbye Never gonna tell a lie and hurt you Never gonna give you up Never gonna let you down Never gonna run around and desert you Never gonna make you cry Never gonna say goodbye Never gonna tell a lie and hurt you Never gonna give you up Never gonna let you down Never gonna run around and desert you Never gonna make you cry Never gonna say goodbye Never gonna tell a lie and hurt you";
    char[] cameraNameArr;
    int cameraIndex;
    int[] cameraNameLength;

    bool hasScaleFactor = false;
    float scaleFactor = 0;
    int scaleFactorCheckIndex = 0;
    Vector3[] scaleFactorPos = new Vector3[2];
    GameObject[] holderChildren;
    Vector2[] pLitePos;
    List<Vector2> dangerPos;
    bool hasHolder = false;

    int iterations = 0;

    float[] possibleScaleFactor;
    GameObject playerObj;

    int currentListIndex = 0;
    int listSize = 5000;
    Vector3 lastPos = Vector3.zero;


    public string GetName()
    {
        return "Lucas Andreasson"; //When asked, tell them our walkers name
    }

    public float Width
    {
        get
        {
            Camera cameraRef = Camera.main;
            return cameraRef.orthographicSize * cameraRef.aspect * 2;
        }
    }

    public float Height
    {
        get
        {
            Camera cameraRef = Camera.main;
            return cameraRef.orthographicSize * 2;
        }
    }



    public Vector2 GetStartPosition(int playAreaWidth, int playAreaHeight)
    {
        moveDir[0] = Vector2.up;
        moveDir[1] = Vector2.right;
        moveDir[2] = Vector2.down;
        moveDir[3] = Vector2.left;

        cameraNameLength = new int[25];
        cameraNameArr = cameraName.ToCharArray();

        Camera.main.gameObject.name = "";
        for (int i = 0; i < cameraNameLength.Length; i++)
        {
            cameraNameLength[i] = cameraIndex;
            if (cameraIndex < cameraNameArr.Length)
                cameraIndex++;
            else
                cameraIndex = 0;

            Camera.main.gameObject.name += cameraNameArr[cameraNameLength[i]];
        }

        Debug.Log("W: " + playAreaHeight + "| h: " + playAreaHeight);

        screenSize = new Vector2(playAreaWidth, playAreaHeight);
        //Random.InitState(10);
        positions = new List<Vector2>();
        dangerPos = new List<Vector2>();
        //Select a starting position or use a random one.
        float x = Random.Range(0, playAreaWidth);
        float y = Random.Range(0, playAreaHeight);

        walkerPos = new Vector2(x, y);
        positions.Add(walkerPos);
        //a PVector holds floats but make sure its whole numbers that are returned!
        return new Vector2(x, y);
    }

    bool NewPosInBounds(Vector2 dir)
    {
        bool inBounds = true;

        if (walkerPos.x + dir.x >= screenSize.x || walkerPos.x + dir.x <= 0)
        {
            inBounds = false;
        }

        if (walkerPos.y + dir.y >= screenSize.y || walkerPos.y + dir.y <= 0)
        {
            inBounds = false;
        }

        return inBounds;
    }
    bool NewPosInBounds(Vector2 dir, Vector2 pos)
    {
        bool inBounds = true;

        if (pos.x + dir.x >= screenSize.x || pos.x + dir.x <= 0)
        {
            inBounds = false;
        }

        if (pos.y + dir.y >= screenSize.y || pos.y + dir.y <= 0)
        {
            inBounds = false;
        }

        return inBounds;
    }

    int NewPosCheck(Vector2 dir)
    {
        int newPos = 0;

        if (!NewPosInBounds(dir))
        {
            return 2;
        }

        if (hasScaleFactor)
        {
            for (int z = 0; z < dangerPos.Count; z++)
            {
                if (Vector2.Distance(dangerPos[z], walkerPos + dir) < scaleFactor)
                {
                    return 2;
                }
            }
        }

        for (int i = 0; i < positions.Count; i++)
        {
            if (walkerPos + dir == positions[i])
            {
                newPos = 1;
            }
        }
        return newPos;
    }

    private bool IsSimilar(float a, float b)
    {
        if (a - b < 0.001f && a - b > -0.001f)
        {
            return true;
        }
        else return false;
    }
    //float GetMoveScore(Vector2 dir)
    //{
    //    Vector2 newPos = walkerPos + dir;
    //    float newMoveScore = 0;
    //    float addToScore = 0;


    //    for (int i = 0; i < 4; i++)
    //    {
    //        addToScore = 0;

    //        if (NewPosInBounds(dir, newPos))
    //        {
    //            for (int x = 0; x < positions.Count; x++)
    //            {
    //                if (newPos + moveDir[i] == positions[x])
    //                {
    //                    addToScore = lowScore;
    //                }
    //                else if (addToScore != lowScore)
    //                {
    //                    addToScore = 20;
    //                }


    //            }

    //            if (hasScaleFactor)
    //            {
    //                for (int z = 0; z < dangerPos.Count; z++)
    //                {
    //                    if (Vector2.Distance(dangerPos[z], newPos + moveDir[i]) < scaleFactor)
    //                    {
    //                        addToScore -= 5;
    //                        // Debug.Log("Too close to player");
    //                    }
    //                }
    //            }

    //            newMoveScore += addToScore;
    //        }
    //        else
    //        {
    //            newMoveScore -= lowScore;
    //        }
    //    }

    //    addToScore = 0;
    //    for (int i = 0; i < positions.Count; i++)
    //    {
    //        if (newPos == positions[i])
    //        {
    //            addToScore = lowScore;
    //        }
    //        else if (addToScore != lowScore)
    //        {
    //            addToScore = 10;
    //        }
    //    }

    //    return newMoveScore;
    //}


    float GetMoveScore(Vector2 dir)
    {
        Vector2 newPos = walkerPos + dir;
        float newMoveScore = 0;


        for (int i = 0; i < 4; i++)
        {
            if (NewPosInBounds(dir, newPos))
            {
                for (int x = 0; x < positions.Count; x++)
                {
                    if (moveDir[i] != -dir)
                    {
                        if (newPos + moveDir[i] == positions[x])
                        {
                            newMoveScore += lowScore;
                        }
                        else
                        {
                            newMoveScore += 20;
                        }
                    }

                }

                if (hasScaleFactor)
                {
                    for (int z = 0; z < dangerPos.Count; z++)
                    {
                        if (Vector2.Distance(dangerPos[z], newPos + moveDir[i]) < scaleFactor)
                        {
                            newMoveScore -= 5;
                            // Debug.Log("Too close to player");
                        }
                    }
                }

            }
        }

        for (int i = 0; i < positions.Count; i++)
        {
            if (newPos == positions[i])
            {

                newMoveScore += lowScore;
            }
            else
            {
                newMoveScore += 20;
            }
        }

        return newMoveScore;
    }
    void PlayerDensityScoreAdder()
    {
        int[] side = new int[4];
        int playerInQuadrant = -1;

        for (int i = 0; i < pLitePos.Length; i++)
        {
            if (holderChildren[i] != playerObj)
            {
                if (pLitePos[i].x >= (screenSize.x / 2) && pLitePos[i].y >= (screenSize.y / 2))
                {
                    //TOP RIGHT
                    side[0]++;

                }
                else if (pLitePos[i].x >= (screenSize.x / 2) && pLitePos[i].y <= (screenSize.y / 2))
                {
                    //BOT RIGHT
                    side[1]++;
                }
                else if (pLitePos[i].x <= (screenSize.x / 2) && pLitePos[i].y >= (screenSize.y / 2))
                {
                    //TOP LEFT
                    side[2]++;
                }
                else if (pLitePos[i].x <= (screenSize.x / 2) && pLitePos[i].y <= (screenSize.y / 2))
                {
                    //BOT LEFT
                    side[3]++;
                }
            }
        }

        if (walkerPos.x >= (screenSize.x / 2) && walkerPos.y >= (screenSize.y / 2))
        {
            //TOP RIGHT
            playerInQuadrant = 0;

        }
        else if (walkerPos.x >= (screenSize.x / 2) && walkerPos.y <= (screenSize.y / 2))
        {
            //BOT RIGHT
            playerInQuadrant = 1;
        }
        else if (walkerPos.x <= (screenSize.x / 2) && walkerPos.y >= (screenSize.y / 2))
        {
            //TOP LEFT
            playerInQuadrant = 2;
        }
        else if (walkerPos.x <= (screenSize.x / 2) && walkerPos.y <= (screenSize.y / 2))
        {
            //BOT LEFT
            playerInQuadrant = 3;
        }


        int lowest = 1000000000;
        int lowestIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            if (side[i] < lowest)
            {
                lowest = side[i];
                lowestIndex = i;
            }
        }

        if (lowestIndex != playerInQuadrant)
        {
            switch (lowestIndex)
            {
                case 0:
                    Debug.Log("Top right");
                    moveScore[0] += 1;
                    moveScore[1] += 1;
                    break;
                case 1:
                    Debug.Log("Bot right");
                    moveScore[2] += 1;
                    moveScore[1] += 1;
                    break;
                case 2:
                    Debug.Log("Top Left");
                    moveScore[0] += 1;
                    moveScore[3] += 1;
                    break;
                case 3:
                    Debug.Log("Bot Left");
                    moveScore[2] += 1;
                    moveScore[3] += 1;
                    break;
                default:
                    break;
            }
        }


    }

    public Vector2 Movement()
    {
        iterations++;
        if (Camera.main != null)
        {
            if (iterations % 5 == 0)
            {
                Camera.main.gameObject.name = "";
                for (int i = 0; i < cameraNameLength.Length; i++)
                {

                    if (i != cameraNameLength.Length - 1)
                    {
                        cameraNameLength[i] = cameraNameLength[i + 1];
                    }
                    else
                    {
                        cameraNameLength[i] = cameraIndex;

                        if (cameraIndex < cameraNameArr.Length - 1)
                            cameraIndex++;
                        else
                            cameraIndex = 0;

                    }
                    if (cameraIndex > cameraNameArr.Length - 5)
                        Debug.Log(cameraIndex + " | " + cameraNameArr.Length);
                    Camera.main.gameObject.name += cameraNameArr[cameraNameLength[i]];
                }
            }
        }



        if (!hasHolder && iterations > 2)
        {
            if (GameObject.Find("Holder"))
            {
                Transform[] test = GameObject.Find("Holder").GetComponentsInChildren<Transform>();
                GameObject[] holderSort = new GameObject[test.Length];
                holderChildren = new GameObject[test.Length - 1];
                int x = 0;
                hasHolder = true;
                for (int i = 1; i < test.Length; i++)
                {
                    holderSort[i] = test[i].gameObject;
                    if (holderSort[i].name != "Holder")
                    {
                        holderChildren[x] = holderSort[i];
                        x++;
                    }
                }
            }
        }
        else if (hasHolder && !hasScaleFactor && iterations > 2)
        {
            //Debug.Log(holderChildren[scaleFactorCheckIndex].name + ": " + holderChildren[scaleFactorCheckIndex].transform.position);
            if (holderChildren[0].transform.position != Vector3.zero && lastPos == Vector3.zero)
            {
                lastPos = holderChildren[0].transform.localPosition;
            }
            else if (lastPos != Vector3.zero && hasScaleFactor == false)
            {
                possibleScaleFactor = new float[holderChildren.Length];

                for (int i = 0; i < holderChildren.Length; i++)
                {
                    //Debug.Log(holderChildren[i]);
                }

                for (int i = 0; i < possibleScaleFactor.Length; i++)
                {
                    possibleScaleFactor[i] = holderChildren[i].transform.position.x / walkerPos.x;
                }

                scaleFactor = (lastPos.x - holderChildren[0].transform.localPosition.x) - (lastPos.y - holderChildren[0].transform.localPosition.y);

                scaleFactor = Mathf.Abs(scaleFactor);

                for (int i = 0; i < possibleScaleFactor.Length; i++)
                {
                    if (IsSimilar(scaleFactor, possibleScaleFactor[i]))
                    {
                        scaleFactor = possibleScaleFactor[i];
                        playerObj = holderChildren[i];
                        playerObj.name = walkerPos.ToString();
                        break;
                    }
                }

                hasScaleFactor = true;
            }
        }


        dangerPos = new List<Vector2>();
        if (hasScaleFactor)
        {
            pLitePos = new Vector2[holderChildren.Length];

            for (int i = 0; i < pLitePos.Length; i++)
            {
                if (holderChildren[i] != playerObj)
                {
                    //Debug.Log(holderChildren[i].name);
                    pLitePos[i] = new Vector2(holderChildren[i].transform.position.x / scaleFactor, holderChildren[i].transform.position.y / scaleFactor);

                    //Debug.Log("Distanse: " + Vector2.Distance(pLitePos[i], walkerPos) + " | plitePos " + pLitePos[i] + " | walkerPos " + walkerPos);
                    if (Vector2.Distance(pLitePos[i], walkerPos) < 10)
                    {
                        dangerPos.Add(pLitePos[i]);
                    }
                }
            }
        }

        Vector2 nextMove = Vector2.zero;
        float bestMoveScore = 0;

        for (int i = 0; i < moveScore.Length; i++)
        {
            canMove[i] = NewPosCheck(moveDir[i]);
            moveScore[i] = GetMoveScore(moveDir[i]);
        }

        if (hasScaleFactor)
        {
            PlayerDensityScoreAdder();
        }

        for (int i = 0; i < 4; i++)
        {
            if (canMove[i] == 0)
            {
                if (moveScore[i] > bestMoveScore)
                {
                    bestMoveScore = moveScore[i];
                    nextMove = moveDir[i];
                }
                else if (moveScore[i] == bestMoveScore)
                {
                    int r = Random.Range(0, 100);
                    if (r <= 25)
                    {
                        bestMoveScore = moveScore[i];
                        nextMove = moveDir[i];
                    }

                }
            }
        }

        if (nextMove == Vector2.zero)
        {
            for (int i = 0; i < 4; i++)
            {
                if (canMove[i] != 2)
                {
                    if (moveScore[i] > bestMoveScore)
                    {
                        bestMoveScore = moveScore[i];
                        nextMove = moveDir[i];
                    }
                    else if (moveScore[i] == bestMoveScore)
                    {
                        int r = Random.Range(0, 100);
                        if (r <= 70)
                        {
                            bestMoveScore = moveScore[i];
                            nextMove = moveDir[i];
                        }

                    }
                }
            }
        }

        walkerPos += nextMove;
        int addToStuckfix = 0;

        if (hasScaleFactor)
        {

            for (int i = 0; i < positions.Count; i++)
            {

                for (int z = 0; z < pLitePos.Length; z++)
                {
                    if (pLitePos[z] == positions[i])
                    {
                        positions[i] = Vector2.one * -100;
                    }
                }

                if (walkerPos == positions[i])
                {
                    addToStuckfix++;
                    //Debug.Log("Stuckfix add" + addToStuckfix + " + " + stuckFix);
                    break;
                }
            }


            if (addToStuckfix == 0 && stuckFix != 0)
            {
                //Debug.Log("Stuckfix reset");
                stuckFix = 0;
            }

            stuckFix += addToStuckfix;

            if (stuckFix >= 150)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    positions[i] = Vector2.zero * -10;
                }
                Debug.Log("Cleared");
                stuckFix = 0;
            }
        }

        if (positions.Count <= listSize)
        {
            //Debug.Log("is here");
            positions.Add(walkerPos);
        }
        else
        {
            //Debug.Log("is there");
            positions[currentListIndex] = walkerPos;
            currentListIndex++;
            if (currentListIndex >= positions.Count)
                currentListIndex = 0;
        }


        return nextMove;
    }
}


//All valid outputs:
// Vector2(-1, 0);
// Vector2(1, 0);
// Vector2(0, 1);
// Vector2(0, -1);

//Any other outputs will kill the walker!
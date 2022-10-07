using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//public class WalkerTest : ProcessingLite.GP21
//{
//    //This file is only for testing your movement/behavior.
//    //The Walkers will compete in a different program!


//    IRandomWalker walker;
//    IRandomWalker[] walker2;
//    public Vector2 walkerPos;
//    public Vector2[] walkerPos2;

//    public float scaleFactor = 0.05f;

//    void Start()
//    {
//        walker2 = new IRandomWalker[10];
//        walkerPos2 = new Vector2[10];
//        //Some adjustments to make testing easier
//        Application.targetFrameRate = 120;
//        QualitySettings.vSyncCount = 0;

//        //Create a walker from the class Example it has the type of WalkerInterface
//        walker = new LucAnd();
//        for (int i = 0; i < walker2.Length; i++)
//        {
//            walker2[i] = new Example();
//        }

//        //Get the start position for our walker.
//        walkerPos = walker.GetStartPosition((int)(Width / scaleFactor), (int)(Height / scaleFactor));
//        for (int i = 0; i < walkerPos2.Length; i++)
//        {
//            walkerPos2[i] = walker2[i].GetStartPosition((int)(Width / scaleFactor), (int)(Height / scaleFactor));
//        }

//    }

//    void Update()
//    {
//        //Draw the walker
//        Stroke(200, 100, 50);
//        Point(walkerPos.x * scaleFactor, walkerPos.y * scaleFactor);
//        for (int i = 0; i < walker2.Length; i++)
//        {
//            Stroke(50 + (i * 10), 200, 100);
//            Point(walkerPos2[i].x * scaleFactor, walkerPos2[i].y * scaleFactor);
//        }
//        //Get the new movement from the walker.
//        walkerPos += walker.Movement();
//        for (int i = 0; i < walker2.Length; i++)
//        {
//            walkerPos2[i] += walker2[i].Movement();
//        }
//    }
//}



//using System;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

public class WalkerTest : ProcessingLite.GP21
{
    //This file is only for testing your movement/behavior.
    //The Walkers will compete in a different program!

    List<IRandomWalker> walkers;
    List<Vector2> walkerPos;
    List<Vector3> walkerColors;
    List<string> walkerNames;
    float scaleFactor = 0.02f;
    List<bool> walkerAlive;
    int[] ownerOfCell;
    [SerializeField] TextMeshProUGUI textObject;
    int gameTicks;

    void Start()
    {
        //Some adjustments to make testing easier
        //Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
        walkers = new List<IRandomWalker>();
        walkerPos = new List<Vector2>();
        walkerColors = new List<Vector3>();
        walkerAlive = new List<bool>();
        walkerNames = new List<string>();
        ownerOfCell = new int[(int)(Width / scaleFactor) * (int)(Height / scaleFactor)];
        for (int i = 0; i < ownerOfCell.Length; i++)
        {
            ownerOfCell[i] = -1;
        }
        //Create a walker from the class Example it has the type of WalkerInterface
        //walkerColors.Add(new Vector3(Random.Range(128, 255), Random.Range(128, 255), Random.Range(128, 255)));

        for (int i = 0; i < 1; i++)
        {
            IRandomWalker walker = new LucAnd();
            walkers.Add(walker);
            //walkerColors.Add(new Vector3(Random.Range(128, 255), Random.Range(128, 255), Random.Range(128, 255)));
            walkerColors.Add(new Vector3(0, 255, 0));
            walkerAlive.Add(true);
            walkerNames.Add(walker.GetName());
        }
        for (int i = 0; i < 10; i++)
        {
            IRandomWalker walker = new Example();
            walkers.Add(walker);
            //walkerColors.Add(new Vector3(Random.Range(128, 255), Random.Range(128, 255), Random.Range(128, 255)));
            walkerColors.Add(new Vector3(0, 0, 255));
            walkerAlive.Add(true);
            walkerNames.Add(walker.GetName());
        }
        //walkers.Add(new Example());
        //walkerColors.Add(new Vector3(0, 255, 0));
        //walkers.Add(new SamKar());
        //walkerColors.Add(new Vector3(0, 0, 255));
        //Get the start position for our walker.
        for (int i = 0; i < walkers.Count; i++)
        {
            walkerPos.Add(walkers[i].GetStartPosition((int)(Width / scaleFactor), (int)(Height / scaleFactor)));
        }
    }

    void Update()
    {
        gameTicks++;
        if (gameTicks >= 14400)
        {
            Debug.LogError("GAmeOVER");
        }
        for (int ticksInFrame = 0; ticksInFrame < 1; ticksInFrame++)
        {
            //Draw the walker
            for (int i = 0; i < walkers.Count; i++)
            {
                if (walkerAlive[i])
                {
                    Stroke((int)walkerColors[i].x, (int)walkerColors[i].y, (int)walkerColors[i].z);
                    Point(walkerPos[i].x * scaleFactor, walkerPos[i].y * scaleFactor);
                    ownerOfCell[Mathf.Max(Mathf.Min(((int)walkerPos[i].y * (int)(Width / scaleFactor)) + (int)(walkerPos[i].x), ownerOfCell.Length - 1), 0)] = i;
                }
            }
            //Get the new movement from the walker.
            for (int i = 0; i < walkers.Count; i++)
            {
                if (walkerAlive[i])
                {
                    walkerPos[i] += walkers[i].Movement();
                }
                for (int j = 0; j < walkers.Count; j++)
                {
                    if (i != j)
                    {
                        if (walkerPos[i] == walkerPos[j] && walkerAlive[j] && walkerAlive[i])
                        {
                            walkerColors[i] = new Vector3(0, 0, 0);
                            walkerColors[j] = new Vector3(0, 0, 0);
                            walkerAlive[i] = false;
                            walkerAlive[j] = false;
                        }
                    }
                }
            }
        }
        UpdateLeaderboardText();
    }

    void UpdateLeaderboardText()
    {
        string text = "";
        int[] owns = new int[walkers.Count];
        for (int i = 0; i < ownerOfCell.Length; i++)
        {
            if (ownerOfCell[i] != -1)
            {
                owns[ownerOfCell[i]]++;
            }
        }
        List<int> sortedOwns = new List<int>();

        for (int i = 0; i < owns.Length; i++)
        {
            int maxFound = 0;
            int maxIndex = 0;
            for (int j = 0; j < owns.Length; j++)
            {
                if (owns[j] > maxFound && !sortedOwns.Contains(j))
                {
                    maxFound = owns[j];
                    maxIndex = j;
                }
            }
            sortedOwns.Add(maxIndex);
        }
        for (int i = 0; i < walkers.Count; i++)
        {
            text += walkerNames[sortedOwns[i]] + ": " + owns[sortedOwns[i]] + "\r\n";
        }
        textObject.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assignment1 : ProcessingLite.GP21
{
    public float curveSize = 10;

    float curveStepX, curveStepY;
    float curveX, curveY, curveX1, curveY1;
    int randomR, randomG, randomB;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 2)
        {
            timer = 0;
            Background(0);


            RandomStrokeColor();
            BeginShape();
            for (int i = 0; i < 5; i++)
            {
                curveX = Random.Range(Width, 0);
                curveY = Random.Range(Height, 0);
                if (i == 0)
                {
                    curveX1 = curveX;
                    curveY1 = curveY;
                }
                Vertex(curveX, curveY);
                if (i == 4)
                {
                    Vertex(curveX1, curveY1);
                }
            }
            EndShape();

            Stroke(166, 0, 150);

            curveStepX = Width / curveSize;
            curveStepY = Height / curveSize;
            for (int i = 0; i < curveSize + 1; i++)
            {
                Stroke(166, 0, 150);

                if (i % 3 == 0)
                {
                    Stroke(166, 0, 50);
                }

                float posx = curveStepX * i;
                float posY = Height - (curveStepY * i);
                Line(0, posY, posx, 0);
            }
            for (int i = 0; i < curveSize; i++)
            {
                Stroke(166, 0, 150);

                if (i % 3 == 0)
                {
                    Stroke(166, 0, 50);
                }

                float posx = Width - (curveStepX * i);
                float posY = curveStepY * i;
                Line(Width, posY, posx, Height);
            }

            Stroke(166, 60, 0);
            //L
            Line(4, 7, 4, 3);
            Line(3.95f, 3, 6, 3);

            //A
            Line(8, 3, 10.0201f, 7);
            Line(12, 3, 9.9799f, 7);
            Line(9, 5, 11, 5);

        }

        void RandomStrokeColor()
        {
            randomR = Random.Range(0, 255);
            randomG = Random.Range(0, 255);
            randomB = Random.Range(0, 255);

            Stroke(randomR, randomG, randomB);
        }

    }
}

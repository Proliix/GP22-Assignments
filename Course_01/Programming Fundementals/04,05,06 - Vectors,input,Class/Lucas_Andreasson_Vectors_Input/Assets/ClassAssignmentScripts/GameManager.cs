using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ProcessingLite.GP21
{
    Player player;
    Ball[] balls;
    int maxBalls = 100;
    int currentBalls = 0;
    bool gameover = false;
    float timer;
    float cooldown;

    void Start()
    {
        ResetGame();
    }

    void ResetGame()
    {
        gameover = false;
        timer = 0;
        cooldown = timer + 3;
        currentBalls = 0;
        balls = new Ball[maxBalls];
        player = new Player(Width / 2, Height / 2, 0.2f);
        AddNewBall();
        AddNewBall();
        AddNewBall();

    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ResetGame();
        }
        if (!gameover)
        {
            timer += Time.deltaTime;
            player.UpdatePlayer();

            for (int i = 0; i < currentBalls; i++)
            {
                balls[i].UpdatePos();
                if (CheckCollision(balls[i].GetPos().x, balls[i].GetPos().y, balls[i].GetSize(), player.GetPos().x, player.GetPos().y, player.GetSize()))
                    gameover = true;

            }

            if (currentBalls < maxBalls && timer > cooldown)
            {
                AddNewBall();
                cooldown = timer + 3;   
            }
        }

        DrawGame();
    }

    void AddNewBall()
    {
        if (currentBalls < maxBalls)
        {
            balls[currentBalls] = new Ball(RandomScreenPos(false, 1));
            balls[currentBalls].SetColor(250, 100, 20);
            currentBalls++;
        }
    }

    bool CheckCollision(float x1, float y1, float size1, float x2, float y2, float size2)
    {
        float maxDistance = (size1 / 2) + (size2 / 2);

        if (Mathf.Abs(x1 - x2) > maxDistance || Mathf.Abs(y1 - y2) > maxDistance)
        {
            return false;
        }
        else if (Vector2.Distance(new Vector2(x1, y1), new Vector2(x2, y2)) > maxDistance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //if true it can return a value ontop of player
    //Size is for checking position
    Vector2 RandomScreenPos(bool overlapPlayer = true, float objSize = 0)
    {
        Vector2 pos = Vector2.zero;
        pos.x = Random.Range(0, Width);
        pos.y = Random.Range(0, Height);

        if (!overlapPlayer)
        {
            while (CheckCollision(pos.x, pos.y, objSize, player.GetPos().x, player.GetPos().y, player.GetSize()))
            {
                pos.x = Random.Range(0, Width);
                pos.y = Random.Range(0, Height);
            }
        }

        return pos;
    }

    public string FormatTime(float time)
    {
        return System.TimeSpan.FromSeconds(time).ToString("mm\\:ss\\.ff");
    }
    private void DrawGame()
    {
        Background(50,166,240);
        if (!gameover)
        {
            Fill(200, 100, 20);
            TextSize(50);
            Text(FormatTime(timer), Width / 2, Height - 0.5f);
            for (int i = 0; i < currentBalls; i++)
            {
                balls[i].Draw();
            }
            player.Draw();
        }
        else if (gameover)
        {
            Fill(200, 100, 20);
            TextSize(250);
            Text("GAME OVER", Width / 2, Height / 2);
            TextSize(125);
            Text("You survived for: " + FormatTime(timer), Width / 2, (Height / 2) - 1.5f);
        }

    }

}

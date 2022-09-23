using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ProcessingLite.GP21
{
    private float size = 0.2f;
    private float maxSpeed = 10f;
    private Vector2 position;
    private float acceleration = 6f;
    private Vector2 velocity;
    private Vector2 inputDirection;

    public void UpdatePlayer()
    {
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inputDirection == Vector2.zero)
        {
            //Stops the ball from "Vibrating" at low values
            if (velocity.x < 0.05f && velocity.x > -0.05f)
            {
                velocity = new Vector2(0, velocity.y);
            }
            else
            {
                if (velocity.x > 0)
                {
                    velocity = new Vector2(velocity.x - acceleration * Time.deltaTime, velocity.y);
                }
                else if (velocity.x < 0)
                {
                    velocity = new Vector2(velocity.x + acceleration * Time.deltaTime, velocity.y);
                }
            }
            if (velocity.y < 0.1f && velocity.y > -0.1f)
            {
                velocity = new Vector2(velocity.x, 0);
            }
            else
            {
                if (velocity.y > 0)
                {
                    velocity = new Vector2(velocity.x, velocity.y - acceleration * Time.deltaTime);
                }
                else if (velocity.y < 0)
                {
                    velocity = new Vector2(velocity.x, velocity.y + acceleration * Time.deltaTime);
                }
            }
        }


        velocity = new Vector2(Mathf.Clamp(velocity.x + (acceleration * inputDirection.x) * Time.deltaTime, -maxSpeed, maxSpeed), Mathf.Clamp(velocity.y + (acceleration * inputDirection.y) * Time.deltaTime, -maxSpeed, maxSpeed));

        position += velocity * Time.deltaTime;

        ScreenCollision();



    }

    public Vector2 GetPos()
    {
        return position;
    }

    public float GetSize()
    {
        return size;
    }

    void ScreenCollision()
    {
        if (position.x + (size / 2) > Width && velocity.x > 0 || position.x - (size / 2) < 0 && velocity.x < 0)
        {
            velocity.x = velocity.x * -0.75f;
        }
        
        if (position.y + (size / 2) > Height && velocity.y > 0 || position.y - (size / 2) < 0 && velocity.y < 0)
        {
            velocity.y = velocity.y * -0.75f;
        }
    }

    public void Draw()
    {
        StrokeWeight(0.5f);
        Fill(20, 100, 200);
        Circle(position.x, position.y, size);
    }


    public Player(float x, float y, float size)
    {
        position = new Vector2(x, y);
        this.size = size;
    }
}

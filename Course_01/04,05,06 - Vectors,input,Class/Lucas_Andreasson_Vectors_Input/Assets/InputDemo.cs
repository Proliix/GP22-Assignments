using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : ProcessingLite.GP21
{
    float diameter = 1;
    [SerializeField]
    float maxSpeed = 2;
    [SerializeField]
    float gravity = 4;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float acceleration = 2f;
    [SerializeField]
    float deacceleration = 2f;
    [SerializeField]
    float screenWarpDistance = 5;

    bool hasGravity = false;

    Vector2 velocity;
    Vector2 circlePos;
    Vector2 circlePosAccel;
    Vector2 inputDirection;

    void Start()
    {
        hasGravity = false;
        circlePos = new Vector2(Width / 2, Height / 2);
        circlePosAccel = circlePos;
    }

    void Update()
    {
        Background(50, 166, 240);

        //gets the rawInput
        inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // switches the gravity bool
        if (Input.GetKeyDown(KeyCode.G))
            hasGravity = !hasGravity;
    

        //give the ball gravity if the bool is active
        if (!hasGravity)
        {
            velocity = new Vector2(Mathf.Clamp(velocity.x + (acceleration * inputDirection.x) * Time.deltaTime, -maxSpeed, maxSpeed), Mathf.Clamp(velocity.y + (acceleration * inputDirection.y) * Time.deltaTime, -maxSpeed, maxSpeed));
        }
        else if (hasGravity)
        {
            velocity = new Vector2(Mathf.Clamp(velocity.x + (acceleration * inputDirection.x) * Time.deltaTime, -maxSpeed * diameter, maxSpeed * diameter), Mathf.Clamp(velocity.y - gravity * Time.deltaTime, -maxSpeed, maxSpeed));
            FloorDetection();
        }

        //Stops the ball from "Vibrating" at low values
        if (inputDirection == Vector2.zero)
        {
            if (velocity.x < 0.05f && velocity.x > -0.05f)
            {
                velocity = new Vector2(0, velocity.y);
            }
            else
            {
                if (velocity.x > 0)
                {
                    velocity = new Vector2(velocity.x - deacceleration * Time.deltaTime, velocity.y);
                }
                else if (velocity.x < 0)
                {
                    velocity = new Vector2(velocity.x + deacceleration * Time.deltaTime, velocity.y);
                }
            }
            if (!hasGravity)
            {
                if (velocity.y < 0.1f && velocity.y > -0.1f)
                {
                    velocity = new Vector2(velocity.x, 0);
                }
                else
                {
                    if (velocity.y > 0)
                    {
                        velocity = new Vector2(velocity.x, velocity.y - deacceleration * Time.deltaTime);
                    }
                    else if (velocity.y < 0)
                    {
                        velocity = new Vector2(velocity.x, velocity.y + deacceleration * Time.deltaTime);
                    }
                }
            }
        }

        //gives the balls direction and speed
        circlePos += inputDirection * speed * Time.deltaTime;
        circlePosAccel += velocity * Time.deltaTime;

        ScreenWarp();

        //draws the balls
        Circle(circlePos.x, circlePos.y, diameter);
        Circle(circlePosAccel.x, circlePosAccel.y, diameter);
    }

    //Detect the floor and bounce the ball
    void FloorDetection()
    {
        if (hasGravity)
        {
            if (circlePosAccel.y - (diameter / 2) <= 0)
            {
                velocity = new Vector2(velocity.x, -velocity.y);
            }
        }
    }
    //Create the screenWarp effect for the accelerating one
    void ScreenWarp()
    {
        if (circlePosAccel.x < screenWarpDistance)
        {
            Circle(circlePosAccel.x + Width, circlePosAccel.y, diameter);
            if (circlePosAccel.x + Width < Width - screenWarpDistance)
            {
                circlePosAccel = new Vector2(circlePosAccel.x + Width, circlePosAccel.y);
            }
        }
        else if (circlePosAccel.x > Width - screenWarpDistance)
        {
            Circle(circlePosAccel.x - Width, circlePosAccel.y, diameter);

            if (circlePosAccel.x - Width > screenWarpDistance)
            {
                circlePosAccel = new Vector2(circlePosAccel.x - Width, circlePosAccel.y);
            }
        }

        if (circlePosAccel.y < screenWarpDistance)
        {
            Circle(circlePosAccel.x, circlePosAccel.y + Height, diameter);
            if (circlePosAccel.y + Height < Height - screenWarpDistance)
            {
                circlePosAccel = new Vector2(circlePosAccel.x, circlePosAccel.y + Height);
            }
        }
        else if (circlePosAccel.y > Height - screenWarpDistance)
        {
            Circle(circlePosAccel.x, circlePosAccel.y - Height, diameter);
            if (circlePosAccel.y - Height > screenWarpDistance)
            {
                circlePosAccel = new Vector2(circlePosAccel.x, circlePosAccel.y - Height);
            }
        }

    }
}

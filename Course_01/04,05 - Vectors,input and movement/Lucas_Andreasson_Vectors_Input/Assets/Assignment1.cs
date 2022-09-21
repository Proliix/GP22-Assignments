using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Assignment1 : ProcessingLite.GP21
{
    public Vector2 circlePos;
    [Range(0.5f, 2)]
    public float speed = 1;
    public float diameter = 4;
    public float speedlimit = 8;
    public float falloffSpeed = 0.95f;

    private Vector2 movementVector;

    // Start is called before the first frame update
    void Start()
    {
        circlePos = new Vector2(Width / 2, Height / 2);
        movementVector = Vector2.zero;
    }

    void CheckCollision()
    {
        float top = circlePos.y + (diameter / 2);
        float bottom = circlePos.y - (diameter / 2);
        float right = circlePos.x + (diameter / 2);
        float left = circlePos.x - (diameter / 2);

        if (top >= Height && movementVector.y > 0 || bottom <= 0 && movementVector.y < 0)
        {
            movementVector = new Vector2(movementVector.x, -movementVector.y);
        }
        else if (right >= Width && movementVector.x > 0 || left <= 0 && movementVector.x < 0)
        {
            movementVector = new Vector2(-movementVector.x, movementVector.y);
        }


    }
    // Update is called once per frame
    void Update()
    {
        Background(Color.black);

        if (Input.GetMouseButton(0))
        {
            //draw line from circle to mouse
            Line(circlePos.x, circlePos.y, MouseX, MouseY);
        }

        if (Input.GetMouseButtonDown(0))
        {
            //reset movementvecotr and move circle to mouse
            circlePos = new Vector2(MouseX, MouseY);
            movementVector = Vector2.zero;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //calculate movementVector
            movementVector = new Vector2(MouseX, MouseY);
            movementVector = movementVector - circlePos;
        }

        //Speedlimit
        if (movementVector.magnitude > speedlimit)
        {
            movementVector = movementVector.normalized * speedlimit;
        }

        CheckCollision();

        circlePos += movementVector * speed * Time.deltaTime;

        Circle(circlePos.x, circlePos.y, diameter);

    }
}

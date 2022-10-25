using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Ball : ProcessingLite.GP21
{
    //Our class variables
    private Vector2 position; //Ball position
    private Vector2 velocity; //Ball direction
    private Vector3Int color;
    private float size = 0.5f;

    //Ball Constructor, called when we type new Ball(x, y);
    public Ball(Vector2 pos)
    {
        //Set our position when we create the code.
        position = pos;

        //Create the velocity vector and give it a random direction.
        velocity = new Vector2();
        velocity.x = Random.Range(-10, 10);
        velocity.y = Random.Range(-10, 10);
    }

    public void SetColor(int r, int g, int b)
    {
        color = new Vector3Int(r, g, b);
    }

    public void SetSize(float size)
    {
        this.size = size;
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
            velocity.x = velocity.x * -1;
        }
        else if (position.y + (size / 2) > Height && velocity.y > 0 || position.y - (size / 2) < 0 && velocity.y < 0)
        {
            velocity.y = velocity.y * -1;
        }
    }


    //Draw our ball
    public void Draw()
    {
        Fill(color.x, color.y, color.z);
        Circle(position.x, position.y, size);
    }

    //Update our ball
    public void UpdatePos()
    {
        ScreenCollision();
        position += velocity * Time.deltaTime;
    }
}

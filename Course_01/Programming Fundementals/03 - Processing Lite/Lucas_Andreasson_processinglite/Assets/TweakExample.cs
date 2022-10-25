using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakExample : ProcessingLite.GP21
{
    public float x;
    public float y;
    public float diameter = 0.2f;

    void Update()
    {
        Background(0); //Clears the background and sets the color to 0.
        Circle(x, y, diameter); //Draws a circle on screen.
    }
}

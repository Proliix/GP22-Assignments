using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Vector2 Size;
    public Transform StartPos;
    public GameObject Box;
    public Material backMaterial;
    public Material floorMaterial;

    private GameObject boxes;
    private GameObject floor;
    private GameObject walls;
    private GameObject backWalls;

    public void CreateStage()
    {
        boxes = new GameObject();
        boxes.name = "Boxes";
        floor = new GameObject();
        floor.name = "Floor";
        walls = new GameObject();
        walls.name = "Walls";

        CreateWall();

        for (int x = 0; x < Size.x; x++)
        {
            for (int z = 0; z < Size.y; z++)
            {
                GameObject zCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                zCube.name = "zCube" + x + " " + z;
                zCube.transform.position = new Vector3(StartPos.position.x + x, StartPos.position.y, StartPos.position.z + z);
                zCube.layer = 6;
                zCube.transform.parent = floor.transform;
                zCube.GetComponent<MeshRenderer>().material = floorMaterial;
                if (x % 2 == 1 && z % 2 == 1)
                {
                    GameObject yCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    yCube.name = "yCube" + x + " " + z;
                    yCube.transform.position = new Vector3(StartPos.position.x + x, StartPos.position.y + 1, StartPos.position.z + z);
                    yCube.tag = "Wall";
                    yCube.transform.parent = walls.transform;
                }
                else if (x + z != 0 || x != Size.x && z != Size.y)
                {
                    Vector3 boxPos = new Vector3(StartPos.position.x + x, StartPos.position.y + 1, StartPos.position.z + z);
                    GameObject newBox = Instantiate(Box, boxPos, Box.transform.rotation);
                    newBox.name = "Box " + x + " " + z;
                    newBox.transform.parent = boxes.transform;
                }

            }
            //GameObject xCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //xCube.name = "xCube" + x;
            //xCube.transform.position = new Vector3(StartPos.position.x + x, StartPos.position.y, StartPos.position.z);
            //xCube.transform.parent = floor.transform;
        }
    }

    private void CreateWall()
    {
        backWalls = new GameObject();
        backWalls.name = "Back Walls";

        for (int x = 0; x < Size.x + 1; x++)
        {
            GameObject xCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            xCube.name = "zWallCube" + x + " ";
            xCube.transform.position = new Vector3(StartPos.transform.position.x + x, StartPos.transform.position.y + 1, StartPos.transform.position.z - 1);
            xCube.tag = "Wall";
            xCube.GetComponent<Renderer>().material = backMaterial;
            xCube.transform.parent = backWalls.transform;
            if (x == Size.x)
            {
                for (int z = 0; z < Size.y + 1; z++)
                {
                    GameObject zCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    zCube.name = "zWallCube" + x + " " + z;
                    zCube.transform.position = new Vector3(StartPos.transform.position.x + x, StartPos.transform.position.y + 1, StartPos.transform.position.z + z);
                    zCube.tag = "Wall";
                    zCube.GetComponent<Renderer>().material = backMaterial;
                    zCube.transform.parent = backWalls.transform;
                }
            }
        }
        for (int z = 0; z < Size.y + 1; z++)
        {
            GameObject zCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zCube.name = "zWallCube" + z + " ";
            zCube.transform.position = new Vector3(StartPos.transform.position.x - 1, StartPos.transform.position.y + 1, StartPos.transform.position.z - 1 - z + Size.y);
            zCube.tag = "Wall";
            zCube.transform.parent = backWalls.transform;
            zCube.GetComponent<Renderer>().material = backMaterial;
            if (z == Size.y)
            {
                for (int x = 0; x < Size.x + 1; x++)
                {
                    GameObject xCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    xCube.name = "zWallCube" + x + " ";
                    xCube.transform.position = new Vector3(StartPos.transform.position.x - 1 + x, StartPos.transform.position.y + 1, StartPos.transform.position.z + z);
                    xCube.tag = "Wall";
                    xCube.GetComponent<Renderer>().material = backMaterial;
                    xCube.transform.parent = backWalls.transform;
                }
            }
        }

    }
}

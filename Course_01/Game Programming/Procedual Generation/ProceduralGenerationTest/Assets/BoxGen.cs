using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Vector3 pos;
    public int Index;


    public Vector3[] GetVertices()
    {
        Vector3[] vertices = {
            new Vector3 (0 + pos.x, 0 + pos.y, 0 + pos.z),
            new Vector3 (1+ pos.x, 0 + pos.y, 0 + pos.z),
            new Vector3 (1 + pos.x, 1 + pos.y, 0 + pos.z),
            new Vector3 (0 + pos.x, 1 + pos.y, 0 + pos.z),
            new Vector3 (0 + pos.x, 1 + pos.y, 1 + pos.z),
            new Vector3 (1 + pos.x, 1 + pos.y, 1 + pos.z),
            new Vector3 (1 + pos.x, 0 + pos.y, 1 + pos.z),
            new Vector3 (0 + pos.x, 0 + pos.y, 1 + pos.z),
        };

        return vertices;
    }
}

public class BoxGen : MonoBehaviour
{

    public int Widht = 20;
    public int height = 20;

    public int noiseWidht = 100;
    public int noiseHeight = 100;

    public int chunkAmountWidth = 10;
    public int chunkAmountHeight = 10;

    public float scale = 0.1f;
    public float yOrg = 5;
    public float xOrg = 5;

    public Material mat;

    float yPos;
    float xPos;

    int oldHeight;
    int oldChunkHeight;
    int oldWidth;
    int oldChunkWidth;

    int newIndex;

    Block[,] blocks;

    GameObject[,] cubes;
    GameObject[,] chunks;
    GameObject parent;
    public static float GetNoiseAt(int x, int z, float scale, float heightMultiplier, int octaves, float persistance, float lacunarity)
    {
        float PerlinValue = 0f;
        float amplitude = 1f;
        float frequency = 1f;

        for (int i = 0; i < octaves; i++)
        {
            // Get the perlin value at that octave and add it to the sum
            PerlinValue += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;

            // Decrease the amplitude and the frequency
            amplitude *= persistance;
            frequency *= lacunarity;
        }

        // Return the noise value
        return PerlinValue * heightMultiplier;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        parent = new GameObject();
        parent.name = "holder";
        Generate();
    }

    private void Generate()
    {

        xOrg = Random.Range(0, noiseWidht);
        yOrg = Random.Range(0, noiseHeight);

        if (cubes != null)
        {
            for (int i = 0; i < oldWidth; i++)
            {
                for (int j = 0; j < oldHeight; j++)
                {
                    Destroy(cubes[i, j]);
                }
            }
            cubes = null;
        }

        if (chunks != null)
        {
            for (int i = 0; i < oldChunkWidth; i++)
            {
                for (int j = 0; j < oldChunkHeight; j++)
                {
                    Destroy(chunks[i, j]);
                }
            }
            chunks = null;
        }


        cubes = new GameObject[Widht, height];
        chunks = new GameObject[chunkAmountWidth, chunkAmountHeight];
        blocks = new Block[Widht, height];
        oldChunkHeight = chunkAmountHeight;
        oldChunkWidth = chunkAmountWidth;
        oldHeight = height;
        oldWidth = Widht;
        newIndex = 0;

        for (int x = 0; x < chunkAmountWidth; x++)
        {
            for (int z = 0; z < chunkAmountHeight; z++)
            {
                yPos = z;
                xPos = x;
                chunks[x, z] = GenerateChunk();
                chunks[x, z].transform.position = new Vector3(x * Widht, 0, z * height);

            }
        }


        //for (int x = 0; x < Widht; x++)
        //{
        //    for (int z = 0; z < height; z++)
        //    {
        //        //OldGeneration(x, z);
        //        //NewGeneration(x, z);
        //        BlockGeneration(x, z);
        //    }
        //}

        //MeshGeneration();
        //CreateAllCubes();
    }

    private GameObject GenerateChunk()
    {
        newIndex = 0;
        for (int x = 0; x < Widht; x++)
        {
            for (int z = 0; z < height; z++)
            {
                BlockGeneration(x, z);
            }
        }

        return MeshGeneration();
    }
    private GameObject MeshGeneration()
    {
        List<int> tri = new List<int>();
        List<Vector3> vertices = new List<Vector3>();
        for (int x = 0; x < Widht; x++)
        {
            for (int z = 0; z < height; z++)
            {
                bool left = false, right = false, back = false, front = false;

                Vector3[] newVert = blocks[x, z].GetVertices();
                int index = blocks[x, z].Index;
                for (int i = 0; i < 8; i++)
                {
                    vertices.Add(newVert[i]);
                }

                //check left
                if (x - 1 >= 0)
                {
                    if (blocks[x - 1, z].pos.y != blocks[x, z].pos.y)
                        left = true;
                }
                else
                {
                    //left = true;
                }

                //check right
                if (x + 1 < Widht)
                {
                    if (blocks[x + 1, z].pos.y != blocks[x, z].pos.y)
                        right = true;
                }
                else
                {
                    //right = true;
                }

                //front
                if (z - 1 >= 0)
                {
                    if (blocks[x, z - 1].pos.y != blocks[x, z].pos.y)
                        front = true;
                }
                else
                {
                    //front = true;
                }

                //back
                if (z + 1 < height)
                {
                    if (blocks[x, z + 1].pos.y != blocks[x, z].pos.y)
                        back = true;
                }
                else
                {
                    //back = true;
                }


                if (front)
                {
                    tri.Add(index + 0);
                    tri.Add(index + 2);
                    tri.Add(index + 1);
                    tri.Add(index + 0);
                    tri.Add(index + 3);
                    tri.Add(index + 2);
                }

                //top
                tri.Add(index + 2);
                tri.Add(index + 3);
                tri.Add(index + 4);
                tri.Add(index + 2);
                tri.Add(index + 4);
                tri.Add(index + 5);

                if (right)
                {
                    tri.Add(index + 1);
                    tri.Add(index + 2);
                    tri.Add(index + 5);
                    tri.Add(index + 1);
                    tri.Add(index + 5);
                    tri.Add(index + 6);
                }

                if (left)
                {
                    tri.Add(index + 0);
                    tri.Add(index + 7);
                    tri.Add(index + 4);
                    tri.Add(index + 0);
                    tri.Add(index + 4);
                    tri.Add(index + 3);
                }

                if (back)
                {
                    tri.Add(index + 5);
                    tri.Add(index + 4);
                    tri.Add(index + 7);
                    tri.Add(index + 5);
                    tri.Add(index + 7);
                    tri.Add(index + 6);
                }

                //             int[] triangles = {
                //         0, 2, 1, //face front
                //0, 3, 2,
                //         2, 3, 4, //face top
                //2, 4, 5,
                //         1, 2, 5, //face right
                //1, 5, 6,
                //         0, 7, 4, //face left
                //0, 4, 3,
                //         5, 4, 7, //face back
                //5, 7, 6,
                //     };

                //Debug.Log(cubes[x, z].name + " front: " + front + "|right: " + right + "|left: " + left + "| back" + back,this);

            }
        }


        GameObject newChunk = new GameObject();
        newChunk.name = "chunk";
        newChunk.AddComponent<MeshRenderer>();
        newChunk.AddComponent<MeshFilter>();
        newChunk.GetComponent<MeshRenderer>().material = mat;
        Mesh mesh = newChunk.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = tri.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        return newChunk;
    }

    private void NewGeneration(int x, int z)
    {
        GameObject cube = new GameObject();
        cube.name = "Cube " + x + " " + z;
        cube.AddComponent<MeshRenderer>();
        cube.AddComponent<MeshFilter>();
        cube.transform.parent = parent.transform;
        float xCoord = xOrg + (x / (noiseWidht * scale));
        float yCoord = yOrg + (z / (noiseHeight * scale));
        cube.transform.position = new Vector3(x, Mathf.RoundToInt(Mathf.PerlinNoise(xCoord, yCoord) * 10), z);
        cube.GetComponent<MeshRenderer>().material.color = new Color(Mathf.PerlinNoise(xCoord, yCoord), Mathf.PerlinNoise(xCoord, yCoord), Mathf.PerlinNoise(xCoord, yCoord));
        //CreateCube(cube);
        cubes[x, z] = cube;
    }

    private void BlockGeneration(int x, int z)
    {
        Block cube = new Block();
        float xCoord = xOrg + ((x + xPos * Widht) / (noiseWidht * scale));
        float yCoord = yOrg + ((z + yPos * height) / (noiseHeight * scale));
        cube.pos = new Vector3(x, Mathf.RoundToInt(Mathf.PerlinNoise(xCoord, yCoord) * 10), z);
        cube.Index = newIndex * 8;
        newIndex++;
        blocks[x, z] = cube;
    }

    private void CreateAllCubes()
    {

        for (int x = 0; x < Widht; x++)
        {
            for (int z = 0; z < height; z++)
            {
                bool left = false, right = false, back = false, front = false;

                List<int> tri = new List<int>();

                Vector3[] vertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

                //check left
                if (x - 1 >= 0)
                {
                    if (cubes[x - 1, z].transform.position.y != cubes[x, z].transform.position.y)
                        left = true;
                }
                else
                {
                    //left = true;
                }

                //check right
                if (x + 1 < Widht)
                {
                    if (cubes[x + 1, z].transform.position.y != cubes[x, z].transform.position.y)
                        right = true;
                }
                else
                {
                    //right = true;
                }

                //front
                if (z - 1 >= 0)
                {
                    if (cubes[x, z - 1].transform.position.y != cubes[x, z].transform.position.y)
                        front = true;
                }
                else
                {
                    //front = true;
                }

                //back
                if (z + 1 < height)
                {
                    if (cubes[x, z + 1].transform.position.y != cubes[x, z].transform.position.y)
                        back = true;
                }
                else
                {
                    //back = true;
                }


                if (front)
                {
                    tri.Add(0);
                    tri.Add(2);
                    tri.Add(1);
                    tri.Add(0);
                    tri.Add(3);
                    tri.Add(2);
                }

                //top
                tri.Add(2);
                tri.Add(3);
                tri.Add(4);
                tri.Add(2);
                tri.Add(4);
                tri.Add(5);

                if (right)
                {
                    tri.Add(1);
                    tri.Add(2);
                    tri.Add(5);
                    tri.Add(1);
                    tri.Add(5);
                    tri.Add(6);
                }

                if (left)
                {
                    tri.Add(0);
                    tri.Add(7);
                    tri.Add(4);
                    tri.Add(0);
                    tri.Add(4);
                    tri.Add(3);
                }

                if (back)
                {
                    tri.Add(5);
                    tri.Add(4);
                    tri.Add(7);
                    tri.Add(5);
                    tri.Add(7);
                    tri.Add(6);
                }

                //             int[] triangles = {
                //         0, 2, 1, //face front
                //0, 3, 2,
                //         2, 3, 4, //face top
                //2, 4, 5,
                //         1, 2, 5, //face right
                //1, 5, 6,
                //         0, 7, 4, //face left
                //0, 4, 3,
                //         5, 4, 7, //face back
                //5, 7, 6,
                //     };

                //Debug.Log(cubes[x, z].name + " front: " + front + "|right: " + right + "|left: " + left + "| back" + back,this);

                Mesh mesh = cubes[x, z].GetComponent<MeshFilter>().mesh;
                mesh.Clear();
                mesh.vertices = vertices;
                mesh.triangles = tri.ToArray();
                mesh.Optimize();
                mesh.RecalculateNormals();
            }
        }
    }

    private void CreateCube(GameObject cube)
    {
        Vector3[] vertices = {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

        int[] triangles = {
            0, 2, 1, //face front
			0, 3, 2,
            2, 3, 4, //face top
			2, 4, 5,
            1, 2, 5, //face right
			1, 5, 6,
            0, 7, 4, //face left
			0, 4, 3,
            5, 4, 7, //face back
			5, 7, 6,
        };

        Mesh mesh = cube.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    private void OldGeneration(int x, int z)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Plane);
        cube.transform.localScale = Vector3.one * 0.1f;
        cube.transform.parent = parent.transform;
        float xCoord = xOrg + (x / (noiseWidht * scale));
        float yCoord = yOrg + (z / (noiseHeight * scale));
        cube.transform.position = new Vector3(x, Mathf.RoundToInt(Mathf.PerlinNoise(xCoord, yCoord) * 10), z);
        cube.GetComponent<MeshRenderer>().material.color = new Color(Mathf.PerlinNoise(xCoord, yCoord), Mathf.PerlinNoise(xCoord, yCoord), Mathf.PerlinNoise(xCoord, yCoord));
        Debug.Log(Mathf.PerlinNoise(xCoord, yCoord) + "\n_________________________________");
        cubes[x, z] = cube;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            Generate();
    }
}

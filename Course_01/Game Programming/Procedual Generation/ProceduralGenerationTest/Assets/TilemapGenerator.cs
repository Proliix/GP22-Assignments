using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public int width = 20, height = 20; //The size we have to work with
    public RuleTile tile; //Reference to our tile that creates our map.

    Tilemap map; //The actual tilemap
    internal bool[,] bufferMap; //our map that we modify
    internal Vector2Int startPosition; //Player start position

    // Start is called before the first frame update
    void Start()
    {
        map = GetComponent<Tilemap>();
        Generate();
    }

    private void Generate()
    {
        bufferMap = new bool[width, height]; //Create a new map / clear the old
        map.ClearAllTiles(); //Clear the tile map

        InitStartPosition(); //Set startposition
        GenerateTilemap(); //Generate the map
        UpdateMapFromBuffer(); //update the real map
    }

    void UpdateMapFromBuffer()
    {
        //loop all position
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (bufferMap[x, y]) //If the buffer is true, spawn/activate a tile
                    map.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public virtual void InitStartPosition()
    {
        //Select a start position for the player
        startPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

        //make sure we are not to close to the wall
        startPosition = ClampVector(startPosition, new Vector2Int(1, 1));

        //Clear the tiles around the player.
        bufferMap[startPosition.x, startPosition.y] = true;
        bufferMap[startPosition.x + 1, startPosition.y] = true;
        bufferMap[startPosition.x - 1, startPosition.y] = true;
        bufferMap[startPosition.x, startPosition.y + 1] = true;
        bufferMap[startPosition.x, startPosition.y - 1] = true;
        bufferMap[startPosition.x - 1, startPosition.y - 1] = true;
        bufferMap[startPosition.x + 1, startPosition.y + 1] = true;
        bufferMap[startPosition.x - 1, startPosition.y + 1] = true;
        bufferMap[startPosition.x + 1, startPosition.y - 1] = true;

        //Find the player and move him to the start point.
        //GameObject.FindGameObjectWithTag("Player").transform.position =
        //	map.CellToWorld(new Vector3Int(startPosition.x, startPosition.y, 0)) + new Vector3(0.5f, 0.5f, 0);
    }

    //This vector clamps a vector from a certian position from the edge of the map
    internal Vector2Int ClampVector(Vector2Int startPosition, Vector2Int limit)
    {
        return new Vector2Int(
            Mathf.Clamp(startPosition.x, limit.x, (width - 1) - limit.x),
            Mathf.Clamp(startPosition.y, limit.y, (height - 1) - limit.y));
    }

    public virtual void GenerateTilemap()
    {
        //This should be replaced with an actual system for procedural generation
        //50% random fill
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (Random.Range(0, 2) == 1)
                    bufferMap[x, y] = true;
    }

    void Update()
    {
        //Debug commands to clear the old map
        if (Input.GetKeyDown(KeyCode.G))
            Generate();
    }
}
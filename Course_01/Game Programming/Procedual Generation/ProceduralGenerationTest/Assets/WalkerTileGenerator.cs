using UnityEngine;

public class WalkerTileGenerator : TilemapGenerator
{
	public int steps = 1000; //I will walk 500 tiles, and I will walk 500 more
	Vector2Int walkerPos;

	public override void InitStartPosition()
	{
		//Run the base class start position code
		base.InitStartPosition();
		walkerPos = startPosition;
	}

	public override void GenerateTilemap()
	{
		int x = walkerPos.x;
		int y = walkerPos.y;

		for (int i = 0; i < steps; i++)
		{
			//randomize direction
			int dir = Random.Range(0, 4);

			//mvoe
			if (dir == 0)
				x++;
			else if (dir == 1)
				x--;
			else if (dir == 2)
				y++;
			else
				y--;

			//stay inbounds
			x = Mathf.Clamp(x, 0, width - 1);
			y = Mathf.Clamp(y, 0, height - 1);

			//update map
			bufferMap[x, y] = true;
		}
	}
}
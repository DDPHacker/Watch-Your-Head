using UnityEngine;
using System.Collections;

public class MapGenerate : MonoBehaviour {

	public float tilesize = 0.32;

	private int row = 15;
	private int col = 20;
	private int[,] block = new int[row, col];
	private int blocknum = 0;

	// Use this for initialization
	void Start () {

		// Initialize the block array
		for (int i = 0; i < row; i++)
			for (int j = 0; j < col; j++)
				block [i, j] = 0;

		// Get the position of the tiles
		int r, c, l;
		r = Random.Range (0, 2);

		while (1) {
			c = Random.Range (0, 8);
			while (1) {
				l = Random.Range (2, 10);
				if (c + l >= col - 1)
					l = col - 1 - c;
				for (int i = 0; i < l; i++)
					block [r, c + i] = 1;
				blocknum += l;
				c += Random.Range (4, 8);
				if (c >= col - 1)
					break;
			}

			r += Random.Range (2, 5);
			if (r >= col - 1)
				break;
		}

		// Set the portals (portal = 6)
		int p1 = Random.Range (1, blocknum/2);
		int p2 = Random.Range (blocknum / 2 + 1, blocknum);
		GameObject PortalTile1 = (GameObject)Instantiate (Resources.Load ("PortalTile"));
		GameObject PortalTile2 = (GameObject)Instantiate (Resources.Load ("PortalTile"));
		GameObject Portal1 = (GameObject)Instantiate (Resources.Load ("Portal"));
		GameObject Portal2 = (GameObject)Instantiate (Resources.Load ("Portal"));

		int cnt = 0;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				if (block [i, j] == 1) {
					cnt += 1;
					if (cnt == p1) {
						block [i, j] = 6;
						SetPosition (PortalTile1, i, j);
						SetPosition (Portal1, i + 1, j);
						blocknum--;
					}
					if (cnt == p2) {
						block [i, j] = 6;
						SetPosition (PortalTile2, i, j);
						SetPosition (Portal2, i + 1, j);
						blocknum--;
					}
				}
			}
		}
		Portal1.getPosition (Portal2);
		Portal2.getPosition (Portal1);

		// Set the other kind of tiles
		int type;
		for (int i = 0; i < row; i++)
			for (int j = 0; j < col; j++) {
				if (block [i, j] == 1) {
					type = Random.Range (0, 7);
					switch (type) {
					case 2: // StoneTile
						GameObject StoneTile = (GameObject)Instantiate (Resources.Load ("TrapTile"));
						SetPosition (StoneTile, i, j);
						break;
					case 3: // IceTile
						GameObject IceTile = (GameObject)Instantiate (Resources.Load ("IceTile"));
						SetPosition (IceTile, i, j);
						GameObject Ice = (GameObject)Instantiate (Resources.Load ("Ice"));
						SetPosition (Ice, i + 1, j);
						break;
					case 4: // SpringTile
						GameObject SpringTile = (GameObject)Instantiate (Resources.Load ("SpringTile"));
						SetPosition (SpringTile, i, j);
						GameObject Trap = (GameObject)Instantiate (Resources.Load ("Spring"));
						SetPosition (Spring, i + 1, j);
						break;
					case 5: // TrapTile
						GameObject TrapTile = (GameObject)Instantiate (Resources.Load ("TrapTile"));
						SetPosition (TrapTile, i, j);
						GameObject Trap = (GameObject)Instantiate (Resources.Load ("Trap"));
						SetPosition (Trap, i + 1, j);
						break;
					default: // WoodTile
						GameObject WoodTile = (GameObject)Instantiate (Resources.Load ("WoodTile"));
						SetPosition (WoodTile, i, j);
						break;
					}
				}
			}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Set the position for the gameobject
	private void SetPosition (GameObject Tile, int i, int j) {
		int x, y;

		if (col % 2 == 1)
			x = (j - (col - 1) / 2) * tilesize;
		else
			x = (j - col / 2) * tilesize;

		if (row % 2 == 1)
			y = (i - (row - 1) / 2) * tilesize;
		else
			y = (i - row / 2) * tilesize;

		Tile.transform.position = new Vector2 (x, y);
	}
}

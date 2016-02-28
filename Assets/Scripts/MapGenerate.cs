using UnityEngine;
using System.Collections;

public class MapGenerate : MonoBehaviour {

	private int col = 20;
	private int row = 15;
	private int rownum;
	private int[,] blocked = new int[row, col]{0};

	// Use this for initialization
	void Start () {
		rownum = Random.Range (2, 5);
		int gap = row / rownum / 3;
		int blocklength;
		int barnum;

		for (int i = 0; i < rownum; i++)
			blocked[i * row / rownum + Random.Range(0, gap), 0] = 1;

		for (int i = 0; i < row; i++) {
			if (blocked [i, 0] == 1) {
				blocked [i, 0] = 0;
				blocklength = Random.Range (col / 5, col / 2);
				barnum = Random.Range (blocklength / 1, blocklength / 2);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

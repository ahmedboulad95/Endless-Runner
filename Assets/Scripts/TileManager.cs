using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour 
{
	public GameObject[] tilePrefabs;
	private List<GameObject> activeTiles;

	private Transform playerTransform;

	private float offset = 38.0f;
	private float spawnZ = -13.0f;
	private float tileLength = 26.0f;
	private int numTilesOnScreen = 20;

	private int lastPrefabIndex = 0;

	private Color[] colors;

	private GameObject player;
	private Color blockColor;
	private Color pathColor;

	private int currentLevel;

	public Material floorMat;
	public Material obstacleMat;

	private Color lastBlockColor;
	private Color lastPathColor;

	void Start () 
	{
		colors = new Color[] { new Color32(255, 0 , 0, 1), new Color32(0, 255, 0, 1), new Color32(0, 0, 255, 1), new Color32( 166 , 254 , 0, 1 ), new Color32( 143 , 0 , 254, 1 ),
			new Color32( 232 , 0 , 254, 1 ), new Color32( 254 , 161 , 0, 1 ), new Color32( 254 , 224 , 0, 1 )};
		
		activeTiles = new List<GameObject>();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;

		player = GameObject.FindGameObjectWithTag("Player");

		obstacleMat.color = colors [Random.Range (0, colors.Length)];
		floorMat.color = colors [Random.Range (0, colors.Length)];

		while (obstacleMat.color == floorMat.color) 
		{
			obstacleMat.color = colors [Random.Range (0, colors.Length)];
			floorMat.color = colors [Random.Range (0, colors.Length)];
		}

		lastBlockColor = obstacleMat.color;
		lastPathColor = floorMat.color;

		currentLevel = player.GetComponent<ScoreKeeper> ().level;

		for (int i = 0; i < numTilesOnScreen; i++) 
		{
			if (i < 2)
				SpawnTile (0);
			else
				SpawnTile ();
		}

		lastBlockColor = obstacleMat.color;
		lastPathColor = floorMat.color;
	}

	void Update () 
	{
		if ((playerTransform.position.z - offset) > (spawnZ - numTilesOnScreen * tileLength)) 
		{
			SpawnTile ();
			DeleteTile ();
		}

		if (player.GetComponent<ScoreKeeper> ().level > currentLevel) 
		{
			obstacleMat.color = colors [Random.Range (0, colors.Length)];
			floorMat.color = colors [Random.Range (0, colors.Length)];
			currentLevel = player.GetComponent<ScoreKeeper> ().level;

			while (obstacleMat.color == floorMat.color || obstacleMat.color == lastBlockColor || floorMat.color == lastPathColor) 
			{
				obstacleMat.color = colors [Random.Range (0, colors.Length)];
				floorMat.color = colors [Random.Range (0, colors.Length)];
			}

			lastBlockColor = obstacleMat.color;
			lastPathColor = floorMat.color;
		}
	}

	void SpawnTile(int prefabIndex = -1)
	{
		GameObject go;

		if (prefabIndex == -1) 
		{
			go = Instantiate (tilePrefabs [RandomPrefab ()]) as GameObject;
			go.transform.SetParent (transform);
			go.transform.position = new Vector3 (0, Vector3.forward.y, spawnZ);
		} 
		else 
		{
			go = Instantiate (tilePrefabs [prefabIndex]) as GameObject;
			go.transform.SetParent (transform);
			go.transform.position = new Vector3(0, Vector3.forward.y, spawnZ);
		}

		spawnZ += tileLength;
		activeTiles.Add (go);
	}

	void DeleteTile()
	{
		Destroy (activeTiles [0]);
		activeTiles.RemoveAt (0);
	}

	int RandomPrefab()
	{
		if (tilePrefabs.Length <= 1)
			return 0;

		int index = lastPrefabIndex;

		while(index == lastPrefabIndex)
			index = Random.Range(1, tilePrefabs.Length);

		lastPrefabIndex = index;

		return index;
	}
}

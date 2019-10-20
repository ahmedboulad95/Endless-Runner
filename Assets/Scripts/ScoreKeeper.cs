using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour 
{
	public Text scoreText;
	private float score = 0.0f;
	private float multiplier = 10.0f;

	public int level = 1;
	private int maxLevel = 20;
	private int scoreToNextLevel = 2000;

	void Update () 
	{
		if (score >= scoreToNextLevel)
			LevelUp ();

		if (GetComponent<PlayerMovement> ().isDead)
			return;

		score += 8 * Time.deltaTime * multiplier * level;
		scoreText.text = ((int)score).ToString ();
	}

	void LevelUp()
	{
		if (level == maxLevel)
			return;

		scoreToNextLevel *= 2;
		level++;

		GetComponent<PlayerMovement> ().SetSpeed (level);
	}
}

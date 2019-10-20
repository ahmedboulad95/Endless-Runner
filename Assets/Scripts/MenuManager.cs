using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	public GameObject replayButton;
	public GameObject returnToMain;

	public void Replay()
	{
		replayButton.SetActive (false);
		returnToMain.SetActive (false);
		SceneManager.LoadScene ("Game");
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}

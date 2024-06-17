using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
	[SerializeField] private float _gameOverScreenDelaySeconds;
	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void GameOver()
	{
		StartCoroutine(GoToGameOver(_gameOverScreenDelaySeconds));
	}
	IEnumerator GoToGameOver(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		SceneManager.LoadScene(2);
	}

	public void Retry()
	{
		print("Retry");
	}


	public void Quit()
	{
		Application.Quit();
	}
}

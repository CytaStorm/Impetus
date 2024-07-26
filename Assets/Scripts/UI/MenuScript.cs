using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
	public static MenuScript Menu 
	{
		get; private set; 
	}

	[SerializeField] private float _gameOverScreenDelaySeconds;

	private void Awake()
    {
        if (Menu != null &&
			Menu != this)
        {
            Destroy(gameObject);
        }
        else Menu = this;
    }

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(2);
	}

	public void GameOver()
	{
		print("here0");
		StartCoroutine(GoToGameOver(_gameOverScreenDelaySeconds));
		print("here");
	}
	IEnumerator GoToGameOver(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		SceneManager.LoadScene(1);
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

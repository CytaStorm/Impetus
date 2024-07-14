using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
	[SerializeField] private GameObject _playerIcon;
	[SerializeField] private Slider _slider;
	// Start is called before the first frame update
	void Start()
	{
		_slider.maxValue = LevelManagerScript.LevelManager.RoomsToMake;
		_slider.minValue = 1;
		LevelManagerScript.LevelManager.ChangeRoom.AddListener(ChangeRoom);
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void ChangeRoom(int roomChange)
	{
		_slider.value += roomChange;
	}
}

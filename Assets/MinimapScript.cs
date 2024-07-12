using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
	[SerializeField] private GameObject _playerIcon;
	[SerializeField] private RectTransform _rectTransform;
	private float _roomIncrementWidth;
	// Start is called before the first frame update
	void Start()
	{
		_roomIncrementWidth =
			(_rectTransform.rect.width -
			_playerIcon.GetComponent<RectTransform>().rect.width) /
			(LevelManagerScript.LevelManager.RoomsToMake - 1);

		print(_rectTransform.rect.width + 
			_playerIcon.GetComponent<RectTransform>().rect.width / 2);
		LevelManagerScript.LevelManager.ChangeRoom.AddListener(ChangeRoom);
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void ChangeRoom(int roomChange)
	{
		_playerIcon.transform.position += 
			new Vector3(roomChange * _roomIncrementWidth, 0, 0);
	}
}

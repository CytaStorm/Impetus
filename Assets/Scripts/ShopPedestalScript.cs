using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPedestalScript : MonoBehaviour
{
	#region Components
	[Header("Components")]
    [SerializeField] private GameObject _itemToSell;
	[SerializeField] private TextMeshPro _goldCostText;
	#endregion

	#region Pedestal specifics
	[Header("Pedestal Stats")]
	[SerializeField] private float _goldCost;

	private GameObject _unBoughtItem;
	private PlayerScript _player;
	#endregion
	// Start is called before the first frame update
	void Start()
    {
        _unBoughtItem = Instantiate(_itemToSell, 
			transform.position + new Vector3(0, 0.6f, 0),
			transform.rotation,
			this.transform);
		_unBoughtItem.GetComponent<Collider2D>().enabled = false;

		_goldCostText.text = _goldCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collider)
	{
		print("here");
        if (collider.transform.tag != "Player")
        {
            return;
        }
		_player = collider.gameObject.GetComponent<PlayerScript>();
		_player.BuyItem.AddListener(SellItem);
		_player.OnPedestal = true;
	}
	private void OnTriggerExit2D(Collider2D collider)
	{
		print("exit");
        if (collider.transform.tag != "Player")
        {
            return;
        }
		_player = collider.gameObject.GetComponent<PlayerScript>();
		_player.BuyItem.RemoveListener(SellItem);
		_player.OnPedestal = false;
	}

	private void SellItem(PlayerScript player, float gold)
	{
		if (_unBoughtItem == null)
		{
			print("Nothing to sell!");
			return;
		}
		if (gold <  _goldCost)
		{
			print("Not enough gold!");
			//Also display it in a message in game, play invalid input sound.
			return;
		}
		//Deduct gold
		player.Gold -= _goldCost;

		//Give item
		_unBoughtItem.GetComponent<Collider2D>().enabled = true;
		//Destroy(_unBoughtItem);
		Destroy(_goldCostText);
	}

}

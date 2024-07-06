using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManagerScript : MonoBehaviour
{
	[Header("Player")] [SerializeField] private GameObject _playerObject;
	private PlayerScript _playerScript;

	[Header("Health")] [SerializeField] private GameObject _healthBar;

	[Header("Aether")] [SerializeField] private GameObject _aetherBar;

	[Header("Flow Bar")] [SerializeField] private GameObject _flowBar;

	[Header("Gold")] [SerializeField] private GameObject _goldCounter;

	[Header("Attack Buff")] [SerializeField] private GameObject _AttackBuffCounter;

	private Slider _healthBarSlider;

	private TextMeshProUGUI _goldCounterText;

	private TextMeshProUGUI _AttackBuffCounterText;

	void Awake()
	{
		_playerScript = _playerObject.GetComponent<PlayerScript>();

		//Setup Health/Flow/Aether
		_flowBar.GetComponent<FlowBarScript>().PlayerScript = _playerScript;
		_aetherBar.GetComponent<AetherBarScript>().PlayerScript = _playerScript;
	}

	// Start is called before the first frame update
	void Start()
	{
		_healthBarSlider = _healthBar.GetComponent<Slider>();
		_goldCounterText = _goldCounter.GetComponent<TextMeshProUGUI>();
		_AttackBuffCounterText = _AttackBuffCounter.GetComponent<TextMeshProUGUI>();
	}

	// Update is called once per frame
	void Update()
	{
		_healthBarSlider.value = _playerScript.Health;
		_healthBarSlider.maxValue = _playerScript.MaxHealth;
		_goldCounterText.text = _playerScript.Gold.ToString();
		_AttackBuffCounterText.text = _playerScript.AttackBuffRoomsLeft.ToString();
	}
}

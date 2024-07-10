using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManagerScript : MonoBehaviour
{
	[Header("Health")] [SerializeField] private GameObject _healthBar;

	[Header("Aether")] [SerializeField] private GameObject _aetherBar;

	[Header("Flow Bar")] [SerializeField] private GameObject _flowBar;

	[Header("Gold")] [SerializeField] private GameObject _goldCounter;

	[Header("Attack Buff")] [SerializeField] private GameObject _AttackBuffCounter;

	private Slider _healthBarSlider;

	private TextMeshProUGUI _goldCounterText;

	private TextMeshProUGUI _AttackBuffCounterText;

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
		_healthBarSlider.value = PlayerScript.Player.Health;
		_healthBarSlider.maxValue = PlayerScript.Player.MaxHealth;
		_goldCounterText.text = PlayerScript.Player.Gold.ToString();
		_AttackBuffCounterText.text = PlayerScript.Player.AttackBuffRoomsLeft.ToString();
	}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManagerScript : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject _playerObject;

    [Header("Health")]
	[SerializeField] private GameObject _healthBar;

    [Header("Aether")]
    [SerializeField] private GameObject _aetherBar;

    [Header("Flow")]
    [SerializeField] private GameObject _flowBar;

    [Header("Gold")]
    [SerializeField] private GameObject _goldCounter;

    [Header("Attack Buff")]
    [SerializeField] private GameObject _AttackBuffCounter;

    private PlayerScript _player;

	private Slider _healthBarSlider;

    private Slider _aetherBarSlider;

    private Slider _flowBarSlider;


    private TextMeshProUGUI _goldCounterText;

    private TextMeshProUGUI _AttackBuffCounterText;

    // Start is called before the first frame update

    void Start()
    {
        _player = _playerObject.GetComponent<PlayerScript>();
        _healthBarSlider = _healthBar.GetComponent<Slider>();
        _aetherBarSlider = _aetherBar.GetComponent<Slider>();
        _flowBarSlider = _flowBar.GetComponent<Slider>();
        _goldCounterText = _goldCounter.GetComponent<TextMeshProUGUI>();
        _AttackBuffCounterText = _AttackBuffCounter.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthBarSlider.value = _player.Health;
        _healthBarSlider.maxValue = _player.MaxHealth;
        _aetherBarSlider.value = _player.Aether;
        _aetherBarSlider.maxValue = _player.MaxAether;
        _flowBarSlider.value = _player.Flow;
        _flowBarSlider.maxValue = _player.MaxFlow;
        _goldCounterText.text = _player.Gold.ToString();
        _AttackBuffCounterText.text = _player.AttackBuffRoomsLeft.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("GameObject Components")]
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _aetherBar;
    [SerializeField] private GameObject _flowBar;
    [SerializeField] private GameObject _flowStateLevel;
    [SerializeField] private GameObject _player;

    private Slider _healthBarSlider;
    private Slider _aetherBarSlider;
    private Slider _flowBarSlider;
    private TextMeshProUGUI _flowStateLevelText;
    private PlayerStats _playerStats;
    void Start()
    {
        _playerStats = _player.GetComponent<PlayerStats>();
        _healthBarSlider = _healthBar.GetComponent<Slider>();
        _aetherBarSlider = _aetherBar.GetComponent<Slider>();
        _flowBarSlider = _flowBar.GetComponent<Slider>();
        _flowStateLevelText = _flowStateLevel.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthBarSlider.value = _playerStats.Health;
        _healthBarSlider.maxValue = _playerStats.MaxHealth;
        _aetherBarSlider.value = _playerStats.Aether;
        _aetherBarSlider.maxValue = _playerStats.MaxAether;
        _flowBarSlider.value = _playerStats.Flow;
        _flowBarSlider.maxValue = _playerStats.MaxFlow;
        _flowStateLevelText.text = _playerStats.FlowState.ToString();
    }
}

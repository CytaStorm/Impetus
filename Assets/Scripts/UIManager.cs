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
    [SerializeField] private GameObject _playerObject;

    private Slider _healthBarSlider;
    private Slider _aetherBarSlider;
    private Slider _flowBarSlider;
    private TextMeshProUGUI _flowStateLevelText;
    private Player _player;
    void Start()
    {
        _player = _playerObject.GetComponent<Player>();
        _healthBarSlider = _healthBar.GetComponent<Slider>();
        _aetherBarSlider = _aetherBar.GetComponent<Slider>();
        _flowBarSlider = _flowBar.GetComponent<Slider>();
        _flowStateLevelText = _flowStateLevel.GetComponent<TextMeshProUGUI>();
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
        _flowStateLevelText.text = _player.FlowState.ToString();
    }
}

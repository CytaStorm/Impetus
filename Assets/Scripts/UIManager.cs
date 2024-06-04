using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("GameObjec Components")]
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _aetherBar;
    [SerializeField] private GameObject _player;

    private Slider _healthBarSlider;
    private Slider _aetherBarSlider;
    private PlayerStats _playerStats;
    void Start()
    {
        _playerStats = _player.GetComponent<PlayerStats>();
        _healthBarSlider = _healthBar.GetComponent<Slider>();
        _aetherBarSlider = _aetherBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthBarSlider.value = _playerStats.PlayerHealth;
        _healthBarSlider.maxValue = _playerStats.PlayerMaxHealth;
        _aetherBarSlider.value = _playerStats.PlayerMana;
        _aetherBarSlider.maxValue = _playerStats.PlayerMaxMana;
    }
}

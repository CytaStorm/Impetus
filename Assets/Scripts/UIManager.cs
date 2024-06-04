using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _player;

    private Slider _healthBarSlider;
    void Start()
    {
        _healthBarSlider = _healthBar.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _healthBarSlider.value = _player.GetComponent<PlayerStats>().PlayerHealth;
    }
}

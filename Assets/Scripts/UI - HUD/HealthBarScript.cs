using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _healthBarSlider.value = PlayerScript.Player.Health;
        _healthBarSlider.maxValue = PlayerScript.Player.MaxHealth;
    }
}

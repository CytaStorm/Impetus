using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AetherBarScript : MonoBehaviour
{
    [SerializeField] private Slider _aetherBarSlider;
    [HideInInspector] public PlayerScript PlayerScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _aetherBarSlider.value = PlayerScript.Aether;
        _aetherBarSlider.maxValue = PlayerScript.MaxAether;
    }
}

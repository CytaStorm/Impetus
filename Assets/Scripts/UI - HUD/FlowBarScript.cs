using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowBarScript : MonoBehaviour
{
    [SerializeField] private Slider _flowBarSlider;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _fill;
    [SerializeField] private Gradient _gradient;
    private Image _image;

    [HideInInspector] public PlayerScript PlayerScript;

    private Animator _animator;

	// Start is called before the first frame update
	void Start()
    {
        _image = _fill.GetComponent<Image>();
        _animator = _star.GetComponent<Animator>();
        PlayerScript.Player.UpgradeFlowState.AddListener(UpgradeFlowState);
        PlayerScript.Player.DegradeFlowState.AddListener(DegradeFlowState);
        _animator.SetInteger("Start State", PlayerScript.Player.FlowState);
        CheckFlowBarGradient();
    }

    // Update is called once per frame
    void Update()
    {
        _flowBarSlider.value = PlayerScript.Player.FlowRatio * 135;
        CheckFlowBarGradient();
    }

    void UpgradeFlowState(int currentState, int newState)
    {
        _animator.SetTrigger("Upgrade Flow State");
    }

    void DegradeFlowState(int currentState, int newState)
    {
        _animator.SetTrigger("Degrade Flow State");
    }

    private void CheckFlowBarGradient()
    {
        _image.color = _gradient.Evaluate(PlayerScript.Player.FlowRatio);
    }
}

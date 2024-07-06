using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowBarScript : MonoBehaviour
{
    [SerializeField] private Slider _flowBarSlider;
    [SerializeField] private GameObject _star;

    [HideInInspector] public PlayerScript PlayerScript;

    private Animator _animator;

	// Start is called before the first frame update
	void Start()
    {
        _animator = _star.GetComponent<Animator>();
        PlayerScript.UpgradeFlowState.AddListener(UpgradeFlowState);
        PlayerScript.DegradeFlowState.AddListener(DegradeFlowState);
        _animator.SetInteger("Start State", PlayerScript.FlowState);
    }

    // Update is called once per frame
    void Update()
    {
        _flowBarSlider.value = PlayerScript.Flow;
        _flowBarSlider.maxValue = PlayerScript.MaxFlow;
    }

    void UpgradeFlowState(int currentState, int newState)
    {
        _animator.SetTrigger("Upgrade Flow State");
    }

    void DegradeFlowState(int currentState, int newState)
    {
        _animator.SetTrigger("Degrade Flow State");
    }
}

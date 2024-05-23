using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
	#region Attack Cooldown Fields
	public float AttackCooldownTime;

    private float _attackCooldownTimer;

    private bool _attackReady 
    {
        get 
        {
            return _attackCooldownTimer == 0f; 
        }
    }
    #endregion

    private GameObject _sword;

	// Start is called before the first frame update
	void Start()
    {
        _sword = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_attackReady)
        {
            _attackCooldownTimer = 
                Mathf.Clamp(_attackCooldownTimer - Time.deltaTime, 0f, AttackCooldownTime);
        }
    }

	#region Attack Methods
	public void OnSlash(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (_attackReady)
        {
            Debug.Log("slash");
            _attackCooldownTimer = AttackCooldownTime;
            return;
        }
        Debug.Log("attack on cooldown!");
    }

    public void OnThrust(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (_attackReady)
        {
            Debug.Log("thrust");
            _attackCooldownTimer = AttackCooldownTime;
            return;
        }
        Debug.Log("attack on cooldown!");
    }

    public void OnSlam(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (_attackReady)
        {
            Debug.Log("slam");
            _attackCooldownTimer = AttackCooldownTime;
            return;
        }
        Debug.Log("attack on cooldown!");
    }
    #endregion

}

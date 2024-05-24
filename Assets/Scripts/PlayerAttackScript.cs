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
        get { return _attackCooldownTimer == 0f; }
    }
	#endregion

	#region Sword Drawing
	private GameObject _swordControlPoint;
    private Vector3 _mousePosition;

    /// <summary>
    /// Used to determine which side of the player sword
    /// should be drawn on.
    /// </summary>
    private byte _swingOffset;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _swordControlPoint = this.gameObject.transform.GetChild(0).gameObject;
        _swingOffset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Point sword in direction of mouse
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(
            _mousePosition.x - _swordControlPoint.transform.position.x,
            _mousePosition.y - _swordControlPoint.transform.position.y);

        _swordControlPoint.transform.up = direction;
        
        if (_swingOffset == 0)
        {
            _swordControlPoint.transform.Rotate(
                new Vector3(0, 0, 100));
        }
        else
        {
            _swordControlPoint.transform.Rotate(
                new Vector3(0, 0, -100));
        }

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
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}

		Debug.Log("slash");
		_attackCooldownTimer = AttackCooldownTime;
		ChangeSwingOffset();
		return;
	}

	

	public void OnThrust(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}

		Debug.Log("thrust");
		_attackCooldownTimer = AttackCooldownTime;
		ChangeSwingOffset();
		return;
	}

	public void OnSlam(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}

		Debug.Log("slam");
		_attackCooldownTimer = AttackCooldownTime;
		ChangeSwingOffset();
		return;
	}
	#endregion

	private void ChangeSwingOffset()
	{
		if (_swingOffset == 0)
		{
			_swingOffset = 1;
		}
		else
		{
			_swingOffset = 0;
		}
	}

}

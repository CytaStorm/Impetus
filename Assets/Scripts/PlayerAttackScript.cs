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
	[Range(0f, 360f)] public int SlashArc;
	private GameObject _swordControlPoint;
    private Vector3 _mousePosition;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _swordControlPoint = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Point sword in direction of mouse
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(
            _mousePosition.x - _swordControlPoint.transform.position.x,
            _mousePosition.y - _swordControlPoint.transform.position.y);

        if (!_attackReady)
        {
            _attackCooldownTimer = 
                Mathf.Clamp(_attackCooldownTimer - Time.deltaTime, 0f, AttackCooldownTime);
			_swordControlPoint.transform.up = direction;
			return;
        }
    }

	#region Attack Methods
	public void OnSlashInput(InputAction.CallbackContext context)
	{
		if (!context.performed)	return;
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}

		Debug.Log("slash");
		_attackCooldownTimer = AttackCooldownTime;

        _swordControlPoint.transform.Rotate(
            new Vector3(0, 0, SlashArc / 2));
		return;
	}

	public void OnThrustInput(InputAction.CallbackContext context)
	{
		if (!context.performed)	return;
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}

		Debug.Log("thrust");
		_attackCooldownTimer = AttackCooldownTime;
		return;
	}

	public void OnSlamInput(InputAction.CallbackContext context)
	{
		if (!context.performed)	return;
		if (!_attackReady)
		{
			Debug.Log("attack on cooldown!");
			return;
		}

		Debug.Log("slam");
		_attackCooldownTimer = AttackCooldownTime;
		return;
	}

	private void Slash()
	{

	}
	#endregion
}
